using System.Drawing;
using System.Text;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Utf32TextPtrNode : BaseTextPtrNode
	{
		public override Size Draw(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadIntPtr(Offset);
			var str = view.Memory.Process.ReadRemoteString(Encoding.UTF32, ptr, 256);

			return DrawText(view, x, y, "Text32Ptr", str);
		}
	}
}
