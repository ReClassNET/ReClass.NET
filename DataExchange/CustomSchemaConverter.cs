using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange
{
	public interface ICustomSchemaConverter
	{
		bool CanReadNode(XElement element);

		bool CanReadNode(BaseNode node);

		bool CanWriteNode(SchemaCustomNode node);

		SchemaCustomNode ReadFromXml(XElement element);
		XElement WriteToXml(SchemaCustomNode node);

		SchemaCustomNode ReadFromNode(BaseNode node);
		BaseNode WriteToNode(SchemaCustomNode schema);
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
