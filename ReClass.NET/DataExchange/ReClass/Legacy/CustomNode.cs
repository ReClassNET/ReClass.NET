using System;
using System.Collections.Generic;
using System.Drawing;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public class CustomNode : BaseNode
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

		public IEnumerable<BaseNode> GetEquivalentNodes(int size)
		{
			while (size != 0)
			{
				BaseNode paddingNode;
#if RECLASSNET64
				if (size >= 8)
				{
					paddingNode = new Hex64Node();
				}
				else
#endif
				if (size >= 4)
				{
					paddingNode = new Hex32Node();
				}
				else if (size >= 2)
				{
					paddingNode = new Hex16Node();
				}
				else
				{
					paddingNode = new Hex8Node();
				}

				paddingNode.Comment = Comment;

				size -= paddingNode.MemorySize;

				yield return paddingNode;
			}
		}
	}
}
