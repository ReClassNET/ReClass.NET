using System.Drawing;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf8TextNode : BaseTextNode
	{
		public override int CharacterSize => 1;

		public override Encoding Encoding => Encoding.UTF8;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text8");
		}
	}
}
