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

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 2;

		/// <summary>Gets informations about this node to show in a tool tip.</summary>
		/// <param name="spot">The spot.</param>
		/// <param name="memory">The process memory.</param>
		/// <returns>The information to show in a tool tip.</returns>
		public override string GetToolTipText(HotSpot spot, Memory memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			var value = memory.ReadObject<UInt16Data>(Offset);

			return $"Int16: {value.ShortValue}\nUInt16: 0x{value.UShortValue:X04}";
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
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
