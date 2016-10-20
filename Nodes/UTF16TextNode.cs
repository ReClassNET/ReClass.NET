using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UTF16TextNode : BaseTextNode
	{
		public override int CharacterSize => 2;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawText(view, x, y, "Text16", MemorySize / CharacterSize, view.Memory.ReadUTF16String(Offset, MemorySize));
		}
	}
}
