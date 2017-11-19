#pragma once

#include <string>
#include <sstream>

#include "../ReClassNET_Plugin.hpp"
#include "../Shared/Keys.hpp"

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

	RC_Pointer InitializeInput();
	bool GetPressedKeys(RC_Pointer handle, Keys* state[], int* count);
	void ReleaseInput(RC_Pointer handle);
}

inline bool is_number(const std::string& s)
{
	auto it = s.begin();
	for (; it != s.end() && std::isdigit(*it); ++it);
	return !s.empty() && it == s.end();
}

template<typename T>
inline T parse_type(const std::string& s)
{
	std::stringstream ss(s);

	T val;
	ss >> val;
	return val;
}
