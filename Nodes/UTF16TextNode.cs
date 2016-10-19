namespace ReClassNET.Nodes
{
	class UTF16TextNode : BaseTextNode
	{
		public override int CharacterSize => 2;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text16", MemorySize / CharacterSize, view.Memory.ReadUTF16String(Offset, MemorySize));
		}
	}
}
