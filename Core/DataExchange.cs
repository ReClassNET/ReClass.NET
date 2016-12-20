using System.Runtime.InteropServices;

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
	public struct InstructionData
	{
		public int Length;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public byte[] Data;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string Instruction;
	};
}
