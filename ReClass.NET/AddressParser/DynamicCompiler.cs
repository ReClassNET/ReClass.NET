using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using ReClassNET.Extensions;
using ReClassNET.Memory;

namespace ReClassNET.AddressParser
{
	public class DynamicCompiler : IExecuter
	{
		public IntPtr Execute(IOperation operation, RemoteProcess process)
		{
			Contract.Requires(operation != null);
			Contract.Requires(process != null);

			return CompileAddressFormula(operation)(process);
		}

		public Func<RemoteProcess, IntPtr> CompileAddressFormula(IOperation operation)
		{
			Contract.Requires(operation != null);

			var processParameter = Expression.Parameter(typeof(RemoteProcess));

			return Expression.Lambda<Func<RemoteProcess, IntPtr>>(
				GenerateMethodBody(operation, processParameter),
				processParameter
			).Compile();
		}

		private Expression GenerateMethodBody(IOperation operation, ParameterExpression processParameter)
		{
			Contract.Requires(operation != null);
			Contract.Requires(processParameter != null);

			switch (operation)
			{
				case AdditionOperation additionOperation:
				{
					var argument1 = GenerateMethodBody(additionOperation.Argument1, processParameter);
					var argument2 = GenerateMethodBody(additionOperation.Argument2, processParameter);

					return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Add)), argument1, argument2);
				}
				case SubtractionOperation subtractionOperation:
				{
					var argument1 = GenerateMethodBody(subtractionOperation.Argument1, processParameter);
					var argument2 = GenerateMethodBody(subtractionOperation.Argument2, processParameter);

					return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Sub)), argument1, argument2);
				}
				case MultiplicationOperation multiplicationOperation:
				{
					var argument1 = GenerateMethodBody(multiplicationOperation.Argument1, processParameter);
					var argument2 = GenerateMethodBody(multiplicationOperation.Argument2, processParameter);

					return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Mul)), argument1, argument2);
				}
				case DivisionOperation divisionOperation:
				{
					var argument1 = GenerateMethodBody(divisionOperation.Dividend, processParameter);
					var argument2 = GenerateMethodBody(divisionOperation.Divisor, processParameter);

					return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Div)), argument1, argument2);
				}
				case ModuleOffsetOperation moduleOffsetOperation:
				{
					var getModuleByNameFunc = typeof(RemoteProcess).GetRuntimeMethod(nameof(RemoteProcess.GetModuleByName), new[] { typeof(string) });
					var moduleNameConstant = Expression.Constant(moduleOffsetOperation.Name);

					var moduleVariable = Expression.Variable(typeof(Memory.Module));
					var assignExpression = Expression.Assign(moduleVariable, Expression.Call(processParameter, getModuleByNameFunc, moduleNameConstant));

					return Expression.Block(
						new[] { moduleVariable },
						assignExpression,
						Expression.Condition(
							Expression.Equal(moduleVariable, Expression.Constant(null)),
							Expression.Constant(IntPtr.Zero),
							Expression.MakeMemberAccess(moduleVariable, typeof(Memory.Module).GetProperty(nameof(Memory.Module.Start)))
						)
					);
				}
				case OffsetOperation offsetOperation:
				{
					return Expression.Constant(offsetOperation.Value);
				}
				case ReadPointerOperation readPointerOperation:
				{
					var argument = GenerateMethodBody(readPointerOperation.Argument, processParameter);

					var readRemoteIntPtrFunc = typeof(RemoteProcess).GetRuntimeMethod(nameof(RemoteProcess.ReadRemoteIntPtr), new[] { typeof(IntPtr) });

					return Expression.Call(processParameter, readRemoteIntPtrFunc, argument);
				}
			}

			throw new ArgumentException($"Unsupported operation '{operation.GetType().FullName}'.");
		}

		private static MethodInfo GetIntPtrExtension(string name)
		{
			Contract.Requires(name != null);

			return typeof(IntPtrExtension).GetRuntimeMethod(name, new[] { typeof(IntPtr), typeof(IntPtr) });
		}
	}
}
