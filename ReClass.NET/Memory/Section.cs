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
		public IntPtr Start { get; set; }
		public IntPtr End { get; set; }
		public IntPtr Size { get; set; }
		public string Name { get; set; }
		public SectionCategory Category { get; set; }
		public SectionProtection Protection { get; set; }
		public SectionType Type { get; set; }
		public string ModuleName { get; set; }
		public string ModulePath { get; set; }
	}
}
