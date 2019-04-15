using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

namespace ReClassNET.AddressParser
{
	/// <summary>
	/// Parses the given text and reads individual tokens from it.
	/// </summary>
	public class Tokenizer : ITokenizer
	{
		private readonly TextReader reader;

		private char currentCharacter;

		public Token Token { get; private set; }

		public string Identifier { get; private set; }

		public long Number { get; private set; }

		public Tokenizer(TextReader reader)
		{
			Contract.Requires(reader != null);

			this.reader = reader;

			ReadNextCharacter();
			ReadNextToken();
		}

		public void ReadNextToken()
		{
			SkipWhitespaces();

			if (currentCharacter == '\0')
			{
				Token = Token.None;
				Identifier = null;
				Number = 0;

				return;
			}

			if (TryReadSimpleToken())
			{
				ReadNextCharacter();

				return;
			}

			if (TryReadNumberToken())
			{
				return;
			}

			if (TryReadIdentifierToken())
			{
				return;
			}

			throw new ParseException($"Invalid character '{currentCharacter}'.");
		}

		private void ReadNextCharacter()
		{
			var c = reader.Read();
			currentCharacter = c < 0 ? '\0' : (char)c;
		}

		private void SkipWhitespaces()
		{
			while (char.IsWhiteSpace(currentCharacter))
			{
				ReadNextCharacter();
			}
		}

		private bool TryReadSimpleToken()
		{
			switch (currentCharacter)
			{
				case '+':
					Token = Token.Add;
					return true;
				case '-':
					Token = Token.Subtract;
					return true;
				case '*':
					Token = Token.Multiply;
					return true;
				case '/':
					Token = Token.Divide;
					return true;
				case '(':
					Token = Token.OpenParenthesis;
					return true;
				case ')':
					Token = Token.CloseParenthesis;
					return true;
				case '[':
					Token = Token.OpenBrackets;
					return true;
				case ']':
					Token = Token.CloseBrackets;
					return true;
				case ',':
					Token = Token.Comma;
					return true;
			}

			return false;
		}

		private bool TryReadNumberToken()
		{
			bool IsHexadecimalDigit(char c) => char.IsDigit(c) || 'a' <= c && c <= 'f' || 'A' <= c && c <= 'F';
			bool IsHexadecimalIdentifier(char c) => c == 'x' || c == 'X';

			if (IsHexadecimalDigit(currentCharacter))
			{
				var sb = new StringBuilder();
				var hasHexadecimalIdentifier = false;

				while (IsHexadecimalDigit(currentCharacter)
					|| IsHexadecimalIdentifier(currentCharacter) && !hasHexadecimalIdentifier && sb.Length == 1 && sb[0] == '0')
				{
					sb.Append(currentCharacter);

					if (!hasHexadecimalIdentifier)
					{
						hasHexadecimalIdentifier = IsHexadecimalIdentifier(currentCharacter);
					}

					ReadNextCharacter();
				}

				if (hasHexadecimalIdentifier)
				{
					sb.Remove(0, 2);
				}

				if (!long.TryParse(sb.ToString(), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var number))
				{
					throw new ParseException($"Could not parse '{sb}' as number.");
				}
				Number = number;

				Token = Token.Number;

				return true;
			}

			return false;
		}

		private bool TryReadIdentifierToken()
		{
			if (currentCharacter == '<')
			{
				ReadNextCharacter();

				var sb = new StringBuilder();

				while (currentCharacter != '\0' && currentCharacter != '>')
				{
					sb.Append(currentCharacter);

					ReadNextCharacter();
				}

				if (currentCharacter != '>')
				{
					throw new ParseException("Invalid identifier, missing '>'.");
				}

				ReadNextCharacter();

				Identifier = sb.ToString();
				Token = Token.Identifier;

				return true;
			}

			return false;
		}
	}
}