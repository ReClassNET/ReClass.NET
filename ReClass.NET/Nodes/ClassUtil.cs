using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class ClassUtil
	{
		/// <summary>
		/// Tests if the class to check can be inserted into the parent class without creating a cycle.
		/// </summary>
		/// <param name="parent">The class into which </param>
		/// <param name="classToCheck">The class which should get inserted.</param>
		/// <param name="classes">An enumeration of all available classes.</param>
		/// <returns>True if a cycle is detected, false otherwise.</returns>
		public static bool IsCyclicIfClassIsAccessibleFromParent(ClassNode parent, ClassNode classToCheck, IEnumerable<ClassNode> classes)
		{
			Contract.Requires(parent != null);
			Contract.Requires(classToCheck != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));

			var graph = new DirectedGraph<ClassNode>();
			graph.AddVertices(classes);

			graph.AddEdge(parent, classToCheck);

			foreach (var c in graph.Vertices)
			{
				foreach (var wrapperNode in c.Nodes.OfType<BaseWrapperNode>())
				{
					if (TryResolveWrappedClassNode(wrapperNode, out var classNode))
					{
						graph.AddEdge(c, classNode);
					}
				}
			}

			return graph.ContainsCycle();
		}

		/// <summary>
		/// Walks the inner nodes of chained <see cref="BaseWrapperNode"/>s until a <see cref="ClassNode"/> is found or
		/// the <see cref="BaseWrapperNode.PerformCycleCheck"/> property of an inner node is false.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="classNode"></param>
		/// <returns>True if the wrapper chain ends in a <see cref="ClassNode"/>, false otherwise.</returns>
		private static bool TryResolveWrappedClassNode(BaseWrapperNode node, out ClassNode classNode)
		{
			Contract.Requires(node != null);

			classNode = null;

			while (true)
			{
				if (!node.PerformCycleCheck)
				{
					return false;
				}

				if (node.InnerNode is BaseWrapperNode innerWrapperNode)
				{
					node = innerWrapperNode;
				}
				else if (node.InnerNode is ClassNode wrappedClassNode)
				{
					classNode = wrappedClassNode;

					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}
}
