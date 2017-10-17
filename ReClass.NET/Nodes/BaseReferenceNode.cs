using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	public abstract class BaseReferenceNode : BaseNode
	{
		/// <summary>Gets or sets the inner node.</summary>
		public ClassNode InnerNode { get; protected set; }

		/// <summary>Gets signaled if the inner node was changed.</summary>
		public event NodeEventHandler InnerNodeChanged;

		/// <summary>True to perform class cycle checks when changing the inner node.</summary>
		public abstract bool PerformCycleCheck { get; }

		/// <summary>Changes the inner node.</summary>
		/// <param name="node">The new node.</param>
		public void ChangeInnerNode(ClassNode node)
		{
			Contract.Requires(node != null);

			if (InnerNode != node)
			{
				InnerNode = node;

				InnerNodeChanged?.Invoke(this);

				ParentNode?.ChildHasChanged(this);
			}
		}
	}
}
