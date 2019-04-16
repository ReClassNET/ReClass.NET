using System;
using NFluent;
using ReClassNET.AddressParser;
using Xunit;

namespace ReClass.NET_Tests.AddressParser
{
	public class ParserTest
	{
		[Theory]
		[InlineData("-")]
		[InlineData("+")]
		[InlineData("*")]
		[InlineData("/")]
		[InlineData(",")]
		[InlineData("(")]
		[InlineData(")")]
		[InlineData("[")]
		[InlineData("]")]
		[InlineData("1-")]
		[InlineData("1(")]
		[InlineData("1)")]
		[InlineData("1[")]
		[InlineData("1]")]
		[InlineData("(1")]
		[InlineData(")1")]
		[InlineData("[1")]
		[InlineData("]1")]
		[InlineData("1+(")]
		[InlineData("1+)")]
		[InlineData("1 + ()")]
		[InlineData("(1 + 2")]
		[InlineData("1 + 2)")]
		[InlineData("[1 + 2)")]
		[InlineData("(1 + 2]")]
		[InlineData("[1,]")]
		[InlineData("[1,")]
		[InlineData("1,")]
		[InlineData("1,2")]
		public void InvalidExpressionTests(string expression)
		{
			Check.ThatCode(() => Parser.Parse(expression)).Throws<ParseException>();
		}

		[Theory]
		[InlineData("1", typeof(ConstantExpression))]
		[InlineData("1 + 2", typeof(AddExpression))]
		[InlineData("1 + 2 * 3", typeof(AddExpression))]
		[InlineData("(1 + 2) * 3", typeof(MultiplyExpression))]
		[InlineData("1 + (2 * 3)", typeof(AddExpression))]
		[InlineData("(1 + (2 * 3))", typeof(AddExpression))]
		public void ValidExpressionTests(string expression, Type type)
		{
			Check.That(Parser.Parse(expression)).IsInstanceOfType(type);
		}
	}
}
