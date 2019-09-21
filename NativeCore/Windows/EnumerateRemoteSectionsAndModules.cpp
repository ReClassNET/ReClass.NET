#include <windows.h>
#include <winternl.h>
#include <tlhelp32.h>
#include <vector>
#include <algorithm>

#include "NativeCore.hpp"

template <typename Proc>
static DWORD EnumerateRemoteModulesNative(HANDLE process, Proc proc)
{
	const auto ntdll = GetModuleHandle(TEXT("ntdll"));
	if (!ntdll)
		return ERROR_MOD_NOT_FOUND;

	using tRtlNtStatusToDosError = ULONG (NTAPI *)(
			_In_ NTSTATUS Status
		);
	const auto _RtlNtStatusToDosError = tRtlNtStatusToDosError(GetProcAddress(ntdll, "RtlNtStatusToDosError"));
	if (!_RtlNtStatusToDosError)
		return ERROR_NOT_FOUND;

	using tNtQueryInformationProcess = NTSTATUS (NTAPI *)(
			_In_ HANDLE ProcessHandle,
			_In_ PROCESSINFOCLASS ProcessInformationClass,
			_Out_writes_bytes_(ProcessInformationLength) PVOID ProcessInformation,
			_In_ ULONG ProcessInformationLength,
			_Out_opt_ PULONG ReturnLength
		);
	
	const auto _NtQueryInformationProcess = tNtQueryInformationProcess(GetProcAddress(ntdll, "NtQueryInformationProcess"));
	if (!_NtQueryInformationProcess)
		return ERROR_NOT_FOUND;
	
	PROCESS_BASIC_INFORMATION pbi;
	const auto status = _NtQueryInformationProcess(process, ProcessBasicInformation, &pbi, sizeof(pbi), nullptr);
	if (!NT_SUCCESS(status))
		return _RtlNtStatusToDosError(status);

	PPEB_LDR_DATA ldr;
	auto success = ReadRemoteMemory(process, &pbi.PebBaseAddress->Ldr, &ldr, 0, sizeof(ldr));
	if (!success)
		return ERROR_READ_FAULT; // we seem to swallow the error anyways, might aswell give a distinctive one back

	const auto list_head = &ldr->InMemoryOrderModuleList; // remote address
	PLIST_ENTRY list_current; // remote address
	success = ReadRemoteMemory(process, &list_head->Flink, &list_current, 0, sizeof(list_current));
	if (!success)
		return ERROR_READ_FAULT;
	
	while (list_current != list_head)
	{
		// TODO: error handling - what do we do if module list changed? We can't un-call the callback

		LDR_DATA_TABLE_ENTRY mod;
		success = ReadRemoteMemory(process, CONTAINING_RECORD(list_current, LDR_DATA_TABLE_ENTRY, InMemoryOrderLinks), &mod, 0, sizeof(mod));
		if (!success)
			return ERROR_SUCCESS; // return success here to prevent running the other one

		EnumerateRemoteModuleData data = {};
		data.BaseAddress = mod.DllBase;
		data.Size = *(ULONG*)&mod.Reserved2[1]; // instead of undocced member could read ImageSize from headers
		const auto path_len = std::min(sizeof(RC_UnicodeChar) * (PATH_MAXIMUM_LENGTH - 1), size_t(mod.FullDllName.Length));
		success = ReadRemoteMemory(process, mod.FullDllName.Buffer, data.Path, 0, int(path_len));
		if (!success)
			return ERROR_SUCCESS; // return success here to prevent running the other one
		
		// UNICODE_STRING is not guaranteed to be null terminated
		data.Path[path_len / 2] = 0;
		
		proc(&data);
		
		list_current = mod.InMemoryOrderLinks.Flink;
	}
	
	return ERROR_SUCCESS;
}

template <typename Proc>
static DWORD EnumerateRemoteModulesWinapi(HANDLE process, Proc proc)
{
	const auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetProcessId(process));
	if (handle == INVALID_HANDLE_VALUE)
		return GetLastError();
	
	MODULEENTRY32W me32 = {};
	me32.dwSize = sizeof(MODULEENTRY32W);
	if (Module32FirstW(handle, &me32))
	{
		do
		{
			EnumerateRemoteModuleData data = {};
			data.BaseAddress = me32.modBaseAddr;
			data.Size = me32.modBaseSize;
			std::memcpy(data.Path, me32.szExePath, std::min(MAX_PATH, PATH_MAXIMUM_LENGTH));

			proc(&data);
		} while (Module32NextW(handle, &me32));
	}

	CloseHandle(handle);

	return ERROR_SUCCESS;
}

