// Design taken from https://github.com/pieterderycke/Jace

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ReClassNET.AddressParser
{
	class TokenReader
	{
		/// <summary>
		/// Read in the provided formula and convert it into a list of takens that can be processed by the
		/// Abstract Syntax Tree Builder.
		/// </summary>
		/// <param name="formula">The formula that must be converted into a list of tokens.</param>
		/// <returns>The list of tokens for the provided formula.</returns>
		public List<Token> Read(string formula)
		{
			Contract.Requires(formula != null);

			var tokens = new List<Token>();

			var isFormulaSubPart = true;

			var characters = formula.ToCharArray();
			for (var i = 0; i < characters.Length; ++i)
			{
				if (characters[i] == '<')
				{
					var buffer = string.Empty;
					while (++i < characters.Length && IsPartOfModuleName(characters[i]))
					{
						buffer += characters[i];
					}

					if (i >= characters.Length)
					{
						throw new ParseException("Unexpected end of input detected.");
					}
					if (characters[i] != '>')
					{
						throw new ParseException($"Invalid token '{characters[i]}' detected at position {i}.");
					}
					++i;

					tokens.Add(new Token(TokenType.ModuleOffset, buffer));
					isFormulaSubPart = false;

					if (i == characters.Length)
					{
						continue;
					}
				}

				if (IsPartOfNumeric(characters[i], true, isFormulaSubPart))
				{
					var buffer = characters[i].ToString();
					while (++i < characters.Length && IsPartOfNumeric(characters[i], false, isFormulaSubPart))
					{
						buffer += characters[i];
					}

					if (buffer.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
					{
						buffer = buffer.Substring(2);
					}

					if (long.TryParse(buffer, NumberStyles.HexNumber, null, out var offsetValue))
					{
#if RECLASSNET64
						var address = (IntPtr)offsetValue;
#else
						var address = (IntPtr)unchecked((int)offsetValue);
#endif

						tokens.Add(new Token(TokenType.Offset, address));
						isFormulaSubPart = false;
					}
					else
					{
						throw new ParseException($"'{buffer}' is not a valid number.");
					}

					if (i == characters.Length)
					{
						continue;
					}
				}

				switch (characters[i])
				{
					case ' ':
						continue;
					case '+':
					case '-':
					case '*':
					case '/':
						tokens.Add(new Token(TokenType.Operation, characters[i]));
						isFormulaSubPart = true;
						break;
					case '[':
						tokens.Add(new Token(TokenType.LeftBracket, characters[i]));
						isFormulaSubPart = true;
						break;
					case ']':
						tokens.Add(new Token(TokenType.RightBracket, characters[i]));
						isFormulaSubPart = false;
						break;
					default:
						throw new ParseException($"Invalid token '{characters[i]}' detected at position {i}.");
				}
			}

			return tokens;
		}

		private bool IsPartOfNumeric(char character, bool isFirstCharacter, bool isFormulaSubPart)
		{
			return (character >= '0' && character <= '9') || (character >= 'a' && character <= 'f') || (character >= 'A' && character <= 'F') || (isFormulaSubPart && !isFirstCharacter && (character == 'x' || character == 'X'));
		}

		private bool IsPartOfModuleName(char character)
		{
			return !Path.GetInvalidFileNameChars().Contains(character);
		}
	}
}
