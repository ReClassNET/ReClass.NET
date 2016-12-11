#pragma once

#include <type_traits>

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
	ControlRemoteProcess
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

// Structures

struct EnumerateProcessData
{
	RC_Size Id;
	RC_UnicodeChar ModulePath[PATH_MAXIMUM_LENGTH];
};

struct InstructionData
{
	int Length;
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

// Callbacks

typedef RC_Pointer(__stdcall *RequestFunctionPtrCallback)(RequestFunction request);

typedef void(__stdcall *EnumerateProcessCallback)(EnumerateProcessData* data);

typedef void(__stdcall EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(__stdcall EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

// Delegates

typedef bool(__stdcall *IsProcessValid_Delegate)(RC_Pointer handle);

typedef RC_Pointer(__stdcall *OpenRemoteProcess_Delegate)(RC_Size processId, ProcessAccess desiredAccess);

typedef void(__stdcall *CloseRemoteProcess_Delegate)(RC_Pointer handle);

typedef bool(__stdcall *ReadRemoteMemory_Delegate)(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, RC_Size size);

typedef bool(__stdcall *WriteRemoteMemory_Delegate)(RC_Pointer handle, RC_Pointer address, RC_Pointer buffer, RC_Size size);

typedef void(__stdcall *EnumerateProcesses_Delegate)(EnumerateProcessCallback callbackProcess);

typedef void(__stdcall *EnumerateRemoteSectionsAndModules_Delegate)(RC_Pointer handle, EnumerateRemoteSectionsCallback callbackSection, EnumerateRemoteModulesCallback callbackModule);

typedef bool(__stdcall *DisassembleCode_Delegate)(RC_Pointer address, RC_Size length, RC_Pointer virtualAddress, InstructionData* instruction);

typedef void(__stdcall *ControlRemoteProcess_Delegate)(RC_Pointer handle, ControlRemoteProcessAction action);
