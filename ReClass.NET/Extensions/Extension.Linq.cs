using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

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

		[DebuggerStepThrough]
		public static bool None<TSource>(this IEnumerable<TSource> source)
		{
			Contract.Requires(source != null);

			return !source.Any();
		}

		[DebuggerStepThrough]
		public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return !source.Any(predicate);
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			return source.Where(item => predicate(item) == false);
		}

		[DebuggerStepThrough]
		public static int FindIndex<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<int>() >= -1);

			int retVal = 0;
			foreach (var item in source)
			{
				if (predicate(item))
				{
					return retVal;
				}
				++retVal;
			}
			return -1;
		}

		[DebuggerStepThrough]
		public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> func)
		{
			Contract.Requires(source != null);
			Contract.Requires(func != null);

			foreach (var item in source)
			{
				func(item);
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> Yield<TSource>(this TSource item)
		{
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			yield return item;
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> Traverse<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> childSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(childSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			var stack = new Stack<TSource>(source);
			while (stack.Any())
			{
				var next = stack.Pop();

				yield return next;

				foreach (var child in childSelector(next))
				{
					stack.Push(child);
				}
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			using (var iterator = source.GetEnumerator())
			{
				while (iterator.MoveNext())
				{
					if (predicate(iterator.Current))
					{
						break;
					}
				}
				while (iterator.MoveNext())
				{
					yield return iterator.Current;
				}
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			foreach (var item in source)
			{
				yield return item;

				if (predicate(item))
				{
					yield break;
				}
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(keySelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			var knownKeys = new HashSet<TKey>();
			foreach (var element in source)
			{
				if (knownKeys.Add(keySelector(element)))
				{
					yield return element;
				}
			}
		}

		[DebuggerStepThrough]
		public static bool SequenceEqualsEx<T>(this IEnumerable<T> first, IEnumerable<T> second)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);

			return SequenceEqualsEx(first, second, EqualityComparer<T>.Default);
		}

		[DebuggerStepThrough]
		public static bool SequenceEqualsEx<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
		{
			Contract.Requires(first != null);
			Contract.Requires(second != null);
			Contract.Requires(comparer != null);

			var counter = new Dictionary<TSource, int>(comparer);
			foreach (var element in first)
			{
				if (counter.ContainsKey(element))
				{
					counter[element]++;
				}
				else
				{
					counter.Add(element, 1);
				}
			}
			foreach (var element in second)
			{
				if (counter.ContainsKey(element))
				{
					counter[element]--;
				}
				else
				{
					return false;
				}
			}
			return counter.Values.All(c => c == 0);
		}

		/// <summary>
		/// Scans the source and returns the first element where the predicate matches.
		/// If the predicate doesn't match the first element of the source is returned.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public static TSource PredicateOrFirst<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);

			var result = default(TSource);
			var first = true;
			foreach (var element in source)
			{
				if (predicate(element))
				{
					return element;
				}
				if (first)
				{
					result = element;
					first = false;
				}
			}
			return result;
		}
	}
}
