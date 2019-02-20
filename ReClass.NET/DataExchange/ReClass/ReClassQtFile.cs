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
	public class ReClassQtFile : IReClassImport
	{
		public const string FormatName = "ReClassQt File";
		public const string FileExtension = ".reclassqt";

		private readonly Type[] typeMap = {
			null,
			null,
			typeof(ClassPointerNode),
			typeof(ClassInstanceNode),
			typeof(Hex64Node),
			typeof(Hex32Node),
			typeof(Hex16Node),
			typeof(Hex8Node),
			typeof(Int64Node),
			typeof(Int32Node),
			typeof(Int16Node),
			typeof(Int8Node),
			typeof(UInt32Node),
			null,
			null,
			typeof(UInt32Node), //bool
			null,
			typeof(FloatNode),
			typeof(DoubleNode),
			typeof(Vector4Node),
			typeof(Vector3Node),
			typeof(Vector2Node)
		};

		private readonly ReClassNetProject project;

		public ReClassQtFile(ReClassNetProject project)
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

			var classes = new List<Tuple<XElement, ClassNode>>();

			foreach (var element in document.Root
				.Elements("Namespace")
				.SelectMany(ns => ns.Elements("Class"))
				.DistinctBy(e => e.Attribute("ClassId")?.Value))
			{
				var node = new ClassNode(false)
				{
					Name = element.Attribute("Name")?.Value ?? string.Empty,
					AddressFormula = ParseAddressString(element)
				};

				project.AddClass(node);

				classes.Add(Tuple.Create(element, node));
			}

			var classMap = classes.ToDictionary(c => c.Item1.Attribute("ClassId")?.Value, c => c.Item2);
			foreach (var t in classes)
			{
				ReadNodeElements(
					t.Item1.Elements("Node"),
					t.Item2,
					classMap,
					logger
				).ForEach(t.Item2.AddNode);
			}
		}

		/// <summary>Parse a ReClassQT address string and transform it into a ReClass.NET formula.</summary>
		/// <param name="element">The class element.</param>
		/// <returns>A string with an address formula.</returns>
		private static string ParseAddressString(XElement element)
		{
			Contract.Requires(element != null);

			var address = element.Attribute("Address")?.Value;
			if (string.IsNullOrEmpty(address))
			{
				return string.Empty;
			}

			if (element.Attribute("DerefTwice")?.Value == "1")
			{
				address = $"[{address}]";
			}

			return address;
		}

		private IEnumerable<BaseNode> ReadNodeElements(IEnumerable<XElement> elements, ClassNode parent, IReadOnlyDictionary<string, ClassNode> classes, ILogger logger)
		{
			Contract.Requires(elements != null);
			Contract.Requires(Contract.ForAll(elements, e => e != null));
			Contract.Requires(parent != null);
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
				node.Comment = element.Attribute("Comments")?.Value ?? string.Empty;

				// ClassInstanceNode, ClassPointerNode
				if (node is BaseWrapperNode wrapperNode)
				{
					var pointToClassId = element.Attribute("PointToClass")?.Value;
					if (pointToClassId == null || !classes.ContainsKey(pointToClassId))
					{
						logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {pointToClassId}");
						logger.Log(LogLevel.Warning, element.ToString());

						continue;
					}

					var innerClassNode = classes[pointToClassId];
					if (wrapperNode.ShouldPerformCycleCheckForInnerNode() && !ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, innerClassNode, project.Classes))
					{
						logger.Log(LogLevel.Error, $"Skipping node with cycle reference: {parent.Name}->{node.Name}");

						continue;
					}

					if (node is ClassPointerNode classPointerNode)
					{
						node = classPointerNode.GetEquivalentNode(innerClassNode);
					}
					else // ClassInstanceNode
					{
						wrapperNode.ChangeInnerNode(innerClassNode);
					}
				}

				yield return node;
			}
		}
	}
}
