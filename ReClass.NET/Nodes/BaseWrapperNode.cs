namespace ReClassNET.Nodes
{
	public abstract class BaseWrapperNode : BaseNode
	{
		/// <summary>Gets or sets the inner node.</summary>
		public BaseNode InnerNode { get; protected set; }

		/// <summary>Gets signaled if the inner node was changed.</summary>
		public event NodeEventHandler InnerNodeChanged;

		/// <summary>True to perform class cycle checks when changing the inner node.</summary>
		public abstract bool PerformCycleCheck { get; }

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
			if (InnerNode != node)
			{
				InnerNode = node;

				InnerNodeChanged?.Invoke(this);

				ParentNode?.ChildHasChanged(this);
			}
		}
	}
}
