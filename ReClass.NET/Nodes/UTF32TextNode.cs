using System.Drawing;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf32TextNode : BaseTextNode
	{
		public override int CharacterSize => 4;

		public override Encoding Encoding => Encoding.UTF32;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text32");
		}
	}
}
