using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Debugger
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct ExceptionDebugInfo
	{
		public IntPtr ExceptionCode;
		public IntPtr ExceptionFlags;
		public IntPtr ExceptionAddress;

		public HardwareBreakpointRegister CausedBy;

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RegisterInfo
		{
#if RECLASSNET64
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
	public struct DebugEvent
	{
		public DebugContinueStatus ContinueStatus;

		public IntPtr ProcessId;
		public IntPtr ThreadId;

		public ExceptionDebugInfo ExceptionInfo;
	}
}
