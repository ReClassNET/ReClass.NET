using System;
using System.Collections.Generic;
using System.Linq;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class EnumerableExtensionTest
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

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(10)]
		public void TestForEach(int expected)
		{
			var counter = 0;
			Enumerable.Repeat(0, expected).ForEach(_ => ++counter);

			Check.That(counter).IsEqualTo(expected);
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

		public static TheoryData<IEnumerable<int>, Func<int, bool>, IEnumerable<int>> GetTestTakeWhileInclusiveData => new TheoryData<IEnumerable<int>, Func<int, bool>, IEnumerable<int>>
		{
			{ Enumerable.Empty<int>(), i => false, Enumerable.Empty<int>() },
			{ new [] { 1 }, i => false, new [] { 1 } },
			{ new [] { 1 }, i => true, new [] { 1 } },
			{ new [] { 1, 1 }, i => false, new [] { 1 } },
			{ new [] { 1, 1 }, i => true, new [] { 1, 1 } },
			{ new [] { 1, 2, 3, 4 }, i => i != 2, new [] { 1, 2 } },
			{ new [] { 1, 2, 3, 4 }, i => i != 3, new [] { 1, 2, 3 } },
			{ new [] { 4, 3, 2, 1 }, i => false, new [] { 4 } }
		};

		[Theory]
		[MemberData(nameof(GetTestTakeWhileInclusiveData))]
		public void TestTakeWhileInclusive(IEnumerable<int> sut, Func<int, bool> predicate, IEnumerable<int> expected)
		{
			Check.That(sut.TakeWhileInclusive(predicate)).IsEquivalentTo(expected);
		}

		public static TheoryData<IEnumerable<int>, Func<int, int, bool>, IEnumerable<IEnumerable<int>>> GetTestGroupWhileData => new TheoryData<IEnumerable<int>, Func<int, int, bool>, IEnumerable<IEnumerable<int>>>
		{
			{ Enumerable.Empty<int>(), (x, y) => false, Enumerable.Empty<IEnumerable<int>>() },
			{ new [] { 1, 2, 3 }, (x, y) => x == y, new [] { new[] { 1 }, new[] { 2 }, new[] { 3 } } },
			{ new [] { 1, 1, 2, 3, 3, 4 }, (x, y) => x == y, new [] { new[] { 1, 1 }, new[] { 2 }, new[] { 3, 3 }, new[] { 4 } } },
			{ new [] { 1, 1, 2, 3, 3, 4 }, (x, y) => x != y, new [] { new[] { 1 }, new[] { 1, 2, 3 }, new[] { 3, 4 } } }
		};

		[Theory]
		[MemberData(nameof(GetTestGroupWhileData))]
		public void TestGroupWhile(IEnumerable<int> sut, Func<int, int, bool> predicate, IEnumerable<IEnumerable<int>> expected)
		{
			using (var expectedIt = expected.GetEnumerator())
			{
				using (var groupIt = sut.GroupWhile(predicate).GetEnumerator())
				{
					while (groupIt.MoveNext())
					{
						Check.That(expectedIt.MoveNext()).IsTrue();

						Check.That(groupIt.Current).IsEquivalentTo(expectedIt.Current);
					}
				}

				Check.That(expectedIt.MoveNext()).IsFalse();
			}
		}

		public static TheoryData<IEnumerable<int>, Func<int, bool>, int> GetTestPredicateOrFirstData => new TheoryData<IEnumerable<int>, Func<int, bool>, int>
		{
			{ new [] { 1 }, i => false, 1 },
			{ new [] { 1 }, i => true, 1 },
			{ new [] { 1, 2, 3, 4 }, i => i == 2, 2 },
			{ new [] { 1, 2, 3, 4 }, i => i == 4, 4 },
			{ new [] { 1, 2, 3, 4 }, i => i == 5, 1 }
		};

		[Theory]
		[MemberData(nameof(GetTestPredicateOrFirstData))]
		public void TestPredicateOrFirst(IEnumerable<int> sut, Func<int, bool> predicate, int expected)
		{
			Check.That(sut.PredicateOrFirst(predicate)).IsEqualTo(expected);
		}

		[Fact]
		public void TestPredicateOrFirstThrows()
		{
			Check.ThatCode(() => Enumerable.Empty<int>().PredicateOrFirst(i => true)).Throws<InvalidOperationException>();
		}

		public class Traversable
		{
			public IList<Traversable> Children { get; } = new List<Traversable>();
		}

		[Fact]
		public void TestTraverse()
		{
			var traversable = new Traversable();
			var child1 = new Traversable();
			child1.Children.Add(new Traversable());
			var child2 = new Traversable();
			child2.Children.Add(new Traversable());
			child2.Children.Add(new Traversable());
			var child3 = new Traversable();
			child3.Children.Add(new Traversable());
			child3.Children.Add(new Traversable());
			child3.Children.Add(new Traversable());
			traversable.Children.Add(child1);
			traversable.Children.Add(child2);
			traversable.Children.Add(child3);

			var expected = new[] { traversable }
				.Append(child1)
				.Append(child2)
				.Append(child3)
				.Concat(child1.Children)
				.Concat(child2.Children)
				.Concat(child3.Children);

			Check.That(new[] { traversable }.Traverse(t => t.Children)).ContainsExactly(expected);
		}
	}
}
