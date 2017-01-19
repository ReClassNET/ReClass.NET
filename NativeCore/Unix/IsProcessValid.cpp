#include <sys/types.h>
#include <signal.h>

#include "NativeCore.hpp"

extern "C" bool IsProcessValid(RC_Pointer handle)
{
	return kill(static_cast<pid_t>(reinterpret_cast<intptr_t>(handle)), 0) == 0;
}
