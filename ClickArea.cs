using ReclassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReclassNET
{
	enum ClickAreaType
	{
		None,
		Edit,
		OpenClose,
		Select,
		Drop,
		Click,
		ChangeA,
		ChangeX,
		Delete,
		RTTI,
		Address,
		Name,
		Comment
	}

	class ClickArea
	{
		public const int NoneId = -1;
		public const int AddressId = 100;
		public const int NameId = 101;
		public const int CommentId = 102;

		public int Id { get; set; }
		public ClickAreaType Type { get; set; }
		public int Level { get; set; }

		public string Text { get; set; }
		public BaseNode Node { get; set; }

		public Rectangle Rect { get; set; }

		public UIntPtr Address { get; set; }
	}
}
