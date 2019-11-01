#include <windows.h>
#include <tlhelp32.h>
#include <psapi.h>
#include <filesystem>

#include "NativeCore.hpp"

enum class Platform
{
	Unknown,
	X86,
	X64
};

Platform GetProcessPlatform(HANDLE process)
{
	static USHORT processorArchitecture = PROCESSOR_ARCHITECTURE_UNKNOWN;
	if (processorArchitecture == PROCESSOR_ARCHITECTURE_UNKNOWN)
	{
		SYSTEM_INFO info = {};
		GetNativeSystemInfo(&info);

		processorArchitecture = info.wProcessorArchitecture;
	}

	switch (processorArchitecture)
	{
		case PROCESSOR_ARCHITECTURE_INTEL:
			return Platform::X86;
		case PROCESSOR_ARCHITECTURE_AMD64:
			auto isWow64 = FALSE;
			if (IsWow64Process(process, &isWow64))
			{
				return isWow64 ? Platform::X86 : Platform::X64;
			}

#ifdef RECLASSNET64
			return Platform::X64;
#else
			return Platform::X86;
#endif
	}
	return Platform::Unknown;
}

void RC_CallConv EnumerateProcesses(EnumerateProcessCallback callbackProcess)
{
	if (callbackProcess == nullptr)
	{
		return;
	}

	const auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (handle != INVALID_HANDLE_VALUE)
	{
		PROCESSENTRY32W pe32 = {};
		pe32.dwSize = sizeof(PROCESSENTRY32W);
		if (Process32FirstW(handle, &pe32))
		{
			do
			{
				const auto process = OpenRemoteProcess(reinterpret_cast<RC_Pointer>(static_cast<size_t>(pe32.th32ProcessID)), ProcessAccess::Read);
				if (IsProcessValid(process))
				{
					const auto platform = GetProcessPlatform(process);
#ifdef RECLASSNET64
					if (platform == Platform::X64)
#else
					if (platform == Platform::X86)
#endif
					{
						EnumerateProcessData data = { };
						data.Id = pe32.th32ProcessID;
						GetModuleFileNameExW(process, nullptr, reinterpret_cast<LPWSTR>(data.Path), PATH_MAXIMUM_LENGTH);
						const auto name = std::filesystem::path(data.Path).filename().u16string();
						str16cpy(data.Name, name.c_str(), std::min<size_t>(name.length(), PATH_MAXIMUM_LENGTH - 1));

						callbackProcess(&data);
					}

				}
				
				CloseRemoteProcess(process);
				
			} while (Process32NextW(handle, &pe32));
		}

		CloseHandle(handle);
	}
}
