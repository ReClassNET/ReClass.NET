//#include <sys/types.h>
#include <csignal>

#include "NativeCore.hpp"

extern "C" void ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action)
{
	int signal = SIGKILL;
	if (action == ControlRemoteProcessAction::Suspend)
	{
		signal = SIGSTOP;
	}
	else if (action == ControlRemoteProcessAction::Resume)
	{
		signal = SIGCONT;
	}

	kill((pid_t)(intptr_t)handle, signal);
}
