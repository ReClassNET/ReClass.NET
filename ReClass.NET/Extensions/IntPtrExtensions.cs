using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Extensions
{
	public static class IntPtrExtension
	{
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
#if RECLASSNET64
			return ptr.IsInRange((IntPtr)0x10000, (IntPtr)long.MaxValue);
#else
			return ptr.IsInRange((IntPtr)0x10000, (IntPtr)int.MaxValue);
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Add(this IntPtr lhs, IntPtr rhs)
		{
#if RECLASSNET64
			return new IntPtr(lhs.ToInt64() + rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() + rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Sub(this IntPtr lhs, IntPtr rhs)
		{
#if RECLASSNET64
			return new IntPtr(lhs.ToInt64() - rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() - rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Mul(this IntPtr lhs, IntPtr rhs)
		{
#if RECLASSNET64
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

#if RECLASSNET64
			return new IntPtr(lhs.ToInt64() / rhs.ToInt64());
#else
			return new IntPtr(lhs.ToInt32() / rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static int Mod(this IntPtr lhs, int mod)
		{
#if RECLASSNET64
			return (int)(lhs.ToInt64() % mod);
#else
			return lhs.ToInt32() % mod;
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr Negate(this IntPtr ptr)
		{
#if RECLASSNET64
			return new IntPtr(-ptr.ToInt64());
#else
			return new IntPtr(-ptr.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static bool IsInRange(this IntPtr address, IntPtr start, IntPtr end)
		{
#if RECLASSNET64
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
#if RECLASSNET64
			return ((ulong)lhs.ToInt64()).CompareTo((ulong)rhs.ToInt64());
#else
			return ((uint)lhs.ToInt32()).CompareTo((uint)rhs.ToInt32());
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static int CompareToRange(this IntPtr address, IntPtr start, IntPtr end)
		{
			if (IsInRange(address, start, end))
			{
				return 0;
			}
			return CompareTo(address, start);
		}

		/// <summary>
		/// Changes the behaviour of ToInt64 in x86 mode.
		/// IntPtr(int.MaxValue + 1) = (int)0x80000000 (-2147483648) = (long)0xFFFFFFFF80000000
		/// This method converts the value to (long)0x0000000080000000 (2147483648).
		/// </summary>
		/// <param name="ptr"></param>
		/// <returns></returns>
		[Pure]
		[DebuggerStepThrough]
		public static long ToInt64Bits(this IntPtr ptr)
		{
#if RECLASSNET64
			return ptr.ToInt64();
#else
			var value = ptr.ToInt64();

			if (value < 0)
			{
				var intValue = ptr.ToInt32();
				if (value == intValue)
				{
					value = intValue & 0xFFFFFFFFL;
				}
			}

			return value;
#endif
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr From(int value)
		{
			return (IntPtr)value;
		}

		[Pure]
		[DebuggerStepThrough]
		public static IntPtr From(long value)
		{
#if RECLASSNET64
			return (IntPtr)value;
#else
			return (IntPtr)unchecked((int)value);
#endif
		}
	}
}
