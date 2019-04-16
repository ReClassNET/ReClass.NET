using System;
using System.Diagnostics.Contracts;
using ReClassNET.Extensions;
using ReClassNET.Memory;

namespace ReClassNET.AddressParser
{
	public class Interpreter : IExecutor
	{
		public IntPtr Execute(IExpression expression, IProcessReader processReader)
		{
			Contract.Requires(expression != null);
			Contract.Requires(processReader != null);

			switch (expression)
			{
				case ConstantExpression constantExpression:
					return IntPtrExtension.From(constantExpression.Value);
				case NegateExpression negateExpression:
					return Execute(negateExpression.Expression, processReader).Negate();
				case ModuleExpression moduleExpression:
				{
					var module = processReader.GetModuleByName(moduleExpression.Name);
					if (module != null)
					{
						return module.Start;
					}

					return IntPtr.Zero;
				}
				case AddExpression addExpression:
					return Execute(addExpression.Lhs, processReader).Add(Execute(addExpression.Rhs, processReader));
				case SubtractExpression subtractExpression:
					return Execute(subtractExpression.Lhs, processReader).Sub(Execute(subtractExpression.Rhs, processReader));
				case MultiplyExpression multiplyExpression:
					return Execute(multiplyExpression.Lhs, processReader).Mul(Execute(multiplyExpression.Rhs, processReader));
				case DivideExpression divideExpression:
					return Execute(divideExpression.Lhs, processReader).Div(Execute(divideExpression.Rhs, processReader));
				case ReadMemoryExpression readMemoryExpression:
					var readFromAddress = Execute(readMemoryExpression.Expression, processReader);
					if (readMemoryExpression.ByteCount == 4)
					{
						return IntPtrExtension.From(processReader.ReadRemoteInt32(readFromAddress));
					}
					else
					{
						return IntPtrExtension.From(processReader.ReadRemoteInt64(readFromAddress));
					}
				default:
					throw new ArgumentException($"Unsupported operation '{expression.GetType().FullName}'.");
			}
		}
	}
}
