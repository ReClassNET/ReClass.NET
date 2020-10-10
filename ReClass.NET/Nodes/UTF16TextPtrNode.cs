using System.Drawing;
using System.Text;
using ReClassNET.Controls;

namespace ReClassNET.Nodes
{
	public class Utf16TextPtrNode : BaseTextPtrNode
	{
		public override Encoding Encoding => Encoding.Unicode;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "UTF16 / Unicode Text Pointer";
			icon = Properties.Resources.B16x16_Button_UText_Pointer;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			return DrawText(context, x, y, "Text16Ptr");
		}
	}
}
