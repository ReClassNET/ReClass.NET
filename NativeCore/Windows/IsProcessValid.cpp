#include <windows.h>

#include "NativeCore.hpp"

bool RC_CallConv IsProcessValid(RC_Pointer handle)
{
	if (!handle || handle == INVALID_HANDLE_VALUE)
	{
		return false;
	}

	const auto retn = WaitForSingleObject(handle, 0);
	if (retn == WAIT_FAILED)
	{
		return false;
	}

	return retn == WAIT_TIMEOUT;
}
