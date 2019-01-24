using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.AddressParser
{
	public class AstBuilder
	{
		private readonly Dictionary<char, int> operationPrecedence;

		private readonly Stack<IOperation> resultStack = new Stack<IOperation>();
		private readonly Stack<Token> operatorStack = new Stack<Token>();

		public AstBuilder()
		{
			Contract.Ensures(operationPrecedence != null);

			operationPrecedence = new Dictionary<char, int>
			{
				['\r'] = 0,
				['['] = 1,
				['+'] = 2,
				['-'] = 2,
				['*'] = 3,
				['/'] = 3,
			};
		}

		public IOperation Build(IEnumerable<Token> tokens)
		{
			Contract.Requires(tokens != null);
			Contract.Ensures(Contract.ForAll(tokens, t => t != null));

			resultStack.Clear();
			operatorStack.Clear();

			foreach (var token in tokens)
			{
				switch (token.TokenType)
				{
					case TokenType.Offset:
						resultStack.Push(new OffsetOperation((IntPtr)token.Value));
						break;
					case TokenType.ModuleOffset:
						resultStack.Push(new ModuleOffsetOperation(((string)token.Value).ToLowerInvariant()));
						break;
					case TokenType.LeftBracket:
						operatorStack.Push(token);
						break;
					case TokenType.RightBracket:
						PopOperations(true);
						break;
					case TokenType.Operation:
						var operation1 = (char)token.Value;

						while (operatorStack.Count > 0 && (operatorStack.Peek().TokenType == TokenType.Operation || operatorStack.Peek().TokenType == TokenType.ModuleOffset))
						{
							var other = operatorStack.Peek();

							var operation2 = (char)other.Value;

							if ((IsLeftAssociativeOperation(operation1) && operationPrecedence[operation1] <= operationPrecedence[operation2])
								|| operationPrecedence[operation1] < operationPrecedence[operation2])
							{
								operatorStack.Pop();

								resultStack.Push(ConvertOperation(other));
							}
							else
							{
								break;
							}
						}

						operatorStack.Push(token);
						break;
				}
			}

			PopOperations(false);

			VerifyResultStack();

			return resultStack.FirstOrDefault();
		}

		private void PopOperations(bool untillLeftBracket)
		{
			while (operatorStack.Count > 0 && operatorStack.Peek().TokenType != TokenType.LeftBracket)
			{
				var token = operatorStack.Pop();

				var lastOperation = ConvertOperation(token);

				resultStack.Push(lastOperation);
			}

			if (untillLeftBracket)
			{
				if (operatorStack.Count > 0 && operatorStack.Peek().TokenType == TokenType.LeftBracket)
				{
					operatorStack.Pop();
					resultStack.Push(ConvertOperation(new Token(TokenType.ReadPointer, '\r')));
				}
				else
				{
					throw new ParseException("No matching left bracket found for the right bracket.");
				}
			}
			else
			{
				if (operatorStack.Count > 0 && operatorStack.Peek().TokenType == TokenType.LeftBracket)
				{
					throw new ParseException("No matching right bracket found for the left bracket.");
				}
			}
		}

		private IOperation ConvertOperation(Token operationToken)
		{
			Contract.Requires(operationToken != null);

			try
			{
				IOperation argument1;
				IOperation argument2;

				switch ((char)operationToken.Value)
				{
					case '+':
						argument2 = resultStack.Pop();
						argument1 = resultStack.Pop();
						return new AdditionOperation(argument1, argument2);
					case '-':
						argument2 = resultStack.Pop();
						argument1 = resultStack.Pop();
						return new SubtractionOperation(argument1, argument2);
					case '*':
						argument2 = resultStack.Pop();
						argument1 = resultStack.Pop();
						return new MultiplicationOperation(argument1, argument2);
					case '/':
						argument2 = resultStack.Pop();
						argument1 = resultStack.Pop();
						return new DivisionOperation(argument1, argument2);
					case '\r':
						argument1 = resultStack.Pop();
						return new ReadPointerOperation(argument1);
					default:
						throw new ArgumentException($"Unknown operation '{operationToken.Value}'.");
				}
			}
			catch (InvalidOperationException)
			{
				throw new ParseException($"There is a syntax issue for the operation '{operationToken.Value}'.");
			}
		}

		private void VerifyResultStack()
		{
			if (resultStack.Count > 1)
			{
				if (resultStack.Skip(1).FirstOrDefault(o => o is OffsetOperation) is OffsetOperation offset)
				{
					throw new ParseException($"Unexpected offset '{offset.Value}' found.");
				}

				throw new ParseException("The syntax of the provided formula is not valid.");
			}
		}

		private bool IsLeftAssociativeOperation(char character)
		{
			return character == '+' || character == '-' || character == '*' || character == '/';
		}
	}
}
