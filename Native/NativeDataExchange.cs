using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
		ControlRemoteProcess
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

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string Instruction;
	};

	#endregion
}
