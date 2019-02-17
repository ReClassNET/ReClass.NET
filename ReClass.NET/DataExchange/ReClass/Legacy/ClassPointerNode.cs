using System;
using System.Drawing;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public class ClassPointerNode : BaseWrapperNode
	{
		public override int MemorySize => throw new NotImplementedException();

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			throw new NotImplementedException();
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			throw new NotImplementedException();
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			throw new NotImplementedException();
		}

		protected override bool PerformCycleCheck => false;

		public override bool CanChangeInnerNodeTo(BaseNode node)
		{
			return node is ClassNode;
		}

		public BaseNode GetEquivalentNode(ClassNode classNode)
		{
			var classInstanceNode = new ClassInstanceNode();
			classInstanceNode.ChangeInnerNode(classNode);

			var pointerNode = new PointerNode();
			pointerNode.ChangeInnerNode(classInstanceNode);
			pointerNode.CopyFromNode(this);

			return pointerNode;
		}
	}
}
