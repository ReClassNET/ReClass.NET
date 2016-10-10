namespace ReClassNET.Nodes
{
	class UTF8TextNode : BaseTextNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawText(view, x, y, "Text8 ", MemorySize, view.Memory.ReadUTF8String(Offset, MemorySize));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 1)
			{
				var length = spot.Text.Length + 1;
				if (length > MemorySize)
				{
					length = MemorySize;
				}
				//WriteMemory(spot.Address
			}
		}
	}
}
