using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ReClassNET.Debugger;
using ReClassNET.Memory;

namespace ReClassNET.Native
{
	#region Enums

	public enum RequestFunction
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
	}

	public enum ProcessAccess
	{
		Read,
		Write,
		Full
	};

	public enum ControlRemoteProcessAction
	{
		Suspend,
		Resume,
		Terminate
	}

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

	#endregion

	#region Structs

	/// <summary>Used by <see cref="NativeHelper.EnumerateProcesses(NativeHelper.EnumerateProcessCallback)"/>.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EnumerateProcessData
	{
		public IntPtr Id;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Path;
	};

	/// <summary>Used by <see cref="NativeHelper.EnumerateRemoteSectionsAndModules(IntPtr, NativeHelper.EnumerateRemoteSectionCallback, NativeHelper.EnumerateRemoteModuleCallback)"/>.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EnumerateRemoteSectionData
	{
		public IntPtr BaseAddress;

		public IntPtr Size;

		public SectionType Type;

		public SectionProtection Protection;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Name;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string ModulePath;
	}

	/// <summary>Used by <see cref="NativeHelper.EnumerateRemoteSectionsAndModules(IntPtr, NativeHelper.EnumerateRemoteSectionCallback, NativeHelper.EnumerateRemoteModuleCallback)"/>.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EnumerateRemoteModuleData
	{
		public IntPtr BaseAddress;

		public IntPtr Size;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Path;
	}

	/// <summary>Used by <see cref="NativeHelper.DisassembleCode(IntPtr, int, IntPtr, out InstructionData)"/>.</summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct InstructionData
	{
		public int Length;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] Data;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string Instruction;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct CreateProcessDebugInfo
	{
		public IntPtr FileHandle;

		public IntPtr ProcessHandle;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct ExitProcessDebugInfo
	{
		public IntPtr ExitCode;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct CreateThreadDebugInfo
	{
		public IntPtr ThreadHandle;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct ExitThreadDebugInfo
	{
		public IntPtr ExitCode;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct LoadDllDebugInfo
	{
		public IntPtr FileHandle;

		public IntPtr BaseOfDll;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct UnloadDllDebugInfo
	{
		public IntPtr BaseOfDll;
	};

	[StructLayout(LayoutKind.Sequential)]
	public struct ExceptionDebugInfo
	{
		public IntPtr ExceptionCode;
		public IntPtr ExceptionFlags;
		public IntPtr ExceptionAddress;
		public HardwareBreakpointRegister CausedBy;
		public bool IsFirstChance;

		[StructLayout(LayoutKind.Sequential)]
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

	[StructLayout(LayoutKind.Sequential)]
	public struct DebugEventHeader
	{
		public DebugContinueStatus ContinueStatus;

		public IntPtr ProcessId;
		public IntPtr ThreadId;

		public DebugEventType Type;
	}

	[StructLayout(LayoutKind.Explicit)]
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

	[StructLayout(LayoutKind.Explicit)]
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

	#endregion
}
