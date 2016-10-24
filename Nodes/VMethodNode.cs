using System;
using System.Diagnostics.Contracts;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	class VMethodNode : BaseFunctionPtrNode
	{
		public string MethodName => string.IsNullOrEmpty(Name) ? $"Function{Offset.ToInt32() / IntPtr.Size}" : Name;

		public VMethodNode()
		{
			Name = string.Empty;
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, $"({Offset.ToInt32() / IntPtr.Size})", MethodName);
		}
	}
}
