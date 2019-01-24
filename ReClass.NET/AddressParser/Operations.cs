using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.AddressParser
{
	public interface IOperation
	{

	}

	public class OffsetOperation : IOperation
	{
		public OffsetOperation(IntPtr value)
		{
			Value = value;
		}

		public IntPtr Value { get; }

		public override bool Equals(object obj)
		{
			if (obj is OffsetOperation other)
			{
				return Value.Equals(other.Value);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}

	public class ReadPointerOperation : IOperation
	{
		public ReadPointerOperation(IOperation argument)
		{
			Contract.Requires(argument != null);

			Argument = argument;
		}

		public IOperation Argument { get; }
	}

	public class AdditionOperation : IOperation
	{
		public AdditionOperation(IOperation argument1, IOperation argument2)
		{
			Contract.Requires(argument1 != null);
			Contract.Requires(argument2 != null);

			Argument1 = argument1;
			Argument2 = argument2;
		}

		public IOperation Argument1 { get; }
		public IOperation Argument2 { get; }
	}

	public class SubtractionOperation : IOperation
	{
		public SubtractionOperation(IOperation argument1, IOperation argument2)
		{
			Contract.Requires(argument1 != null);
			Contract.Requires(argument2 != null);

			Argument1 = argument1;
			Argument2 = argument2;
		}

		public IOperation Argument1 { get; }
		public IOperation Argument2 { get; }
	}

	public class DivisionOperation : IOperation
	{
		public DivisionOperation(IOperation dividend, IOperation divisor)
		{
			Contract.Requires(dividend != null);
			Contract.Requires(divisor != null);

			Dividend = dividend;
			Divisor = divisor;
		}

		public IOperation Dividend { get; }
		public IOperation Divisor { get; }
	}

	public class MultiplicationOperation : IOperation
	{
		public MultiplicationOperation(IOperation argument1, IOperation argument2)
		{
			Contract.Requires(argument1 != null);
			Contract.Requires(argument2 != null);

			Argument1 = argument1;
			Argument2 = argument2;
		}

		public IOperation Argument1 { get; }
		public IOperation Argument2 { get; }
	}

	public class ModuleOffsetOperation : IOperation
	{
		public ModuleOffsetOperation(string name)
		{
			Contract.Requires(name != null);

			Name = name;
		}

		public string Name { get; }

		public override bool Equals(object obj)
		{
			if (obj is ModuleOffsetOperation other)
			{
				return Name.Equals(other.Name);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}
