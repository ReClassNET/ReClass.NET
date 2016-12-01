using System;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public enum SectionCategory
	{
		Unknown,
		CODE,
		DATA,
		HEAP
	}

	public class Section
	{
		public IntPtr Start;
		public IntPtr End;
		public IntPtr Size;
		public string Name;
		public SectionCategory Category;
		public NativeMethods.StateEnum State;
		public NativeMethods.AllocationProtectEnum Protection;
		public NativeMethods.TypeEnum Type;
		public string ModuleName;
		public string ModulePath;
	}
}
