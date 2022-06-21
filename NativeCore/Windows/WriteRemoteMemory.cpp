#include <windows.h>

#include "NativeCore.hpp"
#include "ServerRemoteTool.h"

int WriteMemoryWindows(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size)
{
	SIZE_T numberOfBytesWrited = -1;

	DWORD oldProtect;
	if (VirtualProtectEx(handle, address, size, PAGE_EXECUTE_READWRITE, &oldProtect))
	{
		if (WriteProcessMemory(handle, address, buffer, size, &numberOfBytesWrited))
		{
			VirtualProtectEx(handle, address, size, oldProtect, 0);
		}
	}

	return numberOfBytesWrited;
}


bool RC_CallConv WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	int numberOfBytesWrited;
	buffer = reinterpret_cast<RC_Pointer>(reinterpret_cast<uintptr_t>(buffer) + offset);

	if (ServerManager::getInstance()->PartiallyConnected()) numberOfBytesWrited = WriteMemoryServer(handle, address, buffer, size);
	else numberOfBytesWrited = WriteMemoryWindows(handle, address, buffer, size);

	return size == numberOfBytesWrited;
}
