namespace ReClassNET.Nodes
{
	class UInt8Node : NumericNode
	{
		public override int MemorySize => 1;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "BYTE  ", view.Memory.ReadByte(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				byte val;
				if (byte.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
