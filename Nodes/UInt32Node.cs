namespace ReClassNET.Nodes
{
	class UInt32Node : NumericNode
	{
		public override int MemorySize => 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "DWORD ", view.Memory.ReadObject<uint>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				uint val;
				if (uint.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
