using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange
{
	public interface ICustomSchemaConverter
	{
		bool CanReadNode(XElement element);

		bool CanReadNode(BaseNode node);

		bool CanWriteNode(SchemaCustomNode node);

		SchemaCustomNode ReadFromXml(XElement element, IReadOnlyDictionary<string, SchemaClassNode> classes, ILogger logger);
		XElement WriteToXml(SchemaCustomNode node, ILogger logger);

		SchemaCustomNode ReadFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger);
		BaseNode WriteToNode(SchemaCustomNode schema, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger);
	}

	public class CustomSchemaConvert
	{
		private static readonly List<ICustomSchemaConverter> converters = new List<ICustomSchemaConverter>();

		public static void RegisterCustomType(ICustomSchemaConverter converter)
		{
			Contract.Requires(converter != null);

			converters.Add(converter);
		}

		public static void UnregisterCustomType(ICustomSchemaConverter converter)
		{
			Contract.Requires(converter != null);

			converters.Remove(converter);
		}

		public static ICustomSchemaConverter GetReadConverter(XElement element)
		{
			return converters.Where(c => c.CanReadNode(element)).FirstOrDefault();
		}

		public static ICustomSchemaConverter GetReadConverter(BaseNode node)
		{
			return converters.Where(c => c.CanReadNode(node)).FirstOrDefault();
		}

		public static ICustomSchemaConverter GetWriteConverter(SchemaCustomNode node)
		{
			return converters.Where(c => c.CanWriteNode(node)).FirstOrDefault();
		}
	}
}
