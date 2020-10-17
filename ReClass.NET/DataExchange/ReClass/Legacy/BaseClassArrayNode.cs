using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public abstract class BaseClassArrayNode : BaseWrapperNode
	{
		public override int MemorySize => throw new NotImplementedException();

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			throw new NotImplementedException();
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			throw new NotImplementedException();
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			throw new NotImplementedException();
		}

		public override bool CanChangeInnerNodeTo(BaseNode node)
		{
			return node is ClassNode;
		}

		public abstract BaseNode GetEquivalentNode(int count, ClassNode classNode);
	}
}
