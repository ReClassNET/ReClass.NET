#include "NativeCore.hpp"
#include <mach/mach_init.h>
#include <mach/mach_vm.h>


extern "C" RC_Pointer RC_CallConv OpenRemoteProcess(RC_Pointer id, ProcessAccess desiredAccess)
{
        return id;
}
