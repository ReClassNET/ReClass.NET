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
		private const string FormatClasses = "ReClass.NET::Classes";
		private const string FormatNodes = "ReClass.NET::Nodes";

		public enum Format
		{
			Classes,
			Nodes
		}

		public static bool ContainsData(Format format) => Clipboard.ContainsData(format == Format.Classes ? FormatClasses : FormatNodes);

		public static void CopyClasses(IEnumerable<ClassNode> classesToCopy, IEnumerable<ClassNode> globalClasses, ILogger logger)
		{
			Contract.Requires(classesToCopy != null);
			Contract.Requires(globalClasses != null);
			Contract.Requires(logger != null);

			if (!classesToCopy.Any())
			{
				return;
			}

			/*var schema = SchemaBuilder.FromClasses(classesToCopy, logger);

			Clipboard.Clear();
			Clipboard.SetData(FormatClasses, schema.BuildSchema());*/
		}

		public static List<ClassNode> PasteClasses(IEnumerable<ClassNode> globalClasses, ILogger logger)
		{
			Contract.Requires(globalClasses != null);
			Contract.Requires(logger != null);

			var nodes = new List<ClassNode>();

			if (ContainsData(Format.Classes))
			{
				/*var schemaClassNodes = Clipboard.GetData(FormatNodes) as List<SchemaClassNode>;
				if (schemaClassNodes != null)
				{
					foreach (var schemaClass in schemaClassNodes)
					{
						// Rename classes which already exist.
						var name = schemaClass.Name;
						for (var i = 1; globalClasses.Any(c => c.Name == schemaClass.Name); ++i)
						{
							schemaClass.Name = $"{name}_{i}";
						}

						// Remove all reference types with unknown references.
						schemaClass.Nodes.RemoveAll(n => n is SchemaReferenceNode && !globalClasses.Any(c => c.Name == ((SchemaReferenceNode)n).InnerNode?.Name));
					}

					// Now remove all reference types with unknown references.

					//schemaClassNodes.RemoveAll(n => n is SchemaReferenceNode && !globalClasses.Any(c => c.Name == ((SchemaReferenceNode)n).InnerNode?.Name));

					var classMap = schemaClassNodes.OfType<SchemaReferenceNode>().ToDictionary(
						srn => srn.InnerNode,
						srn => classes.First(c => c.Name == srn.InnerNode.Name)
					);

					foreach (var schemaNode in schemaClassNodes)
					{
						BaseNode node;
						if (SchemaBuilder.TryCreateNodeFromSchema(schemaNode, parentNode, classMap, logger, out node))
						{
							nodes.Add(node);
						}
					}
				}*/
			}

			return nodes;
		}

		public static void CopyNodes(IEnumerable<BaseNode> nodes, IEnumerable<ClassNode> globalClasses, ILogger logger)
		{
			Contract.Requires(nodes != null);
			Contract.Requires(globalClasses != null);
			Contract.Requires(logger != null);

			if (!nodes.Any())
			{
				return;
			}

			/*var classMap = globalClasses.ToDictionary(
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
			Clipboard.SetData(FormatNodes, schemaNodes);*/
		}

		public static List<BaseNode> PasteNodes(ClassNode parentNode, IEnumerable<ClassNode> globalClasses, ILogger logger)
		{
			Contract.Requires(parentNode != null);
			Contract.Requires(globalClasses != null);
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<List<BaseNode>>() != null);

			var nodes = new List<BaseNode>();

			if (ContainsData(Format.Nodes))
			{
				/*var schemaNodes = Clipboard.GetData(FormatNodes) as List<SchemaNode>;
				if (schemaNodes != null)
				{
					// Now remove all reference types with unknown references.
					schemaNodes.RemoveAll(n => n is SchemaReferenceNode && !globalClasses.Any(c => c.Name == ((SchemaReferenceNode)n).InnerNode?.Name));

					var classMap = schemaNodes.OfType<SchemaReferenceNode>().ToDictionary(
						srn => srn.InnerNode,
						srn => globalClasses.First(c => c.Name == srn.InnerNode.Name)
					);

					foreach (var schemaNode in schemaNodes)
					{
						BaseNode node;
						if (SchemaBuilder.TryCreateNodeFromSchema(schemaNode, parentNode, classMap, logger, out node))
						{
							nodes.Add(node);
						}
					}
				}*/
			}

			return nodes;
		}
	}
}
