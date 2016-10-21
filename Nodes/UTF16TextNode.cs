using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UTF16TextNode : BaseTextNode
	{
		public override int CharacterSize => 2;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawText(view, x, y, "Text16", MemorySize / CharacterSize, view.Memory.ReadUTF16String(Offset, MemorySize));
		}
	}
}
