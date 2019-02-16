using System.Drawing;
using System.Text;
using ReClassNET.UI;

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

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text16Ptr");
		}
	}
}
