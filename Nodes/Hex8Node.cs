namespace ReClassNET.Nodes
{
	class Hex8Node : BaseHexNode
	{
		public override int MemorySize => 1;

		public Hex8Node()
		{
			buffer = new byte[1];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var b = memory.ReadByte(Offset);

			return $"Int8: {(int)b}\nByte: 0x{b:X02}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 1) + "        " : null, 1);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 1);
		}
	}
}
