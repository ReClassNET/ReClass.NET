#include <windows.h>

#include "NativeHelper.hpp"

bool __stdcall IsProcessValid(RC_Pointer handle)
{
	if (!handle || handle == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	auto retn = WaitForSingleObject(handle, 0);
	if (retn == WAIT_FAILED)
	{
		return false;
	}

	return retn == WAIT_TIMEOUT;
}
