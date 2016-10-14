using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET
{
	static class Extensions
	{
		public static bool IsNull(this IntPtr ptr)
		{
			return ptr == IntPtr.Zero;
		}

		public static bool MayBeValid(this IntPtr ptr)
		{
#if WIN64
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((long)0x000F000000000000));
#else
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((int)0xFFF00000));
#endif
		}

		public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() + rhs.ToInt32());
#endif
		}

		public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() - rhs.ToInt32());
#endif
		}

		public static IntPtr Mul(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() * rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() * rhs.ToInt32());
#endif
		}

		public static IntPtr Div(this IntPtr lhs, IntPtr rhs)
		{
#if WIN64
			return new IntPtr(lhs.ToInt64() / rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() / rhs.ToInt32());
#endif
		}

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

		public static int ToRgb(this Color color)
		{
			return 0xFFFFFF & color.ToArgb();
		}

		public static bool IsPrintable(this char c)
		{
			return ' ' <= c && c <= '~';
		}

		public static Point OffsetEx(this Point p, int x, int y)
		{
			var temp = p;
			temp.Offset(x, y);
			return temp;
		}

		public static string LimitLength(this string s, int length)
		{
			if (s.Length <= length)
			{
				return s;
			}
			return s.Substring(0, length);
		}

		public static void FillWithZero(this byte[] b)
		{
			for (var i = 0; i < b.Length; ++i)
			{
				b[i] = 0;
			}
		}

		public static IEnumerable<BaseNode> Descendants(this BaseNode root)
		{
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
			foreach (var item in items)
			{
				func(item);
			}
		}
	}
}
