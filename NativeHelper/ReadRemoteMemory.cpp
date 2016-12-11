#include <windows.h>

#include "NativeHelper.hpp"

bool __stdcall ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, RC_Size size)
{
	SIZE_T numberOfBytesRead;
	if (ReadProcessMemory(handle, address, buffer, size, &numberOfBytesRead) && size == numberOfBytesRead)
	{
		return true;
	}

	return false;
}
