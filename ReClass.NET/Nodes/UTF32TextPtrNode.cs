using System.Drawing;
using System.Text;
using ReClassNET.Controls;

namespace ReClassNET.Nodes
{
	public class Utf32TextPtrNode : BaseTextPtrNode
	{
		public override Encoding Encoding => Encoding.UTF32;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "UTF32 Text Pointer";
			icon = Properties.Resources.B16x16_Button_UText_Pointer;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			return DrawText(context, x, y, "Text32Ptr");
		}
	}
}
