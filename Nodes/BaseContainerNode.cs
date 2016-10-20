using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	public abstract class BaseContainerNode : BaseNode
	{
		protected readonly List<BaseNode> nodes = new List<BaseNode>();
		public IEnumerable<BaseNode> Nodes => nodes;

		public void UpdateOffsets()
		{
			var offset = IntPtr.Zero;
			foreach (var node in Nodes)
			{
				node.Offset = offset;
				offset += node.MemorySize;
			}
		}

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

			OnPropertyChanged(nameof(Nodes));

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

		public void AddBytes(int size)
		{
			InsertBytes(nodes.Count, size);
		}

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

		public virtual bool RemoveNode(BaseNode node)
		{
			Contract.Requires(node != null);

			return nodes.Remove(node);
		}
	}
}
