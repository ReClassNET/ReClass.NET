using System.IO;
using NFluent;
using ReClassNET.AddressParser;
using Xunit;

namespace ReClass.NET_Tests.AddressParser
{
	public class TokenReaderTest
	{
		[Theory]
		[InlineData("", Token.None)]
		[InlineData(" ", Token.None)]
		[InlineData("\t", Token.None)]
		[InlineData("\n", Token.None)]
		[InlineData(" \t\n", Token.None)]
		[InlineData("0", Token.Number)]
		[InlineData("1", Token.Number)]
		[InlineData("0x0", Token.Number)]
		[InlineData("0x1", Token.Number)]
		[InlineData("00000000", Token.Number)]
		[InlineData("0x00000000", Token.Number)]
		[InlineData("+", Token.Add)]
		[InlineData("-", Token.Subtract)]
		[InlineData("*", Token.Multiply)]
		[InlineData("/", Token.Divide)]
		[InlineData("(", Token.OpenParenthesis)]
		[InlineData(")", Token.CloseParenthesis)]
		[InlineData("[", Token.OpenBrackets)]
		[InlineData("]", Token.CloseBrackets)]
		[InlineData(",", Token.Comma)]
		[InlineData("<test.exe>", Token.Identifier)]
		public void TestTokenType(string expression, Token type)
		{
			var tokenizer = new Tokenizer(new StringReader(expression));

			Check.That(tokenizer.Token).IsEqualTo(type);
		}

		[Theory]
		[InlineData("0", 0)]
		[InlineData("1", 1)]
		[InlineData("0x0", 0)]
		[InlineData("0x1", 1)]
		[InlineData("00000000", 0)]
		[InlineData("0x00000000", 0)]
		[InlineData("12345678", 0x12345678)]
		[InlineData("0x12345678", 0x12345678)]
		public void TestNumberValue(string expression, long value)
		{
			var tokenizer = new Tokenizer(new StringReader(expression));

			Check.That(tokenizer.Number).IsEqualTo(value);
		}

		[Theory]
		[InlineData("<>", "")]
		[InlineData("<test>", "test")]
		[InlineData("<module.test>", "module.test")]
		public void TestIdentifierValue(string expression, string value)
		{
			var tokenizer = new Tokenizer(new StringReader(expression));

			Check.That(tokenizer.Identifier).IsEqualTo(value);
		}

		[Theory]
		[InlineData("<")]
		[InlineData(">")]
		[InlineData("10000000000000000")]
		[InlineData("0x")]
		[InlineData("x")]
		public void TestInvalidExpression(string expression)
		{
			Check.ThatCode(() => new Tokenizer(new StringReader(expression))).Throws<ParseException>();
		}
	}
}
