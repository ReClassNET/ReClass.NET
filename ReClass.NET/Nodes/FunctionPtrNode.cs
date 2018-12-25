using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FunctionPtrNode : BaseFunctionPtrNode
	{
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "FunctionPtr", Name);
		}
	}
}
