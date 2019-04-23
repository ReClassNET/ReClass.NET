using System;
using System.Collections.Generic;
using System.Linq;

namespace ReClassNET.Util
{
	public class DirectedGraph<T>
	{
		private readonly IDictionary<T, HashSet<T>> adjacencyList = new Dictionary<T, HashSet<T>>();

		/// <summary>
		/// Gets an enumeration of all vertices in the graph.
		/// </summary>
		public IEnumerable<T> Vertices => adjacencyList.Keys;

		/// <summary>
		/// Adds the vertex to the graph.
		/// </summary>
		/// <param name="vertex"></param>
		/// <returns></returns>
		public bool AddVertex(T vertex)
		{
			if (adjacencyList.ContainsKey(vertex))
			{
				return false;
			}

			adjacencyList.Add(vertex, new HashSet<T>());

			return true;
		}

		/// <summary>
		/// Adds the vertices to the graph.
		/// </summary>
		/// <param name="vertices"></param>
		public void AddVertices(IEnumerable<T> vertices)
		{
			foreach (var vertex in vertices)
			{
				AddVertex(vertex);
			}
		}

		/// <summary>
		/// Tests if the graph contains the given vertex.
		/// </summary>
		/// <param name="vertex"></param>
		/// <returns></returns>
		public bool ContainsVertex(T vertex)
		{
			return adjacencyList.ContainsKey(vertex);
		}

		/// <summary>
		/// Adds an edge between both vertices to the graph.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns>True if a new edge was added, false otherwise.</returns>
		public bool AddEdge(T from, T to)
		{
			if (!ContainsVertex(to) || !adjacencyList.TryGetValue(from, out var edges))
			{
				throw new ArgumentException("Vertex does not exist in graph.");
			}

			return edges.Add(to);
		}

		/// <summary>
		/// Tests if the graph contains an edge between both vertices.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public bool ContainsEdge(T from, T to)
		{
			if (!ContainsVertex(to) || !adjacencyList.TryGetValue(from, out var edges))
			{
				throw new ArgumentException("Vertex does not exist in graph.");
			}

			return edges.Contains(to);
		}

		/// <summary>
		/// Gets all neighbours of the given vertex.
		/// </summary>
		/// <param name="vertex">The vertex to check.</param>
		/// <returns>An enumeration of all neighbours of the given vertex.</returns>
		public IEnumerable<T> GetNeighbours(T vertex)
		{
			if (!adjacencyList.TryGetValue(vertex, out var edges))
			{
				throw new ArgumentException("Vertex does not exist in graph.");
			}

			return edges;
		}

		/// <summary>
		/// Tests with a depth first search if the graph contains a cycle.
		/// </summary>
		/// <returns>True if a cycle exists, false otherwise.</returns>
		public bool ContainsCycle()
		{
			var visited = new HashSet<T>();
			var recursionStack = new HashSet<T>();

			bool IsCyclic(T source)
			{
				if (visited.Add(source))
				{
					recursionStack.Add(source);

					foreach (var adjacent in GetNeighbours(source))
					{
						if (!visited.Contains(adjacent) && IsCyclic(adjacent))
						{
							return true;
						}

						if (recursionStack.Contains(adjacent))
						{
							return true;
						}
					}
				}

				recursionStack.Remove(source);

				return false;
			}

			return adjacencyList.Keys.Any(IsCyclic);
		}
	}
}
