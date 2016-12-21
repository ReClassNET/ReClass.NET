#include <windows.h>

#include "NativeCore.hpp"

bool __stdcall WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
	buffer = (RC_Pointer)((uintptr_t)buffer + offset);

	DWORD oldProtect;
	if (VirtualProtectEx(handle, address, size, PAGE_EXECUTE_READWRITE, &oldProtect))
	{
		SIZE_T numberOfBytesWritten;
		if (WriteProcessMemory(handle, address, buffer, size, &numberOfBytesWritten))
		{
			VirtualProtectEx(handle, address, size, oldProtect, nullptr);

			if (size == numberOfBytesWritten)
			{
				return true;
			}
		}
	}

	return false;
}
