using System;
using System.Diagnostics.Contracts;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseReferenceNode : BaseNode
	{
		private readonly bool performCycleCheck;

		/// <summary>Gets or sets the inner node.</summary>
		public ClassNode InnerNode { get; protected set; }

		public event NodeEventHandler InnerNodeChanged;

		public BaseReferenceNode(bool performCycleCheck)
		{
			this.performCycleCheck = performCycleCheck;
		}

		public void ChangeInnerNode(ClassNode node)
		{
			Contract.Requires(node != null);

			if (InnerNode != node)
			{
				if (performCycleCheck)
				{
					PerformCycleCheck(node);
				}

				InnerNode = node;

				InnerNodeChanged?.Invoke(this);

				ParentNode.ChildHasChanged(this);
			}
		}

		private void PerformCycleCheck(ClassNode node)
		{
			Contract.Requires(node != null);

			if (!ClassManager.IsCycleFree(ParentNode as ClassNode, node))
			{
				throw new ClassCycleException();
			}
		}
	}

	public class ClassCycleException : Exception
	{

	}
}
