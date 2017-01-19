using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Nodes;

namespace ReClassNET.Util
{
	public static class Extensions
	{
		[Pure]
		[DebuggerStepThrough]
		public static int ToRgb(this Color color)
		{
			return 0xFFFFFF & color.ToArgb();
		}

		[DebuggerStepThrough]
		public static void FillWithZero(this byte[] b)
		{
			Contract.Requires(b != null);

			for (var i = 0; i < b.Length; ++i)
			{
				b[i] = 0;
			}
		}

		[Pure]
		[DebuggerStepThrough]
		public static Point OffsetEx(this Point p, int x, int y)
		{
			var temp = p;
			temp.Offset(x, y);
			return temp;
		}

		public static IEnumerable<BaseNode> Descendants(this BaseNode root)
		{
			Contract.Requires(root != null);

			var nodes = new Stack<BaseNode>();
			nodes.Push(root);
			while (nodes.Any())
			{
				var node = nodes.Pop();
				yield return node;

				var classNode = node as ClassNode;
				if (classNode != null)
				{
					foreach (var child in classNode.Nodes)
					{
						nodes.Push(child);
					}
				}
			}
		}

		#region Pointer

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNull(this IntPtr ptr)
		{
			return ptr == IntPtr.Zero;
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool MayBeValid(this IntPtr ptr)
		{
#if WIN64
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((long)0x00FF000000000000));
#else
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((int)0xFFFFF000));
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() + rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() - rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Mul(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() * rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() * rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Div(this IntPtr lhs, IntPtr rhs)
		{
			Contract.Requires(!rhs.IsNull());

#if WIN64
			return new IntPtr(lhs.ToInt64() / rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() / rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static int Mod(this IntPtr lhs, int mod)
		{
#if WIN64
			return (int)(lhs.ToInt64() % mod);
#else
			return lhs.ToInt32() % mod;
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool InRange(this IntPtr address, IntPtr start, IntPtr end)
		{
#if WIN64
			var val = (ulong)address.ToInt64();
			return (ulong)start.ToInt64() <= val && val <= (ulong)end.ToInt64();
#else
			var val = (uint)address.ToInt32();
			return (uint)start.ToInt32() <= val && val <= (uint)end.ToInt32();
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static int CompareTo(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return ((ulong)lhs.ToInt64()).CompareTo((ulong)rhs.ToInt64());
#else
			return ((uint)lhs.ToInt32()).CompareTo((uint)rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static int CompareToRange(this IntPtr address, IntPtr start, IntPtr end)
		{
			if (InRange(address, start, end))
			{
				return 0;
			}
			return CompareTo(address, start);
		}

		#endregion

		#region String

		[Pure]
		[DebuggerStepThrough]
		public static bool IsPrintable(this char c)
		{
			return ' ' <= c && c <= '~';
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsUTF8(this IEnumerable<byte> source)
		{
			Contract.Requires(source != null);

			return source.Select(b => (char)b);
		}

		[DebuggerStepThrough]
		public static IEnumerable<char> InterpretAsUTF16(this IEnumerable<byte> source)
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

		#endregion

		#region Floating Point

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this float val, float other)
		{
			return IsNearlyEqual(val, other, float.Epsilon);
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this float val, float other, float epsilon)
		{
			var diff = Math.Abs(val - other);

			if (val == other)
			{
				return true;
			}
			else if (val == 0.0f || other == 0.0f || diff < float.Epsilon)
			{
				return diff < epsilon;
			}
			else
			{
				return diff / (Math.Abs(val) + Math.Abs(other)) < epsilon;
			}
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this double val, double other)
		{
			return IsNearlyEqual(val, other, double.Epsilon);
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this double val, double other, double epsilon)
		{
			var diff = Math.Abs(val - other);

			if (val == other)
			{
				return true;
			}
			else if (val == 0.0 || other == 0.0 || diff < double.Epsilon)
			{
				return diff < epsilon;
			}
			else
			{
				return diff / (Math.Abs(val) + Math.Abs(other)) < epsilon;
			}
		}

		#endregion

		#region List

		[DebuggerStepThrough]
		public static T BinaryFind<T>(this IList<T> source, Func<T, int> comparer)
		{
			Contract.Requires(source != null);
			Contract.Requires(comparer != null);

			var lo = 0;
			var hi = source.Count - 1;

			while (lo <= hi)
			{
				var median = lo + (hi - lo >> 1);
				var num = comparer(source[median]);
				if (num == 0)
				{
					return source[median];
				}
				if (num > 0)
				{
					lo = median + 1;
				}
				else
				{
					hi = median - 1;
				}
			}

			return default(T);
		}

		#endregion

		#region Linq

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

		#endregion
	}
}
