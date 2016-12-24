#include <sys/types.h>
#include <signal.h>

#include "NativeCore.hpp"

extern "C" bool IsProcessValid(RC_Pointer handle)
{
	return kill((pid_t)(intptr_t)handle, 0) == 0;
}
