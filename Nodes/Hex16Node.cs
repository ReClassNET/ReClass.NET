using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Hex16Node : BaseHexNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct UInt16Data
		{
			/*[FieldOffset(0)]
			public fixed byte ByteValue[2];*/

			[FieldOffset(0)]
			public short ShortValue;

			[FieldOffset(0)]
			public ushort UShortValue;
		}

		public override int MemorySize => 2;

		public Hex16Node()
		{
			buffer = new byte[2];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var value = memory.ReadObject<UInt16Data>(Offset);

			return $"Int16: {value.ShortValue}\nWORD: 0x{value.UShortValue:X04}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 2) + "       " : null, 2);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 2);
		}
	}
}
