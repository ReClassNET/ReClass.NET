namespace ReClassNET.AddressParser
{
	public interface IExpression
	{
	}

	public abstract class BinaryExpression : IExpression
	{
		public IExpression Lhs { get; }
		public IExpression Rhs { get; }

		protected BinaryExpression(IExpression lhs, IExpression rhs)
		{
			Lhs = lhs;
			Rhs = rhs;
		}
	}

	public class AddExpression : BinaryExpression
	{
		public AddExpression(IExpression lhs, IExpression rhs)
			: base(lhs, rhs)
		{
		}
	}

	public class SubtractExpression : BinaryExpression
	{
		public SubtractExpression(IExpression lhs, IExpression rhs)
			: base(lhs, rhs)
		{
		}
	}

	public class MultiplyExpression : BinaryExpression
	{
		public MultiplyExpression(IExpression lhs, IExpression rhs)
			: base(lhs, rhs)
		{
		}
	}

	public class DivideExpression : BinaryExpression
	{
		public DivideExpression(IExpression lhs, IExpression rhs)
			: base(lhs, rhs)
		{
		}
	}

	public class ConstantExpression : IExpression
	{
		public long Value { get; }

		public ConstantExpression(long value)
		{
			Value = value;
		}
	}

	public abstract class UnaryExpression : IExpression
	{
		public IExpression Expression { get; }

		protected UnaryExpression(IExpression expression)
		{
			Expression = expression;
		}
	}

	public class NegateExpression : UnaryExpression
	{
		public NegateExpression(IExpression expression)
			: base(expression)
		{
		}
	}

	public class ReadMemoryExpression : UnaryExpression
	{
		public int ByteCount { get; }

		public ReadMemoryExpression(IExpression expression, int byteCount)
			: base(expression)
		{
			ByteCount = byteCount;
		}
	}

	public class ModuleExpression : IExpression
	{
		public string Name { get; }

		public ModuleExpression(string name)
		{
			Name = name;
		}
	}
}
