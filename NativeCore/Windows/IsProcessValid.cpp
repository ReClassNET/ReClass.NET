#include <windows.h>

#include "NativeCore.hpp"

bool IsProcessValidWindows(RC_Pointer handle)
{
	if (handle == nullptr)
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

bool RC_CallConv IsProcessValid(RC_Pointer handle)
{
	return IsProcessValidWindows(handle);
}
