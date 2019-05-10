using System;
using System.Collections.Generic;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class DirectedGraphTest
	{
		[Fact]
		public void TestAddContainsVertex()
		{
			const int Vertex = 0;

			var sut = new DirectedGraph<int>();

			Check.That(sut.AddVertex(Vertex)).IsTrue();

			Check.That(sut.ContainsVertex(Vertex)).IsTrue();
		}

		[Fact]
		public void TestAddExistingVertex()
		{
			const int Vertex = 0;

			var sut = new DirectedGraph<int>();

			sut.AddVertex(Vertex);

			Check.That(sut.AddVertex(Vertex)).IsFalse();
		}

		[Fact]
		public void TestContainsWithEmptyGraph()
		{
			const int Vertex = 0;

			var sut = new DirectedGraph<int>();

			Check.That(sut.ContainsVertex(Vertex)).IsFalse();
		}

		[Fact]
		public void TestAddContainsMultipleVertices()
		{
			var vertices = new[] { 0, 1, 2, 3 };

			var sut = new DirectedGraph<int>();

			sut.AddVertices(vertices);

			foreach (var vertex in vertices)
			{
				Check.That(sut.ContainsVertex(vertex)).IsTrue();
			}

			Check.That(sut.Vertices).IsEquivalentTo(vertices);
		}

		[Fact]
		public void TestAddEdgeToNonExistingVertex()
		{
			const int Vertex1 = 0;
			const int Vertex2 = 1;

			var sut = new DirectedGraph<int>();

			Check.ThatCode(() => sut.AddEdge(Vertex1, Vertex2)).Throws<ArgumentException>();

			sut.AddVertex(Vertex1);

			Check.ThatCode(() => sut.AddEdge(Vertex1, Vertex2)).Throws<ArgumentException>();
			Check.ThatCode(() => sut.AddEdge(Vertex2, Vertex1)).Throws<ArgumentException>();
		}

		[Fact]
		public void TestContainsEdgeToNonExistingVertex()
		{
			const int Vertex1 = 0;
			const int Vertex2 = 1;

			var sut = new DirectedGraph<int>();

			Check.ThatCode(() => sut.ContainsEdge(Vertex1, Vertex2)).Throws<ArgumentException>();

			sut.AddVertex(Vertex1);

			Check.ThatCode(() => sut.ContainsEdge(Vertex1, Vertex2)).Throws<ArgumentException>();
			Check.ThatCode(() => sut.ContainsEdge(Vertex2, Vertex1)).Throws<ArgumentException>();
		}

		[Fact]
		public void TestAddContainsEdge()
		{
			const int Vertex1 = 0;
			const int Vertex2 = 1;

			var sut = new DirectedGraph<int>();

			sut.AddVertex(Vertex1);
			sut.AddVertex(Vertex2);

			Check.That(sut.ContainsEdge(Vertex1, Vertex2)).IsFalse();

			Check.That(sut.AddEdge(Vertex1, Vertex2)).IsTrue();

			Check.That(sut.ContainsEdge(Vertex1, Vertex2)).IsTrue();
		}

		[Fact]
		public void TestAddExistingEdge()
		{
			const int Vertex1 = 0;
			const int Vertex2 = 1;

			var sut = new DirectedGraph<int>();

			sut.AddVertex(Vertex1);
			sut.AddVertex(Vertex2);
			sut.AddEdge(Vertex1, Vertex2);

			Check.That(sut.AddEdge(Vertex1, Vertex2)).IsFalse();
		}

		[Fact]
		public void TestGetNeighboursOfNonExistingVertex()
		{
			const int Vertex = 0;

			var sut = new DirectedGraph<int>();

			Check.ThatCode(() => sut.GetNeighbours(Vertex)).Throws<ArgumentException>();
		}

		public static IEnumerable<object[]> GetTestGetNeighboursData() => new List<object[]>
		{
			new object[] { new[] { 1 }, new[] { new[] { 1, 1 } }, new[] { 1 } },
			new object[] { new[] { 1, 2 }, new[] { new[] { 2, 1 } }, new int[0] },
			new object[] { new[] { 1, 2 }, new[] { new[] { 1, 2 } }, new[] { 2 } },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 2, 1 }, new[] { 2, 3 } }, new int[0] },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 2, 3 } }, new[] { 2 } },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 1, 3 } }, new[] { 2, 3 } },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 1 }, new[] { 1, 2 }, new[] { 1, 3 } }, new[] { 1, 2, 3 } }
		};

		[Theory]
		[MemberData(nameof(GetTestGetNeighboursData))]
		public void TestGetNeighbours(int[] vertices, int[][] edges, int[] neighbours)
		{
			var sut = new DirectedGraph<int>();

			sut.AddVertices(vertices);

			foreach (var edge in edges)
			{
				sut.AddEdge(edge[0], edge[1]);
			}

			Check.That(sut.GetNeighbours(vertices[0])).IsEquivalentTo(neighbours);
		}

		public static IEnumerable<object[]> GetTestContainsCycleData() => new List<object[]>
		{
			new object[] { new[] { 1 }, new[] { new[] { 1, 1 } }, true },
			new object[] { new[] { 1, 2 }, new[] { new[] { 1, 2 } }, false },
			new object[] { new[] { 1, 2 }, new[] { new[] { 1, 2 }, new[] { 2, 1 } }, true },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 2, 3 } }, false },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 } }, false },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 2, 3 }, new[] { 3, 1 } }, true },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 2, 3 }, new[] { 3, 2 } }, true },
			new object[] { new[] { 1, 2, 3 }, new[] { new[] { 1, 2 }, new[] { 2, 1 }, new[] { 2, 3 }, new[] { 3, 2 } }, true }
		};

		[Theory]
		[MemberData(nameof(GetTestContainsCycleData))]
		public void TestContainsCycle(int[] vertices, int[][] edges, bool containsCycle)
		{
			var sut = new DirectedGraph<int>();

			sut.AddVertices(vertices);

			foreach (var edge in edges)
			{
				sut.AddEdge(edge[0], edge[1]);
			}

			Check.That(sut.ContainsCycle()).IsEqualTo(containsCycle);
		}
	}
}
