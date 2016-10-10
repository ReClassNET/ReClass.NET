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

		public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
		{
			return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
		}

		public static IntPtr Add(this IntPtr lhs, long rhs)
		{
			return new IntPtr(lhs.ToInt64() + rhs);
		}

		public static IntPtr Add(this IntPtr lhs, ulong rhs)
		{
			return new IntPtr(lhs.ToInt64() + (long)rhs);
		}

		public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
		{
			return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
		}

		public static bool IsNull(this UIntPtr ptr)
		{
			return ptr == UIntPtr.Zero;
		}

		public static UIntPtr Add(this UIntPtr lhs, UIntPtr rhs)
		{
			return new UIntPtr(lhs.ToUInt64() + rhs.ToUInt64());
		}

		public static UIntPtr Add(this UIntPtr lhs, long rhs)
		{
			return new UIntPtr(lhs.ToUInt64() + (ulong)rhs);
		}

		public static UIntPtr Add(this UIntPtr lhs, ulong rhs)
		{
			return new UIntPtr(lhs.ToUInt64() + rhs);
		}

		public static UIntPtr Sub(this UIntPtr lhs, UIntPtr rhs)
		{
			return new UIntPtr(lhs.ToUInt64() - rhs.ToUInt64());
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
