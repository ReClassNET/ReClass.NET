using System;
using System.Collections.Generic;

namespace ReClassNET.Util
{
	public static class LinqExtension
	{
		public static string Join(this IEnumerable<string> source)
		{
			return Join(source, string.Empty);
		}

		public static string Join(this IEnumerable<string> source, string separator)
		{
			return string.Join(separator, source);
		}

		public static TResult Max<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
		{
			comparer = comparer ?? Comparer<TResult>.Default;

			using (var it = source.GetEnumerator())
			{
				if (!it.MoveNext())
				{
					throw new InvalidOperationException();
				}

				var max = selector(it.Current);
				while (it.MoveNext())
				{
					var current = selector(it.Current);
					if (comparer.Compare(current, max) > 0)
					{
						max = current;
					}
				}
				return max;
			}
		}

		public static TResult Min<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, IComparer<TResult> comparer)
		{
			comparer = comparer ?? Comparer<TResult>.Default;

			using (var it = source.GetEnumerator())
			{
				if (!it.MoveNext())
				{
					throw new InvalidOperationException();
				}

				var min = selector(it.Current);
				while (it.MoveNext())
				{
					var current = selector(it.Current);
					if (comparer.Compare(current, min) < 0)
					{
						min = current;
					}
				}
				return min;
			}
		}
	}
}
