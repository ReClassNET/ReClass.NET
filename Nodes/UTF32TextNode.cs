using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UTF32TextNode : BaseTextNode
	{
		public override int CharacterSize => 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawText(view, x, y, "Text32", MemorySize / CharacterSize, view.Memory.ReadUTF32String(Offset, MemorySize));
		}
	}
}
