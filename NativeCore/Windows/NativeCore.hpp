#pragma once

#include "../ReClassNET_Plugin.hpp"

typedef void(__stdcall *EnumerateProcessCallback)(EnumerateProcessData* data);

typedef void(__stdcall EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(__stdcall EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

void __stdcall EnumerateProcesses(EnumerateProcessCallback callbackProcess);
void __stdcall EnumerateRemoteSectionsAndModules(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule);

RC_Pointer __stdcall OpenRemoteProcess(RC_Pointer id, ProcessAccess desiredAccess);
bool __stdcall IsProcessValid(RC_Pointer handle);
void __stdcall CloseRemoteProcess(RC_Pointer handle);

bool __stdcall ReadRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size);
bool __stdcall WriteRemoteMemory(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, int offset, int size);

void __stdcall ControlRemoteProcess(RC_Pointer handle, ControlRemoteProcessAction action);

bool __stdcall AttachDebuggerToProcess(RC_Pointer id);
void __stdcall DetachDebuggerFromProcess(RC_Pointer id);
bool __stdcall AwaitDebugEvent(DebugEvent* evt, int timeoutInMilliseconds);
void __stdcall HandleDebugEvent(DebugEvent* evt);
bool __stdcall SetHardwareBreakpoint(RC_Pointer id, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set);
