using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Hex8Node : BaseHexNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct UInt8Data
		{
			[FieldOffset(0)]
			public sbyte SByteValue;

			[FieldOffset(0)]
			public byte ByteValue;
		}

		public override int MemorySize => 1;

		public Hex8Node()
		{
			buffer = new byte[1];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			var b = memory.ReadByte(Offset);

			return $"Int8: {(int)b}\nUInt8: 0x{b:X02}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, Program.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 1) + "        " : null, 1);
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			Update(spot, 1);
		}
	}
}
