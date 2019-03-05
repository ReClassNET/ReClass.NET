#include "NativeCore.hpp"
#include <mach/mach_init.h>
#include <mach/mach_vm.h>


extern "C" RC_Pointer RC_CallConv OpenRemoteProcess(RC_Pointer id, ProcessAccess desiredAccess)
{
    #ifdef __linux__
        return id;
    #elif __APPLE__
        task_t task;
    
        task_for_pid(current_task(), *(int*)&id, &task);
        return (RC_Pointer)task;
    #endif
}
