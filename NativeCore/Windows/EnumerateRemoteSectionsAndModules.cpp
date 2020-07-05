#include <windows.h>
#include <winternl.h>
#include <tlhelp32.h>
#include <vector>
#include <algorithm>
#include <functional>

#include "NativeCore.hpp"

PPEB GetRemotePeb(const HANDLE process)
{
	static auto* const ntdll = GetModuleHandle(TEXT("ntdll"));
	if (!ntdll)
	{
		return nullptr;
	}

	using tNtQueryInformationProcess = NTSTATUS (NTAPI*)(_In_ HANDLE ProcessHandle, _In_ PROCESSINFOCLASS ProcessInformationClass, _Out_writes_bytes_(ProcessInformationLength) PVOID ProcessInformation, _In_ ULONG ProcessInformationLength, _Out_opt_ PULONG ReturnLength);

	static const auto pNtQueryInformationProcess = tNtQueryInformationProcess(GetProcAddress(ntdll, "NtQueryInformationProcess"));
	if (!pNtQueryInformationProcess)
	{
		return nullptr;
	}

	PROCESS_BASIC_INFORMATION pbi;
	if (!NT_SUCCESS(pNtQueryInformationProcess(process, ProcessBasicInformation, &pbi, sizeof(PROCESS_BASIC_INFORMATION), nullptr)))
	{
		return nullptr;
	}

	return pbi.PebBaseAddress;
}

using InternalEnumerateRemoteModulesCallback = std::function<void(EnumerateRemoteModuleData&)>;

bool EnumerateRemoteModulesNative(const RC_Pointer process, const InternalEnumerateRemoteModulesCallback& callback)
{
	auto* const ppeb = GetRemotePeb(process);
	if (ppeb == nullptr)
	{
		return false;
	}
	
	PPEB_LDR_DATA ldr;
	if (!ReadRemoteMemory(process, &ppeb->Ldr, &ldr, 0, sizeof(PPEB_LDR_DATA)))
	{
		return false;
	}

	auto* const head = &ldr->InMemoryOrderModuleList;
	PLIST_ENTRY current;
	if (!ReadRemoteMemory(process, &head->Flink, &current, 0, sizeof(PLIST_ENTRY)))
	{
		return false;
	}
	
	while (current != head)
	{
		LDR_DATA_TABLE_ENTRY entry;
		if (!ReadRemoteMemory(process, CONTAINING_RECORD(current, LDR_DATA_TABLE_ENTRY, InMemoryOrderLinks), &entry, 0, sizeof(entry)))
		{
			break;
		}

		EnumerateRemoteModuleData data = {};
		data.BaseAddress = entry.DllBase;
		data.Size = *reinterpret_cast<ULONG*>(&entry.Reserved3[1]); // instead of undocced member could read ImageSize from headers

		const auto length = std::min<int>(sizeof(RC_UnicodeChar) * (PATH_MAXIMUM_LENGTH - 1), entry.FullDllName.Length);
		if (!ReadRemoteMemory(process, entry.FullDllName.Buffer, data.Path, 0, length))
		{
			break;
		}
		data.Path[length / 2] = 0;
		
		callback(data);
		
		current = entry.InMemoryOrderLinks.Flink;
	}
	
	return true;
}

bool EnumerateRemoteModulesWinapi(const RC_Pointer process, const InternalEnumerateRemoteModulesCallback& callback)
{
	auto* const handle = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetProcessId(process));
	if (handle == INVALID_HANDLE_VALUE)
	{
		return false;
	}
	
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

			callback(data);
		} while (Module32NextW(handle, &me32));
	}

	CloseHandle(handle);

	return true;
}

