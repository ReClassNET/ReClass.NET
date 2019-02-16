using System.Drawing;
using System.Text;
using ReClassNET.UI;

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

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text32Ptr");
		}
	}
}
