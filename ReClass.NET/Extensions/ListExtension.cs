using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Extensions
{
	public static class ListExtension
	{
		/// <summary>
		/// Searches a range of elements in the sorted list for an element using the specified comparer and returns the zero-based index of the element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source"></param>
		/// <param name="comparer">The comparer to use</param>
		/// <returns>The zero-based index in the sorted list, if an item is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger or, if there is no larger element, the bitwise complement of <see cref="IList{T}.Count"/>.</returns>
		[Pure]
		[DebuggerStepThrough]
		public static int BinarySearch<T>(this IList<T> source, Func<T, int> comparer)
		{
			Contract.Requires(source != null);
			Contract.Requires(comparer != null);

			var lo = 0;
			var hi = source.Count - 1;

			while (lo <= hi)
			{
				var i = lo + (hi - lo >> 1);

				var order = comparer(source[i]);
				if (order == 0)
				{
					return i;
				}
				if (order > 0)
				{
					lo = i + 1;
				}
				else
				{
					hi = i - 1;
				}
			}

			return ~lo;
		}
	}
}
