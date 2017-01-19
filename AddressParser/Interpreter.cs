// Design taken from https://github.com/pieterderycke/Jace

using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.AddressParser
{
	class Interpreter
	{
		public IntPtr Execute(Operation operation, RemoteProcess process)
		{
			Contract.Requires(operation != null);
			Contract.Requires(process != null);

			var offsetOperation = operation as OffsetOperation;
			if (offsetOperation != null)
			{
				return offsetOperation.Value;
			}

			var moduleOffsetOperation = operation as ModuleOffsetOperation;
			if (moduleOffsetOperation != null)
			{
				var module = process.GetModuleByName(moduleOffsetOperation.Name);
				if (module != null)
				{
					return module.Start;
				}

				return IntPtr.Zero;
			}

			var additionOperation = operation as AdditionOperation;
			if (additionOperation != null)
			{
				var addition = additionOperation;
				return Execute(addition.Argument1, process).Add(Execute(addition.Argument2, process));
			}

			var subtractionOperation = operation as SubtractionOperation;
			if (subtractionOperation != null)
			{
				var addition = subtractionOperation;
				return Execute(addition.Argument1, process).Sub(Execute(addition.Argument2, process));
			}

			var multiplicationOperation = operation as MultiplicationOperation;
			if (multiplicationOperation != null)
			{
				var multiplication = multiplicationOperation;
				return Execute(multiplication.Argument1, process).Mul(Execute(multiplication.Argument2, process));
			}

			var divisionOperation = operation as DivisionOperation;
			if (divisionOperation != null)
			{
				var division = divisionOperation;
				return Execute(division.Dividend, process).Div(Execute(division.Divisor, process));
			}

			var pointerOperation = operation as ReadPointerOperation;
			if (pointerOperation != null)
			{
				return process.ReadRemoteObject<IntPtr>(Execute(pointerOperation.Argument, process));
			}

			throw new ArgumentException($"Unsupported operation '{operation.GetType().FullName}'.");
		}
	}
}
