using System;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	public enum HotSpotType
	{
		None,
		Edit,
		OpenClose,
		Select,
		Click,
		DoubleClick,
		ChangeClassType,
		ChangeWrappedType,
		ChangeEnumType,
		Context,
		Delete,
		Address,
		Name,
		Comment
	}

	public class HotSpot
	{
		public const int NoneId = -1;
		public const int AddressId = 100;
		public const int NameId = 101;
		public const int CommentId = 102;
		public const int ReadOnlyId = 999;

		public int Id { get; set; }
		public HotSpotType Type { get; set; }
		public int Level { get; set; }

		public string Text { get; set; }
		public BaseNode Node { get; set; }

		public Rectangle Rect { get; set; }

		public IntPtr Address { get; set; }

		public RemoteProcess Process { get; set; }

		public MemoryBuffer Memory { get; set; }
	}
}
