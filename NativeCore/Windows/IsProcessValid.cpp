#include <windows.h>

#include "NativeCore.hpp"
#include "ServerRemoteTool.h"

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
	if(!ServerManager::getInstance()->IsConnected()) return IsProcessValidWindows(handle);
	return true; // TODO
}
