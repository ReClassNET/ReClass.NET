using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange
{
	[ContractClass(typeof(ICustomSchemaConverterContract))]
	public interface ICustomSchemaConverter
	{
		bool CanHandleElement(XElement element);

		bool CanHandleNode(BaseNode node);

		bool CanHandleSchema(SchemaCustomNode node);

		SchemaCustomNode CreateSchemaFromElement(XElement element, IReadOnlyDictionary<string, SchemaClassNode> classes, ILogger logger);
		XElement CreateElementFromSchema(SchemaCustomNode node, ILogger logger);

		SchemaCustomNode CreateSchemaFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger);
		BaseNode CreateNodeFromSchema(SchemaCustomNode schema, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger);
	}

	[ContractClassFor(typeof(ICustomSchemaConverter))]
	internal abstract class ICustomSchemaConverterContract : ICustomSchemaConverter
	{
		public bool CanHandleNode(BaseNode node)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public bool CanHandleElement(XElement element)
		{
			Contract.Requires(element != null);

			throw new NotImplementedException();
		}

		public bool CanHandleSchema(SchemaCustomNode node)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public SchemaCustomNode CreateSchemaFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<SchemaCustomNode>() != null);

			throw new NotImplementedException();
		}

		public SchemaCustomNode CreateSchemaFromElement(XElement element, IReadOnlyDictionary<string, SchemaClassNode> classes, ILogger logger)
		{
			Contract.Requires(element != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			throw new NotImplementedException();
		}

		public BaseNode CreateNodeFromSchema(SchemaCustomNode schema, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger)
		{
			Contract.Requires(schema != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<BaseNode>() != null);

			throw new NotImplementedException();
		}

		public XElement CreateElementFromSchema(SchemaCustomNode node, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<XElement>() != null);

			throw new NotImplementedException();
		}
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
			Contract.Requires(element != null);

			return converters.Where(c => c.CanHandleElement(element)).FirstOrDefault();
		}

		public static ICustomSchemaConverter GetReadConverter(BaseNode node)
		{
			Contract.Requires(node != null);

			return converters.Where(c => c.CanHandleNode(node)).FirstOrDefault();
		}

		public static ICustomSchemaConverter GetWriteConverter(SchemaCustomNode node)
		{
			Contract.Requires(node != null);

			return converters.Where(c => c.CanHandleSchema(node)).FirstOrDefault();
		}
	}
}
