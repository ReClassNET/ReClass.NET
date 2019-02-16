using System.Drawing;
using System.Text;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf8TextPtrNode : BaseTextPtrNode
	{
		public override Encoding Encoding => Encoding.UTF8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "UTF8 / ASCII Text Pointer";
			icon = Properties.Resources.B16x16_Button_Text_Pointer;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text8Ptr");
		}
	}
}
