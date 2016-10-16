using System;

namespace ReClassNET.Nodes
{
	class VMethodNode : BaseFunctionPtrNode
	{
		public VMethodNode()
		{
			Name = string.Empty;
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			var index = Offset.ToInt32() / IntPtr.Size;

			return Draw(view, x, y, $"({index})", string.IsNullOrEmpty(Name) ? $"Function{index}" : Name);
		}
	}
}
