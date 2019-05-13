using System;

namespace ReClassNET.Nodes
{
	public abstract class BaseWrapperNode : BaseNode
	{
		/// <summary>Gets or sets the inner node.</summary>
		public BaseNode InnerNode { get; private set; }

		/// <summary>Gets signaled if the inner node was changed.</summary>
		public event NodeEventHandler InnerNodeChanged;

		/// <summary>True to perform class cycle checks when changing the inner node.</summary>
		protected abstract bool PerformCycleCheck { get; }

		/// <summary>
		/// Should be called before <see cref="ChangeInnerNode"/> to test if the node can handle the inner node type.
		/// </summary>
		/// <param name="node">The new inner node type.</param>
		/// <returns>True if the class can handle the inner node type or false otherwise.</returns>
		public abstract bool CanChangeInnerNodeTo(BaseNode node);

		/// <summary>Changes the inner node.</summary>
		/// <param name="node">The new node.</param>
		public void ChangeInnerNode(BaseNode node)
		{
			if (!CanChangeInnerNodeTo(node))
			{
				throw new InvalidOperationException($"Can't change inner node to '{node?.GetType().ToString() ?? "null"}'");
			}

			if (InnerNode != node)
			{
				InnerNode = node;

				if (node != null)
				{
					node.ParentNode = this;
				}

				InnerNodeChanged?.Invoke(this);

				GetParentContainer()?.ChildHasChanged(this);
			}
		}

		/// <summary>
		/// Resolve the most inner node of a <see cref="BaseWrapperNode"/> chain.
		/// </summary>
		/// <returns>The most inner node or null.</returns>
		public BaseNode ResolveMostInnerNode()
		{
			if (InnerNode == null)
			{
				return null;
			}
			if (InnerNode is BaseWrapperNode baseWrapperNode)
			{
				return baseWrapperNode.ResolveMostInnerNode();
			}
			return InnerNode;
		}

		/// <summary>
		/// Tests if the cycle check is really needed in a <see cref="BaseWrapperNode"/> chain.
		/// </summary>
		/// <returns></returns>
		public bool ShouldPerformCycleCheckForInnerNode()
		{
			// TODO Should there be a "is ClassNode" for the last inner node?

			if (!PerformCycleCheck)
			{
				return false;
			}

			var wrapperNode = this;
			while (wrapperNode.InnerNode is BaseWrapperNode wrappedNode)
			{
				if (!wrappedNode.PerformCycleCheck)
				{
					return false;
				}

				wrapperNode = wrappedNode;
			}

			return true;
		}

		/// <summary>
		/// Tests if the given node type is present in the chain of wrapped nodes.
		/// </summary>
		/// <typeparam name="TNode">The node type to check.</typeparam>
		/// <returns>True if the given node type is present in the chain of wrapped nodes, false otherwise.</returns>
		public bool IsNodePresentInChain<TNode>() where TNode : BaseNode
		{
			BaseNode node = this;
			while (node is BaseWrapperNode wrapperNode)
			{
				if (node is TNode)
				{
					return true;
				}

				node = wrapperNode.InnerNode;
			}

			return false;
		}
	}
}
