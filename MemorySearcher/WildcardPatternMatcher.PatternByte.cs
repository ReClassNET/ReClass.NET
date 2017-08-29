using System.Diagnostics.Contracts;
using System.IO;

namespace ReClassNET.MemorySearcher
{
	public partial class WildcardPatternMatcher
	{
		private struct PatternByte
		{
			private struct Nibble
			{
				public int Value;
				public bool IsWildcard;
			}

			private Nibble Nibble1;
			private Nibble Nibble2;

			private static bool IsHexValue(char c)
			{
				return ('0' <= c && c <= '9')
					|| ('A' <= c && c <= 'F')
					|| ('a' <= c && c <= 'f');
			}

			private static int HexToInt(char c)
			{
				if ('0' <= c && c <= '9')
					return c - '0';
				if ('A' <= c && c <= 'F')
					return c - 'A' + 10;
				return c - 'a' + 10;
			}

			public bool TryRead(StringReader sr)
			{
				Contract.Requires(sr != null);

				var temp = sr.ReadSkipWhitespaces();
				if (temp == -1 || (!IsHexValue((char)temp) && (char)temp != '?'))
				{
					return false;
				}

				Nibble1.Value = HexToInt((char)temp) & 0xF;
				Nibble1.IsWildcard = (char)temp == '?';

				temp = sr.Read();
				if (temp == -1 || char.IsWhiteSpace((char)temp) || (char)temp == '?')
				{
					Nibble2.IsWildcard = true;

					return true;
				}

				if (!IsHexValue((char)temp))
				{
					return false;
				}
				Nibble2.Value = HexToInt((char)temp) & 0xF;
				Nibble2.IsWildcard = false;

				return true;
			}

			public bool Equals(byte b)
			{
				var matched = 0;
				if (Nibble1.IsWildcard || ((b >> 4) & 0xF) == Nibble1.Value)
				{
					++matched;
				}
				if (Nibble2.IsWildcard || (b & 0xF) == Nibble2.Value)
				{
					++matched;
				}
				return matched == 2;
			}
		}
	}
}