void RC_CallConv EnumerateRemoteSectionsAndModules(RC_Pointer process, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return;
	}

	std::vector<EnumerateRemoteSectionData> sections;

	MEMORY_BASIC_INFORMATION memInfo = { };
	memInfo.RegionSize = 0x1000;
	size_t address = 0;
	while (VirtualQueryEx(process, reinterpret_cast<LPCVOID>(address), &memInfo, sizeof(MEMORY_BASIC_INFORMATION)) != 0 && address + memInfo.RegionSize > address)
	{
		if (memInfo.State == MEM_COMMIT)
		{
			EnumerateRemoteSectionData section = {};
			section.BaseAddress = memInfo.BaseAddress;
			section.Size = memInfo.RegionSize;
			
			section.Protection = SectionProtection::NoAccess;
			if ((memInfo.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) section.Protection |= SectionProtection::Execute;
			if ((memInfo.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) section.Protection |= SectionProtection::Execute | SectionProtection::Read;
			if ((memInfo.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
			if ((memInfo.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memInfo.Protect & PAGE_READONLY) == PAGE_READONLY) section.Protection |= SectionProtection::Read;
			if ((memInfo.Protect & PAGE_READWRITE) == PAGE_READWRITE) section.Protection |= SectionProtection::Read | SectionProtection::Write;
			if ((memInfo.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) section.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memInfo.Protect & PAGE_GUARD) == PAGE_GUARD) section.Protection |= SectionProtection::Guard;
			
			switch (memInfo.Type)
			{
			case MEM_IMAGE:
				section.Type = SectionType::Image;
				break;
			case MEM_MAPPED:
				section.Type = SectionType::Mapped;
				break;
			case MEM_PRIVATE:
				section.Type = SectionType::Private;
				break;
			}

			section.Category = section.Type == SectionType::Private ? SectionCategory::HEAP : SectionCategory::Unknown;

			sections.push_back(std::move(section));
		}
		address = reinterpret_cast<size_t>(memInfo.BaseAddress) + memInfo.RegionSize;
	}

	const auto moduleEnumerator = [&](EnumerateRemoteModuleData* data)
	{
		if (callbackModule != nullptr)
			callbackModule(data);

		if (callbackSection != nullptr)
		{
			auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(data->BaseAddress), [&sections](const auto& lhs, const LPVOID& rhs)
				{
					return lhs.BaseAddress < rhs;
				});

			IMAGE_DOS_HEADER DosHdr = {};
			IMAGE_NT_HEADERS NtHdr = {};

			ReadRemoteMemory(process, data->BaseAddress, &DosHdr, 0, sizeof(IMAGE_DOS_HEADER));
			ReadRemoteMemory(process, PUCHAR(data->BaseAddress) + DosHdr.e_lfanew, &NtHdr, 0, sizeof(IMAGE_NT_HEADERS));

			std::vector<IMAGE_SECTION_HEADER> sectionHeaders(NtHdr.FileHeader.NumberOfSections);
			ReadRemoteMemory(process, PUCHAR(data->BaseAddress) + DosHdr.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), 0, NtHdr.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER));
			for (auto i = 0; i < NtHdr.FileHeader.NumberOfSections; ++i)
			{
				auto&& sectionHeader = sectionHeaders[i];

				const auto sectionAddress = reinterpret_cast<size_t>(data->BaseAddress) + sectionHeader.VirtualAddress;
				for (auto j = it; j != std::end(sections); ++j)
				{
					if (sectionAddress >= reinterpret_cast<size_t>(j->BaseAddress) && sectionAddress < reinterpret_cast<size_t>(j->BaseAddress) + static_cast<size_t>(j->Size))
					{
						// Copy the name because it is not null padded.
						char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
						std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);

						if (std::strcmp(buffer, ".text") == 0 || std::strcmp(buffer, "code") == 0)
						{
							j->Category = SectionCategory::CODE;
						}
						else if (std::strcmp(buffer, ".data") == 0 || std::strcmp(buffer, "data") == 0 || std::strcmp(buffer, ".rdata") == 0 || std::strcmp(buffer, ".idata") == 0)
						{
							j->Category = SectionCategory::DATA;
						}

						MultiByteToUnicode(buffer, j->Name, IMAGE_SIZEOF_SHORT_NAME);
						std::memcpy(j->ModulePath, data->Path, std::min(MAX_PATH, PATH_MAXIMUM_LENGTH));

						break;
					}
				}

			}
		}
	};
	
	if(EnumerateRemoteModulesNative(process, moduleEnumerator) != ERROR_SUCCESS)
		EnumerateRemoteModulesWinapi(process, moduleEnumerator);

	if (callbackSection != nullptr)
	{
		for (auto&& section : sections)
		{
			callbackSection(&section);
		}
	}
}
