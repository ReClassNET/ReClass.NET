using System;

namespace ReClassNET.Nodes
{
	class UTF32TextPtrNode : BaseTextPtrNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadObject<IntPtr>(Offset);
			var str = view.Memory.Process.ReadUTF32String(ptr, 256);

			return DrawText(view, x, y, "Text32Ptr ", MemorySize, str);
		}
	}
}
