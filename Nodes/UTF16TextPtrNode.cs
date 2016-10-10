using System;

namespace ReClassNET.Nodes
{
	class UTF16TextPtrNode : BaseTextPtrNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadObject<IntPtr>(Offset);
			var str = view.Memory.Process.ReadUTF16String(ptr, 128);

			return DrawText(view, x, y, "Text16Ptr ", MemorySize, str);
		}
	}
}
