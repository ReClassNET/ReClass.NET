using System;
using System.Collections.Generic;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class ListExtensionTest
	{
		public static TheoryData<IList<int>, Func<int, int>, int> GetTestBinarySearchData() => new TheoryData<IList<int>, Func<int, int>, int>
		{
			{ new List<int> { 0, 2, 4, 6, 8, 10, 12 }, i => 2.CompareTo(i), 1 },
			{ new List<int> { 0, 2, 4, 6, 8, 10, 12 }, i => 8.CompareTo(i), 4 },
			{ new List<int> { 1, 3, 5, 7, 9, 11, 13 }, i => 1.CompareTo(i), 0 },
			{ new List<int> { 1, 3, 5, 7, 9, 11, 13 }, i => 2.CompareTo(i), ~1 },
			{ new List<int> { 1, 3, 5, 7, 9, 11, 13 }, i => 14.CompareTo(i), ~7 },
		};

		[Theory]
		[MemberData(nameof(GetTestBinarySearchData))]
		public void TestBinarySearch(IList<int> sut, Func<int, int> comparer, int expected)
		{
			Check.That(sut.BinarySearch(comparer)).IsEqualTo(expected);
		}
	}
}
