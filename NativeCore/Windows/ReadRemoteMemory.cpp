#include <windows.h>

#include "NativeCore.hpp"

bool RC_CallConv ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	buffer = reinterpret_cast<RC_Pointer>(reinterpret_cast<uintptr_t>(buffer) + offset);

	SIZE_T numberOfBytesRead;
	if (ReadProcessMemory(handle, address, buffer, size, &numberOfBytesRead) && size == numberOfBytesRead)
	{
		return true;
	}

	return false;
}
