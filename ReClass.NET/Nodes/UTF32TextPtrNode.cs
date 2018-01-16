using System.Drawing;
using System.Text;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf32TextPtrNode : BaseTextPtrNode
	{
		public override Encoding Encoding => Encoding.UTF32;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text32Ptr");
		}
	}
}
