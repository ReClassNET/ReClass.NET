#define EXTERN_DLL_EXPORT 

#include <windows.h>
#include <Psapi.h>
#include <tlhelp32.h>
#include <vector>
#include <algorithm>

#include <beaengine/BeaEngine.h>

const int PATH_MAXIMUM_LENGTH = 260;

enum class RequestFunction
{
	IsProcessValid,
	OpenRemoteProcess,
	CloseRemoteProcess,
	ReadRemoteMemory,
	WriteRemoteMemory,
	EnumerateProcesses,
	EnumerateRemoteSectionsAndModules,
	DisassembleRemoteCode,
	ControlRemoteProcess
};

typedef LPVOID(__stdcall *RequestFunctionPtrCallback)(RequestFunction request);
RequestFunctionPtrCallback requestFunction;

EXTERN_DLL_EXPORT VOID __stdcall Initialize(RequestFunctionPtrCallback requestCallback)
{
	requestFunction = requestCallback;
}

DWORD lastError = 0;
EXTERN_DLL_EXPORT DWORD __stdcall GetLastErrorCode()
{
	return lastError;
}

EXTERN_DLL_EXPORT BOOL __stdcall IsProcessValid(HANDLE process)
{
	if (!process)
	{
		return FALSE;
	}

	auto retn = WaitForSingleObject(process, 0);
	if (retn == WAIT_FAILED)
	{
		return FALSE;
	}
	return retn == WAIT_TIMEOUT;
}

EXTERN_DLL_EXPORT LPVOID __stdcall OpenRemoteProcess(DWORD pid, DWORD desiredAccess)
{
	return OpenProcess(desiredAccess, FALSE, pid);
}

EXTERN_DLL_EXPORT VOID __stdcall CloseRemoteProcess(HANDLE process)
{
	CloseHandle(process);
}

EXTERN_DLL_EXPORT BOOL __stdcall ReadRemoteMemory(HANDLE process, LPCVOID address, LPVOID buffer, SIZE_T size)
{
	if (ReadProcessMemory(process, address, buffer, size, nullptr))
	{
		lastError = 0;

		return TRUE;
	}

	lastError = GetLastError();

	return FALSE;
}

EXTERN_DLL_EXPORT BOOL __stdcall WriteRemoteMemory(HANDLE process, LPVOID address, LPCVOID buffer, SIZE_T size)
{
	DWORD oldProtect;
	if (VirtualProtectEx(process, address, size, PAGE_EXECUTE_READWRITE, &oldProtect))
	{
		if (WriteProcessMemory(process, address, buffer, size, nullptr))
		{
			VirtualProtectEx(process, address, size, oldProtect, nullptr);

			lastError = 0;

			return TRUE;
		}
	}

	lastError = GetLastError();

	return FALSE;
}

enum class Platform
{
	Unknown,
	X86,
	X64
};

Platform GetProcessPlatform(HANDLE process)
{
	auto GetProcessorArchitecture = []()
	{
		static USHORT processorArchitecture = PROCESSOR_ARCHITECTURE_UNKNOWN;
		if (processorArchitecture == PROCESSOR_ARCHITECTURE_UNKNOWN)
		{
			SYSTEM_INFO info = {};
			GetNativeSystemInfo(&info);

			processorArchitecture = info.wProcessorArchitecture;
		}
		return processorArchitecture;
	};

	switch (GetProcessorArchitecture())
	{
		case PROCESSOR_ARCHITECTURE_INTEL:
			return Platform::X86;
		case PROCESSOR_ARCHITECTURE_AMD64:
			BOOL isWow64 = FALSE;
			if (IsWow64Process(process, &isWow64))
			{
				return isWow64 ? Platform::X86 : Platform::X64;
			}

#ifdef _WIN64
			return Platform::X64;
#else
			return Platform::X86;
#endif
	}
	return Platform::Unknown;
}

typedef VOID(__stdcall EnumerateProcessCallback)(DWORD pid, WCHAR modulePath[PATH_MAXIMUM_LENGTH]);

