using System;
using System.Drawing;
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
			ChangeInnerNode(IntPtr.Size == 4 ? (BaseNode)new Hex32Node() : new Hex64Node());
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "Array");
		}

		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			var v = view.Clone();
			v.Address = view.Address + Offset + InnerNode.MemorySize * CurrentIndex;
			v.Memory = view.Memory.Clone();
			v.Memory.Offset += Offset + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(v, x, y);
		}
	}
}
