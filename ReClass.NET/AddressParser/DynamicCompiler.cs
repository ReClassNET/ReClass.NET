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
		public IntPtr Execute(IExpression operation, RemoteProcess process)
		{
			Contract.Requires(operation != null);
			Contract.Requires(process != null);

			return CompileAddressFormula(operation)(process);
		}

		public static Func<RemoteProcess, IntPtr> CompileAddressFormula(IExpression expression)
		{
			Contract.Requires(expression != null);

			var processParameter = Expression.Parameter(typeof(RemoteProcess));

			return Expression.Lambda<Func<RemoteProcess, IntPtr>>(
				GenerateMethodBody(expression, processParameter),
				processParameter
			).Compile();
		}

		private static Expression GenerateMethodBody(IExpression operation, ParameterExpression processParameter)
		{
			Contract.Requires(operation != null);
			Contract.Requires(processParameter != null);

			switch (operation)
			{
				case AddExpression addExpression:
					{
						var argument1 = GenerateMethodBody(addExpression.Lhs, processParameter);
						var argument2 = GenerateMethodBody(addExpression.Rhs, processParameter);

						return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Add)), argument1, argument2);
					}
				case SubtractExpression subtractExpression:
					{
						var argument1 = GenerateMethodBody(subtractExpression.Lhs, processParameter);
						var argument2 = GenerateMethodBody(subtractExpression.Rhs, processParameter);

						return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Sub)), argument1, argument2);
					}
				case MultiplyExpression multiplyExpression:
					{
						var argument1 = GenerateMethodBody(multiplyExpression.Lhs, processParameter);
						var argument2 = GenerateMethodBody(multiplyExpression.Rhs, processParameter);

						return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Mul)), argument1, argument2);
					}
				case DivideExpression divideExpression:
					{
						var argument1 = GenerateMethodBody(divideExpression.Lhs, processParameter);
						var argument2 = GenerateMethodBody(divideExpression.Rhs, processParameter);

						return Expression.Call(null, GetIntPtrExtension(nameof(IntPtrExtension.Div)), argument1, argument2);
					}
				case ModuleExpression moduleExpression:
					{
						var getModuleByNameFunc = typeof(RemoteProcess).GetRuntimeMethod(nameof(RemoteProcess.GetModuleByName), new[] { typeof(string) });
						var moduleNameConstant = Expression.Constant(moduleExpression.Name);

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
				case ConstantExpression constantExpression:
					{
#if RECLASSNET64
						// long -> IntPtr
						return Expression.Convert(Expression.Constant(constantExpression.Value), typeof(IntPtr));
#else
						// long -> int -> IntPtr
						return Expression.Convert(Expression.Convert(Expression.Constant(constantExpression.Value), typeof(int)), typeof(IntPtr));
#endif
					}
				case ReadMemoryExpression readMemoryExpression:
					{
						var argument = GenerateMethodBody(readMemoryExpression.Expression, processParameter);

						var functionName = readMemoryExpression.ByteCount == 4 ? nameof(RemoteProcess.ReadRemoteInt32) : nameof(RemoteProcess.ReadRemoteInt64);
						var readRemoteIntFn = typeof(RemoteProcess).GetRuntimeMethod(functionName, new[] { typeof(IntPtr) });

						var callExpression = Expression.Call(processParameter, readRemoteIntFn, argument);

#if RECLASSNET64
						return Expression.Convert(callExpression, typeof(IntPtr));
#else
						return Expression.Convert(Expression.Convert(callExpression, typeof(int)), typeof(IntPtr));
#endif
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
