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
		/// <summary>Determine if the instance can handle the xml element.</summary>
		/// <param name="element">The xml element to check.</param>
		/// <returns>True if the instance can handle the element, false if not.</returns>
		bool CanHandleElement(XElement element);

		/// <summary>Determine if the instance can handle the node.</summary>
		/// <param name="node">The node.</param>
		/// <returns>True if the instance can handle the node, false if not.</returns>
		bool CanHandleNode(BaseNode node);

		/// <summary>Determine if the instance can handle the schema.</summary>
		/// <param name="node">The node.</param>
		/// <returns>True if the instance can handle the schema, false if not.</returns>
		bool CanHandleSchema(SchemaNode node);

		/// <summary>Creates a schema node from the xml element. This method gets only called if <see cref="CanHandleElement(XElement)"/> returned true.</summary>
		/// <param name="element">The element to create the schema from.</param>
		/// <param name="classes">The list of classes which correspond to the schema.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The schema node for the xml element.</returns>
		SchemaNode CreateSchemaFromElement(XElement element, IReadOnlyDictionary<string, SchemaClassNode> classes, ILogger logger);

		/// <summary>Creates a xml element from the schema node. This method gets only called if <see cref="CanHandleSchema(SchemaCustomNode)"/> returned true.</summary>
		/// <param name="node">The schema node to create the xml element from.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The xml element for the schema node.</returns>
		XElement CreateElementFromSchema(SchemaNode node, ILogger logger);

		/// <summary>Creates a schema node from the memory node. This method gets only called if <see cref="CanHandleNode(BaseNode)"/> returned true.</summary>
		/// <param name="node">The node to create the schema from.</param>
		/// <param name="classes">The list of classes which correspond to the schema.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The schema node for the memory node.</returns>
		SchemaNode CreateSchemaFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger);

		/// <summary>Creates a node from the schema node. This method gets only called if <see cref="CanHandleSchema(SchemaCustomNode)"/> returned true.</summary>
		/// <param name="schema">The schema node to create the memory node from.</param>
		/// <param name="classes">The list of classes which correspond to the schema.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The memory node for the schema node.</returns>
		BaseNode CreateNodeFromSchema(SchemaNode schema, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger);
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

		public bool CanHandleSchema(SchemaNode node)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public SchemaNode CreateSchemaFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<SchemaNode>() != null);

			throw new NotImplementedException();
		}

		public SchemaNode CreateSchemaFromElement(XElement element, IReadOnlyDictionary<string, SchemaClassNode> classes, ILogger logger)
		{
			Contract.Requires(element != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			throw new NotImplementedException();
		}

		public BaseNode CreateNodeFromSchema(SchemaNode schema, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger)
		{
			Contract.Requires(schema != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			Contract.Ensures(Contract.Result<BaseNode>() != null);

			throw new NotImplementedException();
		}

		public XElement CreateElementFromSchema(SchemaNode node, ILogger logger)
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

		public static ICustomSchemaConverter GetWriteConverter(SchemaNode node)
		{
			Contract.Requires(node != null);

			return converters.Where(c => c.CanHandleSchema(node)).FirstOrDefault();
		}
	}
}
