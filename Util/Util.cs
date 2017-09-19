using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;

namespace ReClassNET.Util
{
	public static class Utils
	{
		public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
		{
			Contract.Requires(keySelector != null);

			return Min(item1, item2, keySelector, Comparer<U>.Default);
		}

		public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector, IComparer<U> comparer)
		{
			Contract.Requires(keySelector != null);
			Contract.Requires(comparer != null);

			if (comparer.Compare(keySelector(item1), keySelector(item2)) < 0)
			{
				return item1;
			}
			return item2;
		}

		public static T Max<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
		{
			Contract.Requires(keySelector != null);

			return Max(item1, item2, keySelector, Comparer<U>.Default);
		}

		public static T Max<T, U>(T item1, T item2, Func<T, U> keySelector, IComparer<U> comparer)
		{
			Contract.Requires(keySelector != null);
			Contract.Requires(comparer != null);

			if (comparer.Compare(keySelector(item1), keySelector(item2)) > 0)
			{
				return item1;
			}
			return item2;
		}

		public static Size AggregateNodeSizes(Size baseSize, Size newSize)
		{
			return new Size(Math.Max(baseSize.Width, newSize.Width), baseSize.Height + newSize.Height);
		}

		public static NumberFormatInfo GuessNumberFormat(string input)
		{
			Contract.Requires(input != null);
			Contract.Ensures(Contract.Result<NumberFormatInfo>() != null);

			if (input.Contains(",") && !input.Contains("."))
			{
				return new NumberFormatInfo
				{
					NumberDecimalSeparator = ",",
					NumberGroupSeparator = "."
				};
			}
			return new NumberFormatInfo
			{
				NumberDecimalSeparator = ".",
				NumberGroupSeparator = ","
			};
		}

		private static readonly uint[] hexLookup = CreateHexLookup();

		private static uint[] CreateHexLookup()
		{
			var result = new uint[256];
			for (var i = 0; i < 256; i++)
			{
				var s = i.ToString("X2");
				result[i] = (uint)s[0] + ((uint)s[1] << 16);
			}
			return result;
		}

		public static string ByteArrayToHexString(byte[] data)
		{
			Contract.Requires(data != null);

			if (data.Length == 0)
			{
				return string.Empty;
			}

			var lookup = hexLookup;
			var result = new char[data.Length * 2 + data.Length - 1];

			var val = lookup[data[0]];
			result[0] = (char)val;
			result[1] = (char)(val >> 16);

			for (var i = 1; i < data.Length; i++)
			{
				val = lookup[data[i]];
				result[3 * i - 1] = ' ';
				result[3 * i] = (char)val;
				result[3 * i + 1] = (char)(val >> 16);
			}
			return new string(result);
		}
	}
}
