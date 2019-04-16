using System;
using System.IO;

namespace ReClassNET.AddressParser
{
	public class Parser
	{
		private readonly ITokenizer tokenizer;

		public Parser(ITokenizer tokenizer)
		{
			this.tokenizer = tokenizer;
		}

		public IExpression ParseExpression()
		{
			var expr = ParseAddSubtract();

			if (tokenizer.Token != Token.None)
			{
				throw new ParseException("Unexpected characters at end of expression");
			}

			return expr;
		}

		private IExpression ParseAddSubtract()
		{
			var lhs = ParseMultiplyDivide();

			while (true)
			{
				if (tokenizer.Token == Token.Add || tokenizer.Token == Token.Subtract)
				{
					var token = tokenizer.Token;

					tokenizer.ReadNextToken();

					var rhs = ParseMultiplyDivide();

					if (token == Token.Add)
					{
						lhs = new AddExpression(lhs, rhs);
					}
					else
					{
						lhs = new SubtractExpression(lhs, rhs);
					}
				}
				else
				{
					return lhs;
				}
			}
		}

		private IExpression ParseMultiplyDivide()
		{
			var lhs = ParseUnary();

			while (true)
			{
				if (tokenizer.Token == Token.Multiply || tokenizer.Token == Token.Divide)
				{
					var token = tokenizer.Token;

					tokenizer.ReadNextToken();

					var rhs = ParseUnary();

					if (token == Token.Multiply)
					{
						lhs = new MultiplyExpression(lhs, rhs);
					}
					else
					{
						lhs = new DivideExpression(lhs, rhs);
					}
				}
				else
				{
					return lhs;
				}
			}
		}

		private IExpression ParseUnary()
		{
			while (true)
			{
				if (tokenizer.Token == Token.Add)
				{
					tokenizer.ReadNextToken();

					continue;
				}

				if (tokenizer.Token == Token.Subtract)
				{
					tokenizer.ReadNextToken();

					var rhs = ParseUnary();

					return new NegateExpression(rhs);
				}

				return ParseLeaf();
			}
		}

		private IExpression ParseLeaf()
		{
			switch (tokenizer.Token)
			{
				case Token.Number:
					{
						var node = new ConstantExpression(tokenizer.Number);

						tokenizer.ReadNextToken();

						return node;
					}
				case Token.OpenParenthesis:
					{
						tokenizer.ReadNextToken();

						var node = ParseAddSubtract();

						if (tokenizer.Token != Token.CloseParenthesis)
						{
							throw new ParseException("Missing close parenthesis");
						}

						tokenizer.ReadNextToken();

						return node;
					}
				case Token.OpenBrackets:
					{
						tokenizer.ReadNextToken();

						var node = ParseAddSubtract();

						var byteCount = IntPtr.Size;
						if (tokenizer.Token == Token.Comma)
						{
							tokenizer.ReadNextToken();

							if (tokenizer.Token != Token.Number)
							{
								throw new ParseException("Missing read byte count");
							}

							if (tokenizer.Number != 4 && tokenizer.Number != 8)
							{
								throw new ParseException("The byte count must be 4 or 8.");
							}

							byteCount = (int)tokenizer.Number;

							tokenizer.ReadNextToken();
						}

						if (tokenizer.Token != Token.CloseBrackets)
						{
							throw new ParseException("Missing close bracket");
						}

						tokenizer.ReadNextToken();

						return new ReadMemoryExpression(node, byteCount);
					}
				case Token.Identifier:
					{
						var node = new ModuleExpression(tokenizer.Identifier);

						tokenizer.ReadNextToken();

						return node;
					}
				default:
					throw new ParseException($"Unexpect token: {tokenizer.Token}");
			}
		}

		public static IExpression Parse(string str)
		{
			using (var sr = new StringReader(str))
			{
				return Parse(new Tokenizer(sr));
			}
		}

		private static IExpression Parse(ITokenizer tokenizer)
		{
			var parser = new Parser(tokenizer);
			return parser.ParseExpression();
		}
	}
}
