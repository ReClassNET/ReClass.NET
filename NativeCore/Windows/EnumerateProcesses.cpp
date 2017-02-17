#include <windows.h>
#include <tlhelp32.h>
#include <Psapi.h>

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
			BOOL isWow64 = FALSE;
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

void __stdcall EnumerateProcesses(EnumerateProcessCallback callbackProcess)
{
	if (callbackProcess == nullptr)
	{
		return;
	}

	auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
	if (handle != INVALID_HANDLE_VALUE)
	{
		PROCESSENTRY32W pe32 = {};
		pe32.dwSize = sizeof(PROCESSENTRY32W);
		if (Process32FirstW(handle, &pe32))
		{
			do
			{
				auto process = OpenRemoteProcess(reinterpret_cast<RC_Pointer>(pe32.th32ProcessID), ProcessAccess::Read);
				if (IsProcessValid(process))
				{
					auto platform = GetProcessPlatform(process);
#ifdef RECLASSNET64
					if (platform == Platform::X64)
#else
					if (platform == Platform::X86)
#endif
					{
						EnumerateProcessData data;
						data.Id = pe32.th32ProcessID;
						GetModuleFileNameExW(process, nullptr, reinterpret_cast<LPWSTR>(data.ModulePath), PATH_MAXIMUM_LENGTH);

						callbackProcess(&data);
					}

					CloseRemoteProcess(process);
				}
			} while (Process32NextW(handle, &pe32));
		}

		CloseHandle(handle);
	}
}
