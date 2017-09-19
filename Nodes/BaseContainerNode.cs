using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseContainerNode : BaseNode
	{
		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(nodes != null);
		}

		protected readonly List<BaseNode> nodes = new List<BaseNode>();

		/// <summary>The child nodes of the node.</summary>
		public IEnumerable<BaseNode> Nodes => nodes;

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

		/// <summary>Searches for the node index.</summary>
		/// <param name="node">The node to search.</param>
		/// <returns>The found node index or -1 if the node was not found.</returns>
		public int FindNodeIndex(BaseNode node)
		{
			Contract.Requires(node != null);
			Contract.Ensures(Contract.Result<int>() >= -1);

			return Nodes.FindIndex(n => n == node);
		}

		public bool ReplaceChildNode(BaseNode child, Type nodeType)
		{
			Contract.Requires(nodeType != null);
			Contract.Requires(nodeType.IsSubclassOf(typeof(BaseNode)));

			List<BaseNode> dummy = null;
			return ReplaceChildNode(child, nodeType, ref dummy);
		}

		public bool ReplaceChildNode(BaseNode child, Type nodeType, ref List<BaseNode> createdNodes)
		{
			Contract.Requires(child != null);
			Contract.Requires(nodeType != null);
			Contract.Requires(nodeType.IsSubclassOf(typeof(BaseNode)));

			return ReplaceChildNode(FindNodeIndex(child), nodeType, ref createdNodes);
		}

		/// <summary>Replaces the child at the specific position with the provided node.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="nodeType">The type of the node.</param>
		/// <param name="createdNodes">[in,out] A list with the created nodes.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public virtual bool ReplaceChildNode(int index, Type nodeType, ref List<BaseNode> createdNodes)
		{
			Contract.Requires(nodeType != null);
			Contract.Requires(nodeType.IsSubclassOf(typeof(BaseNode)));

			if (index < 0 || index >= nodes.Count)
			{
				return false;
			}

			var oldNode = nodes[index];

			var node = Activator.CreateInstance(nodeType) as BaseNode;
			if (node == null)
			{
				return false;
			}

			node.Intialize();
			node.CopyFromNode(oldNode);

			createdNodes?.Add(node);

			node.ParentNode = this;

			nodes[index] = node;

			var oldSize = oldNode.MemorySize;
			var newSize = node.MemorySize;

			if (newSize < oldSize)
			{
				InsertBytes(index + 1, oldSize - newSize, ref createdNodes);
			}
			/*else if (newSize > oldSize)
			{
				RemoveNodes(index + 1, newSize - oldSize);
			}*/

			return true;
		}

		/// <summary>Adds the specific amount of bytes at the end of the node.</summary>
		/// <param name="size">The number of bytes to insert.</param>
		public void AddBytes(int size)
		{
			InsertBytes(nodes.Count, size);
		}

		public void InsertBytes(BaseNode position, int size)
		{
			InsertBytes(FindNodeIndex(position), size);
		}

		/// <summary>Inserts <paramref name="size"/> bytes at the specified position.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="size">The number of bytes to insert.</param>
		public void InsertBytes(int index, int size)
		{
			List<BaseNode> dummy = null;
			InsertBytes(index, size, ref dummy);
		}

		/// <summary>Inserts <paramref name="size"/> bytes at the specified position.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="size">The number of bytes to insert.</param>
		/// <param name="createdNodes">[in,out] A list with the created nodes.</param>
		public virtual void InsertBytes(int index, int size, ref List<BaseNode> createdNodes)
		{
			if (index < 0 || index > nodes.Count || size == 0)
			{
				return;
			}

			var offset = IntPtr.Zero;
			if (index > 0)
			{
				var node = nodes[index - 1];
				offset = node.Offset + node.MemorySize;
			}

			while (size != 0)
			{
				BaseNode node;
#if RECLASSNET64
				if (size >= 8)
				{
					node = new Hex64Node();
				}
				else 
#endif
				if (size >= 4)
				{
					node = new Hex32Node();
				}
				else if (size >= 2)
				{
					node = new Hex16Node();
				}
				else
				{
					node = new Hex8Node();
				}

				node.ParentNode = this;
				node.Offset = offset;

				nodes.Insert(index, node);

				createdNodes?.Add(node);

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}
		}

		public void AddNode(BaseNode node)
		{
			Contract.Requires(node != null);

			InsertNode(nodes.Count, node);
		}

		public virtual void InsertNode(BaseNode position, BaseNode node)
		{
			var index = FindNodeIndex(position);
			if (index == -1)
			{
				throw new ArgumentException();
			}

			InsertNode(index, node);
		}

		public virtual void InsertNode(int index, BaseNode node)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(node != null);

			node.ParentNode = this;

			nodes.Insert(index, node);
		}

		/// <summary>Removes the specified node.</summary>
		/// <param name="node">The node to remove.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public virtual bool RemoveNode(BaseNode node)
		{
			Contract.Requires(node != null);

			return nodes.Remove(node);
		}

		/// <summary>Called by a child if it has changed.</summary>
		/// <param name="child">The child.</param>
		protected internal virtual void ChildHasChanged(BaseNode child)
		{

		}
	}
}