EXTERN_DLL_EXPORT VOID __stdcall EnumerateProcesses(EnumerateProcessCallback callbackProcess)
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
			auto openRemoteProcess = reinterpret_cast<decltype(OpenRemoteProcess)*>(requestFunction(RequestFunction::OpenRemoteProcess));
			auto closeRemoteProcess = reinterpret_cast<decltype(CloseRemoteProcess)*>(requestFunction(RequestFunction::CloseRemoteProcess));

			do
			{
				auto process = openRemoteProcess(pe32.th32ProcessID, PROCESS_QUERY_INFORMATION | PROCESS_VM_READ);
				if (process != nullptr && process != INVALID_HANDLE_VALUE)
				{
					auto platform = GetProcessPlatform(process);
					
#ifdef _WIN64
					if (platform == Platform::X64)
#else
					if (platform == Platform::X86)
#endif
					{
						WCHAR process_path[MAX_PATH] = { };
						GetModuleFileNameExW(process, NULL, process_path, MAX_PATH);

						callbackProcess(pe32.th32ProcessID, process_path);
					}

					closeRemoteProcess(process);
				}
			} while (Process32NextW(handle, &pe32));
		}

		CloseHandle(handle);

		lastError = 0;

		return;
	}

	lastError = GetLastError();
}

typedef VOID(__stdcall EnumerateRemoteSectionsCallback)(LPVOID baseAddress, SIZE_T regionSize, BYTE name[IMAGE_SIZEOF_SHORT_NAME + 2], DWORD state, DWORD protection, DWORD type, WCHAR modulePath[PATH_MAXIMUM_LENGTH]);
typedef VOID(__stdcall EnumerateRemoteModulesCallback)(LPVOID baseAddress, SIZE_T regionSize, WCHAR modulePath[PATH_MAXIMUM_LENGTH]);

