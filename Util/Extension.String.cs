using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.Util
{
	public static class StringExtension
	{
		[Pure]
		[DebuggerStepThrough]
		public static bool IsPrintable(this char c)
		{
			return ' ' <= c && c <= '~';
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsUtf8(this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);

			return source.Select(b => (char)b);
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsUtf16(this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);

			var bytes = source.ToArray();
			var chars = new char[bytes.Length / 2];
			Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
			return chars;
		}

		[DebuggerStepThrough]
		public static bool IsPrintableData(this IEnumerable<char> source)
		{
			Contract.Requires(source != null);

			return IsLikelyPrintableData(source) >= 1.0f;
		}

		[DebuggerStepThrough]
		public static float IsLikelyPrintableData(this IEnumerable<char> source)
		{
			Contract.Requires(source != null);

			bool doCountValid = true;
			int countValid = 0;
			int countAll = 0;

			foreach (var c in source)
			{
				countAll++;

				if (doCountValid)
				{
					if (c.IsPrintable())
					{
						countValid++;
					}
					else
					{
						doCountValid = false;
					}
				}
			}

			return countValid / (float)countAll;
		}

		[Pure]
		[DebuggerStepThrough]
		public static string LimitLength(this string s, int length)
		{
			Contract.Requires(s != null);
			Contract.Ensures(Contract.Result<string>() != null);

			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(0, length);
		}
	}
}
