#include <sys/uio.h>
#ifdef __APPLE__
#include <mach/mach_init.h>
#include <mach/mach_vm.h>
#include <mach/mach.h>
#endif

#include "NativeCore.hpp"

extern "C" bool RC_CallConv WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
    
    #ifdef __linux__
        iovec local[1];
        iovec remote[1];
    
        local[0].iov_base = (static_cast<uint8_t*>(buffer) + offset);
        local[0].iov_len = size;
        remote[0].iov_base = address;
        remote[0].iov_len = size;
    
        if (process_vm_writev(static_cast<pid_t>(reinterpret_cast<intptr_t>(handle)), local, 1, remote, 1, 0) != size)
        {
            return false;
        }
    
        return true;
    #elif __APPLE__
    return vm_write(*(vm_map_t*)&handle , (vm_address_t) address, (vm_offset_t)(static_cast<uint8_t*>(buffer) + offset), size) == KERN_SUCCESS;
    #endif
    
}
