// Design taken from https://github.com/pieterderycke/Jace

using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.AddressParser
{
	class Interpreter
	{
		public IntPtr Execute(IOperation operation, RemoteProcess process)
		{
			Contract.Requires(operation != null);
			Contract.Requires(process != null);

			if (operation is OffsetOperation offsetOperation)
			{
				return offsetOperation.Value;
			}

			if (operation is ModuleOffsetOperation moduleOffsetOperation)
			{
				var module = process.GetModuleByName(moduleOffsetOperation.Name);
				if (module != null)
				{
					return module.Start;
				}

				return IntPtr.Zero;
			}

			if (operation is AdditionOperation additionOperation)
			{
				var addition = additionOperation;
				return Execute(addition.Argument1, process).Add(Execute(addition.Argument2, process));
			}

			if (operation is SubtractionOperation subtractionOperation)
			{
				var addition = subtractionOperation;
				return Execute(addition.Argument1, process).Sub(Execute(addition.Argument2, process));
			}

			if (operation is MultiplicationOperation multiplicationOperation)
			{
				var multiplication = multiplicationOperation;
				return Execute(multiplication.Argument1, process).Mul(Execute(multiplication.Argument2, process));
			}

			if (operation is DivisionOperation divisionOperation)
			{
				var division = divisionOperation;
				return Execute(division.Dividend, process).Div(Execute(division.Divisor, process));
			}

			if (operation is ReadPointerOperation pointerOperation)
			{
				return process.ReadRemoteIntPtr(Execute(pointerOperation.Argument, process));
			}

			throw new ArgumentException($"Unsupported operation '{operation.GetType().FullName}'.");
		}
	}
}
