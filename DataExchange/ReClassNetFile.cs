using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;

namespace ReClassNET.DataExchange
{
	public class ReClassNetFile : IReClassImport, IReClassExport
	{
		public const string FormatName = "ReClass.NET File";
		public const string FileExtension = ".rcnet";

		private const string ClassFileName = "Data.xml";

		public const string XmlRootElement = "reclass";
		public const string XmlClassesElement = "classes";
		public const string XmlClassElement = "class";
		public const string XmlNodeElement = "node";
		public const string XmlMethodElement = "method";
		public const string XmlVersionAttribute = "version";
		public const string XmlNameAttribute = "name";
		public const string XmlCommentAttribute = "comment";
		public const string XmlAddressAttribute = "address";
		public const string XmlTypeAttribute = "type";
		public const string XmlReferenceAttribute = "reference";
		public const string XmlSizeAttribute = "size";

		#region Load

		private Dictionary<string, SchemaClassNode> classes;

		public SchemaBuilder Load(string filePath, ILogger logger)
		{
			try
			{
				using (var fs = new FileStream(filePath, FileMode.Open))
				{
					using (var archive = new ZipArchive(fs, ZipArchiveMode.Read))
					{
						var classesEntry = archive.GetEntry(ClassFileName);
						if (classesEntry == null)
						{
							logger.Log(LogLevel.Error, $"The file '{filePath}' is corrupt.");

							return null;
						}
						using (var reader = new StreamReader(classesEntry.Open()))
						{
							var document = XDocument.Load(reader);

							var version = document.Root.Attribute(XmlVersionAttribute)?.Value;
							var platform = document.Root.Attribute(XmlTypeAttribute)?.Value;
							if (platform != Constants.Platform)
							{
								logger.Log(LogLevel.Warning, $"The platform of the file ({platform}) doesn't match the program platform ({Constants.Platform}).");
							}

							classes = document.Root
								.Element(XmlClassesElement)
								.Elements(XmlClassElement)
								.ToDictionary(
									cls => cls.Attribute(XmlNameAttribute)?.Value,
									cls => new SchemaClassNode
									{
										AddressFormula = cls.Attribute(XmlAddressAttribute)?.Value ?? string.Empty,
										Name = cls.Attribute(XmlNameAttribute)?.Value ?? string.Empty,
										Comment = cls.Attribute(XmlCommentAttribute)?.Value ?? string.Empty
									}
								);

							var schema = document.Root
								.Element(XmlClassesElement)
								.Elements(XmlClassElement)
								.Select(cls => new { Data = cls, Class = classes[cls.Attribute(XmlNameAttribute)?.Value] })
								.Select(x =>
								{
									x.Class.Nodes.AddRange(x.Data.Elements(XmlNodeElement).Select(n => ReadNode(n, logger)).Where(n => n != null));
									return x.Class;
								});

							return SchemaBuilder.FromSchema(schema);
						}
					}
				}
			}
			catch (Exception ex)
			{
				logger.Log(ex);

				return null;
			}
		}

		private SchemaNode ReadNode(XElement node, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(logger != null);

			var type = SchemaType.None;
			if (!Enum.TryParse(node.Attribute(XmlTypeAttribute)?.Value, out type))
			{
				// No build in type matched, try the custom converters.
				var converter = CustomSchemaConvert.GetReadConverter(node);
				if (converter != null)
				{
					return converter.ReadFromXml(node, classes, logger);
				}

				logger.Log(LogLevel.Error, $"Skipping node with unknown type: {node.Attribute(XmlTypeAttribute)?.Value}");
				logger.Log(LogLevel.Warning, node.ToString());

				return null;
			}

			SchemaNode sn;

			if (type == SchemaType.Array || type == SchemaType.ClassPtrArray || type == SchemaType.ClassInstance || type == SchemaType.ClassPtr)
			{
				var reference = node.Attribute(XmlReferenceAttribute)?.Value;
				if (reference == null || !classes.ContainsKey(reference))
				{
					logger.Log(LogLevel.Error, $"Skipping node with unknown reference: {reference}");
					logger.Log(LogLevel.Warning, node.ToString());

					return null;
				}

				sn = new SchemaReferenceNode(type, classes[reference]);
			}
			else if (type == SchemaType.VTable)
			{
				var vtableNode = new SchemaVTableNode();

				vtableNode.Nodes.AddRange(node.Elements(XmlMethodElement).Select(e => new SchemaNode(SchemaType.VMethod)
				{
					Name = e.Attribute(XmlNameAttribute)?.Value,
					Comment = e.Attribute(XmlCommentAttribute)?.Value
				}));

				sn = vtableNode;
			}
			else
			{
				sn = new SchemaNode(type);
			}

			sn.Name = node.Attribute(XmlNameAttribute)?.Value;
			sn.Comment = node.Attribute(XmlCommentAttribute)?.Value;

			switch (type)
			{
				case SchemaType.Array:
				case SchemaType.ClassPtrArray:
				case SchemaType.UTF8Text:
				case SchemaType.UTF16Text:
				case SchemaType.UTF32Text:
				case SchemaType.BitField:
					int size;
					if (!int.TryParse(node.Attribute(XmlSizeAttribute)?.Value, out size))
					{
						logger.Log(LogLevel.Warning, "Node is missing the size attribute, defaulting to 0.");
						logger.Log(LogLevel.Warning, node.ToString());
					}
					sn.Count = size;
					break;
			}

			return sn;
		}

