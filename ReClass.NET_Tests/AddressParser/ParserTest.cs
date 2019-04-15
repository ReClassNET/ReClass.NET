using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using ReClassNET.AddressParser;
using Xunit;

namespace ReClass.NET_Tests.AddressParser
{
	public class ParserTest
	{
		private class TokenizerStub : ITokenizer
		{
			private readonly Tuple<Token, string, long>[] values;

			private int index = 0;

			public Token Token => values[index].Item1;
			public string Identifier => values[index].Item2;
			public long Number => values[index].Item3;

			public TokenizerStub(params Tuple<Token, string, long>[] values)
			{
				this.values = values
					.Append(Tuple.Create<Token, string, long>(Token.None, null, 0))
					.ToArray();
			}

			public void ReadNextToken()
			{
				if (index < values.Length - 1)
				{
					++index;
				}
			}
		}

		public static IEnumerable<object[]> InvalidExpressionData()
		{
			return new List<object[]>
			{
				new object[] { new TokenizerStub() },
				new object[] { new TokenizerStub(Tuple.Create(Token.Add, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.Subtract, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.Multiply, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.Divide, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.Comma, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.OpenParenthesis, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.CloseParenthesis, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.OpenBrackets, "", 0L)) },
				new object[] { new TokenizerStub(Tuple.Create(Token.CloseBrackets, "", 0L)) },

				new object[] { new TokenizerStub(
					Tuple.Create(Token.Number, "", 0L),
					Tuple.Create(Token.Subtract, "", 0L)
				) },
				new object[] { new TokenizerStub(
					Tuple.Create(Token.Number, "", 0L),
					Tuple.Create(Token.OpenParenthesis, "", 0L)
				) },
				new object[] { new TokenizerStub(
					Tuple.Create(Token.Number, "", 0L),
					Tuple.Create(Token.CloseParenthesis, "", 0L)
				) },
				new object[] { new TokenizerStub(
					Tuple.Create(Token.OpenParenthesis, "", 0L),
					Tuple.Create(Token.CloseParenthesis, "", 0L)
				) },
				new object[] { new TokenizerStub(
					Tuple.Create(Token.OpenParenthesis, "", 0L),
					Tuple.Create(Token.Number, "", 0L)
				) },
			};
		}

		[Theory]
		[MemberData(nameof(InvalidExpressionData))]
		public void InvalidExpressionTests(ITokenizer tokenizer)
		{
			var parser = new Parser(tokenizer);

			Check.ThatCode(() => parser.ParseExpression()).Throws<ParseException>();
		}

		[Theory]
		[InlineData("1", typeof(ConstantExpression))]
		[InlineData("1 + 2", typeof(AddExpression))]
		[InlineData("1 + 2 * 3", typeof(MultiplyExpression))]
		[InlineData("(1 + 2) * 3", typeof(MultiplyExpression))]
		[InlineData("1 + (2 * 3)", typeof(AddExpression))]
		[InlineData("(1 + (2 * 3))", typeof(AddExpression))]
		public void ValidExpressionTests(string expression, Type type)
		{
			var tokenizer = new Tokenizer(new StringReader(expression));
			var parser = new Parser(tokenizer);

			Check.That(parser.ParseExpression()).IsInstanceOfType(type);
		}
	}
}
