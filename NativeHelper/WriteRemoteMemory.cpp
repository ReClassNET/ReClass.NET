#include <windows.h>

#include "NativeHelper.hpp"

bool __stdcall WriteRemoteMemory(RC_Pointer process, RC_Pointer address, RC_Pointer buffer, RC_Size size)
{
	DWORD oldProtect;
	if (VirtualProtectEx(process, address, size, PAGE_EXECUTE_READWRITE, &oldProtect))
	{
		SIZE_T numberOfBytesWritten;
		if (WriteProcessMemory(process, address, buffer, size, &numberOfBytesWritten))
		{
			VirtualProtectEx(process, address, size, oldProtect, nullptr);

			if (size == numberOfBytesWritten)
			{
				return true;
			}
		}
	}

	return false;
}
