#include <windows.h>
#include <tlhelp32.h>

#include "NativeCore.hpp"

void RC_CallConv ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action)
{
	if (action == ControlRemoteProcessAction::Suspend || action == ControlRemoteProcessAction::Resume)
	{
		const auto processId = GetProcessId(handle);
		if (processId != 0)
		{
			const auto snapshotHandle = CreateToolhelp32Snapshot(TH32CS_SNAPTHREAD, 0);
			if (snapshotHandle != INVALID_HANDLE_VALUE)
			{
				const auto fn = action == ControlRemoteProcessAction::Suspend ? SuspendThread : ResumeThread;

				THREADENTRY32 te32 = {};
				te32.dwSize = sizeof(THREADENTRY32);
				if (Thread32First(snapshotHandle, &te32))
				{
					do
					{
						if (te32.th32OwnerProcessID == processId)
						{
							const auto threadHandle = OpenThread(THREAD_SUSPEND_RESUME, FALSE, te32.th32ThreadID);
							if (threadHandle)
							{
								fn(threadHandle);

								CloseHandle(threadHandle);
							}
						}
					} while (Thread32Next(snapshotHandle, &te32));
				}

				CloseHandle(snapshotHandle);
			}
		}
	}
	else if (action == ControlRemoteProcessAction::Terminate)
	{
		TerminateProcess(handle, 0);
	}
}
