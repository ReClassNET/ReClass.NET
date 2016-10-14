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
			Contract.Requires(!string.IsNullOrEmpty(formula));

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

					if (characters[i] != '>')
					{
						throw new ParseException($"Invalid token '{characters[i]}' detected at position {i}.");
					}
					++i;

					tokens.Add(new Token { TokenType = TokenType.ModuleOffset, Value = buffer });
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

					long offsetValue;
					if (long.TryParse(buffer, NumberStyles.HexNumber, null, out offsetValue))
					{
#if WIN64
						var address = (IntPtr)offsetValue;
#else
						var address = (IntPtr)unchecked((int)offsetValue);
#endif

						tokens.Add(new Token { TokenType = TokenType.Offset, Value = address });
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
						tokens.Add(new Token { TokenType = TokenType.Operation, Value = characters[i] });
						isFormulaSubPart = true;
						break;
					case '[':
						tokens.Add(new Token { TokenType = TokenType.LeftBracket, Value = characters[i] });
						isFormulaSubPart = true;
						break;
					case ']':
						tokens.Add(new Token { TokenType = TokenType.RightBracket, Value = characters[i] });
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
