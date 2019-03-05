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
    #ifdef __linux__
        kill(static_cast<pid_t>(reinterpret_cast<intptr_t>(handle)), signal);
    #elif __APPLE__
        task_t task;
    
        task_for_pid(current_task(), (int)id, &task);
        return (RC_Pointer)task;
    #endif
	
}
