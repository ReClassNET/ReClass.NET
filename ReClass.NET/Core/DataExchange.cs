using System;
using System.Runtime.InteropServices;
using ReClassNET.Memory;

namespace ReClassNET.Core
{
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

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct EnumerateProcessData
	{
		public IntPtr Id;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Name;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Path;
	};

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct EnumerateRemoteSectionData
	{
		public IntPtr BaseAddress;

		public IntPtr Size;

		public SectionType Type;

		public SectionCategory Category;

		public SectionProtection Protection;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string Name;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string ModulePath;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct EnumerateRemoteModuleData
	{
		public IntPtr BaseAddress;

		public IntPtr Size;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string Path;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
	public struct InstructionData
	{
		public IntPtr Address;

		public int Length;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] Data;

		public int StaticInstructionBytes;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string Instruction;
	};
}
