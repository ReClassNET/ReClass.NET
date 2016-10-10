namespace ReClassNET.Nodes
{
	class UTF32TextNode : BaseTextNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text32 ", MemorySize / 4, view.Memory.ReadUTF32String(Offset, MemorySize));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 1)
			{
				var length = spot.Text.Length + 1;
				if (length > MemorySize / 4)
				{
					length = MemorySize / 4;
				}
				//WriteMemory(spot.Address
			}
		}
	}
}
