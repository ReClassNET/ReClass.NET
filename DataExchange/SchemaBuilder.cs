using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.DataExchange
{
	/// <summary>Converts nodes to schema nodes and vice verca.</summary>
	public class SchemaBuilder
	{
		/// <summary>This map contains the mapping to build in nodes.</summary>
		private static readonly Dictionary<SchemaType, Type> SchemaTypeToNodeTypeMap = new Dictionary<SchemaType, Type>
		{
			[SchemaType.Array] = typeof(ClassInstanceArrayNode),
			[SchemaType.ClassInstance] = typeof(ClassInstanceNode),
			[SchemaType.Class] = typeof(ClassNode),
			[SchemaType.ClassPtr] = typeof(ClassPtrNode),
			[SchemaType.ClassPtrArray] = typeof(ClassPtrArrayNode),
			[SchemaType.Double] = typeof(DoubleNode),
			[SchemaType.Float] = typeof(FloatNode),
			[SchemaType.FunctionPtr] = typeof(FunctionPtrNode),
			[SchemaType.Hex8] = typeof(Hex8Node),
			[SchemaType.Hex16] = typeof(Hex16Node),
			[SchemaType.Hex32] = typeof(Hex32Node),
			[SchemaType.Hex64] = typeof(Hex64Node),
			[SchemaType.Int8] = typeof(Int8Node),
			[SchemaType.Int16] = typeof(Int16Node),
			[SchemaType.Int32] = typeof(Int32Node),
			[SchemaType.Int64] = typeof(Int64Node),
			[SchemaType.Matrix3x3] = typeof(Matrix3x3Node),
			[SchemaType.Matrix3x4] = typeof(Matrix3x4Node),
			[SchemaType.Matrix4x4] = typeof(Matrix4x4Node),
			[SchemaType.UInt8] = typeof(UInt8Node),
			[SchemaType.UInt16] = typeof(UInt16Node),
			[SchemaType.UInt32] = typeof(UInt32Node),
			[SchemaType.UInt64] = typeof(UInt64Node),
			[SchemaType.UTF8Text] = typeof(UTF8TextNode),
			[SchemaType.UTF8TextPtr] = typeof(UTF8TextPtrNode),
			[SchemaType.UTF16Text] = typeof(UTF16TextNode),
			[SchemaType.UTF16TextPtr] = typeof(UTF16TextPtrNode),
			[SchemaType.UTF32Text] = typeof(UTF32TextNode),
			[SchemaType.UTF32TextPtr] = typeof(UTF32TextPtrNode),
			[SchemaType.Vector2] = typeof(Vector2Node),
			[SchemaType.Vector3] = typeof(Vector3Node),
			[SchemaType.Vector4] = typeof(Vector4Node),
			[SchemaType.VTable] = typeof(VTableNode),
			[SchemaType.BitField] = typeof(BitFieldNode)
		};
		private static readonly Dictionary<Type, SchemaType> NodeTypeToSchemaTypeMap = SchemaTypeToNodeTypeMap.ToDictionary(kp => kp.Value, kp => kp.Key);

		private readonly List<SchemaClassNode> schema;

		private SchemaBuilder(IEnumerable<SchemaClassNode> classes)
		{
			Contract.Requires(classes != null);
			Contract.Ensures(schema != null);

			schema = classes.ToList();
		}

		/// <summary>Initializes this object from the given list of <see cref="SchemaClassNode"/>.</summary>
		/// <param name="classes">The classes.</param>
		/// <returns>An instance of <see cref="SchemaBuilder"/>.</returns>
		public static SchemaBuilder FromSchema(IEnumerable<SchemaClassNode> classes)
		{
			Contract.Requires(classes != null);
			Contract.Ensures(Contract.Result<SchemaBuilder>() != null);

			return new SchemaBuilder(classes);
		}

		/// <summary>Initializes this object from the given list of <see cref="ClassNode"/>.</summary>
		/// <param name="classes">The classes.</param>
		/// <param name="logger">The logger.</param>
		/// <returns>An instance of <see cref="SchemaBuilder"/> which contains the schema of the classes.</returns>
		public static SchemaBuilder FromClasses(IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(classes != null);
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<SchemaBuilder>() != null);

			var classMap = classes.ToDictionary(
				c => c,
				c => new SchemaClassNode
				{
					Name = c.Name,
					AddressFormula = c.AddressFormula,
					Comment = c.Comment
				}
			);

			foreach (var classNode in classes)
			{
				var schemaClass = classMap[classNode];

				foreach (var node in classNode.Nodes)
				{
					SchemaNode schemaNode;
					if (TryCreateSchemaFromNode(node, classMap, logger, out schemaNode))
					{
						schemaClass.Nodes.Add(schemaNode);
					}
				}
			}

			return new SchemaBuilder(classMap.Values);
		}

		public static bool TryCreateSchemaFromNode(BaseNode node, IReadOnlyDictionary<ClassNode, SchemaClassNode> classes, ILogger logger, out SchemaNode schemaNode)
		{
			Contract.Requires(node != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			schemaNode = null;

			SchemaType type;
			if (!NodeTypeToSchemaTypeMap.TryGetValue(node.GetType(), out type))
			{
				var converter = CustomSchemaConvert.GetReadConverter(node);
				if (converter != null)
				{
					schemaNode = converter.CreateSchemaFromNode(node, classes, logger);

					return true;
				}

				logger.Log(LogLevel.Error, $"Skipping node with unknown type: {node.GetType()}");

				return false;
			}

			if (node is BaseReferenceNode)
			{
				schemaNode = new SchemaReferenceNode(
					type,
					classes[(node as BaseReferenceNode).InnerNode as ClassNode]
				);
			}
			else if (node is VTableNode)
			{
				var vtableNode = new SchemaVTableNode();

				vtableNode.Nodes.AddRange(((VTableNode)node).Nodes.Select(m => new SchemaNode(SchemaType.None)
				{
					Name = m.Name,
					Comment = m.Comment
				}));

				schemaNode = vtableNode;
			}
			else
			{
				schemaNode = new SchemaNode(type);
			}
			schemaNode.Name = node.Name;
			schemaNode.Comment = node.Comment;

			var arrayNode = node as BaseArrayNode;
			if (arrayNode != null)
			{
				schemaNode.Count = arrayNode.Count;
			}
			var textNode = node as BaseTextNode;
			if (textNode != null)
			{
				schemaNode.Count = textNode.CharacterCount;
			}
			var bitFieldNode = node as BitFieldNode;
			if (bitFieldNode != null)
			{
				schemaNode.Count = bitFieldNode.Bits;
			}

			return true;
		}

		/// <summary>Builds the schema.</summary>
		/// <returns>A list of <see cref="SchemaClassNode"/>.</returns>
		public List<SchemaClassNode> BuildSchema()
		{
			Contract.Ensures(Contract.Result<IList<SchemaClassNode>>() != null);

			return schema;
		}

		/// <summary>Builds the nodes.</summary>
		/// <param name="logger">The logger.</param>
		/// <returns>A list of.</returns>
		public List<ClassNode> BuildNodes(ILogger logger)
		{
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<IList<ClassNode>>() != null);

			var classes = schema.ToDictionary(
				sc => sc,
				sc => new ClassNode
				{
					Name = sc.Name ?? string.Empty,
					AddressFormula = sc.AddressFormula ?? string.Empty,
					Comment = sc.Comment ?? string.Empty
				}
			);

			foreach (var schemaClassNode in schema)
			{
				var classNode = classes[schemaClassNode];
				foreach (var schemaNode in schemaClassNode.Nodes)
				{
					// Special case padding type (known as Custom in original ReClass)
					if (schemaNode.Type == SchemaType.Padding)
					{
						var size = schemaNode.Count;
						while (size != 0)
						{
							BaseNode node = null;
#if WIN64
							if (size >= 8)
							{
								node = new Hex64Node();
							}
							else 
#endif
							if (size >= 4)
							{
								node = new Hex32Node();
							}
							else if (size >= 2)
							{
								node = new Hex16Node();
							}
							else
							{
								node = new Hex8Node();
							}

							node.Comment = schemaNode.Comment ?? string.Empty;

							size -= node.MemorySize;

							classNode.AddNode(node);
						}
					}
					else
					{
						BaseNode node;
						if (TryCreateNodeFromSchema(schemaNode, classNode, classes, logger, out node))
						{
							classNode.AddNode(node);
						}
					}
				}
			}

			classes.Values.ForEach(c => c.UpdateOffsets());

			return classes.Values.ToList();
		}

		public static bool TryCreateNodeFromSchema(SchemaNode schemaNode, ClassNode parentNode, IReadOnlyDictionary<SchemaClassNode, ClassNode> classes, ILogger logger, out BaseNode node)
		{
			Contract.Requires(schemaNode != null);
			Contract.Requires(parentNode != null);
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes.Keys, k => k != null));
			Contract.Requires(Contract.ForAll(classes.Values, v => v != null));
			Contract.Requires(logger != null);

			node = null;

			if (schemaNode.Type == SchemaType.Custom)
			{
				var converter = CustomSchemaConvert.GetWriteConverter(schemaNode);
				if (converter != null)
				{
					node = converter.CreateNodeFromSchema(schemaNode, classes, logger);

					return true;
				}

				logger.Log(LogLevel.Error, $"Skipping node with unhandled type: {schemaNode.GetType()}");

				return false;
			}

			node = Activator.CreateInstance(SchemaTypeToNodeTypeMap[schemaNode.Type]) as BaseNode;
			if (node == null)
			{
				logger.Log(LogLevel.Error, $"Could not create node of type: {SchemaTypeToNodeTypeMap[schemaNode.Type]}");

				return false;
			}

			if (!string.IsNullOrEmpty(schemaNode.Name))
			{
				node.Name = schemaNode.Name;
			}
			node.Comment = schemaNode.Comment ?? string.Empty;

			var referenceNode = node as BaseReferenceNode;
			if (referenceNode != null)
			{
				var schemaReferenceNode = schemaNode as SchemaReferenceNode;

				if (referenceNode.PerformCycleCheck && !ClassManager.IsCycleFree(parentNode, classes[schemaReferenceNode.InnerNode], classes.Values))
				{
					logger.Log(LogLevel.Error, $"Skipping node with cycle reference: {schemaReferenceNode.InnerNode.Name}->{node.Name}");

					return false;
				}

				referenceNode.ChangeInnerNode(classes[schemaReferenceNode.InnerNode]);
			}

			var vtableNode = node as VTableNode;
			if (vtableNode != null)
			{
				(schemaNode as SchemaVTableNode).Nodes.Select(n => new VMethodNode()
				{
					Name = n.Name,
					Comment = n.Comment
				}).ForEach(n => vtableNode.AddNode(n));
			}

			if (schemaNode.Count > 0)
			{
				var arrayNode = node as BaseArrayNode;
				if (arrayNode != null)
				{
					arrayNode.Count = schemaNode.Count;
				}
				var textNode = node as BaseTextNode;
				if (textNode != null)
				{
					textNode.CharacterCount = schemaNode.Count;
				}
				var bitFieldNode = node as BitFieldNode;
				if (bitFieldNode != null)
				{
					bitFieldNode.Bits = schemaNode.Count;
				}
			}

			return true;
		}
	}
}
