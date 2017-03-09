using System;
using System.Drawing;
using System.Text;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UTF32TextPtrNode : BaseTextPtrNode
	{
		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadObject<IntPtr>(Offset);
			var str = view.Memory.Process.ReadRemoteString(Encoding.UTF32, ptr, 256);

			return DrawText(view, x, y, "Text32Ptr", str);
		}
	}
}
