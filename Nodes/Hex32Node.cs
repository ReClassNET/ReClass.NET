using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Hex32Node : BaseHexCommentNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct UInt32FloatData
		{
			[FieldOffset(0)]
			public int IntValue;

			public IntPtr IntPtr => unchecked((IntPtr)IntValue);

			[FieldOffset(0)]
			public uint UIntValue;

			public UIntPtr UIntPtr => unchecked((UIntPtr)UIntValue);

			[FieldOffset(0)]
			public float FloatValue;
		}

		public override int MemorySize => 4;

		public Hex32Node()
		{
			buffer = new byte[4];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var value = memory.ReadObject<UInt32FloatData>(Offset);

			return $"Int32: {value.IntValue}\nUInt32: 0x{value.UIntValue:X08}\nFloat: {value.FloatValue:0.000}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 4) + "     " : null, 4);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 4);
		}

		protected override int AddComment(ViewInfo view, int x, int y)
		{
			x = base.AddComment(view, x, y);

			var value = view.Memory.ReadObject<UInt32FloatData>(Offset);

			x = AddComment(view, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}
	}
}
