using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class VMethodNode : BaseFunctionPtrNode
	{
		public string MethodName => string.IsNullOrEmpty(Name) ? $"Function{Offset.ToInt32() / IntPtr.Size}" : Name;

		public VMethodNode()
		{
			Name = string.Empty;
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, $"({Offset.ToInt32() / IntPtr.Size})", MethodName);
		}
	}
}
