using System;

namespace ReClassNET.Memory
{
	public enum SectionCategory
	{
		Unknown,
		CODE,
		DATA,
		HEAP
	}

	[Flags]
	public enum SectionProtection
	{
		NoAccess = 0,

		Read = 1,
		Write = 2,
		CopyOnWrite = 4,
		Execute = 8,

		Guard = 16
	}

	public enum SectionType
	{
		Unknown,

		Private,
		Mapped,
		Image
	}

	public class Section
	{
		public IntPtr Start;
		public IntPtr End;
		public IntPtr Size;
		public string Name;
		public SectionCategory Category;
		public SectionProtection Protection;
		public SectionType Type;
		public string ModuleName;
		public string ModulePath;
	}
}
