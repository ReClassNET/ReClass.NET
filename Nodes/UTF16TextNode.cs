namespace ReClassNET.Nodes
{
	class UTF16TextNode : BaseTextNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text16 ", MemorySize / 2, view.Memory.ReadUTF16String(Offset, MemorySize));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 1)
			{
				var length = spot.Text.Length + 1;
				if (length > MemorySize / 2)
				{
					length = MemorySize / 2;
				}
				//WriteMemory(spot.Address
			}
		}
	}
}
