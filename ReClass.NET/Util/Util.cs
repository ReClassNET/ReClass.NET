using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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

		public static void Swap<T>(ref T lhs, ref T rhs)
		{
			var temp = lhs;
			lhs = rhs;
			rhs = temp;
		}

		//thx again stack overflow https://stackoverflow.com/a/1344242
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
			  .Select(s => s[Program.GlobalRandom.Next(s.Length)]).ToArray());
		}
	}
}
