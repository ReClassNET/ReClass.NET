using System;
using ReClassNET.Memory;

namespace ReClassNET.AddressParser
{
	public interface IExecuter
	{
		IntPtr Execute(IExpression expression, IProcessReader processReader);
	}
}
