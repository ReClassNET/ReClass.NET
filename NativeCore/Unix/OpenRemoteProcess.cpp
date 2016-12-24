#include "NativeCore.hpp"

extern "C" RC_Pointer OpenRemoteProcess(RC_Pointer id, ProcessAccess desiredAccess)
{
	return id;
}
