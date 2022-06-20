#include <windows.h>

#include "NativeCore.hpp"
#include "ServerRemoteTool.h"

int ReadMemoryWindows(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int size)
{
	SIZE_T numberOfBytesRead;

	if (!ReadProcessMemory(handle, address, buffer, size, &numberOfBytesRead))
		return -1;

	return numberOfBytesRead;
}

bool RC_CallConv ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	int numberOfBytesReaded;
	buffer = reinterpret_cast<RC_Pointer>(reinterpret_cast<uintptr_t>(buffer) + offset);

	if (ServerManager::getInstance()->PartiallyConnected()) numberOfBytesReaded = ReadMemoryServer(handle, address, buffer, size);
	else numberOfBytesReaded = ReadMemoryWindows(handle, address, buffer, size);

	return size == numberOfBytesReaded;
}
