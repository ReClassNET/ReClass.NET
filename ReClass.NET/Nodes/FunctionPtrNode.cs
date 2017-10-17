using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FunctionPtrNode : BaseFunctionPtrNode
	{
		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "FunctionPtr", Name);
		}
	}
}
