using System;
using System.Collections.Generic;
using ReClassNET.Extensions;

namespace ReClassNET.Util
{
	public class IntPtrComparer : IComparer<IntPtr>
	{
		public static IntPtrComparer Instance { get; } = new IntPtrComparer();

		public int Compare(IntPtr x, IntPtr y)
		{
			return x.CompareTo(y);
		}
	}
}
