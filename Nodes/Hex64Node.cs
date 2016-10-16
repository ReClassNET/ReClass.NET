using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Hex64Node : BaseHexCommentNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct UInt64FloatDoubleData
		{
			[FieldOffset(0)]
			public long LongValue;

			public IntPtr IntPtr =>
#if WIN32
			unchecked((IntPtr)(int)LongValue);
#else
			unchecked((IntPtr)LongValue);
#endif

			[FieldOffset(0)]
			public ulong ULongValue;

			public UIntPtr UIntPtr =>
#if WIN32
			unchecked((UIntPtr)(uint)ULongValue);
#else
			unchecked((UIntPtr)ULongValue);
#endif

			[FieldOffset(0)]
			public float FloatValue;

			[FieldOffset(0)]
			public double DoubleValue;
		}

		public override int MemorySize => 8;

		public Hex64Node()
		{
			buffer = new byte[8];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var value = memory.ReadObject<UInt64FloatDoubleData>(Offset);

			return $"Int64: {value.LongValue}\nUInt64: 0x{value.ULongValue:X016}\nFloat: {value.FloatValue:0.000}\nDouble: {value.DoubleValue:0.000}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 8) + " " : null, 8);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 8);
		}

		protected override int AddComment(ViewInfo view, int x, int y)
		{
			x = base.AddComment(view, x, y);

			var value = view.Memory.ReadObject<UInt64FloatDoubleData>(Offset);

			x = AddComment(view, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}
	}
}
