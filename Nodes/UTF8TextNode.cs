using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UTF8TextNode : BaseTextNode
	{
		public override int CharacterSize => 1;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawText(view, x, y, "Text8", MemorySize, view.Memory.ReadUTF8String(Offset, MemorySize));
		}
	}
}
