#include <windows.h>
#include <tlhelp32.h>
#include <vector>
#include <algorithm>

#include "NativeCore.hpp"

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

	const auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetProcessId(process));
	if (handle != INVALID_HANDLE_VALUE)
	{
		MODULEENTRY32W me32 = {};
		me32.dwSize = sizeof(MODULEENTRY32W);
		if (Module32FirstW(handle, &me32))
		{
			do
			{
				if (callbackModule != nullptr)
				{
					EnumerateRemoteModuleData data = {};
					data.BaseAddress = me32.modBaseAddr;
					data.Size = me32.modBaseSize;
					std::memcpy(data.Path, me32.szExePath, std::min(MAX_PATH, PATH_MAXIMUM_LENGTH));

					callbackModule(&data);
				}

				if (callbackSection != nullptr)
				{
					auto it = std::lower_bound(std::begin(sections), std::end(sections), static_cast<LPVOID>(me32.modBaseAddr), [&sections](const auto& lhs, const LPVOID& rhs)
					{
						return lhs.BaseAddress < rhs;
					});

					IMAGE_DOS_HEADER DosHdr = {};
					IMAGE_NT_HEADERS NtHdr = {};

					ReadRemoteMemory(process, me32.modBaseAddr, &DosHdr, 0, sizeof(IMAGE_DOS_HEADER));
					ReadRemoteMemory(process, me32.modBaseAddr + DosHdr.e_lfanew, &NtHdr, 0, sizeof(IMAGE_NT_HEADERS));

					std::vector<IMAGE_SECTION_HEADER> sectionHeaders(NtHdr.FileHeader.NumberOfSections);
					ReadRemoteMemory(process, me32.modBaseAddr + DosHdr.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), 0, NtHdr.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER));
					for (auto i = 0; i < NtHdr.FileHeader.NumberOfSections; ++i)
					{
						auto&& sectionHeader = sectionHeaders[i];

						const auto sectionAddress = reinterpret_cast<size_t>(me32.modBaseAddr) + sectionHeader.VirtualAddress;
						for (auto j = it; j != std::end(sections); ++j)
						{
							if (sectionAddress >= reinterpret_cast<size_t>(j->BaseAddress) 
								&& sectionAddress < reinterpret_cast<size_t>(j->BaseAddress) + static_cast<size_t>(j->Size)
								&& sectionHeader.VirtualAddress + sectionHeader.Misc.VirtualSize <= me32.modBaseSize )
							{
								if ((sectionHeader.Characteristics & IMAGE_SCN_CNT_CODE) == IMAGE_SCN_CNT_CODE)
								{
									j->Category = SectionCategory::CODE;
								}
								else if (sectionHeader.Characteristics & (IMAGE_SCN_CNT_INITIALIZED_DATA | IMAGE_SCN_CNT_UNINITIALIZED_DATA))
								{
									j->Category = SectionCategory::DATA;
								}

								try {
									// Copy the name because it is not null padded.
									char buffer[IMAGE_SIZEOF_SHORT_NAME + 1] = { 0 };
									std::memcpy(buffer, sectionHeader.Name, IMAGE_SIZEOF_SHORT_NAME);
									MultiByteToUnicode(buffer, j->Name, IMAGE_SIZEOF_SHORT_NAME);
								} catch (std::range_error &) {
									std::memset(j->Name, 0, sizeof j->Name);
								}
								std::memcpy(j->ModulePath, me32.szExePath, std::min(MAX_PATH, PATH_MAXIMUM_LENGTH));

								break;
							}
						}

					}
				}
			} while (Module32NextW(handle, &me32));
		}

		CloseHandle(handle);

		if (callbackSection != nullptr)
		{
			for (auto&& section : sections)
			{
				callbackSection(&section);
			}
		}
	}
}
