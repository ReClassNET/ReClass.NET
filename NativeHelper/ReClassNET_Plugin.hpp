#pragma once

#include <type_traits>
#include <cstdint>

// Types

using RC_Pointer = void*;
using RC_Size = size_t;
using RC_UnicodeChar = wchar_t;

// Constants

const int PATH_MAXIMUM_LENGTH = 260;

// Enumerations

enum class RequestFunction
{
	IsProcessValid,
	OpenRemoteProcess,
	CloseRemoteProcess,
	ReadRemoteMemory,
	WriteRemoteMemory,
	EnumerateProcesses,
	EnumerateRemoteSectionsAndModules,
	DisassembleCode,
	ControlRemoteProcess,
	DebuggerAttachToProcess,
	DebuggerDetachFromProcess,
	DebuggerWaitForDebugEvent,
	DebuggerContinueEvent,
	DebuggerSetHardwareBreakpoint
};

enum class ProcessAccess
{
	Read,
	Write,
	Full
};

enum class SectionProtection
{
	NoAccess = 0,

	Read = 1,
	Write = 2,
	Execute = 4,

	Guard = 8
};

inline SectionProtection operator|(SectionProtection lhs, SectionProtection rhs)
{
	using T = std::underlying_type_t<SectionProtection>;

	return static_cast<SectionProtection>(static_cast<T>(lhs) | static_cast<T>(rhs));
}

inline SectionProtection& operator|=(SectionProtection& lhs, SectionProtection rhs)
{
	using T = std::underlying_type_t<SectionProtection>;

	lhs = static_cast<SectionProtection>(static_cast<T>(lhs) | static_cast<T>(rhs));
	
	return lhs;
}

enum class SectionType
{
	Unknown,

	Private,
	Mapped,
	Image
};

enum class ControlRemoteProcessAction
{
	Suspend,
	Resume,
	Terminate
};

enum DebugContinueStatus
{
	Handled,
	NotHandled
};

enum class HardwareBreakpointRegister
{
	InvalidRegister,

	Dr0,
	Dr1,
	Dr2,
	Dr3
};

enum class HardwareBreakpointTrigger
{
	Execute,
	Access,
	Write,
};

enum class HardwareBreakpointSize
{
	Size1 = 1,
	Size2 = 2,
	Size4 = 4,
	Size8 = 8
};

enum class DebugEventType
{
	CreateProcess,
	ExitProcess,
	CreateThread,
	ExitThread,
	LoadDll,
	UnloadDll,
	Exception
};

// Structures

struct EnumerateProcessData
{
	RC_Size Id;
	RC_UnicodeChar ModulePath[PATH_MAXIMUM_LENGTH];
};

struct InstructionData
{
	int Length;
	uint8_t Data[15];
	RC_UnicodeChar Instruction[64];
};

struct EnumerateRemoteSectionData
{
	RC_Pointer BaseAddress;
	RC_Size Size;
	SectionType Type;
	SectionProtection Protection;
	RC_UnicodeChar Name[16];
	RC_UnicodeChar ModulePath[PATH_MAXIMUM_LENGTH];
};

struct EnumerateRemoteModuleData
{
	RC_Pointer BaseAddress;
	RC_Size Size;
	RC_UnicodeChar Path[PATH_MAXIMUM_LENGTH];
};

struct CreateProcessDebugInfo
{
	RC_Pointer FileHandle;
	RC_Pointer ProcessHandle;
};

struct ExitProcessDebugInfo
{
	RC_Size ExitCode;
};

struct CreateThreadDebugInfo
{
	RC_Pointer ThreadHandle;
};

struct ExitThreadDebugInfo
{
	RC_Size ExitCode;
};

struct LoadDllDebugInfo
{
	RC_Pointer FileHandle;
	RC_Pointer BaseOfDll;
};

struct UnloadDllDebugInfo
{
	RC_Pointer BaseOfDll;
};

struct ExceptionDebugInfo
{
	RC_Size ExceptionCode;
	RC_Size ExceptionFlags;
	RC_Pointer ExceptionAddress;

	HardwareBreakpointRegister CausedBy;

	bool IsFirstChance;

	struct RegisterInfo
	{
#ifdef _WIN64
		RC_Pointer Rax;
		RC_Pointer Rbx;
		RC_Pointer Rcx;
		RC_Pointer Rdx;
		RC_Pointer Rdi;
		RC_Pointer Rsi;
		RC_Pointer Rsp;
		RC_Pointer Rbp;
		RC_Pointer Rip;

		RC_Pointer R8;
		RC_Pointer R9;
		RC_Pointer R10;
		RC_Pointer R11;
		RC_Pointer R12;
		RC_Pointer R13;
		RC_Pointer R14;
		RC_Pointer R15;
#else
		RC_Pointer Eax;
		RC_Pointer Ebx;
		RC_Pointer Ecx;
		RC_Pointer Edx;
		RC_Pointer Edi;
		RC_Pointer Esi;
		RC_Pointer Esp;
		RC_Pointer Ebp;
		RC_Pointer Eip;
#endif
	};
	RegisterInfo Registers;
};

struct DebugEvent
{
	DebugContinueStatus ContinueStatus;

	RC_Pointer ProcessId;
	RC_Pointer ThreadId;

	DebugEventType Type;

	union
	{
		CreateProcessDebugInfo CreateProcessInfo;
		ExitProcessDebugInfo ExitProcessInfo;
		CreateThreadDebugInfo CreateThreadInfo;
		ExitThreadDebugInfo ExitThreadInfo;
		LoadDllDebugInfo LoadDllInfo;
		UnloadDllDebugInfo UnloadDllInfo;
		ExceptionDebugInfo ExceptionInfo;
	};
};

// Callbacks

typedef RC_Pointer(__stdcall *RequestFunctionPtrCallback)(RequestFunction request);

typedef void(__stdcall *EnumerateProcessCallback)(EnumerateProcessData* data);

typedef void(__stdcall EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(__stdcall EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

// Delegates

typedef bool(__stdcall *IsProcessValid_Delegate)(RC_Pointer handle);

typedef RC_Pointer(__stdcall *OpenRemoteProcess_Delegate)(RC_Size id, ProcessAccess desiredAccess);

typedef void(__stdcall *CloseRemoteProcess_Delegate)(RC_Pointer handle);

typedef bool(__stdcall *ReadRemoteMemory_Delegate)(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, RC_Size size);

typedef bool(__stdcall *WriteRemoteMemory_Delegate)(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, RC_Size size);

typedef void(__stdcall *EnumerateProcesses_Delegate)(EnumerateProcessCallback callbackProcess);

typedef void(__stdcall *EnumerateRemoteSectionsAndModules_Delegate)(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule);

typedef bool(__stdcall *DisassembleCode_Delegate)(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, InstructionData* instruction);

typedef void(__stdcall *ControlRemoteProcess_Delegate)(RC_Pointer handle, ControlRemoteProcessAction action);

typedef bool(__stdcall *DebuggerAttachToProcess_Delegate)(RC_Pointer id);

typedef void(__stdcall *DebuggerDetachFromProcess_Delegate)(RC_Pointer id);

typedef bool(__stdcall *DebuggerWaitForDebugEvent_Delegate)(DebugEvent* evt, int timeoutInMilliseconds);

typedef void(__stdcall *DebuggerContinueEvent_Delegate)(DebugEvent* evt);

typedef bool(__stdcall *DebuggerSetHardwareBreakpoint_Delegate)(RC_Pointer processId, RC_Pointer address, HardwareBreakpointRegister reg, HardwareBreakpointTrigger type, HardwareBreakpointSize size, bool set);
