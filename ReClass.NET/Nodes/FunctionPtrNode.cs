using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FunctionPtrNode : BaseFunctionPtrNode
	{
		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Function Pointer";
			icon = Properties.Resources.B16x16_Button_Function_Pointer;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "FunctionPtr", Name);
		}
	}
}
