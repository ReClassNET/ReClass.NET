using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class FunctionPtrNode : BaseFunctionPtrNode
	{
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, "Function", Name);
		}
	}
}
