using System;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class IntPtrExtensionTest
	{
		public static TheoryData<IntPtr, bool> GetTestIsNullData => new TheoryData<IntPtr, bool>
		{
			{ IntPtr.Zero, true },
			{ (IntPtr)1, false }
		};

		[Theory]
		[MemberData(nameof(GetTestIsNullData))]
		public void TestIsNull(IntPtr ptr, bool expected)
		{
			Check.That(ptr.IsNull()).IsEqualTo(expected);
		}

		public static TheoryData<IntPtr, bool> GetTestMayBeValidData => new TheoryData<IntPtr, bool>
		{
			{ IntPtr.Zero, false },
			{ (IntPtr)1, false },
			{ (IntPtr)0x10000, true },
			{ (IntPtr)int.MaxValue, true },
#if RECLASSNET64
			{ (IntPtr)long.MaxValue + 1, false }
#else
			{ (IntPtr)int.MaxValue + 1, false }
#endif
		};

		[Theory]
		[MemberData(nameof(GetTestMayBeValidData))]
		public void TestMayBeValid(IntPtr ptr, bool expected)
		{
			Check.That(ptr.MayBeValid()).IsEqualTo(expected);
		}

		public static TheoryData<IntPtr, IntPtr, IntPtr, bool> GetTestIsInRangeData => new TheoryData<IntPtr, IntPtr, IntPtr, bool>
		{
			{ (IntPtr)10, (IntPtr)100, (IntPtr)1000, false },
			{ (IntPtr)100, (IntPtr)100, (IntPtr)1000, true },
			{ (IntPtr)500, (IntPtr)100, (IntPtr)1000, true },
			{ (IntPtr)1000, (IntPtr)100, (IntPtr)1000, true },
			{ (IntPtr)1500, (IntPtr)100, (IntPtr)1000, false }
		};

		[Theory]
		[MemberData(nameof(GetTestIsInRangeData))]
		public void TestIsInRange(IntPtr ptr, IntPtr start, IntPtr end, bool expected)
		{
			Check.That(ptr.IsInRange(start, end)).IsEqualTo(expected);
		}

		public static TheoryData<IntPtr, IntPtr, int> GetTestCompareToData => new TheoryData<IntPtr, IntPtr, int>
		{
			{ (IntPtr)10, (IntPtr)100, -1 },
			{ (IntPtr)100, (IntPtr)100, 0 },
			{ (IntPtr)500, (IntPtr)100, 1 }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareToData))]
		public void TestCompareTo(IntPtr ptr, IntPtr other, int expected)
		{
			Check.That(ptr.CompareTo(other)).IsEqualTo(expected);
		}

		public static TheoryData<IntPtr, IntPtr, IntPtr, int> GetTestCompareToRangeData => new TheoryData<IntPtr, IntPtr, IntPtr, int>
		{
			{ (IntPtr)10, (IntPtr)100, (IntPtr)1000, -1 },
			{ (IntPtr)100, (IntPtr)100, (IntPtr)1000, 0 },
			{ (IntPtr)500, (IntPtr)100, (IntPtr)1000, 0 },
			{ (IntPtr)1000, (IntPtr)100, (IntPtr)1000, 0 },
			{ (IntPtr)1500, (IntPtr)100, (IntPtr)1000, 1 }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareToRangeData))]
		public void TestCompareToRange(IntPtr ptr, IntPtr start, IntPtr end, int expected)
		{
			Check.That(ptr.CompareToRange(start, end)).IsEqualTo(expected);
		}

		public static TheoryData<IntPtr, long> GetTestToInt64BitsData => new TheoryData<IntPtr, long>
		{
			{ (IntPtr)0x10, 0x10L },
			{ (IntPtr)int.MaxValue, 0x7FFF_FFFFL },
			{ (IntPtr)int.MaxValue + 1, 0x8000_0000L }
		};

		[Theory]
		[MemberData(nameof(GetTestToInt64BitsData))]
		public void TestToInt64Bits(IntPtr ptr, long expected)
		{
			Check.That(ptr.ToInt64Bits()).IsEqualTo(expected);
		}
	}
}
