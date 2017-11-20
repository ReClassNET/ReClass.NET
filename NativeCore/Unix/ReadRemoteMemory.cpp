#include <sys/uio.h>

#include "NativeCore.hpp"

extern "C" bool RC_CallConv ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size)
{
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
}
