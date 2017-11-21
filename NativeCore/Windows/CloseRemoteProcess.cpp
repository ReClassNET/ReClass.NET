#include <windows.h>

#include "NativeCore.hpp"

extern "C" void RC_CallConv CloseRemoteProcess(RC_Pointer handle)
{
	CloseHandle(handle);
}
