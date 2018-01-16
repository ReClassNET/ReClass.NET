using System.Drawing;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf16TextNode : BaseTextNode
	{
		public override int CharacterSize => 2;

		public override Encoding Encoding => Encoding.Unicode;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text16");
		}
	}
}
