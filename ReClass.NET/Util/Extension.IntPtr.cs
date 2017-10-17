using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.Util
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
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((long)0x00FF000000000000));
#else
			return ptr.InRange((IntPtr)0x10000, (IntPtr)unchecked((int)0xFFFFF000));
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
		public static bool InRange(this IntPtr address, IntPtr start, IntPtr end)
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
			if (InRange(address, start, end))
			{
				return 0;
			}
			return CompareTo(address, start);
		}
	}
}
