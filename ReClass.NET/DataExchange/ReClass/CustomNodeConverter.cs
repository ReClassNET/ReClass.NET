using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass
{
	[ContractClass(typeof(CustomNodeConverterContract))]
	public interface ICustomNodeConverter
	{
		/// <summary>Determine if the instance can handle the xml element.</summary>
		/// <param name="element">The xml element to check.</param>
		/// <returns>True if the instance can handle the element, false if not.</returns>
		bool CanHandleElement(XElement element);

		/// <summary>Determine if the instance can handle the node.</summary>
		/// <param name="node">The node to check.</param>
		/// <returns>True if the instance can handle the node, false if not.</returns>
		bool CanHandleNode(BaseNode node);

		/// <summary>Creates a node from the xml element. This method gets only called if <see cref="CanHandleElement(XElement)"/> returned true.</summary>
		/// <param name="element">The element to create the node from.</param>
		/// <param name="parent">The parent of the node.</param>
		/// <param name="classes">The list of classes which correspond to the node.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <param name="node">[out] The node for the xml element.</param>
		/// <returns>True if a node was created, otherwise false.</returns>
		bool TryCreateNodeFromElement(XElement element, ClassNode parent, IEnumerable<ClassNode> classes, ILogger logger, out BaseNode node);

		/// <summary>Creates a xml element from the node. This method gets only called if <see cref="CanHandleNode(BaseNode)"/> returned true.</summary>
		/// <param name="node">The node to create the xml element from.</param>
		/// <param name="logger">The logger used to output messages.</param>
		/// <returns>The xml element for the node.</returns>
		XElement CreateElementFromNode(BaseNode node, ILogger logger);
	}

	[ContractClassFor(typeof(ICustomNodeConverter))]
	internal abstract class CustomNodeConverterContract : ICustomNodeConverter
	{
		public bool CanHandleElement(XElement element)
		{
			Contract.Requires(element != null);

			throw new NotImplementedException();
		}

		public bool CanHandleNode(BaseNode node)
		{
			Contract.Requires(node != null);

			throw new NotImplementedException();
		}

		public bool TryCreateNodeFromElement(XElement element, ClassNode parent, IEnumerable<ClassNode> classes, ILogger logger, out BaseNode node)
		{
			Contract.Requires(element != null);
			Contract.Requires(CanHandleElement(element));
			Contract.Requires(parent != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));
			Contract.Requires(logger != null);

			throw new NotImplementedException();
		}

		public XElement CreateElementFromNode(BaseNode node, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(CanHandleNode(node));
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<XElement>() != null);

			throw new NotImplementedException();
		}
	}

	public class CustomNodeConvert
	{
		private static readonly List<ICustomNodeConverter> converters = new List<ICustomNodeConverter>();

		public static void RegisterCustomType(ICustomNodeConverter converter)
		{
			Contract.Requires(converter != null);

			converters.Add(converter);
		}

		public static void DeregisterCustomType(ICustomNodeConverter converter)
		{
			Contract.Requires(converter != null);

			converters.Remove(converter);
		}

		public static ICustomNodeConverter GetReadConverter(XElement element)
		{
			Contract.Requires(element != null);

			return converters.FirstOrDefault(c => c.CanHandleElement(element));
		}

		public static ICustomNodeConverter GetWriteConverter(BaseNode node)
		{
			Contract.Requires(node != null);

			return converters.FirstOrDefault(c => c.CanHandleNode(node));
		}
	}
}
