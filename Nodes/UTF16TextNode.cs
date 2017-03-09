using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UTF16TextNode : BaseTextNode
	{
		public override int CharacterSize => 2;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text16", MemorySize / CharacterSize, view.Memory.ReadUTF16String(Offset, MemorySize));
		}
	}
}
