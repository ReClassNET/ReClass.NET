using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf32TextNode : BaseTextNode
	{
		public override int CharacterSize => 4;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text32", MemorySize / CharacterSize, view.Memory.ReadUtf32String(Offset, MemorySize));
		}

		public string ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadUtf32String(Offset, MemorySize);
		}
	}
}
