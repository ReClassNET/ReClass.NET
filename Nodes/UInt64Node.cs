namespace ReClassNET.Nodes
{
	class UInt64Node : NumericNode
	{
		public override int MemorySize => 8;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "QWORD ", view.Memory.ReadObject<ulong>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				ulong val;
				if (ulong.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
