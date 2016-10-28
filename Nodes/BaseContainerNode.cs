using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	public abstract class BaseContainerNode : BaseNode
	{
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

		/// <summary>Replaces the child at the specific position with the provided node.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="node">The node to add.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public virtual bool ReplaceChildNode(int index, BaseNode node)
		{
			if (node == null)
			{
				return false;
			}
			if (index < 0 || index >= nodes.Count)
			{
				return false;
			}

			var oldNode = nodes[index];

			node.CopyFromNode(oldNode);

			node.ParentNode = this;
			node.ClearSelection();

			nodes[index] = node;

			var oldSize = oldNode.MemorySize;
			var newSize = node.MemorySize;

			if (newSize < oldSize)
			{
				InsertBytes(index + 1, oldSize - newSize);
			}
			else if (newSize > oldSize)
			{
				//RemoveNodes(index + 1, newSize - oldSize);
			}

			return true;
		}

		/// <summary>Adds the specific amount of bytes at the end of the node.</summary>
		/// <param name="size">The number of bytes to insert.</param>
		public void AddBytes(int size)
		{
			InsertBytes(nodes.Count, size);
		}

		/// <summary>Inserts <paramref name="size"/> bytes at the specified position.</summary>
		/// <param name="index">Zero-based position.</param>
		/// <param name="size">The number of bytes to insert.</param>
		public virtual void InsertBytes(int index, int size)
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
				BaseNode node = null;
#if WIN64
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

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}
		}

		/// <summary>Removes the specified node.</summary>
		/// <param name="node">The node to remove.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		public virtual bool RemoveNode(BaseNode node)
		{
			Contract.Requires(node != null);

			return nodes.Remove(node);
		}
	}
}
