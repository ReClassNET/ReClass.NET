using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;

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
	}
}
