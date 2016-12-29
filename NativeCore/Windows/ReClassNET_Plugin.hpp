#pragma once

#include <type_traits>
#include <algorithm>
#include <cstdint>
#include <codecvt>

// Types

using RC_Pointer = void*;
using RC_Size = size_t;
using RC_UnicodeChar = char16_t;

// Constants

const int PATH_MAXIMUM_LENGTH = 260;

// Enumerations

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

enum class SectionCategory
{
	Unknown,
	CODE,
	DATA,
	HEAP
};

enum class ControlRemoteProcessAction
{
	Suspend,
	Resume,
	Terminate
};

enum class DebugContinueStatus
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

// Structures

#pragma pack(push, 1)

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
	SectionCategory Category;
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

struct ExceptionDebugInfo
{
	RC_Size ExceptionCode;
	RC_Size ExceptionFlags;
	RC_Pointer ExceptionAddress;

	HardwareBreakpointRegister CausedBy;

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

	ExceptionDebugInfo ExceptionInfo;
};

#pragma pack(pop)

// Callbacks

typedef void(__stdcall *EnumerateProcessCallback)(EnumerateProcessData* data);

typedef void(__stdcall EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(__stdcall EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

// Helpers

inline void MultiByteToUnicode(const char* src, RC_UnicodeChar* dst, int size)
{
#if _MSC_VER == 1900
	// VS Bug: https://connect.microsoft.com/VisualStudio/feedback/details/1348277/link-error-when-using-std-codecvt-utf8-utf16-char16-t
	
	auto temp = std::wstring_convert<std::codecvt_utf8_utf16<int16_t>, int16_t>{}.from_bytes(src);
#else
	auto temp = std::wstring_convert<std::codecvt_utf8_utf16<char16_t>, char16_t>{}.from_bytes(src);
#endif

	std::memcpy(dst, temp.c_str(), std::min<int>((int)temp.length(), size) * sizeof(char16_t));
}
