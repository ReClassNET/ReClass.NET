using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	public abstract class BaseContainerNode : BaseNode
	{
		private readonly List<BaseNode> nodes = new List<BaseNode>();

		/// <summary>The child nodes of the container.</summary>
		public IReadOnlyList<BaseNode> Nodes => nodes;

		/// <summary>
		/// If true and the size of replaced nodes differs, the gap will be padded with default nodes (see <see cref="CreateDefaultNodeForSize"/>).
		/// </summary>
		protected abstract bool ShouldCompensateSizeChanges { get; }

		/// <summary>
		/// Should be called before adding a child to test if the container can handle the node type.
		/// </summary>
		/// <param name="node">The new child node.</param>
		/// <returns>True if the container can handle the child node or false otherwise.</returns>
		public abstract bool CanHandleChildNode(BaseNode node);

		private void CheckCanHandleChildNode(BaseNode node)
		{
			if (!CanHandleChildNode(node))
			{
				throw new ArgumentException();
			}
		}

		public override void ClearSelection()
		{
			base.ClearSelection();

			foreach (var node in Nodes)
			{
				node.ClearSelection();
			}
		}

		/// <summary>Calculates the offset of every child node.</summary>
		public void UpdateOffsets()
		{
			var offset = IntPtr.Zero;
			foreach (var node in Nodes)
			{
				node.Offset = offset;
				offset += node.MemorySize;
			}
		}

		/// <summary>Searches for the node and returns the zero based index.</summary>
		/// <param name="node">The node to search.</param>
		/// <returns>The found node index or -1 if the node was not found.</returns>
		public int FindNodeIndex(BaseNode node)
		{
			Contract.Requires(node != null);
			Contract.Ensures(Contract.Result<int>() >= -1 && Contract.Result<int>() < nodes.Count);

			return nodes.FindIndex(n => n == node);
		}

		/// <summary>
		/// Checks if the node exists in the container.
		/// </summary>
		/// <param name="node">The node to search.</param>
		/// <returns>True if the node exists in the container, false otherwise.</returns>
		public bool ContainsNode(BaseNode node)
		{
			return FindNodeIndex(node) != -1;
		}

		/// <summary>
		/// Tries to get the predecessor of the given node in the container.
		/// </summary>
		/// <param name="node">The root node.</param>
		/// <param name="predecessor">The predecessor of the given node.</param>
		/// <returns>True if a predecessor exists, otherwise false.</returns>
		public bool TryGetPredecessor(BaseNode node, out BaseNode predecessor)
		{
			Contract.Requires(node != null);

			return TryGetNeighbour(node, -1, out predecessor);
		}

		/// <summary>
		/// Tries to get the successor of the given node in the container.
		/// </summary>
		/// <param name="node">The root node.</param>
		/// <param name="successor">The successor of the given node.</param>
		/// <returns>True if a successor exists, otherwise false.</returns>
		public bool TryGetSuccessor(BaseNode node, out BaseNode successor)
		{
			Contract.Requires(node != null);

			return TryGetNeighbour(node, 1, out successor);
		}

		private bool TryGetNeighbour(BaseNode node, int offset, out BaseNode neighbour)
		{
			Contract.Requires(node != null);

			neighbour = null;

			var index = FindNodeIndex(node);
			if (index == -1)
			{
				return false;
			}

			var neighbourIndex = index + offset;
			if (neighbourIndex < 0 || neighbourIndex >= nodes.Count)
			{
				return false;
			}

			neighbour = nodes[neighbourIndex];

			return true;
		}

		/// <summary>Replaces the old node with the new node.</summary>
		/// <param name="oldNode">The old node to replacce.</param>
		/// <param name="newNode">The new node.</param>
		public void ReplaceChildNode(BaseNode oldNode, BaseNode newNode)
		{
			Contract.Requires(oldNode != null);
			Contract.Requires(newNode != null);

			List<BaseNode> dummy = null;
			ReplaceChildNode(oldNode, newNode, ref dummy);
		}

		/// <summary>Replaces the old node with the new node.</summary>
		/// <param name="oldNode">The old node to replacce.</param>
		/// <param name="newNode">The new node.</param>
		/// <param name="insertedNodes">[out] A list with additional created nodes (see <see cref="ShouldCompensateSizeChanges"/>).</param>
		public void ReplaceChildNode(BaseNode oldNode, BaseNode newNode, ref List<BaseNode> insertedNodes)
		{
			Contract.Requires(oldNode != null);
			Contract.Requires(newNode != null);

			CheckCanHandleChildNode(newNode);

			var index = FindNodeIndex(oldNode);
			if (index == -1)
			{
				throw new ArgumentException($"Node {oldNode} is not a child of {this}.");
			}

			newNode.CopyFromNode(oldNode);

			newNode.ParentNode = this;

			nodes[index] = newNode;

			if (ShouldCompensateSizeChanges)
			{
				insertedNodes?.Add(newNode);

				var oldSize = oldNode.MemorySize;
				var newSize = newNode.MemorySize;

				if (newSize < oldSize)
				{
					InsertBytes(index + 1, oldSize - newSize, ref insertedNodes);
				}
				/*else if (newSize > oldSize)
				{
					RemoveNodes(index + 1, newSize - oldSize);
				}*/
			}
		}

		/// <summary>
		/// Creates the default container node which takes up to <paramref name="size"/> bytes.
		/// </summary>
		/// <param name="size">The maximum size in bytes.</param>
		/// <returns>A new node or null if no node is available for this size.</returns>
		protected virtual BaseNode CreateDefaultNodeForSize(int size)
		{
			Contract.Requires(size > 0);

#if RECLASSNET64
			if (size >= 8)
			{
				return new Hex64Node();
			}
#endif
			if (size >= 4)
			{
				return new Hex32Node();
			}
			if (size >= 2)
			{
				return new Hex16Node();
			}

			return new Hex8Node();
		}

		/// <summary>Adds the specific amount of bytes at the end of the node.</summary>
		/// <param name="size">The number of bytes to insert.</param>
		public void AddBytes(int size)
		{
			List<BaseNode> dummy = null;
			InsertBytes(nodes.Count, size, ref dummy);
		}

		public void InsertBytes(BaseNode position, int size)
		{
			List<BaseNode> dummy = null;
			InsertBytes(FindNodeIndex(position), size, ref dummy);
		}

		/// <summary>Inserts <paramref name="size"/> bytes at the specified position.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="size">The number of bytes to insert.</param>
		/// <param name="createdNodes">[out] A list with the created nodes.</param>
		protected void InsertBytes(int index, int size, ref List<BaseNode> createdNodes)
		{
			if (index < 0 || index > nodes.Count)
			{
				throw new ArgumentOutOfRangeException($"The index {index} is not in the range [0, {nodes.Count}].");
			}

			if (size == 0)
			{
				return;
			}

			var offset = IntPtr.Zero;
			if (index > 0)
			{
				var node = nodes[index - 1];
				offset = node.Offset + node.MemorySize;
			}

			while (size > 0)
			{
				var node = CreateDefaultNodeForSize(size);
				if (node == null)
				{
					break;
				}

				node.ParentNode = this;
				node.Offset = offset;

				nodes.Insert(index, node);

				createdNodes?.Add(node);

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}

			UpdateOffsets();

			GetParentContainer()?.ChildHasChanged(this);
		}

		/// <summary>
		/// Adds all nodes at the end of the container.
		/// </summary>
		/// <param name="nodes">The nodes to add.</param>
		public void AddNodes(IEnumerable<BaseNode> nodes)
		{
			Contract.Requires(nodes != null);

			foreach (var node in nodes)
			{
				AddNode(node);
			}
		}

		/// <summary>
		/// Adds the node at the end of the container.
		/// </summary>
		/// <param name="node">The node to add.</param>
		public void AddNode(BaseNode node)
		{
			Contract.Requires(node != null);

			CheckCanHandleChildNode(node);

			node.ParentNode = this;

			nodes.Add(node);

			UpdateOffsets();

			GetParentContainer()?.ChildHasChanged(this);
		}

		/// <summary>
		/// Inserts the node infront of the <paramref name="position"/> node.
		/// </summary>
		/// <param name="position">The target node.</param>
		/// <param name="node">The node to insert.</param>
		public void InsertNode(BaseNode position, BaseNode node)
		{
			Contract.Requires(node != null);

			CheckCanHandleChildNode(node);

			var index = FindNodeIndex(position);
			if (index == -1)
			{
				throw new ArgumentException();
			}

			node.ParentNode = this;

			nodes.Insert(index, node);

			UpdateOffsets();

			GetParentContainer()?.ChildHasChanged(this);
		}

		/// <summary>Removes the specified node.</summary>
		/// <param name="node">The node to remove.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public bool RemoveNode(BaseNode node)
		{
			Contract.Requires(node != null);

			var result = nodes.Remove(node);
			if (result)
			{
				UpdateOffsets();

				GetParentContainer()?.ChildHasChanged(this);
			}
			return result;
		}

		/// <summary>Called by a child if it has changed.</summary>
		/// <param name="child">The child.</param>
		protected internal virtual void ChildHasChanged(BaseNode child)
		{
			// TODO Add BaseNode.GetParentContainer
		}
	}
}
