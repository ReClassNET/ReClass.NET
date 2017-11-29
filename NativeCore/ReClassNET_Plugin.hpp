#pragma once

#include <type_traits>
#include <algorithm>
#include <cstdint>
#include <codecvt>
#include <locale>
#include <cstring>

// OS Specific

#ifdef __linux__
	#define RC_CallConv
#elif _WIN32
	#define RC_CallConv __stdcall
#else
	static_assert(false, "Missing RC_CallConv specification");
#endif

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
	CopyOnWrite = 4,
	Execute = 8,

	Guard = 16
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
	RC_UnicodeChar Name[PATH_MAXIMUM_LENGTH];
	RC_UnicodeChar Path[PATH_MAXIMUM_LENGTH];
};

struct InstructionData
{
	RC_Pointer Address;
	int Length;
	uint8_t Data[15];
	int StaticInstructionBytes;
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
#ifdef RECLASSNET64
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

struct DebugRegister6
{
	union
	{
		uintptr_t Value;
		struct
		{
			unsigned DR0 : 1;
			unsigned DR1 : 1;
			unsigned DR2 : 1;
			unsigned DR3 : 1;
			unsigned Reserved : 9;
			unsigned BD : 1;
			unsigned BS : 1;
			unsigned BT : 1;
		};
	};
};

struct DebugRegister7
{
	union
	{
		uintptr_t Value;
		struct
		{
			unsigned G0 : 1;
			unsigned L0 : 1;
			unsigned G1 : 1;
			unsigned L1 : 1;
			unsigned G2 : 1;
			unsigned L2 : 1;
			unsigned G3 : 1;
			unsigned L3 : 1;
			unsigned GE : 1;
			unsigned LE : 1;
			unsigned Reserved : 6;
			unsigned RW0 : 2;
			unsigned Len0 : 2;
			unsigned RW1 : 2;
			unsigned Len1 : 2;
			unsigned RW2 : 2;
			unsigned Len2 : 2;
			unsigned RW3 : 2;
			unsigned Len3 : 2;
		};
	};
};

#pragma pack(pop)

typedef void(RC_CallConv EnumerateProcessCallback)(EnumerateProcessData* data);

typedef void(RC_CallConv EnumerateRemoteSectionsCallback)(EnumerateRemoteSectionData* data);
typedef void(RC_CallConv EnumerateRemoteModulesCallback)(EnumerateRemoteModuleData* data);

// Helpers

inline void MultiByteToUnicode(const char* src, const int srcOffset, RC_UnicodeChar* dst, const int dstOffset, const int size)
{
#if _MSC_VER >= 1900
	// VS Bug: https://connect.microsoft.com/VisualStudio/feedback/details/1348277/link-error-when-using-std-codecvt-utf8-utf16-char16-t

	using converter = std::wstring_convert<std::codecvt_utf8_utf16<int16_t>, int16_t>;
#else
	using converter = std::wstring_convert<std::codecvt_utf8_utf16<RC_UnicodeChar>, RC_UnicodeChar>;
#endif

	const auto temp = converter{}.from_bytes(src + srcOffset);

	std::memcpy(dst + dstOffset, temp.c_str(), std::min<int>(static_cast<int>(temp.length()), size) * sizeof(RC_UnicodeChar));
}

inline void MultiByteToUnicode(const char* src, RC_UnicodeChar* dst, const int size)
{
	MultiByteToUnicode(src, 0, dst, 0, size);
}

inline char16_t* str16cpy(char16_t* destination, const char16_t* source, size_t n)
{
	char16_t* temp = destination;
	while (n > 0 && *source != 0)
	{
		*temp++ = *source++;
		--n;
	}
	while (n > 0)
	{
		*temp++ = 0;
		--n;
	}
	return destination;
}
