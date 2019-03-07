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
    task_t task;
    
    task_for_pid(current_task(), (int)handle, &task);
    
    mach_port_t task_port;
    vm_region_basic_info_data_t info;
    mach_msg_type_number_t info_count = VM_REGION_BASIC_INFO_COUNT;
    vm_region_flavor_t flavor = VM_REGION_BASIC_INFO;
    
    vm_address_t region = (vm_address_t)address;
    vm_size_t region_size = 0;

    
    vm_region(task, &region, &region_size, flavor, (vm_region_info_t)&info, (mach_msg_type_number_t*)&info_count, (mach_port_t*)&task_port);
    
    vm_protect(task, region, region_size, false, VM_PROT_READ | VM_PROT_WRITE | VM_PROT_COPY);
    
    return vm_write((vm_map_t)task , (vm_address_t) address, (vm_offset_t)(static_cast<uint8_t*>(buffer) + offset), size) == KERN_SUCCESS;
    #endif
    
}
