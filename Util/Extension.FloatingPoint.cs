using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Util
{
	public static class FloatingPointExtension
	{
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
			if (val == other)
			{
				return true;
			}

			var diff = Math.Abs(val - other);
			if (val == 0.0f || other == 0.0f || diff < float.Epsilon)
			{
				return diff < epsilon;
			}

			return diff / (Math.Abs(val) + Math.Abs(other)) < epsilon;
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
			if (val == other)
			{
				return true;
			}

			var diff = Math.Abs(val - other);
			if (val == 0.0 || other == 0.0 || diff < double.Epsilon)
			{
				return diff < epsilon;
			}

			return diff / (Math.Abs(val) + Math.Abs(other)) < epsilon;
		}
	}
}
