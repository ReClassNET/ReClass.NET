using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using Microsoft.SqlServer.MessageBox;
using ReClassNET.Nodes;

namespace ReClassNET.Util
{
	public static class Extensions
	{
		[Pure]
		public static bool IsNull(this IntPtr ptr)
		{
			return ptr == IntPtr.Zero;
		}

		[Pure]
		public static bool MayBeValid(this IntPtr ptr)
		{
#if WIN64
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((long)0x000F000000000000));
#else
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((int)0xFFF00000));
#endif
		}

		[Pure]
		public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() + rhs.ToInt32());
#endif
		}

		[Pure]
		public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() - rhs.ToInt32());
#endif
		}

		[Pure]
		public static IntPtr Mul(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() * rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() * rhs.ToInt32());
#endif
		}

		[Pure]
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
		public static int ToRgb(this Color color)
		{
			return 0xFFFFFF & color.ToArgb();
		}

		[Pure]
		public static bool IsPrintable(this char c)
		{
			return ' ' <= c && c <= '~';
		}

		[Pure]
		public static Point OffsetEx(this Point p, int x, int y)
		{
			var temp = p;
			temp.Offset(x, y);
			return temp;
		}

		[Pure]
		public static string LimitLength(this string s, int length)
		{
			Contract.Requires(s != null);

			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(0, length);
		}

		[Pure]
		public static void FillWithZero(this byte[] b)
		{
			Contract.Requires(b != null);

			for (var i = 0; i < b.Length; ++i)
			{
				b[i] = 0;
			}
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

		public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
		{
			Contract.Requires(items != null);
			Contract.Requires(predicate != null);
			Contract.Ensures(Contract.Result<int>() >= -1);

			int retVal = 0;
			foreach (var item in items)
			{
				if (predicate(item))
				{
					return retVal;
				}
				++retVal;
			}
			return -1;
		}

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> func)
		{
			Contract.Requires(items != null);
			Contract.Requires(func != null);

			foreach (var item in items)
			{
				func(item);
			}
		}

		public static IEnumerable<T> Yield<T>(this T item)
		{
			yield return item;
		}

		public static IEnumerable<T> Traverse<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> childSelector)
		{
			Contract.Requires(items != null);
			Contract.Requires(childSelector != null);

			var stack = new Stack<T>(items);
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

		public static IEnumerable<T> EveryNth<T>(this IEnumerable<T> items, int n)
		{
			Contract.Requires(items != null);
			Contract.Requires(n > 0);

			int i = 0;
			foreach (var item in items)
			{
				if (i++ % n == 0)
				{
					yield return item;
				}
			}
		}

		public static void ShowDialog(this Exception ex)
		{
			Contract.Requires(ex != null);

			// This doesn't look good...
			ex.HelpLink = "https://github.com/KN4CK3R/ReClass.NET/issues";

			var msg = new ExceptionMessageBox(ex);
			msg.ShowToolBar = true;
			msg.Symbol = ExceptionMessageBoxSymbol.Error;
			msg.Show(null);
		}
	}
}
