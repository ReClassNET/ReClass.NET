using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Debugger
{
	public enum DebugEventType
	{
		CreateProcess,
		ExitProcess,
		CreateThread,
		ExitThread,
		LoadDll,
		UnloadDll,
		Exception
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CreateProcessDebugInfo
	{
		public IntPtr FileHandle;

		public IntPtr ProcessHandle;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ExitProcessDebugInfo
	{
		public IntPtr ExitCode;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct CreateThreadDebugInfo
	{
		public IntPtr ThreadHandle;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ExitThreadDebugInfo
	{
		public IntPtr ExitCode;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LoadDllDebugInfo
	{
		public IntPtr FileHandle;

		public IntPtr BaseOfDll;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct UnloadDllDebugInfo
	{
		public IntPtr BaseOfDll;
	};

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ExceptionDebugInfo
	{
		public IntPtr ExceptionCode;
		public IntPtr ExceptionFlags;
		public IntPtr ExceptionAddress;
		public HardwareBreakpointRegister CausedBy;
		public bool IsFirstChance;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RegisterInfo
		{
#if WIN64
			public IntPtr Rax;
			public IntPtr Rbx;
			public IntPtr Rcx;
			public IntPtr Rdx;
			public IntPtr Rdi;
			public IntPtr Rsi;
			public IntPtr Rsp;
			public IntPtr Rbp;
			public IntPtr Rip;

			public IntPtr R8;
			public IntPtr R9;
			public IntPtr R10;
			public IntPtr R11;
			public IntPtr R12;
			public IntPtr R13;
			public IntPtr R14;
			public IntPtr R15;
#else
			public IntPtr Eax;
			public IntPtr Ebx;
			public IntPtr Ecx;
			public IntPtr Edx;
			public IntPtr Edi;
			public IntPtr Esi;
			public IntPtr Esp;
			public IntPtr Ebp;
			public IntPtr Eip;
#endif
		};

		public RegisterInfo Registers;
	}

	public enum DebugContinueStatus
	{
		Handled,
		NotHandled
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct DebugEventHeader
	{
		public DebugContinueStatus ContinueStatus;

		public IntPtr ProcessId;
		public IntPtr ThreadId;

		public DebugEventType Type;
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct DebugEventUnion
	{
		[FieldOffset(0)]
		public CreateProcessDebugInfo CreateProcessInfo;
		[FieldOffset(0)]
		public ExitProcessDebugInfo ExitProcessInfo;
		[FieldOffset(0)]
		public CreateThreadDebugInfo CreateThreadInfo;
		[FieldOffset(0)]
		public ExitThreadDebugInfo ExitThreadInfo;
		[FieldOffset(0)]
		public LoadDllDebugInfo LoadDllInfo;
		[FieldOffset(0)]
		public UnloadDllDebugInfo UnloadDllInfo;
		[FieldOffset(0)]
		public ExceptionDebugInfo ExceptionInfo;
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct DebugEvent
	{
		[FieldOffset(0)]
		public DebugEventHeader Header;

#if WIN64
		[FieldOffset(24)]
#else
		[FieldOffset(16)]
#endif
		public DebugEventUnion Data;
	}
}
