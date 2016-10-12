namespace ReClassNET.Nodes
{
	class Int16Node : NumericNode
	{
		public override int MemorySize => 2;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Signed, "Int16", view.Memory.ReadObject<short>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				short val;
				if (short.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