void RC_CallConv EnumerateRemoteSectionsAndModules(RC_Pointer process, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return;
	}

	std::vector<EnumerateRemoteSectionData> sections;

	MEMORY_BASIC_INFORMATION memory = { };
	memory.RegionSize = 0x1000;
	size_t address = 0;
	while (VirtualQueryEx(process, reinterpret_cast<LPCVOID>(address), &memory, sizeof(MEMORY_BASIC_INFORMATION)) != 0 && address + memory.RegionSize > address)
	{
		if (memory.State == MEM_COMMIT)
		{
			EnumerateRemoteSectionData section = {};
			section.BaseAddress = memory.BaseAddress;
			section.Size = memory.RegionSize;
			
			section.Protection = SectionProtection::NoAccess;
			if ((memory.Protect & PAGE_EXECUTE) == PAGE_EXECUTE) section.Protection |= SectionProtection::Execute;
			if ((memory.Protect & PAGE_EXECUTE_READ) == PAGE_EXECUTE_READ) section.Protection |= SectionProtection::Execute | SectionProtection::Read;
			if ((memory.Protect & PAGE_EXECUTE_READWRITE) == PAGE_EXECUTE_READWRITE) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_EXECUTE_WRITECOPY) == PAGE_EXECUTE_WRITECOPY) section.Protection |= SectionProtection::Execute | SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_READONLY) == PAGE_READONLY) section.Protection |= SectionProtection::Read;
			if ((memory.Protect & PAGE_READWRITE) == PAGE_READWRITE) section.Protection |= SectionProtection::Read | SectionProtection::Write;
			if ((memory.Protect & PAGE_WRITECOPY) == PAGE_WRITECOPY) section.Protection |= SectionProtection::Read | SectionProtection::CopyOnWrite;
			if ((memory.Protect & PAGE_GUARD) == PAGE_GUARD) section.Protection |= SectionProtection::Guard;
			
			switch (memory.Type)
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

			sections.push_back(section);
		}
		address = reinterpret_cast<size_t>(memory.BaseAddress) + memory.RegionSize;
	}

	const auto moduleEnumerator = [&](EnumerateRemoteModuleData& data)
	{
		if (callbackModule != nullptr)
		{
			callbackModule(&data);
		}

		if (callbackSection != nullptr)
		{
			auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(data.BaseAddress), [&sections](const auto& lhs, const LPVOID& rhs)
			{
				return lhs.BaseAddress < rhs;
			});

			IMAGE_DOS_HEADER imageDosHeader = {};
			IMAGE_NT_HEADERS imageNtHeaders = {};

			if (!ReadRemoteMemory(process, data.BaseAddress, &imageDosHeader, 0, sizeof(IMAGE_DOS_HEADER))
				|| !ReadRemoteMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew, &imageNtHeaders, 0, sizeof(IMAGE_NT_HEADERS)))
			{
				return;
			}

			std::vector<IMAGE_SECTION_HEADER> sectionHeaders(imageNtHeaders.FileHeader.NumberOfSections);
			ReadRemoteMemory(process, PUCHAR(data.BaseAddress) + imageDosHeader.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), 0, imageNtHeaders.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER));
			for (auto&& sectionHeader : sectionHeaders)
			{
				const auto sectionAddress = reinterpret_cast<size_t>(data.BaseAddress) + sectionHeader.VirtualAddress;

				for (; it != std::end(sections); ++it)
				{
					auto&& section = *it;
					
					if (sectionAddress >= reinterpret_cast<size_t>(section.BaseAddress) 
						&& sectionAddress < reinterpret_cast<size_t>(section.BaseAddress) + static_cast<size_t>(section.Size)
						&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= data.Size)
					{
						if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
						{
							section.Category = SectionCategory::CODE;
						}
						else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
						{
							section.Category = SectionCategory::DATA;
						}

						try
						{
							// Copy the name because it is not null padded.
							char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
							std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
							MultiByteToUnicode(buffer, section.Name, IMAGE_SIZEOF_SHORT_NAME);
						}
						catch (std::range_error &)
						{
							std::memset(section.Name, 0, sizeof(section.Name));
						}
						std::memcpy(section.ModulePath, data.Path, std::min(MAX_PATH, PATH_MAXIMUM_LENGTH));

						break;
					}
				}
			}
		}
	};
	
	if (!EnumerateRemoteModulesNative(process, moduleEnumerator))
	{
		EnumerateRemoteModulesWinapi(process, moduleEnumerator);
	}

	if (callbackSection != nullptr)
	{
		for (auto&& section : sections)
		{
			callbackSection(&section);
		}
	}
}
