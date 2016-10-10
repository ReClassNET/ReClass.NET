namespace ReClassNET.Nodes
{
	class DoubleNode : NumericNode
	{
		public override int MemorySize => 8;

		public override int Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Double, "double ", view.Memory.ReadObject<double>(Offset).ToString("0.000"));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				double val;
				if (double.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
