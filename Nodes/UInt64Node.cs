using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UInt64Node : BaseNumericNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 8;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt64", view.Memory.ReadObject<ulong>(Offset).ToString());
		}

		/// <summary>Updates the node from the given spot. Sets the value of the node.</summary>
		/// <param name="spot">The spot.</param>
		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				ulong val;
				if (ulong.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
