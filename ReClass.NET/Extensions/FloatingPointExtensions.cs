using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Extensions
{
	public static class FloatingPointExtension
	{
		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this float val, float other, float epsilon)
		{
			if (val == other)
			{
				return true;
			}

			return Math.Abs(val - other) <= epsilon;
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool IsNearlyEqual(this double val, double other, double epsilon)
		{
			if (val == other)
			{
				return true;
			}

			return Math.Abs(val - other) <= epsilon;
		}
	}
}
