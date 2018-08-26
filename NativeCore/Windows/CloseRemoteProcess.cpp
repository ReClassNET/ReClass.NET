#include <windows.h>

#include "NativeCore.hpp"

void RC_CallConv CloseRemoteProcess(RC_Pointer handle)
{
	if (handle == nullptr)
	{
		return;
	}

	CloseHandle(handle);
}
