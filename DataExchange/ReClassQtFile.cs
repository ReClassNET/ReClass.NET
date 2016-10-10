using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace ReClassNET.DataExchange
{
	class ReClassQtFile : IReClassImport
	{
		public const string FormatName = "ReClassQt File";
		public const string FileExtension = ".reclassqt";

		private Dictionary<string, SchemaClassNode> classes;

		public SchemaBuilder Load(string filePath, ReportError report)
		{
			try
			{
				var document = XDocument.Load(filePath);

				classes = document.Root
					.Elements("Namespace")
					.SelectMany(ns => ns.Elements("Class"))
					.ToDictionary(
						cls => cls.Attribute("ClassId")?.Value,
						cls =>
						{
							long address;
							long.TryParse(cls.Attribute("Address")?.Value, NumberStyles.HexNumber, null, out address);
							return new SchemaClassNode
							{
#if WIN32
								Offset = unchecked((IntPtr)(int)address),
#else
								Offset = unchecked((IntPtr)address),
#endif

								Name = cls.Attribute("Name")?.Value
							};
						}
					);

				var schema = document.Root
					.Elements("Namespace")
					.SelectMany(ns => ns.Elements("Class"))
					.Select(cls => new { Data = cls, Class = classes[cls.Attribute("ClassId")?.Value] })
					.Select(x =>
					{
						x.Class.Nodes.AddRange(x.Data.Elements("Node").Select(n => ReadNode(n, report)).Where(n => n != null));
						return x.Class;
					});

				return SchemaBuilder.FromSchema(schema);
			}
			catch (Exception ex)
			{
				report?.Invoke(ex.Message);

				return null;
			}
		}

		private SchemaType[] typeMap = new SchemaType[]
		{
			SchemaType.None,
			SchemaType.None,
			SchemaType.ClassPtr,
			SchemaType.ClassInstance,
			SchemaType.Hex64,
			SchemaType.Hex32,
			SchemaType.Hex16,
			SchemaType.Hex8,
			SchemaType.Int64,
			SchemaType.Int32,
			SchemaType.Int16,
			SchemaType.Int8,
			SchemaType.UInt32,
			SchemaType.None,
			SchemaType.None,
			SchemaType.UInt32, //bool
			SchemaType.None,
			SchemaType.Float,
			SchemaType.Double,
			SchemaType.Vector4,
			SchemaType.Vector3,
			SchemaType.Vector2
		};

		private SchemaNode ReadNode(XElement node, ReportError report)
		{
			var type = SchemaType.None;

			int typeVal;
			if (int.TryParse(node.Attribute("Type")?.Value, out typeVal))
			{
				if (typeVal >= 0 && typeVal < typeMap.Length)
				{
					type = typeMap[typeVal];
				}
			}

			if (type == SchemaType.None)
			{
				report?.Invoke($"Node has unknown type: " + node.ToString());

				return null;
			}

			SchemaNode sn = null;

			if (type == SchemaType.ClassInstance || type == SchemaType.ClassPtr)
			{
				var pointToClassId = node.Attribute("PointToClass")?.Value;
				if (pointToClassId == null || !classes.ContainsKey(pointToClassId))
				{
					report?.Invoke("Can't resolve referenced class: " + node.ToString());

					return null;
				}

				sn = new SchemaReferenceNode(type, classes[pointToClassId]);
			}
			else
			{
				sn = new SchemaNode(type);
			}

			sn.Name = node.Attribute("Name")?.Value;
			if (sn.Name == "m_Unknown")
			{
				sn.Name = null;
			}
			sn.Comment = node.Attribute("Comments")?.Value;

			return sn;
		}
	}
}
