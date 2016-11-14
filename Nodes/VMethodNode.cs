using System;
using System.Diagnostics.Contracts;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class VMethodNode : BaseFunctionPtrNode
	{
		public string MethodName => string.IsNullOrEmpty(Name) ? $"Function{Offset.ToInt32() / IntPtr.Size}" : Name;

		public VMethodNode()
		{
			Contract.Ensures(Name != null);

			Name = string.Empty;
		}

		protected override BaseNode CreateCloneInstance() => new VMethodNode();

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, $"({Offset.ToInt32() / IntPtr.Size})", MethodName);
		}
	}
}
