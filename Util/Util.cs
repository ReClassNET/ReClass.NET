using System;
using System.Collections.Generic;

namespace ReClassNET.Util
{
	public static class Utils
	{
		public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
		{
			return Min(item1, item2, keySelector, Comparer<U>.Default);
		}

		public static T Min<T, U>(T item1, T item2, Func<T, U> keySelector, IComparer<U> comparer)
		{
			if (comparer.Compare(keySelector(item1), keySelector(item2)) < 0)
			{
				return item1;
			}
			return item2;
		}

		public static T Max<T, U>(T item1, T item2, Func<T, U> keySelector) where U : IComparable
		{
			return Max(item1, item2, keySelector, Comparer<U>.Default);
		}

		public static T Max<T, U>(T item1, T item2, Func<T, U> keySelector, IComparer<U> comparer)
		{
			if (comparer.Compare(keySelector(item1), keySelector(item2)) > 0)
			{
				return item1;
			}
			return item2;
		}
	}
}
