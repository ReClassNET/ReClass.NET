using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.DataExchange
{
	enum SchemaType
	{
		None,
		Array,
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
		VTable
	}

	class SchemaNode
	{
		public SchemaType Type { get; }
		public string Name { get; set; }
		public string Comment { get; set; }

		public SchemaNode(SchemaType type)
		{
			Type = type;
		}
	}

	class SchemaReferenceNode : SchemaNode
	{
		public SchemaClassNode InnerNode { get; }

		public SchemaReferenceNode(SchemaType type, SchemaClassNode inner)
			: base(type)
		{
			InnerNode = inner;
		}
	}

	class SchemaClassNode : SchemaNode
	{
		public IntPtr Offset { get; set; }
		public List<SchemaNode> Nodes { get; } = new List<SchemaNode>();

		public SchemaClassNode()
			: base(SchemaType.Class)
		{

		}
	}

	class SchemaBuilder
	{
		private readonly List<SchemaClassNode> schema;

		private static Dictionary<SchemaType, Type> SchemaTypeToNodeTypeMap = new Dictionary<SchemaType, Type>
		{
			[SchemaType.Array] = typeof(ArrayNode),
			[SchemaType.ClassInstance] = typeof(ClassInstanceNode),
			[SchemaType.Class] = typeof(ClassNode),
			[SchemaType.ClassPtr] = typeof(ClassPtrNode),
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
			[SchemaType.VTable] = typeof(VTableNode)
		};
		private static Dictionary<Type, SchemaType> NodeTypeToSchemaTypeMap = SchemaTypeToNodeTypeMap.ToDictionary(kp => kp.Value, kp => kp.Key);

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

		public static SchemaBuilder FromNodes(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);

			var sclasses = classes.ToDictionary(
				c => c,
				c => new SchemaClassNode
				{
					Name = c.Name,
					Offset = c.Offset,
					Comment = c.Comment
				}
			);

			foreach (var c in classes)
			{
				var sc = sclasses[c];
				foreach (var n in c.Nodes)
				{
					SchemaNode node;
					if (n is BaseReferenceNode)
					{
						node = new SchemaReferenceNode(
							NodeTypeToSchemaTypeMap[n.GetType()],
							sclasses[(n as BaseReferenceNode).InnerNode as ClassNode]
						);
					}
					else
					{
						node = new SchemaNode(
							NodeTypeToSchemaTypeMap[n.GetType()]
						);
					}
					node.Name = n.Name;
					node.Comment = n.Comment;

					sc.Nodes.Add(node);
				}
			}

			return new SchemaBuilder(sclasses.Values);
		}

		public IList<SchemaClassNode> BuildSchema()
		{
			return schema;
		}

		public IList<ClassNode> BuildNodes()
		{
			var classes = schema.ToDictionary(
				sc => sc,
				sc => new ClassNode(false)
				{
					Name = sc.Name,
					Offset = sc.Offset,
					Comment = sc.Comment
				}
			);

			foreach (var sc in schema)
			{
				var cn = classes[sc];
				foreach (var sn in sc.Nodes)
				{
					var node = Activator.CreateInstance(SchemaTypeToNodeTypeMap[sn.Type]) as BaseNode;
					node.Name = sn.Name;
					node.Comment = sn.Comment;

					var referenceNode = node as BaseReferenceNode;
					if (referenceNode != null)
					{
						referenceNode.InnerNode = classes[(sn as SchemaReferenceNode).InnerNode];
					}

					cn.AddNode(node);
				}
			}

			classes.Values.ForEach(c => c.UpdateOffsets());

			return classes.Values.ToList();
		}
	}
}
