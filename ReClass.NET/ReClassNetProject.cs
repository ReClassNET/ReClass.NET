using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET
{
	public class ReClassNetProject : IDisposable
	{
		public delegate void ClassesChangedEvent(ClassNode sender);
		public event ClassesChangedEvent ClassAdded;
		public event ClassesChangedEvent ClassRemoved;

		private readonly List<ClassNode> classes = new List<ClassNode>();

		public IEnumerable<ClassNode> Classes => classes;

		public string Path { get; set; }

		public void Dispose()
		{
			Clear();

			ClassAdded = null;
			ClassRemoved = null;
		}

		public void AddClass(ClassNode node)
		{
			Contract.Requires(node != null);

			classes.Add(node);

			node.NodesChanged += NodesChanged_Handler;

			ClassAdded?.Invoke(node);
		}

		public bool ContainsClass(NodeUuid uuid)
		{
			Contract.Requires(uuid != null);

			return classes.Any(c => c.Uuid.Equals(uuid));
		}

		public ClassNode GetClassByUuid(NodeUuid uuid)
		{
			Contract.Requires(uuid != null);

			return classes.First(c => c.Uuid.Equals(uuid));
		}

		private void NodesChanged_Handler(BaseNode sender)
		{
			classes.ForEach(c => c.UpdateOffsets());
		}

		public void Clear()
		{
			var temp = classes.ToList();

			classes.Clear();

			foreach (var node in temp)
			{
				node.NodesChanged -= NodesChanged_Handler;

				ClassRemoved?.Invoke(node);
			}
		}

		private IEnumerable<ClassNode> GetClassReferences(ClassNode node)
		{
			Contract.Requires(node != null);

			return classes.Where(c => c != node).Where(c => c.Descendants().Any(n => (n as BaseReferenceNode)?.InnerNode == node));
		}

		public void Remove(ClassNode node)
		{
			Contract.Requires(node != null);

			var references = GetClassReferences(node).ToList();
			if (references.Any())
			{
				throw new ClassReferencedException(references);
			}

			if (classes.Remove(node))
			{
				node.NodesChanged -= NodesChanged_Handler;

				ClassRemoved?.Invoke(node);
			}
		}

		public void RemoveUnusedClasses()
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
