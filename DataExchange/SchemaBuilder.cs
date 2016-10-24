using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.DataExchange
{
	public enum SchemaType
	{
		None,
		Array,
		ClassPtrArray,
		ClassInstance,
		Class,
		ClassPtr,
		Double,
		Float,
		FunctionPtr,
		Hex8,
		Hex16,
		Hex32,
		Hex64,
		Int8,
		Int16,
		Int32,
		Int64,
		Matrix3x3,
		Matrix3x4,
		Matrix4x4,
		UInt8,
		UInt16,
		UInt32,
		UInt64,
		UTF8Text,
		UTF8TextPtr,
		UTF16Text,
		UTF16TextPtr,
		UTF32Text,
		UTF32TextPtr,
		Vector2,
		Vector3,
		Vector4,
		VTable,
		VMethod,
		BitField,

		Padding,

		Custom
	}

	public class SchemaNode
	{
		public SchemaType Type { get; }
		public string Name { get; set; }
		public string Comment { get; set; }
		public int Count { get; set; } = -1;

		public SchemaNode(SchemaType type)
		{
			Type = type;
		}
	}

	public class SchemaCustomNode : SchemaNode
	{
		public SchemaCustomNode()
			: base(SchemaType.Custom)
		{

		}
	}

	public class SchemaReferenceNode : SchemaNode
	{
		public SchemaClassNode InnerNode { get; }

		public SchemaReferenceNode(SchemaType type, SchemaClassNode inner)
			: base(type)
		{
			Contract.Requires(inner != null);

			InnerNode = inner;
		}
	}

	public class SchemaVTableNode : SchemaNode
	{
		public List<SchemaNode> Nodes { get; } = new List<SchemaNode>();

		public SchemaVTableNode()
			: base(SchemaType.VTable)
		{

		}
	}

	public class SchemaClassNode : SchemaNode
	{
		public string AddressFormula { get; set; }
		public List<SchemaNode> Nodes { get; } = new List<SchemaNode>();

		public SchemaClassNode()
			: base(SchemaType.Class)
		{

		}
	}

	public class SchemaBuilder
	{
		private readonly List<SchemaClassNode> schema;

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

		private SchemaBuilder(IEnumerable<SchemaClassNode> classes)
		{
			Contract.Requires(classes != null);

			schema = classes.ToList();
		}

		public static SchemaBuilder FromSchema(IEnumerable<SchemaClassNode> classes)
		{
			Contract.Requires(classes != null);

			return new SchemaBuilder(classes);
		}

		public static SchemaBuilder FromNodes(IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(classes != null);

			var sclasses = classes.ToDictionary(
				c => c,
				c => new SchemaClassNode
				{
					Name = c.Name,
					AddressFormula = c.AddressFormula,
					Comment = c.Comment
				}
			);

			foreach (var c in classes)
			{
				var sc = sclasses[c];
				foreach (var n in c.Nodes)
				{
					SchemaType type;
					if (!NodeTypeToSchemaTypeMap.TryGetValue(n.GetType(), out type))
					{
						var converter = CustomSchemaConvert.GetReadConverter(n);
						if (converter != null)
						{
							sc.Nodes.Add(converter.ReadFromNode(n, sclasses, logger));
							continue;
						}

						logger.Log(LogLevel.Warning, $"Skipping node with unknown type: {n.GetType()}");

						continue;
					}

					SchemaNode node;
					if (n is BaseReferenceNode)
					{
						node = new SchemaReferenceNode(
							type,
							sclasses[(n as BaseReferenceNode).InnerNode as ClassNode]
						);
					}
					else if (n is VTableNode)
					{
						var vtableNode = new SchemaVTableNode();

						vtableNode.Nodes.AddRange(((VTableNode)n).Nodes.Select(m => new SchemaNode(SchemaType.None)
						{
							Name = m.Name,
							Comment = m.Comment
						}));

						node = vtableNode;
					}
					else
					{
						node = new SchemaNode(type);
					}
					node.Name = n.Name;
					node.Comment = n.Comment;

					var arrayNode = n as BaseArrayNode;
					if (arrayNode != null)
					{
						node.Count = arrayNode.Count;
					}
					var textNode = n as BaseTextNode;
					if (textNode != null)
					{
						node.Count = textNode.CharacterCount;
					}
					var bitFieldNode = n as BitFieldNode;
					if (bitFieldNode != null)
					{
						node.Count = bitFieldNode.Bits;
					}

					sc.Nodes.Add(node);
				}
			}

			return new SchemaBuilder(sclasses.Values);
		}

		public IList<SchemaClassNode> BuildSchema()
		{
			return schema;
		}

		public IList<ClassNode> BuildNodes(ILogger logger)
		{
			var classes = schema.ToDictionary(
				sc => sc,
				sc => new ClassNode
				{
					Name = sc.Name ?? string.Empty,
					AddressFormula = sc.AddressFormula ?? string.Empty,
					Comment = sc.Comment ?? string.Empty
				}
			);

			foreach (var sc in schema)
			{
				var cn = classes[sc];
				foreach (var sn in sc.Nodes)
				{
					// Special case padding type (known as Custom in original ReClass)
					if (sn.Type == SchemaType.Padding)
					{
						var size = sn.Count;
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

							node.Comment = sn.Comment ?? string.Empty;

							size -= node.MemorySize;

							cn.AddNode(node);
						}
					}
					else
					{
						if (sn is SchemaCustomNode)
						{
							var converter = CustomSchemaConvert.GetWriteConverter(sn as SchemaCustomNode);
							if (converter != null)
							{
								cn.AddNode(converter.WriteToNode(sn as SchemaCustomNode, classes, logger));
								continue;
							}

							logger.Log(LogLevel.Warning, $"Skipping node with unknown type: {sn.GetType()}");

							continue;
						}

						var node = Activator.CreateInstance(SchemaTypeToNodeTypeMap[sn.Type]) as BaseNode;
						node.Name = sn.Name ?? string.Empty;
						node.Comment = sn.Comment ?? string.Empty;

						var referenceNode = node as BaseReferenceNode;
						if (referenceNode != null)
						{
							var srn = sn as SchemaReferenceNode;
							referenceNode.InnerNode = classes[srn.InnerNode];
						}

						var vtableNode = node as VTableNode;
						if (vtableNode != null)
						{
							(sn as SchemaVTableNode).Nodes.Select(n => new VMethodNode()
							{
								Name = n.Name,
								Comment = n.Comment
							}).ForEach(n => vtableNode.AddNode(n));
						}

						if (sn.Count > 0)
						{
							var arrayNode = node as BaseArrayNode;
							if (arrayNode != null)
							{
								arrayNode.Count = sn.Count;
							}
							var textNode = node as BaseTextNode;
							if (textNode != null)
							{
								textNode.CharacterCount = sn.Count;
							}
							var bitFieldNode = node as BitFieldNode;
							if (bitFieldNode != null)
							{
								bitFieldNode.Bits = sn.Count;
							}
						}

						cn.AddNode(node);
					}
				}
			}

			classes.Values.ForEach(c => c.UpdateOffsets());

			return classes.Values.ToList();
		}
	}
}
