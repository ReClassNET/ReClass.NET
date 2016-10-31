using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Nodes;

namespace ReClassNET.Util
{
	public class ClassManager
	{
		public delegate void ClassesChangedEvent(ClassNode sender);
		public static event ClassesChangedEvent ClassAdded;
		public static event ClassesChangedEvent ClassRemoved;

		private static readonly List<ClassNode> classes = new List<ClassNode>();

		public static IEnumerable<ClassNode> Classes => classes;

		public static ClassNode CreateClass()
		{
			var node = new ClassNode();
			AddClass(node);
			return node;
		}

		internal static void AddClass(ClassNode node)
		{
			Contract.Requires(node != null);

			if (!classes.Contains(node))
			{
				node.NodesChanged += ClassHasChanged;

				classes.Add(node);

				ClassAdded?.Invoke(node);
			}
		}

		private static void ClassHasChanged(BaseNode sender)
		{
			classes.ForEach(c => c.UpdateOffsets());
		}

		public static void Clear()
		{
			var temp = classes.ToList();

			classes.Clear();

			foreach (var node in temp)
			{
				ClassRemoved?.Invoke(node);
			}
		}

		private static IEnumerable<ClassNode> GetClassReferences(ClassNode node)
		{
			Contract.Requires(node != null);

			return classes.Where(c => c != node).Where(c => c.Descendants().Where(n => (n as BaseReferenceNode)?.InnerNode == node).Any());
		}

		public static void Remove(ClassNode node)
		{
			Contract.Requires(node != null);

			var references = GetClassReferences(node).ToList();
			if (references.Any())
			{
				throw new ClassReferencedException(references);
			}

			if (classes.Remove(node))
			{
				ClassRemoved?.Invoke(node);
			}
		}

		public static void RemoveUnusedClasses()
		{
			var toRemove = classes
				.Except(classes.Where(x => GetClassReferences(x).Any())) // check for references
				.Where(c => c.Nodes.All(n => n is BaseHexNode)) // check if only hex nodes are present
				.ToList();
			foreach (var node in toRemove)
			{
				if (classes.Remove(node))
				{
					ClassRemoved?.Invoke(node);
				}
			}
		}

		public static bool IsCycleFree(ClassNode parent, ClassNode check)
		{
			Contract.Requires(parent != null);
			Contract.Requires(check != null);

			return IsCycleFree(parent, check, classes);
		}

		public static bool IsCycleFree(ClassNode parent, ClassNode check, IEnumerable<ClassNode> classes)
		{
			Contract.Requires(parent != null);
			Contract.Requires(check != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));

			var toCheck = new HashSet<ClassNode>(
				check.Yield()
				.Traverse(
					c => c.Nodes
					.Where(n => n is ClassInstanceNode || n is ClassInstanceArrayNode)
					.Select(n => ((BaseReferenceNode)n).InnerNode as ClassNode)
				)
			);

			if (!IsCycleFreeUp(parent, toCheck, classes))
			{
				return false;
			}

			return true;
		}

		private static bool IsCycleFreeUp(ClassNode root, HashSet<ClassNode> seen, IEnumerable<ClassNode> classes)
		{
			Contract.Requires(root != null);
			Contract.Requires(seen != null);
			Contract.Requires(Contract.ForAll(seen, c => c != null));
			Contract.Requires(Contract.ForAll(classes, c => c != null));

			if (!seen.Add(root))
			{
				return false;
			}

			foreach (var cls in classes/*.Except(seen)*/)
			{
				if (cls.Nodes
					.OfType<BaseReferenceNode>()
					.Where(n => n is ClassInstanceNode || n is ClassInstanceArrayNode)
					.Where(n => n.InnerNode == root)
					.Any())
				{
					if (!IsCycleFreeUp(cls, seen, classes))
					{
						return false;
					}
				}
			}

			return true;
		}
	}

	public class ClassReferencedException : Exception
	{
		public IEnumerable<ClassNode> References { get; }

		public ClassReferencedException(IEnumerable<ClassNode> references)
			: base("This class has references.")
		{
			Contract.Requires(references != null);
			Contract.Requires(Contract.ForAll(references, c => c != null));

			References = references;
		}
	}
}
