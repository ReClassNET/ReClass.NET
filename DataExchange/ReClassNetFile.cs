using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.DataExchange
{
	class ReClassNetFile : IReClassImport, IReClassExport
	{
		public const string FormatName = "ReClass.NET File";
		public const string FileExtension = ".rcnet";

		private const string ClassFileName = "Classes.xml";

		private const string XmlRootElement = "reclass";
		private const string XmlClassesElement = "classes";
		private const string XmlClassElement = "class";
		private const string XmlNodeElement = "node";
		private const string XmlVersionAttribute = "version";
		private const string XmlNameAttribute = "name";
		private const string XmlCommentAttribute = "comment";
		private const string XmlAddressAttribute = "address";
		private const string XmlTypeAttribute = "type";
		private const string XmlReferenceAttribute = "reference";

		#region Load

		private Dictionary<string, SchemaClassNode> classes;

		public SchemaBuilder Load(string filePath, ReportError report)
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
							report?.Invoke("File is corrupt.");

							return null;
						}
						using (var reader = new StreamReader(classesEntry.Open()))
						{
							var document = XDocument.Load(reader);

							classes = document.Root
								.Element(XmlClassesElement)
								.Elements(XmlClassElement)
								.ToDictionary(
									cls => cls.Attribute(XmlNameAttribute)?.Value,
									cls =>
									{
										long address;
										long.TryParse(cls.Attribute(XmlAddressAttribute)?.Value, NumberStyles.HexNumber, null, out address);
										return new SchemaClassNode
										{
#if WIN32
											Offset = unchecked((IntPtr)(int)address),
#else
											Offset = unchecked((IntPtr)address),
#endif
											Name = cls.Attribute(XmlNameAttribute)?.Value,
											Comment = cls.Attribute(XmlCommentAttribute)?.Value
										};
									}
								);

							var schema = document.Root
								.Element(XmlClassesElement)
								.Elements(XmlClassElement)
								.Select(cls => new { Data = cls, Class = classes[cls.Attribute(XmlNameAttribute)?.Value] })
								.Select(x =>
								{
									x.Class.Nodes.AddRange(x.Data.Elements(XmlNodeElement).Select(n => ReadNode(n, report)).Where(n => n != null));
									return x.Class;
								});

							return SchemaBuilder.FromSchema(schema);
						}
					}
				}
			}
			catch (Exception ex)
			{
				report?.Invoke(ex.Message);

				return null;
			}
		}

		private SchemaNode ReadNode(XElement node, ReportError report)
		{
			var type = SchemaType.None;
			if (!Enum.TryParse(node.Attribute(XmlTypeAttribute)?.Value, out type))
			{
				report?.Invoke($"Node has unknown type: " + node.ToString());

				return null;
			}

			SchemaNode sn = null;

			if (type == SchemaType.Array || type == SchemaType.ClassInstance || type == SchemaType.ClassPtr)
			{
				var reference = node.Attribute(XmlReferenceAttribute)?.Value;
				if (reference == null || !classes.ContainsKey(reference))
				{
					report?.Invoke("Can't resolve referenced class: " + node.ToString());

					return null;
				}

				sn = new SchemaReferenceNode(type, classes[reference]);
			}
			else
			{
				sn = new SchemaNode(type);
			}

			sn.Name = node.Attribute(XmlNameAttribute)?.Value;
			sn.Comment = node.Attribute(XmlCommentAttribute)?.Value;

			return sn;
		}

		#endregion

		#region Write

		public void Save(string filePath, SchemaBuilder schema)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(schema != null);

			using (var fs = new FileStream(filePath, FileMode.Create))
			{
				using (var archive = new ZipArchive(fs, ZipArchiveMode.Create))
				{
					var classesEntry = archive.CreateEntry(ClassFileName);
					using (var writer = new StreamWriter(classesEntry.Open()))
					{
						writer.Write(GenerateXml(schema));
					}
				}
			}
		}

		private XDocument GenerateXml(SchemaBuilder schema)
		{
			Contract.Requires(schema != null);

			var document = new XDocument(
				new XComment("ReClass.NET by KN4CK3R"),
				new XElement(XmlRootElement, new XAttribute(XmlVersionAttribute, "1"),
					new XElement(XmlClassesElement, schema.BuildSchema().Select(c => WriteNode(c)))
				)
			);

			return document;
		}

		private XElement WriteNode(SchemaNode node)
		{
			Contract.Requires(node != null);

			if (node is SchemaClassNode)
			{
				var classNode = node as SchemaClassNode;

				return new XElement(
					XmlClassElement,
					new XAttribute(XmlNameAttribute, node.Name ?? string.Empty),
					new XAttribute(XmlCommentAttribute, node.Comment ?? string.Empty),
					new XAttribute(XmlAddressAttribute, classNode.Offset.ToInt64().ToString("X")),
					classNode.Nodes.Select(n => WriteNode(n))
				);
			}

			var element = new XElement(
				XmlNodeElement,
				new XAttribute(XmlNameAttribute, node.Name ?? string.Empty),
				new XAttribute(XmlCommentAttribute, node.Comment ?? string.Empty),
				new XAttribute(XmlTypeAttribute, node.Type)
			);

			if (node is SchemaReferenceNode)
			{
				element.SetAttributeValue(XmlReferenceAttribute, (node as SchemaReferenceNode).InnerNode.Name);
			}

			return element;
		}

		#endregion
	}
}
