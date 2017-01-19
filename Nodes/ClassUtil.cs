using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class ClassUtil
	{
		internal static IEnumerable<ClassNode> Classes;

		public static bool IsCycleFree(ClassNode parent, ClassNode check)
		{
			Contract.Requires(parent != null);
			Contract.Requires(check != null);

			if (Classes == null)
			{
				return true;
			}

			return IsCycleFree(parent, check, Classes);
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
					.Select(n => ((BaseReferenceNode)n).InnerNode)
				)
			);

			return IsCycleFree(parent, toCheck, classes);
		}

		private static bool IsCycleFree(ClassNode root, HashSet<ClassNode> seen, IEnumerable<ClassNode> classes)
		{
			Contract.Requires(root != null);
			Contract.Requires(seen != null);
			Contract.Requires(Contract.ForAll(seen, c => c != null));
			Contract.Requires(Contract.ForAll(classes, c => c != null));

			if (!seen.Add(root))
			{
				return false;
			}

			var classNodes = classes as IList<ClassNode> ?? classes.ToList();
			foreach (var cls in classNodes/*.Except(seen)*/)
			{
				if (cls.Nodes
					.OfType<BaseReferenceNode>()
					.Where(n => n is ClassInstanceNode || n is ClassInstanceArrayNode)
					.Any(n => n.InnerNode == root))
				{
					if (!IsCycleFree(cls, seen, classNodes))
					{
						return false;
					}
				}
			}

			return true;
		}
	}
}
