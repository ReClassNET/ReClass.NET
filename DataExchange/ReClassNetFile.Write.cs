using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange
{
	public partial class ReClassNetFile
	{
		public void Save(string filePath, ILogger logger)
		{
			using (var fs = new FileStream(filePath, FileMode.Create))
			{
				Save(fs, logger);
			}
		}

		public void Save(Stream output, ILogger logger)
		{
			using (var archive = new ZipArchive(output, ZipArchiveMode.Create))
			{
				var dataEntry = archive.CreateEntry(DataFileName);
				using (var entryStream = dataEntry.Open())
				{
					var document = new XDocument(
						new XComment("ReClass.NET by KN4CK3R"),
						new XElement(
							XmlRootElement,
							new XAttribute(XmlVersionAttribute, Version1),
							new XAttribute(XmlTypeAttribute, Constants.Platform),
							new XElement(XmlClassesElement, CreateClassElements(project.Classes, logger))
						)
					);

					document.Save(entryStream);
				}
			}
		}

		private IEnumerable<XElement> CreateClassElements(IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<IEnumerable<XElement>>() != null);

			return classes.Select(c => new XElement(
				XmlClassElement,
				new XAttribute(XmlUuidAttribute, c.Uuid.ToBase64String()),
				new XAttribute(XmlNameAttribute, c.Name ?? string.Empty),
				new XAttribute(XmlCommentAttribute, c.Comment ?? string.Empty),
				new XAttribute(XmlAddressAttribute, c.AddressFormula ?? string.Empty),
				CreateNodeElements(c.Nodes, logger)
			));
		}

		private IEnumerable<XElement> CreateNodeElements(IEnumerable<BaseNode> nodes, ILogger logger)
		{
			Contract.Requires(nodes != null);
			Contract.Requires(Contract.ForAll(nodes, n => n != null));
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<IEnumerable<XElement>>() != null);

			foreach (var node in nodes)
			{
				var converter = CustomNodeConvert.GetWriteConverter(node);
				if (converter != null)
				{
					yield return converter.CreateElementFromNode(node, logger);

					continue;
				}

				string typeString;
				if (!BuildInTypeToStringMap.TryGetValue(node.GetType(), out typeString))
				{
					logger.Log(LogLevel.Error, $"Skipping node with unknown type: {node.Name}");
					logger.Log(LogLevel.Warning, node.GetType().ToString());

					continue;
				}

				var element = new XElement(
					XmlNodeElement,
					new XAttribute(XmlNameAttribute, node.Name ?? string.Empty),
					new XAttribute(XmlCommentAttribute, node.Comment ?? string.Empty),
					new XAttribute(XmlTypeAttribute, typeString)
				);

				var referenceNode = node as BaseReferenceNode;
				if (referenceNode != null)
				{
					element.SetAttributeValue(XmlReferenceAttribute, referenceNode.InnerNode.Uuid.ToBase64String());
				}
				var vtableNode = node as VTableNode;
				if (vtableNode != null)
				{
					element.Add(vtableNode.Nodes.Select(n => new XElement(
						XmlMethodElement,
						new XAttribute(XmlNameAttribute, n.Name ?? string.Empty),
						new XAttribute(XmlCommentAttribute, n.Comment ?? string.Empty)
					)));
				}
				var arrayNode = node as BaseArrayNode;
				if (arrayNode != null)
				{
					element.SetAttributeValue(XmlCountAttribute, arrayNode.Count);
				}
				var textNode = node as BaseTextNode;
				if (textNode != null)
				{
					element.SetAttributeValue(XmlLengthAttribute, textNode.Length);
				}
				var bitFieldNode = node as BitFieldNode;
				if (bitFieldNode != null)
				{
					element.SetAttributeValue(XmlBitsAttribute, bitFieldNode.Bits);
				}
				var functionNode = node as FunctionNode;
				if (functionNode != null)
				{
					var uuid = functionNode.BelongsToClass == null ? NodeUuid.Zero : functionNode.BelongsToClass.Uuid;
					element.SetAttributeValue(XmlReferenceAttribute, uuid.ToBase64String());
					element.SetAttributeValue(XmlSignatureAttribute, functionNode.Signature);
				}

				yield return element;
			}
		}

		public static void WriteNodes(Stream output, IEnumerable<BaseNode> nodes, ILogger logger)
		{
			Contract.Requires(output != null);
			Contract.Requires(nodes != null);
			Contract.Requires(Contract.ForAll(nodes, n => n != null));
			Contract.Requires(logger != null);

			using (var project = new ReClassNetProject())
			{
				void RecursiveAddReferences(BaseReferenceNode referenceNode)
				{
					if (project.ContainsClass(referenceNode.InnerNode.Uuid))
					{
						return;
					}

					project.AddClass(referenceNode.InnerNode);

					foreach (var reference in referenceNode.InnerNode.Nodes.OfType<BaseReferenceNode>())
					{
						RecursiveAddReferences(reference);
					}
				}

				var serialisationClass = new ClassNode(false)
				{
					Name = SerialisationClassName
				};

				project.AddClass(serialisationClass);

				foreach (var node in nodes)
				{
					var classNode = node as ClassNode;
					if (classNode != null)
					{
						project.AddClass(classNode);

						continue;
					}

					var referenceNode = node as BaseReferenceNode;
					if (referenceNode != null)
					{
						RecursiveAddReferences(referenceNode);
					}

					serialisationClass.AddNode(node);
				}

				var file = new ReClassNetFile(project);
				file.Save(output, logger);
			}
		}
	}
}
