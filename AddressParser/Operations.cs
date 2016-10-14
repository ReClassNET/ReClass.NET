// Design taken from https://github.com/pieterderycke/Jace

using System;

namespace ReClassNET.AddressParser
{
	interface Operation
	{

	}

	class OffsetOperation : Operation
	{
		public OffsetOperation(IntPtr value)
		{
			Value = value;
		}

		public IntPtr Value { get; }

		public override bool Equals(object obj)
		{
			var other = obj as OffsetOperation;
			if (other != null)
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

	class ReadPointerOperation : Operation
	{
		public ReadPointerOperation(Operation argument)
		{
			Argument = argument;
		}

		public Operation Argument { get; }
	}

	class AdditionOperation : Operation
	{
		public AdditionOperation(Operation argument1, Operation argument2)
		{
			Argument1 = argument1;
			Argument2 = argument2;
		}

		public Operation Argument1 { get; }
		public Operation Argument2 { get; }
	}

	class SubtractionOperation : Operation
	{
		public SubtractionOperation(Operation argument1, Operation argument2)
		{
			Argument1 = argument1;
			Argument2 = argument2;
		}

		public Operation Argument1 { get; }
		public Operation Argument2 { get; }
	}

	class DivisionOperation : Operation
	{
		public DivisionOperation(Operation dividend, Operation divisor)
		{
			Dividend = dividend;
			Divisor = divisor;
		}

		public Operation Dividend { get; }
		public Operation Divisor { get; }
	}

	class MultiplicationOperation : Operation
	{
		public MultiplicationOperation(Operation argument1, Operation argument2)
		{
			Argument1 = argument1;
			Argument2 = argument2;
		}

		public Operation Argument1 { get; }
		public Operation Argument2 { get; }
	}

	class ModuleOffsetOperation : Operation
	{
		public ModuleOffsetOperation(string name)
		{
			Name = name;
		}

		public string Name { get; }

		public override bool Equals(object obj)
		{
			var other = obj as ModuleOffsetOperation;
			if (other != null)
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
