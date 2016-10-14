// Design taken from https://github.com/pieterderycke/Jace

using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.AddressParser
{
	class Interpreter
	{
		public IntPtr Execute(Operation operation, RemoteProcess process)
		{
			Contract.Requires(operation != null);
			Contract.Requires(process != null);

			if (operation is OffsetOperation)
			{
				return ((OffsetOperation)operation).Value;
			}
			else if (operation is ModuleOffsetOperation)
			{
				var module = process.Modules
					.Where(m => m.Name.Equals(((ModuleOffsetOperation)operation).Name, StringComparison.InvariantCultureIgnoreCase))
					.FirstOrDefault();
				if (module != null)
				{
					return module.Start;
				}

				throw new Exception($"The module '{module.Name}' can not be found.");
			}
			else if (operation is AdditionOperation)
			{
				var addition = (AdditionOperation)operation;
				return Execute(addition.Argument1, process).Add(Execute(addition.Argument2, process));
			}
			else if (operation is SubtractionOperation)
			{
				var addition = (SubtractionOperation)operation;
				return Execute(addition.Argument1, process).Sub(Execute(addition.Argument2, process));
			}
			else if (operation is MultiplicationOperation)
			{
				var multiplication = (MultiplicationOperation)operation;
				return Execute(multiplication.Argument1, process).Mul(Execute(multiplication.Argument2, process));
			}
			else if (operation is DivisionOperation)
			{
				var division = (DivisionOperation)operation;
				return Execute(division.Dividend, process).Div(Execute(division.Divisor, process));
			}
			else if (operation is ReadPointerOperation)
			{
				return process.ReadRemoteObject<IntPtr>(Execute(((ReadPointerOperation)operation).Argument, process));
			}

			throw new ArgumentException($"Unsupported operation '{operation.GetType().FullName}'.");
		}
	}
}
