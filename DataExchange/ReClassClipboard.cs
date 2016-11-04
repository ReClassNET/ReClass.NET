using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange
{
	public class ReClassClipboard
	{
		private const string Format = "ReClass.NET SchemaNodes";

		public static bool ContainsData => Clipboard.ContainsData(Format);

		public static void Copy(IEnumerable<BaseNode> nodes, IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(nodes != null);
			Contract.Requires(classes != null);
			Contract.Requires(logger != null);

			if (!nodes.Any())
			{
				return;
			}

			var classMap = classes.ToDictionary(
				c => c,
				c => new SchemaClassNode { Name = c.Name }
			);

			var schemaNodes = new List<SchemaNode>();
			foreach (var node in nodes)
			{
				SchemaNode schemaNode;
				if (SchemaBuilder.TryCreateSchemaFromNode(node, classMap, logger, out schemaNode))
				{
					schemaNodes.Add(schemaNode);
				}
			}

			Clipboard.Clear();
			Clipboard.SetData(Format, schemaNodes);
		}

		public static List<BaseNode> Paste(ClassNode parentNode, IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(parentNode != null);
			Contract.Requires(classes != null);
			Contract.Requires(logger != null);
			Contract.Requires(Contract.Result<List<BaseNode>>() != null);

			var nodes = new List<BaseNode>();

			if (ContainsData)
			{
				var schemaNodes = Clipboard.GetData(Format) as List<SchemaNode>;
				if (schemaNodes != null)
				{
					// Now remove all reference types with unknown references.
					schemaNodes.RemoveAll(n => n is SchemaReferenceNode && !classes.Any(c => c.Name == ((SchemaReferenceNode)n).InnerNode?.Name));

					var classMap = schemaNodes.OfType<SchemaReferenceNode>().ToDictionary(
						srn => srn.InnerNode,
						srn => classes.First(c => c.Name == srn.InnerNode.Name)
					);

					foreach (var schemaNode in schemaNodes)
					{
						BaseNode node;
						if (SchemaBuilder.TryCreateNodeFromSchema(schemaNode, parentNode, classMap, logger, out node))
						{
							nodes.Add(node);
						}
					}
				}
			}

			return nodes;
		}
	}
}
