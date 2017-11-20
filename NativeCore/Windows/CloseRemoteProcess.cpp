#include <windows.h>

#include "NativeCore.hpp"

void RC_CallConv CloseRemoteProcess(RC_Pointer handle)
{
	CloseHandle(handle);
}
