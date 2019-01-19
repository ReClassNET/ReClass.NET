using System;
using System.Drawing;
using ReClassNET.Extensions;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ArrayNode : BaseWrapperArrayNode
	{
		public ArrayNode()
		{
			IsReadOnly = false;
		}

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Array";
			icon = Properties.Resources.B16x16_Button_Array;
		}

		public override void Initialize()
		{
			InnerNode = IntPtr.Size == 4 ? (BaseNode)new Hex32Node() : new Hex64Node();
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "Array");
		}

		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			var v = view.Clone();
			v.Address = view.Address.Add(Offset) + InnerNode.MemorySize * CurrentIndex;
			v.Memory = view.Memory.Clone();
			v.Memory.Offset += Offset.ToInt32() + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(v, x, y);
		}
	}
}
