using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.DataExchange.ReClass.Legacy;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.DataExchange.ReClass
{
	public partial class ReClassNetFile
	{
		public void Load(string filePath, ILogger logger)
		{
			using var fs = new FileStream(filePath, FileMode.Open);

			Load(fs, logger);
		}

		public void Load(Stream input, ILogger logger)
		{
			Contract.Requires(input != null);
			Contract.Requires(logger != null);

			using var archive = new ZipArchive(input, ZipArchiveMode.Read);
			var dataEntry = archive.GetEntry(DataFileName);
			if (dataEntry == null)
			{
				throw new FormatException();
			}

			using var entryStream = dataEntry.Open();
			var document = XDocument.Load(entryStream);
			if (document.Root?.Element(XmlClassesElement) == null)
			{
				throw new FormatException("The data has not the correct format.");
			}

			uint.TryParse(document.Root.Attribute(XmlVersionAttribute)?.Value, out var fileVersion);
			if ((fileVersion & FileVersionCriticalMask) > (FileVersion & FileVersionCriticalMask))
			{
				throw new FormatException($"The file version is unsupported. A newer {Constants.ApplicationName} version is required to read it.");
			}

			var platform = document.Root.Attribute(XmlPlatformAttribute)?.Value;
			if (platform != Constants.Platform)
			{
				logger.Log(LogLevel.Warning, $"The platform of the file ({platform}) doesn't match the program platform ({Constants.Platform}).");
			}

			var customDataElement = document.Root.Element(XmlCustomDataElement);
			if (customDataElement != null)
			{
				project.CustomData.Deserialize(customDataElement);
			}

			var typeMappingElement = document.Root.Element(XmlTypeMappingElement);
			if (typeMappingElement != null)
			{
				project.TypeMapping.Deserialize(typeMappingElement);
			}

			var enumsElement = document.Root.Element(XmlEnumsElement);
			if (enumsElement != null)
			{
				foreach (var enumElement in enumsElement.Elements(XmlEnumElement))
				{
					var name = enumElement.Attribute(XmlNameAttribute)?.Value ?? string.Empty;
					var useFlagsMode = (bool?)enumElement.Attribute(XmlFlagsAttribute) ?? false;
					var size = enumElement.Attribute(XmlSizeAttribute).GetEnumValue<EnumDescription.UnderlyingTypeSize>();

					var values = new Dictionary<string, long>();
					foreach (var itemElement in enumElement.Elements(XmlItemElement))
					{
						var itemName = itemElement.Attribute(XmlNameAttribute)?.Value ?? string.Empty;
						var itemValue = (long?)itemElement.Attribute(XmlValueAttribute) ?? 0L;

						values.Add(itemName, itemValue);
					}

					var @enum = new EnumDescription
					{
						Name = name
					};
					@enum.SetData(useFlagsMode, size, values);

					project.AddEnum(@enum);
				}
			}

			var classes = new List<(XElement, ClassNode)>();

			var classesElement = document.Root.Element(XmlClassesElement);
			if (classesElement != null)
			{
				foreach (var element in classesElement
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

						classes.Add((element, node));
					}
				}
			}

			foreach (var (element, classNode) in classes)
			{
				var nodes = element.Elements(XmlNodeElement)
					.Select(e => CreateNodeFromElement(e, classNode, logger))
					.Where(n => n != null);

				classNode.BeginUpdate();
				classNode.AddNodes(nodes);
				classNode.EndUpdate();
			}
		}

		private BaseNode CreateNodeFromElement(XElement element, BaseNode parent, ILogger logger)
		{
			Contract.Requires(element != null);
			Contract.Requires(logger != null);

			BaseNode CreateNode()
			{
				var converter = CustomNodeSerializer.GetReadConverter(element);
				if (converter != null)
				{
					return converter.CreateNodeFromElement(element, parent, project.Classes, logger, CreateNodeFromElement);
				}

				if (!buildInStringToTypeMap.TryGetValue(element.Attribute(XmlTypeAttribute)?.Value ?? string.Empty, out var nodeType))
				{
					logger.Log(LogLevel.Error, $"Skipping node with unknown type: {element.Attribute(XmlTypeAttribute)?.Value}");
					logger.Log(LogLevel.Warning, element.ToString());

					return null;
				}

				return BaseNode.CreateInstanceFromType(nodeType, false);
			}

			var node = CreateNode();
			if (node == null)
			{
				logger.Log(LogLevel.Error, "Could not create node.");

				return null;
			}

			node.ParentNode = parent;

			node.Name = element.Attribute(XmlNameAttribute)?.Value ?? string.Empty;
			node.Comment = element.Attribute(XmlCommentAttribute)?.Value ?? string.Empty;
			node.IsHidden = bool.TryParse(element.Attribute(XmlHiddenAttribute)?.Value, out var val) && val;

			if (node is BaseWrapperNode wrapperNode)
			{
				ClassNode GetClassNodeFromElementReference()
				{
					var reference = NodeUuid.FromBase64String(element.Attribute(XmlReferenceAttribute)?.Value, false);
					if (!project.ContainsClass(reference))
					{
						logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {reference}");
						logger.Log(LogLevel.Warning, element.ToString());

						return null;
					}

					return project.GetClassByUuid(reference);
				}

				// Legacy Support
				if (node is ClassPointerNode || node is ClassInstanceArrayNode || node is ClassPointerArrayNode)
				{
					var innerClass = GetClassNodeFromElementReference();
					if (innerClass == null)
					{
						return null;
					}

					node = node switch
					{
						BaseClassArrayNode classArrayNode => classArrayNode.GetEquivalentNode(0, innerClass),
						ClassPointerNode classPointerNode => classPointerNode.GetEquivalentNode(innerClass)
					};
				}
				else
				{
					BaseNode innerNode = null;

					if (node is BaseClassWrapperNode)
					{
						innerNode = GetClassNodeFromElementReference();
						if (innerNode == null)
						{
							return null;
						}
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
							IsHidden = (bool?)e.Attribute(XmlHiddenAttribute) ?? false
						});

					vtableNode.AddNodes(nodes);
					break;
				}
				case UnionNode unionNode:
				{
					var nodes = element
						.Elements()
						.Select(e => CreateNodeFromElement(e, unionNode, logger));

					unionNode.AddNodes(nodes);
					break;
				}
				case BaseWrapperArrayNode arrayNode:
				{
					arrayNode.Count = (int?)element.Attribute(XmlCountAttribute) ?? 0;
					break;
				}
				case BaseTextNode textNode:
				{
					textNode.Length = (int?)element.Attribute(XmlLengthAttribute) ?? 0;
					break;
				}
				case BitFieldNode bitFieldNode:
				{
					bitFieldNode.Bits = (int?)element.Attribute(XmlBitsAttribute) ?? 0;
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
				case EnumNode enumNode:
				{
					var enumName = element.Attribute(XmlReferenceAttribute)?.Value ?? string.Empty;
					var @enum = project.Enums.FirstOrDefault(e => e.Name == enumName) ?? EnumDescription.Default;
					
					enumNode.ChangeEnum(@enum);
					break;
				}
			}

			return node;
		}

		public static Tuple<List<ClassNode>, List<BaseNode>> DeserializeNodesFromStream(Stream input, ReClassNetProject templateProject, ILogger logger)
		{
			Contract.Requires(input != null);
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<Tuple<List<ClassNode>, List<BaseNode>>>() != null);

			using var project = new ReClassNetProject();
			templateProject?.Classes.ForEach(project.AddClass);

			var file = new ReClassNetFile(project);
			file.Load(input, logger);

			var classes = project.Classes
				.Where(c => c.Name != SerializationClassName);
			if (templateProject != null)
			{
				classes = classes.Where(c => !templateProject.ContainsClass(c.Uuid));
			}

			var nodes = project.Classes
				.Where(c => c.Name == SerializationClassName)
				.SelectMany(c => c.Nodes);

			return Tuple.Create(classes.ToList(), nodes.ToList());
		}
	}
}
