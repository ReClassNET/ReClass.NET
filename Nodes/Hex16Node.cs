using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Hex16Node : BaseHexNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct UInt16Data
		{
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

		public override string GetToolTipText(HotSpot spot, Memory memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			var value = memory.ReadObject<UInt16Data>(Offset);

			return $"Int16: {value.ShortValue}\nUInt16: 0x{value.UShortValue:X04}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, Program.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 2) + "       " : null, 2);
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			Update(spot, 2);
		}
	}
}
