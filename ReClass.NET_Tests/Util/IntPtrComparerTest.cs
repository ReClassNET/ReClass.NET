using System;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class IntPtrComparerTest
	{
		public static TheoryData<IntPtr, IntPtr, bool> GetTestCompareData() => new TheoryData<IntPtr, IntPtr, bool>
		{
			{ IntPtr.Zero, IntPtr.Zero, false },
			{ (IntPtr)0x1, IntPtr.Zero, false },
			{ (IntPtr)0x1, (IntPtr)0x10, true },
			{ (IntPtr)0x1, unchecked((IntPtr)(int)0xFFFFFFFF), true },
			{ unchecked((IntPtr)(int)0xFFFFFFFF), unchecked((IntPtr)(int)0xFFFFFFFF), false },
			{ unchecked((IntPtr)(int)0xFFFFFFFF), IntPtr.Zero, false }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareData))]
		public void TestCompare(IntPtr lhs, IntPtr rhs, bool lhsIsSmaller)
		{
			var comparer = IntPtrComparer.Instance;

			Check.That(comparer.Compare(lhs, rhs) < 0).IsEqualTo(lhsIsSmaller);
		}
	}
}
