using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Controls;

namespace ReClassNET.Nodes
{
	public class VirtualMethodNode : BaseFunctionPtrNode
	{
		public string MethodName => string.IsNullOrEmpty(Name) ? $"Function{Offset / IntPtr.Size}" : Name;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			throw new InvalidOperationException($"The '{nameof(VirtualMethodNode)}' node should not be accessible from the ui.");
		}

		public VirtualMethodNode()
		{
			Contract.Ensures(Name != null);

			Name = string.Empty;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			return Draw(context, x, y, $"({Offset / IntPtr.Size})", MethodName);
		}
	}
}
