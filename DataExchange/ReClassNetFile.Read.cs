using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.DataExchange
{
	public partial class ReClassNetFile
	{
		public void Load(string filePath, ILogger logger)
		{
			using (var fs = new FileStream(filePath, FileMode.Open))
			{
				Load(fs, logger);
			}
		}

		public void Load(Stream input, ILogger logger)
		{
			Contract.Requires(input != null);
			Contract.Requires(logger != null);

			using (var archive = new ZipArchive(input, ZipArchiveMode.Read))
			{
				var dataEntry = archive.GetEntry(DataFileName);
				if (dataEntry == null)
				{
					throw new FormatException();
				}
				using (var entryStream = dataEntry.Open())
				{
					var document = XDocument.Load(entryStream);

					var version = document.Root.Attribute(XmlVersionAttribute)?.Value;
					var platform = document.Root.Attribute(XmlTypeAttribute)?.Value;
					if (platform != Constants.Platform)
					{
						logger.Log(LogLevel.Warning, $"The platform of the file ({platform}) doesn't match the program platform ({Constants.Platform}).");
					}

					var classes = new List<Tuple<XElement, ClassNode>>();

					foreach (var element in document.Root
						.Element(XmlClassesElement)
						.Elements(XmlClassElement)
						.DistinctBy(e => e.Attribute(XmlUuidAttribute)?.Value))
					{
						var node = new ClassNode(false)
						{
							Uuid = NodeUuid.FromBase64String(element.Attribute(XmlUuidAttribute)?.Value, true),
							Name = element.Attribute(XmlNameAttribute)?.Value ?? string.Empty,
							Comment = element.Attribute(XmlCommentAttribute)?.Value ?? string.Empty,
							AddressFormula = element.Attribute(XmlAddressAttribute)?.Value ?? string.Empty
						};

						if (!project.ContainsClass(node.Uuid))
						{
							project.AddClass(node);

							classes.Add(Tuple.Create(element, node));
						}
					}

					foreach (var t in classes)
					{
						ReadNodeElements(
							t.Item1.Elements(XmlNodeElement),
							t.Item2,
							logger
						).ForEach(t.Item2.AddNode);
					}
				}
			}
		}

		private IEnumerable<BaseNode> ReadNodeElements(IEnumerable<XElement> elements, ClassNode parent, ILogger logger)
		{
			Contract.Requires(elements != null);
			Contract.Requires(Contract.ForAll(elements, e => e != null));
			Contract.Requires(parent != null);
			Contract.Requires(logger != null);

			foreach (var element in elements)
			{
				var converter = CustomNodeConvert.GetReadConverter(element);
				if (converter != null)
				{
					BaseNode customNode;
					if (converter.TryCreateNodeFromElement(element, parent, project.Classes, logger, out customNode))
					{
						yield return customNode;
					}

					continue;
				}

				Type nodeType;
				if (!BuildInStringToTypeMap.TryGetValue(element.Attribute(XmlTypeAttribute)?.Value, out nodeType))
				{
					logger.Log(LogLevel.Error, $"Skipping node with unknown type: {element.Attribute(XmlTypeAttribute)?.Value}");
					logger.Log(LogLevel.Warning, element.ToString());

					continue;
				}

				var node = Activator.CreateInstance(nodeType) as BaseNode;
				if (node == null)
				{
					logger.Log(LogLevel.Error, $"Could not create node of type: {nodeType}");

					continue;
				}

				node.Name = element.Attribute(XmlNameAttribute)?.Value ?? string.Empty;
				node.Comment = element.Attribute(XmlCommentAttribute)?.Value ?? string.Empty;

				var referenceNode = node as BaseReferenceNode;
				if (referenceNode != null)
				{
					var reference = NodeUuid.FromBase64String(element.Attribute(XmlReferenceAttribute)?.Value, false);
					if (!project.ContainsClass(reference))
					{
						logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {reference}");
						logger.Log(LogLevel.Warning, element.ToString());

						continue;
					}

					var innerClassNode = project.GetClassByUuid(reference);
					if (referenceNode.PerformCycleCheck && !ClassUtil.IsCycleFree(parent, innerClassNode, project.Classes))
					{
						logger.Log(LogLevel.Error, $"Skipping node with cycle reference: {parent.Name}->{node.Name}");

						continue;
					}

					referenceNode.ChangeInnerNode(innerClassNode);
				}
				var vtableNode = node as VTableNode;
				if (vtableNode != null)
				{
					element
						.Elements(XmlMethodElement)
						.Select(e => new VMethodNode
						{
							Name = e.Attribute(XmlNameAttribute)?.Value ?? string.Empty,
							Comment = e.Attribute(XmlCommentAttribute)?.Value ?? string.Empty
						})
						.ForEach(vtableNode.AddNode);
				}
				var arrayNode = node as BaseArrayNode;
				if (arrayNode != null)
				{
					int count;
					TryGetAttributeValue(element, XmlCountAttribute, out count, logger);
					arrayNode.Count = count;
				}
				var textNode = node as BaseTextNode;
				if (textNode != null)
				{
					int length;
					TryGetAttributeValue(element, XmlLengthAttribute, out length, logger);
					textNode.Length = length;
				}
				var bitFieldNode = node as BitFieldNode;
				if (bitFieldNode != null)
				{
					int bits;
					TryGetAttributeValue(element, XmlBitsAttribute, out bits, logger);
					bitFieldNode.Bits = bits;
				}
				var functionNode = node as FunctionNode;
				if (functionNode != null)
				{
					functionNode.Signature = element.Attribute(XmlSignatureAttribute)?.Value ?? string.Empty;

					var reference = NodeUuid.FromBase64String(element.Attribute(XmlReferenceAttribute)?.Value, false);
					if (project.ContainsClass(reference))
					{
						functionNode.BelongsToClass = project.GetClassByUuid(reference);
					}
				}

				yield return node;
			}
		}

		private static void TryGetAttributeValue(XElement element, string attribute, out int val, ILogger logger)
		{
			Contract.Requires(element != null);
			Contract.Requires(attribute != null);
			Contract.Requires(logger != null);

			if (!int.TryParse(element.Attribute(attribute)?.Value, out val))
			{
				val = 0;

				logger.Log(LogLevel.Error, $"Node is missing a valid '{attribute}' attribute, defaulting to 0.");
				logger.Log(LogLevel.Warning, element.ToString());
			}
		}

		public static Tuple<List<ClassNode>, List<BaseNode>> ReadNodes(Stream input, ReClassNetProject templateProject, ILogger logger)
		{
			Contract.Requires(input != null);
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<Tuple<List<ClassNode>, List<BaseNode>>>() != null);

			using (var project = new ReClassNetProject())
			{
				templateProject?.Classes.ForEach(project.AddClass);

				var file = new ReClassNetFile(project);
				file.Load(input, logger);

				var classes = new List<ClassNode>();

				var nodes = new List<BaseNode>();

				var serialisationClassNode = project.Classes.FirstOrDefault(c => c.Name == SerialisationClassName);
				if (serialisationClassNode != null)
				{
					if (templateProject != null)
					{
						foreach (var classNode in project.Classes.Where(c => c != serialisationClassNode))
						{
							if (!templateProject.ContainsClass(classNode.Uuid))
							{
								classes.Add(classNode);
							}
						}
					}

					nodes.AddRange(serialisationClassNode.Nodes);

					project.Remove(serialisationClassNode);
				}

				return Tuple.Create(classes, nodes);
			}
		}
	}
}
