#include <windows.h>
#include <tlhelp32.h>

#include "NativeHelper.hpp"

void __stdcall ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action)
{
	if (action == ControlRemoteProcessAction::Suspend || action == ControlRemoteProcessAction::Resume)
	{
		auto processId = GetProcessId(handle);
		if (processId != 0)
		{
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
	}
	else if (action == ControlRemoteProcessAction::Terminate)
	{
		TerminateProcess(handle, 0);
	}
}
