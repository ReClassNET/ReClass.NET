//#include <sys/types.h>
#include <csignal>

#include "NativeCore.hpp"

extern "C" void RC_CallConv ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action)
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

	kill(static_cast<pid_t>(reinterpret_cast<intptr_t>(handle)), signal);
}
