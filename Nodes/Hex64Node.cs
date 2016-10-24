using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ReClassNET.UI;
using ReClassNET.Util;

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

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 8;

		/// <summary>Gets informations about this node to show in a tool tip.</summary>
		/// <param name="spot">The spot.</param>
		/// <param name="memory">The process memory.</param>
		/// <returns>The information to show in a tool tip.</returns>
		public override string GetToolTipText(HotSpot spot, Memory memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			var value = memory.ReadObject<UInt64FloatDoubleData>(Offset);

			return $"Int64: {value.LongValue}\nUInt64: 0x{value.ULongValue:X016}\nFloat: {value.FloatValue:0.000}\nDouble: {value.DoubleValue:0.000}";
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, Program.Settings.ShowNodeText ? view.Memory.ReadPrintableASCIIString(Offset, 8) + " " : null, 8);
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			Update(spot, 8);
		}

		protected override int AddComment(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			x = base.AddComment(view, x, y);

			var value = view.Memory.ReadObject<UInt64FloatDoubleData>(Offset);

			x = AddComment(view, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}
	}
}
