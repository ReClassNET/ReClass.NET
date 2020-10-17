using System;
using System.Drawing;
using ReClassNET.Controls;

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

		public override Size Draw(DrawContext context, int x, int y)
		{
			return Draw(context, x, y, "Array");
		}

		protected override Size DrawChild(DrawContext context, int x, int y)
		{
			var innerContext = context.Clone();
			innerContext.Address = context.Address + Offset + InnerNode.MemorySize * CurrentIndex;
			innerContext.Memory = context.Memory.Clone();
			innerContext.Memory.Offset += Offset + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(innerContext, x, y);
		}
	}
}
