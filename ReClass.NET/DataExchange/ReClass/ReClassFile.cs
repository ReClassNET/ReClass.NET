using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.DataExchange.ReClass.Legacy;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.DataExchange.ReClass
{
	public class ReClassFile : IReClassImport
	{
		public const string FormatName = "ReClass File";
		public const string FileExtension = ".reclass";

		private readonly ReClassNetProject project;

		public ReClassFile(ReClassNetProject project)
		{
			Contract.Requires(project != null);

			this.project = project;
		}

		public void Load(string filePath, ILogger logger)
		{
			var document = XDocument.Load(filePath);
			if (document.Root == null)
			{
				return;
			}

			Type[] typeMap = null;

			if (document.Root.FirstNode is XComment versionComment)
			{
				switch (versionComment.Value.Substring(0, 12).ToLower())
				{
					case "reclass 2011":
					case "reclass 2013":
						typeMap = typeMap2013;
						break;
					case "reclass 2015":
					case "reclass 2016":
						typeMap = typeMap2016;
						break;
					default:
						logger.Log(LogLevel.Warning, $"Unknown file version: {versionComment.Value}");
						logger.Log(LogLevel.Warning, "Defaulting to ReClass 2016.");

						typeMap = typeMap2016;
						break;
				}
			}

			var classes = new List<Tuple<XElement, ClassNode>>();

			foreach (var element in document.Root
				.Elements("Class")
				.DistinctBy(e => e.Attribute("Name")?.Value))
			{
				var node = new ClassNode(false)
				{
					Name = element.Attribute("Name")?.Value ?? string.Empty,
					AddressFormula = TransformAddressString(element.Attribute("strOffset")?.Value ?? string.Empty)
				};

				project.AddClass(node);

				classes.Add(Tuple.Create(element, node));
			}

			var classMap = classes.ToDictionary(t => t.Item2.Name, t => t.Item2);
			foreach (var t in classes)
			{
				ReadNodeElements(
					t.Item1.Elements("Node"),
					t.Item2,
					classMap,
					typeMap,
					logger
				).ForEach(t.Item2.AddNode);
			}
		}

		/// <summary>Parse ReClass address string and transform it into a ReClass.NET formula.</summary>
		/// <param name="address">The address string.</param>
		/// <returns>A string.</returns>
		private static string TransformAddressString(string address)
		{
			Contract.Requires(address != null);

			var parts = address.Split('+')
				.Select(s => s.Trim().ToLower().Replace("\"", string.Empty))
				.Where(s => s != string.Empty)
				.ToArray();

			for (var i = 0; i < parts.Length; ++i)
			{
				var part = parts[i];

				var isModule = part.Contains(".exe") || part.Contains(".dll");

				var isPointer = false;
				if (part.StartsWith("*"))
				{
					isPointer = true;
					part = part.Substring(1);
				}

				if (isModule)
				{
					part = $"<{part}>";
				}
				if (isPointer)
				{
					part = $"[{part}]";
				}

				parts[i] = part;
			}

			return string.Join(" + ", parts);
		}

		private IEnumerable<BaseNode> ReadNodeElements(IEnumerable<XElement> elements, ClassNode parent, IReadOnlyDictionary<string, ClassNode> classes, Type[] typeMap, ILogger logger)
		{
			Contract.Requires(elements != null);
			Contract.Requires(parent != null);
			Contract.Requires(classes != null);
			Contract.Requires(typeMap != null);
			Contract.Requires(logger != null);

			foreach (var element in elements)
			{
				Type nodeType = null;

				if (int.TryParse(element.Attribute("Type")?.Value, out var typeVal))
				{
					if (typeVal >= 0 && typeVal < typeMap.Length)
					{
						nodeType = typeMap[typeVal];
					}
				}

				if (nodeType == null)
				{
					logger.Log(LogLevel.Error, $"Skipping node with unknown type: {element.Attribute("Type")?.Value}");
					logger.Log(LogLevel.Warning, element.ToString());

					continue;
				}

				var node = BaseNode.CreateInstanceFromType(nodeType, false);
				if (node == null)
				{
					logger.Log(LogLevel.Error, $"Could not create node of type: {nodeType}");

					continue;
				}

				node.Name = element.Attribute("Name")?.Value ?? string.Empty;
				node.Comment = element.Attribute("Comment")?.Value ?? string.Empty;
				node.IsHidden = element.Attribute("bHidden")?.Value.Equals("1") ?? false;

				// Convert the Custom node into normal hex nodes.
				if (node is CustomNode customNode)
				{
					int.TryParse(element.Attribute("Size")?.Value, out var size);

					foreach (var paddingNode in customNode.GetEquivalentNodes(size))
					{
						yield return paddingNode;
					}

					continue;
				}

				// ClassInstanceNode, ClassPointerNode, ClassInstanceArrayNode, ClassPointerArrayNode
				if (node is BaseWrapperNode baseWrapperNode)
				{
					string reference;
					int arrayCount = 0;
					if (node is BaseClassArrayNode) // ClassInstanceArrayNode, ClassPointerArrayNode
					{
						reference = element.Element("Array")?.Attribute("Name")?.Value;

						if (node is ClassInstanceArrayNode)
						{
							TryGetAttributeValue(element, "Total", out arrayCount, logger);
						}
						else
						{
							TryGetAttributeValue(element, "Count", out arrayCount, logger);
						}
					}
					else // ClassInstanceNode, ClassPointerNode
					{
						reference = element.Attribute("Pointer")?.Value ?? element.Attribute("Instance")?.Value;
					}

					if (reference == null || !classes.ContainsKey(reference))
					{
						logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {reference}");
						logger.Log(LogLevel.Warning, element.ToString());

						continue;
					}

					var innerClassNode = classes[reference];
					if (baseWrapperNode.ShouldPerformCycleCheckForInnerNode() && !ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, innerClassNode, project.Classes))
					{
						logger.Log(LogLevel.Error, $"Skipping node with cycle reference: {parent.Name}->{node.Name}");

						continue;
					}

					// ClassPointerNode, ClassInstanceArrayNode and ClassPointerArrayNode need to be converted to supported nodes.
					if (node is BaseClassArrayNode classArrayNode) // ClassInstanceArrayNode, ClassPointerArrayNode
					{
						node = classArrayNode.GetEquivalentNode(arrayCount, innerClassNode);
					}
					else if (node is ClassPointerNode classPointerNode) // ClassPointerNode
					{
						node = classPointerNode.GetEquivalentNode(innerClassNode);
					}
					else // ClassInstanceNode, ClassPointerNode
					{
						baseWrapperNode.ChangeInnerNode(innerClassNode);
					}
				}

				switch (node)
				{
					case VirtualMethodTableNode vtableNode:
						element
							.Elements("Function")
							.Select(e => new VirtualMethodNode
							{
								Name = e.Attribute("Name")?.Value ?? string.Empty,
								Comment = e.Attribute("Comment")?.Value ?? string.Empty,
								IsHidden = e.Attribute("bHidden")?.Value.Equals("1") ?? false
							})
							.ForEach(vtableNode.AddNode);
						break;
					case BaseTextNode textNode:
					{
						TryGetAttributeValue(element, "Size", out var length, logger);
						textNode.Length = textNode is Utf16TextNode ? length / 2 : length;
						break;
					}
					case BitFieldNode bitFieldNode:
					{
						TryGetAttributeValue(element, "Size", out var bits, logger);
						bitFieldNode.Bits = bits * 8;
						break;
					}
				}

				yield return node;
			}
		}

		private static void TryGetAttributeValue(XElement element, string attribute, out int val, ILogger logger)
		{
			if (!int.TryParse(element.Attribute(attribute)?.Value, out val))
			{
				val = 0;

				logger.Log(LogLevel.Error, $"Node is missing a valid '{attribute}' attribute, defaulting to 0.");
				logger.Log(LogLevel.Warning, element.ToString());
			}
		}

		#region ReClass 2011 / ReClass 2013

		private static readonly Type[] typeMap2013 =
		{
			null,
			typeof(ClassInstanceNode),
			null,
			null,
			typeof(Hex32Node),
			typeof(Hex16Node),
			typeof(Hex8Node),
			typeof(ClassPointerNode),
			typeof(Int32Node),
			typeof(Int16Node),
			typeof(Int8Node),
			typeof(FloatNode),
			typeof(UInt32Node),
			typeof(UInt16Node),
			typeof(UInt8Node),
			typeof(Utf8TextNode),
			typeof(FunctionPtrNode),
			typeof(CustomNode),
			typeof(Vector2Node),
			typeof(Vector3Node),
			typeof(Vector4Node),
			typeof(Matrix4x4Node),
			typeof(VirtualMethodTableNode),
			typeof(ClassInstanceArrayNode),
			null,
			null,
			null,
			typeof(Int64Node),
			typeof(DoubleNode),
			typeof(Utf16TextNode),
			typeof(ClassPointerArrayNode)
		};

		#endregion

		#region ReClass 2015 / ReClass 2016

		private static readonly Type[] typeMap2016 =
		{
			null,
			typeof(ClassInstanceNode),
			null,
			null,
			typeof(Hex32Node),
			typeof(Hex64Node),
			typeof(Hex16Node),
			typeof(Hex8Node),
			typeof(ClassPointerNode),
			typeof(Int64Node),
			typeof(Int32Node),
			typeof(Int16Node),
			typeof(Int8Node),
			typeof(FloatNode),
			typeof(DoubleNode),
			typeof(UInt32Node),
			typeof(UInt16Node),
			typeof(UInt8Node),
			typeof(Utf8TextNode),
			typeof(Utf16TextNode),
			typeof(FunctionPtrNode),
			typeof(CustomNode),
			typeof(Vector2Node),
			typeof(Vector3Node),
			typeof(Vector4Node),
			typeof(Matrix4x4Node),
			typeof(VirtualMethodTableNode),
			typeof(ClassInstanceArrayNode),
			null,
			typeof(Utf8TextPtrNode),
			typeof(Utf16TextPtrNode),
			typeof(BitFieldNode),
			typeof(UInt64Node),
			typeof(FunctionNode)
		};

		#endregion
	}
}
