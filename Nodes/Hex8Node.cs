using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Hex8Node : BaseHexNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 1;

		/// <summary>Gets informations about this node to show in a tool tip.</summary>
		/// <param name="spot">The spot.</param>
		/// <param name="memory">The process memory.</param>
		/// <returns>The information to show in a tool tip.</returns>
		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			var b = memory.ReadByte(Offset);

			return $"Int8: {(int)b}\nUInt8: 0x{b:X02}";
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowNodeText ? view.Memory.ReadPrintableASCIIString(Offset, 1) + "        " : null, 1);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 1);
		}
	}
}
