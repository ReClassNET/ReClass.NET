#include <windows.h>

#include "NativeHelper.hpp"

void __stdcall CloseRemoteProcess(RC_Pointer handle)
{
	CloseHandle(handle);
}
