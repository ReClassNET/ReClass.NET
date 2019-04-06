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
		public void ReadBasicToken(string formula, Token type)
		{
			var tokenizer = new Tokenizer(new StringReader(formula));

			Check.That(tokenizer.Token).IsEqualTo(type);
		}
	}
}
