using System;
using System.Collections.Generic;
using System.Linq;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class LinqExtensionTests
	{
		public static TheoryData<IEnumerable<int>, bool> GetTestNoneData => new TheoryData<IEnumerable<int>, bool>
		{
			{ new int[0], true },
			{ new int[1], false },
			{ Enumerable.Empty<int>(), true }
		};

		[Theory]
		[MemberData(nameof(GetTestNoneData))]
		public void TestNone(IEnumerable<int> sut, bool expected)
		{
			Check.That(sut.None()).IsEqualTo(expected);
		}

		public static TheoryData<IEnumerable<int>, Func<int, bool>, bool> GetTestNoneWithSelectorData => new TheoryData<IEnumerable<int>, Func<int, bool>, bool>
		{
			{ new int[0], i => false, true },
			{ new int[0], i => true, true },
			{ new [] { 1 }, i => i == 1, false },
			{ new [] { 1 }, i => i != 1, true },
			{ new [] { 1, 3, 5 }, i => i % 2 == 0, true }
		};

		[Theory]
		[MemberData(nameof(GetTestNoneWithSelectorData))]
		public void TestNoneWithSelector(IEnumerable<int> sut, Func<int, bool> selector, bool expected)
		{
			Check.That(sut.None(selector)).IsEqualTo(expected);
		}

		public static TheoryData<IEnumerable<int>, Func<int, bool>> GetTestWhereNotData => new TheoryData<IEnumerable<int>, Func<int, bool>>
		{
			{ new int[0], i => false },
			{ new int[0], i => true },
			{ new [] { 1 }, i => i == 1 },
			{ new [] { 1, 3, 5 }, i => i % 2 == 1 }
		};

		[Theory]
		[MemberData(nameof(GetTestWhereNotData))]
		public void TestWhereNot(IEnumerable<int> sut, Func<int, bool> selector)
		{
			Check.That(sut.WhereNot(selector)).IsEmpty();
		}

		public static TheoryData<IEnumerable<int>, int, int> GetTestFindIndexData => new TheoryData<IEnumerable<int>, int, int>
		{
			{ new int[0], 1, -1 },
			{ new [] { 1 }, 1, 0 },
			{ new [] { 1 }, 2, -1 },
			{ new [] { 1, 3, 5 }, 1, 0 },
			{ new [] { 1, 3, 5 }, 2, -1 },
			{ new [] { 1, 3, 5 }, 3, 1 },
			{ new [] { 1, 3, 5 }, 4, -1 },
			{ new [] { 1, 3, 5 }, 5, 2 }
		};

		[Theory]
		[MemberData(nameof(GetTestFindIndexData))]
		public void TestFindIndex(IEnumerable<int> sut, int item, int expected)
		{
			Check.That(sut.FindIndex(i => i == item)).IsEqualTo(expected);
		}

		public static TheoryData<IEnumerable<int>> GetTestForEachData => new TheoryData<IEnumerable<int>>
		{
			{ Enumerable.Empty<int>() },
			{ Enumerable.Repeat(0, 1) },
			{ Enumerable.Repeat(0, 2) },
			{ Enumerable.Repeat(0, 10) }
		};

		[Theory]
		[MemberData(nameof(GetTestForEachData))]
		public void TestForEach(IEnumerable<int> sut)
		{
			var sutCpy = sut.ToList();

			var counter = 0;
			sutCpy.ForEach(_ => counter++);

			Check.That(counter).IsEqualTo(sutCpy.Count);
		}

		public static TheoryData<IEnumerable<int>, Func<int, int>, IEnumerable<int>> GetTestDistinctByData => new TheoryData<IEnumerable<int>, Func<int, int>, IEnumerable<int>>
		{
			{ Enumerable.Empty<int>(), i => i, Enumerable.Empty<int>() },
			{ new [] { 1 }, i => i, new [] { 1 } },
			{ new [] { 1, 1, 1, 1 }, i => i, new [] { 1 } },
			{ new [] { 1, 2, 3, 4 }, i => i, new [] { 1, 2, 3, 4 } },
			{ new [] { 1, 1, 2, 2, 3, 3 }, i => i, new [] { 1, 2, 3 } },
			{ new [] { 1, 1, 2, 2, 3, 4 }, i => i, new [] { 1, 2, 3, 4 } },
			{ new [] { 1, 1, 2, 2, 3, 4 }, i => 0, new [] { 1 } }
		};

		[Theory]
		[MemberData(nameof(GetTestDistinctByData))]
		public void TestDistinctBy(IEnumerable<int> sut, Func<int, int> selector, IEnumerable<int> expected)
		{
			Check.That(sut.DistinctBy(selector)).IsEquivalentTo(expected);
		}

		public static TheoryData<IEnumerable<int>, IEnumerable<int>, bool> GetTestIsEquivalentToData => new TheoryData<IEnumerable<int>, IEnumerable<int>, bool>
		{
			{ Enumerable.Empty<int>(), Enumerable.Empty<int>(), true },
			{ Enumerable.Empty<int>(), new int[0], true },
			{ new [] { 1 }, new int[0], false },
			{ new [] { 1 }, new [] { 2 }, false },
			{ new [] { 1, 2, 3 }, new [] { 1, 2, 3 }, true },
			{ new [] { 1, 2, 3 }, new [] { 3, 1, 2 }, true }
		};

		[Theory]
		[MemberData(nameof(GetTestIsEquivalentToData))]
		public void TestIsEquivalentTo(IEnumerable<int> sut, IEnumerable<int> other, bool expected)
		{
			Check.That(sut.IsEquivalentTo(other)).IsEqualTo(expected);
		}
	}
}
