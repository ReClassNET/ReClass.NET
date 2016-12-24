#pragma once

#include "ReClassNET_Plugin.hpp"

extern "C"
{
	void EnumerateProcesses(EnumerateProcessCallback callbackProcess);
	void EnumerateRemoteSectionsAndModules(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule);

	RC_Pointer OpenRemoteProcess(RC_Pointer id, ProcessAccess desiredAccess);
	bool IsProcessValid(RC_Pointer handle);
	void CloseRemoteProcess(RC_Pointer handle);

	bool ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size);
	bool WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size);

	void ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action);

	bool AttachDebuggerToProcess(RC_Pointer id);
	void DetachDebuggerFromProcess(RC_Pointer id);
	bool AwaitDebugEvent(DebugEvent* evt, int timeoutInMilliseconds);
	void HandleDebugEvent(DebugEvent* evt);
	bool SetHardwareBreakpoint(RC_Pointer id, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set);
}