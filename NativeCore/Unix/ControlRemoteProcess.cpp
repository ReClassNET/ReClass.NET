//#include <sys/types.h>
#include <csignal>

#if __APPLE__
#include <sys/proc_info.h>
#include <libproc.h>
#include <mach/mach_init.h>
#include <mach/mach_vm.h>
#endif
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
