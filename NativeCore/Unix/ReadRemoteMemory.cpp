#include <sys/uio.h>
#ifdef __APPLE__
#include <mach/mach_init.h>
#include <mach/mach_vm.h>
#include <mach/mach.h>
#endif
#include "NativeCore.hpp"

extern "C" bool RC_CallConv ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
    #ifdef __linux__
        iovec local[1];
        iovec remote[1];
    
        local[0].iov_base = (static_cast<uint8_t*>(buffer) + offset);
        local[0].iov_len = size;
        remote[0].iov_base = address;
        remote[0].iov_len = size;
    
        if (process_vm_readv(static_cast<pid_t>(reinterpret_cast<intptr_t>(handle)), local, 1, remote, 1, 0) != size)
        {
            return false;
        }
    
        return true;
    #elif __APPLE__
        uint32_t sz;
    
        task_t task;
    
        task_for_pid(current_task(), (int)handle, &task);

        return vm_read_overwrite((vm_map_t)task, (vm_address_t) address, (vm_size_t) size, (vm_offset_t)(static_cast<uint8_t*>(buffer) + offset), &sz) == KERN_SUCCESS;
    #endif
	
}
