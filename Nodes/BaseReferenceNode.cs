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

		/// <summary>Gets signaled if the inner node was changed.</summary>
		public event NodeEventHandler InnerNodeChanged;

		/// <summary>True to perform class cycle checks when changing the inner node.</summary>
		public abstract bool PerformCycleCheck { get; }

		/// <summary>Changes the inner node.</summary>
		/// <exception cref="ClassCycleException">Thrown when a class cycle is present.</exception>
		/// <param name="node">The new node.</param>
		public void ChangeInnerNode(ClassNode node)
		{
			Contract.Requires(node != null);

			if (InnerNode != node)
			{
				if (PerformCycleCheck && ParentNode != null)
				{
					if (!ClassManager.IsCycleFree(ParentNode as ClassNode, node))
					{
						throw new ClassCycleException();
					}
				}

				InnerNode = node;

				InnerNodeChanged?.Invoke(this);

				ParentNode?.ChildHasChanged(this);
			}
		}
	}


	/// <summary>Exception for signaling class cycle errors.</summary>
	public class ClassCycleException : Exception
	{

	}
}
