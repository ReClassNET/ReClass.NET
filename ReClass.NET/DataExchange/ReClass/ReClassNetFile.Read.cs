using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass
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
					if (document.Root?.Element(XmlClassesElement) == null)
					{
						logger.Log(LogLevel.Error, "File has not the correct format.");
						return;
					}

					var version = document.Root.Attribute(XmlVersionAttribute)?.Value;
					if (version != CurrentVersion)
					{
						logger.Log(LogLevel.Warning, $"Version mismatch ({version} vs {CurrentVersion}).");
					}
					var platform = document.Root.Attribute(XmlPlatformAttribute)?.Value;
					if (platform != Constants.Platform)
					{
						logger.Log(LogLevel.Warning, $"The platform of the file ({platform}) doesn't match the program platform ({Constants.Platform}).");
					}

					var customDataElement = document.Root.Element(XmlCustomDataElement);
					if (customDataElement != null)
					{
						foreach (var kv in customDataElement.Elements())
						{
							project.CustomData[kv.Name.ToString()] = kv.Value;
						}
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
						var nodes = t.Item1.Elements(XmlNodeElement)
							.Select(e => CreateNodeFromElement(e, t.Item2, logger))
							.Where(n => n != null);

						foreach (var node in nodes)
						{
							t.Item2.AddNode(node);
						}
					}
				}
			}
		}

		private BaseNode CreateNodeFromElement(XElement element, BaseNode parent, ILogger logger)
		{
			Contract.Requires(element != null);
			Contract.Requires(logger != null);

			var converter = CustomNodeConvert.GetReadConverter(element);
			if (converter != null)
			{
				if (converter.TryCreateNodeFromElement(element, parent, project.Classes, logger, CreateNodeFromElement, out var customNode))
				{
					return customNode;
				}
			}

			if (!buildInStringToTypeMap.TryGetValue(element.Attribute(XmlTypeAttribute)?.Value ?? string.Empty, out var nodeType))
			{
				logger.Log(LogLevel.Error, $"Skipping node with unknown type: {element.Attribute(XmlTypeAttribute)?.Value}");
				logger.Log(LogLevel.Warning, element.ToString());

				return null;
			}

			var node = BaseNode.CreateInstanceFromType(nodeType);
			if (node == null)
			{
				logger.Log(LogLevel.Error, $"Could not create node of type: {nodeType}");

				return null;
			}

			node.ParentNode = parent;

			node.Name = element.Attribute(XmlNameAttribute)?.Value ?? string.Empty;
			node.Comment = element.Attribute(XmlCommentAttribute)?.Value ?? string.Empty;
			node.IsHidden = bool.TryParse(element.Attribute(XmlHiddenAttribute)?.Value, out var val) && val;

			if (node is BaseWrapperNode wrapperNode)
			{
				BaseNode innerNode = null;

				if (node is ClassInstanceNode classInstanceNode)
				{
					var reference = NodeUuid.FromBase64String(element.Attribute(XmlReferenceAttribute)?.Value, false);
					if (!project.ContainsClass(reference))
					{
						logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {reference}");
						logger.Log(LogLevel.Warning, element.ToString());

						return null;
					}

					innerNode = project.GetClassByUuid(reference);
				}
				else
				{
					var innerElement = element.Elements().FirstOrDefault();
					if (innerElement != null)
					{
						innerNode = CreateNodeFromElement(innerElement, node, logger);
					}
				}

				if (wrapperNode.CanChangeInnerNodeTo(innerNode))
				{
					var rootWrapperNode = node.GetRootWrapperNode();
					if (rootWrapperNode.ShouldPerformCycleCheckForInnerNode()
						&& innerNode is ClassNode classNode
						&& ClassUtil.IsCyclicIfClassIsAccessibleFromParent(node.GetParentClass(), classNode, project.Classes))
					{
						logger.Log(LogLevel.Error, $"Skipping node with cyclic class reference: {node.GetParentClass().Name}->{rootWrapperNode.Name}");

						return null;
					}

					wrapperNode.ChangeInnerNode(innerNode);
				}
				else
				{
					logger.Log(LogLevel.Error, $"The node {innerNode} is not a valid child for {node}.");
				}
			}

			switch (node)
			{
				case VirtualMethodTableNode vtableNode:
				{
					var nodes = element
						.Elements(XmlMethodElement)
						.Select(e => new VirtualMethodNode
						{
							Name = e.Attribute(XmlNameAttribute)?.Value ?? string.Empty,
							Comment = e.Attribute(XmlCommentAttribute)?.Value ?? string.Empty,
							IsHidden = e.Attribute(XmlHiddenAttribute)?.Value.Equals("True") ?? false
						});

					foreach (var vmethodNode in nodes)
					{
						vtableNode.AddNode(vmethodNode);
					}
					break;
				}
				case BaseWrapperArrayNode arrayNode:
				{
					TryGetAttributeValue(element, XmlCountAttribute, out var count, logger);
					arrayNode.Count = count;
					break;
				}
				case BaseTextNode textNode:
				{
					TryGetAttributeValue(element, XmlLengthAttribute, out var length, logger);
					textNode.Length = length;
					break;
				}
				case BitFieldNode bitFieldNode:
				{
					TryGetAttributeValue(element, XmlBitsAttribute, out var bits, logger);
					bitFieldNode.Bits = bits;
					break;
				}
				case FunctionNode functionNode:
				{
					functionNode.Signature = element.Attribute(XmlSignatureAttribute)?.Value ?? string.Empty;

					var reference = NodeUuid.FromBase64String(element.Attribute(XmlReferenceAttribute)?.Value, false);
					if (project.ContainsClass(reference))
					{
						functionNode.BelongsToClass = project.GetClassByUuid(reference);
					}
					break;
				}
			}

			return node;
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
						var collection = project.Classes
							.Where(c => c != serialisationClassNode)
							.Where(classNode => !templateProject.ContainsClass(classNode.Uuid));

						classes.AddRange(collection);
					}

					nodes.AddRange(serialisationClassNode.Nodes);

					project.Remove(serialisationClassNode);
				}

				return Tuple.Create(classes, nodes);
			}
		}
	}
}
