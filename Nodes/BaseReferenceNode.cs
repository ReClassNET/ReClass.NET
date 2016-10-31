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

		/// <summary>Constructor.</summary>
		/// <param name="performCycleCheck">True to perform class cycle checks when changing the inner node.</param>
		public BaseReferenceNode(bool performCycleCheck)
		{
			this.performCycleCheck = performCycleCheck;
		}

		/// <summary>Changes the inner node.</summary>
		/// <param name="node">The new node.</param>
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

				ParentNode?.ChildHasChanged(this);
			}
		}

		/// <summary>Performs a class cycle check for the given class.</summary>
		/// <exception cref="ClassCycleException">Thrown when a class cycle is present.</exception>
		/// <param name="node">The class to check.</param>
		private void PerformCycleCheck(ClassNode node)
		{
			Contract.Requires(node != null);

			if (!ClassManager.IsCycleFree(ParentNode as ClassNode, node))
			{
				throw new ClassCycleException();
			}
		}
	}


	/// <summary>Exception for signalling class cycle errors.</summary>
	public class ClassCycleException : Exception
	{

	}
}
