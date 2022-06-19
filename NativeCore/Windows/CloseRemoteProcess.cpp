#include <windows.h>

#include "NativeCore.hpp"

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

	CloseWindowsHandle(handle);
}
