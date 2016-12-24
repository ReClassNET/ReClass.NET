#include "NativeCore.hpp"

extern "C" bool AttachDebuggerToProcess(RC_Pointer id)
{
	return false;
}

extern "C" void DetachDebuggerFromProcess(RC_Pointer id)
{
	
}

extern "C" bool AwaitDebugEvent(DebugEvent* evt, int timeoutInMilliseconds)
{
	return false;
}

extern "C" void HandleDebugEvent(DebugEvent* evt)
{
	
}

extern "C" bool SetHardwareBreakpoint(RC_Pointer id, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set)
{
	return false;
}
