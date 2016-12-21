#include <windows.h>

#include "NativeCore.hpp"

void __stdcall CloseRemoteProcess(RC_Pointer handle)
{
	CloseHandle(handle);
}