EXTERN_DLL_EXPORT VOID __stdcall EnumerateRemoteSectionsAndModules(HANDLE process, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule)
{
	if (callbackSection == nullptr && callbackModule == nullptr)
	{
		return;
	}

	struct SectionInfo
	{
		LPVOID BaseAddress;
		SIZE_T RegionSize;
		BYTE Name[IMAGE_SIZEOF_SHORT_NAME + 2];
		DWORD State;
		DWORD Protection;
		DWORD Type;
		WCHAR ModulePath[PATH_MAXIMUM_LENGTH];
	};
	std::vector<SectionInfo> sections;

	SYSTEM_INFO sysInfo;
	GetSystemInfo(&sysInfo);

	MEMORY_BASIC_INFORMATION memInfo;
	size_t address = (size_t)sysInfo.lpMinimumApplicationAddress;
	while (address < (size_t)sysInfo.lpMaximumApplicationAddress)
	{
		if (VirtualQueryEx(process, (LPCVOID)address, &memInfo, sizeof(MEMORY_BASIC_INFORMATION)) != 0)
		{
			if (memInfo.State == MEM_COMMIT /*&& memInfo.Type == MEM_PRIVATE*/)
			{
				SectionInfo section = {};
				section.BaseAddress = memInfo.BaseAddress;
				section.RegionSize = memInfo.RegionSize;
				section.State = memInfo.State;
				section.Protection = memInfo.Protect;
				section.Type = memInfo.Type;

				sections.push_back(std::move(section));
			}
			address = (ULONG_PTR)memInfo.BaseAddress + memInfo.RegionSize;
		}
		else
		{
			address += 1024;
		}
	}

	auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, GetProcessId(process));
	if (handle != INVALID_HANDLE_VALUE)
	{
		MODULEENTRY32W me32 = {};
		me32.dwSize = sizeof(MODULEENTRY32W);
		if (Module32FirstW(handle, &me32))
		{
			auto readRemoteMemory = reinterpret_cast<decltype(ReadRemoteMemory)*>(requestFunction(RequestFunction::ReadRemoteMemory));

			do
			{
				if (callbackModule != nullptr)
				{
					callbackModule(me32.modBaseAddr, me32.modBaseSize, me32.szExePath);
				}

				if (callbackSection != nullptr)
				{
					auto it = std::lower_bound(std::begin(sections), std::end(sections), (LPVOID)me32.modBaseAddr, [&sections](const SectionInfo& lhs, const LPVOID& rhs)
					{
						return lhs.BaseAddress < rhs;
					});

					IMAGE_DOS_HEADER DosHdr = {};
					IMAGE_NT_HEADERS NtHdr = {};

					readRemoteMemory(process, me32.modBaseAddr, &DosHdr, sizeof(IMAGE_DOS_HEADER));
					readRemoteMemory(process, me32.modBaseAddr + DosHdr.e_lfanew, &NtHdr, sizeof(IMAGE_NT_HEADERS));

					std::vector<IMAGE_SECTION_HEADER> sectionHeaders(NtHdr.FileHeader.NumberOfSections);
					readRemoteMemory(process, me32.modBaseAddr + DosHdr.e_lfanew + sizeof(IMAGE_NT_HEADERS), sectionHeaders.data(), NtHdr.FileHeader.NumberOfSections * sizeof(IMAGE_SECTION_HEADER));
					for (int i = 0; i < NtHdr.FileHeader.NumberOfSections; ++i)
					{
						auto&& section = sectionHeaders[i];

						auto sectionAddress = (size_t)me32.modBaseAddr + section.VirtualAddress;
						for (auto j = it; j != std::end(sections); ++j)
						{
							if (sectionAddress >= (size_t)j->BaseAddress && sectionAddress < (size_t)j->BaseAddress + (size_t)j->RegionSize)
							{
								std::memcpy(j->Name, section.Name, IMAGE_SIZEOF_SHORT_NAME);
								std::memcpy(j->ModulePath, me32.szExePath, sizeof(SectionInfo::ModulePath));
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
				callbackSection(section.BaseAddress, section.RegionSize, section.Name, section.State, section.Protection, section.Type, section.ModulePath);
			}
		}

		lastError = 0;

		return;
	}

	lastError = GetLastError();
}

typedef VOID(__stdcall DisassembleRemoteCodeCallback)(LPVOID address, DWORD length, CHAR instruction[64]);

EXTERN_DLL_EXPORT VOID __stdcall DisassembleRemoteCode(HANDLE process, LPVOID address, int length, DisassembleRemoteCodeCallback callbackDisassembledCode)
{
	if (callbackDisassembledCode == nullptr)
	{
		return;
	}

	UIntPtr start = (UIntPtr)address;

	DISASM disasm = { };
	disasm.Options = NasmSyntax + PrefixedNumeral;
#ifdef _WIN64
	disasm.Archi = 64;
#endif

	auto readRemoteMemory = reinterpret_cast<decltype(ReadRemoteMemory)*>(requestFunction(RequestFunction::ReadRemoteMemory));

	std::vector<uint8_t> buffer(length);
	readRemoteMemory(process, address, buffer.data(), buffer.size());

	UIntPtr end = (UIntPtr)buffer.data() + length;

	disasm.EIP = (UIntPtr)buffer.data();
	disasm.VirtualAddr = start;

	while (true)
	{
		disasm.SecurityBlock = end - disasm.EIP;

		auto disamLength = Disasm(&disasm);
		if (disamLength == OUT_OF_BLOCK || disamLength == UNKNOWN_OPCODE)
		{
			break;
		}

		callbackDisassembledCode((LPVOID)disasm.VirtualAddr, disamLength, disasm.CompleteInstr);

		disasm.EIP += disamLength;
		if (disasm.EIP >= end || buffer[disasm.EIP - (UIntPtr)buffer.data()] == 0xCC)
		{
			break;
		}
		disasm.VirtualAddr += disamLength;
	}
}

enum class ControlRemoteProcessAction
{
	Suspend,
	Resume,
	Terminate
};

EXTERN_DLL_EXPORT VOID __stdcall ControlRemoteProcess(HANDLE process, ControlRemoteProcessAction action)
{
	if (action == ControlRemoteProcessAction::Suspend || action == ControlRemoteProcessAction::Resume)
	{
		auto processId = GetProcessId(process);

		auto handle = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
		if (handle != INVALID_HANDLE_VALUE)
		{
			auto fn = action == ControlRemoteProcessAction::Suspend ? SuspendThread : ResumeThread;

			THREADENTRY32 te32 = {};
			te32.dwSize = sizeof(THREADENTRY32);
			if (Thread32First(handle, &te32))
			{
				do
				{
					if (te32.th32OwnerProcessID == processId)
					{
						auto threadHandle = OpenThread(THREAD_SUSPEND_RESUME, FALSE, te32.th32ThreadID);
						if (threadHandle)
						{
							fn(threadHandle);

							CloseHandle(threadHandle);
						}
					}
				} while (Thread32Next(handle, &te32));
			}

			CloseHandle(handle);
		}
	}
	else if (action == ControlRemoteProcessAction::Terminate)
	{
		TerminateProcess(process, 0);
	}
}
