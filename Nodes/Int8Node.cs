namespace ReClassNET.Nodes
{
	class Int8Node : NumericNode
	{
		public override int MemorySize => 1;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Signed, "Int8  ", view.Memory.ReadObject<sbyte>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				sbyte val;
				if (sbyte.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
