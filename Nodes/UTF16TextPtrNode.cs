using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace ReClassNET.Nodes
{
	class UTF16TextPtrNode : BaseTextPtrNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			var ptr = view.Memory.ReadObject<IntPtr>(Offset);
			var str = view.Memory.Process.ReadRemoteString(Encoding.Unicode, ptr, 128);

			return DrawText(view, x, y, "Text16Ptr", MemorySize, str);
		}
	}
}
