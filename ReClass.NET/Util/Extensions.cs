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

				if (node is ClassNode classNode)
				{
					foreach (var child in classNode.Nodes)
					{
						nodes.Push(child);
					}
				}
			}
		}

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
	}
}
