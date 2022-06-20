#include <windows.h>

#include "NativeCore.hpp"
#include "ServerRemoteTool.h"

void CloseWindowsHandle(RC_Pointer handle)
{
	CloseHandle(handle);
}

void RC_CallConv CloseRemoteProcess(RC_Pointer handle)
{
	if (handle == nullptr)
	{
		return;
	}

	if (ServerManager::getInstance()->IsConnected()) CloseServerProcess(handle);
	else CloseWindowsHandle(handle);
}
