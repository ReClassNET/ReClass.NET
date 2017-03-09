using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FloatNode : BaseNumericNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 4;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Float, "Float", view.Memory.ReadObject<float>(Offset).ToString("0.000"));
		}

		/// <summary>Updates the node from the given spot. Sets the value of the node.</summary>
		/// <param name="spot">The spot.</param>
		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				float val;
				if (float.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
