using System;
using ReClassNET.Memory;

namespace ReClassNET.AddressParser
{
	public interface IExecutor
	{
		IntPtr Execute(IExpression expression, IProcessReader processReader);
	}
}