		#endregion

		#region Write

		public void Save(string filePath, SchemaBuilder schema, ILogger logger)
		{
			using (var fs = new FileStream(filePath, FileMode.Create))
			{
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
				{
					var classesEntry = archive.CreateEntry(ClassFileName);
					using (var writer = new StreamWriter(classesEntry.Open()))
					{
						writer.Write(GenerateXml(schema, logger));
					}
				}
			}
		}

		private XDocument GenerateXml(SchemaBuilder schema, ILogger logger)
		{
			Contract.Requires(schema != null);
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<XDocument>() != null);

			var document = new XDocument(
				new XComment("ReClass.NET by KN4CK3R"),
				new XElement(
					XmlRootElement,
					new XAttribute(XmlVersionAttribute, "1"),
					new XAttribute(XmlTypeAttribute, Constants.Platform),
					new XElement(XmlClassesElement, schema.BuildSchema().Select(c => WriteNode(c, logger)).Where(e => e != null))
				)
			);

			return document;
		}

		private XElement WriteNode(SchemaNode node, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(logger != null);

			if (node is SchemaClassNode)
			{
				var classNode = node as SchemaClassNode;

				return new XElement(
					XmlClassElement,
					new XAttribute(XmlNameAttribute, node.Name ?? string.Empty),
					new XAttribute(XmlCommentAttribute, node.Comment ?? string.Empty),
					new XAttribute(XmlAddressAttribute, classNode.AddressFormula ?? string.Empty),
					classNode.Nodes.Select(n => WriteNode(n, logger)).Where(e => e != null)
				);
			}

			if (node is SchemaCustomNode)
			{
				var converter = CustomSchemaConvert.GetWriteConverter(node as SchemaCustomNode);
				if (converter != null)
				{
					return converter.WriteToXml(node as SchemaCustomNode, logger);
				}

				logger.Log(LogLevel.Error, $"Skipping node with unknown type converter: {node.Name}");
				logger.Log(LogLevel.Warning, node.GetType().ToString());

				return null;
			}

			var element = new XElement(
				XmlNodeElement,
				new XAttribute(XmlNameAttribute, node.Name ?? string.Empty),
				new XAttribute(XmlCommentAttribute, node.Comment ?? string.Empty),
				new XAttribute(XmlTypeAttribute, node.Type)
			);

			if (node is SchemaReferenceNode)
			{
				element.SetAttributeValue(XmlReferenceAttribute, (node as SchemaReferenceNode).InnerNode.Name ?? string.Empty);
			}
			if (node is SchemaVTableNode)
			{
				element.Add(((SchemaVTableNode)node).Nodes.Select(n => new XElement(
					XmlMethodElement,
					new XAttribute(XmlNameAttribute, n.Name ?? string.Empty),
					new XAttribute(XmlCommentAttribute, n.Comment ?? string.Empty)
				)));
			}

			if (node.Count > 0)
			{
				element.SetAttributeValue(XmlSizeAttribute, node.Count);
			}

			return element;
		}

		#endregion
	}
}
