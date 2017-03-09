using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Int16Node : BaseNumericNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 2;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Signed, "Int16", view.Memory.ReadObject<short>(Offset).ToString());
		}

		/// <summary>Updates the node from the given spot. Sets the value of the node.</summary>
		/// <param name="spot">The spot.</param>
		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				short val;
				if (short.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
