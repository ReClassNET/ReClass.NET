using System.Drawing;
using ReClassNET.Controls;

namespace ReClassNET.Nodes
{
	public class FunctionPtrNode : BaseFunctionPtrNode
	{
		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Function Pointer";
			icon = Properties.Resources.B16x16_Button_Function_Pointer;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			return Draw(context, x, y, "FunctionPtr", Name);
		}
	}
}
