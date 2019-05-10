using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.Extensions
{
	public static class EnumerableExtension
	{
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

			var index = 0;
			foreach (var item in source)
			{
				if (predicate(item))
				{
					return index;
				}
				++index;
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
		public static IEnumerable<TSource> Traverse<TSource>(this IEnumerable<TSource> source, Func<TSource, IEnumerable<TSource>> childSelector)
		{
			Contract.Requires(source != null);
			Contract.Requires(childSelector != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			var queue = new Queue<TSource>(source);
			while (queue.Count > 0)
			{
				var next = queue.Dequeue();

				yield return next;

				foreach (var child in childSelector(next))
				{
					queue.Enqueue(child);
				}
			}
		}

		[DebuggerStepThrough]
		public static IEnumerable<TSource> TakeWhileInclusive<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			Contract.Requires(source != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<IEnumerable<TSource>>() != null);

			foreach (var item in source)
			{
				yield return item;

				if (!predicate(item))
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
		public static bool IsEquivalentTo<T>(this IEnumerable<T> source, IEnumerable<T> other)
		{
			Contract.Requires(source != null);
			Contract.Requires(other != null);

			var expected = new List<T>(source);

			if (other.Any(item => !expected.Remove(item)))
			{
				return false;
			}

			return expected.Count == 0;
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

			if (first)
			{
				throw new InvalidOperationException("Sequence contains no elements");
			}

			return result;
		}

		public static IEnumerable<IEnumerable<T>> GroupWhile<T>(this IEnumerable<T> source, Func<T, T, bool> condition)
		{
			Contract.Requires(source != null);
			Contract.Requires(condition != null);

			using (var it = source.GetEnumerator())
			{
				if (it.MoveNext())
				{
					var previous = it.Current;
					var list = new List<T> { previous };

					while (it.MoveNext())
					{
						var item = it.Current;

						if (condition(previous, item) == false)
						{
							yield return list;

							list = new List<T>();
						}

						list.Add(item);

						previous = item;
					}

					yield return list;
				}
			}
		}
	}
}
