using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.CodeGenerator
{
	public delegate void WriteNodeFunc(IndentedTextWriter writer, BaseNode node, ILogger logger);

	public delegate string GetTypeDefinitionFunc(BaseNode node, ILogger logger);

	public delegate string ResolveWrappedTypeFunc(BaseNode node, bool isAnonymousExpression, ILogger logger);

	/// <summary>
	/// A C++ code generator for custom nodes.
	/// </summary>
	public abstract class CustomCppCodeGenerator
	{
		/// <summary>
		/// Returns <c>true</c> if the code generator can handle the given node.
		/// </summary>
		/// <param name="node">The node to check.</param>
		/// <returns>True if the code generator can handle the given node, false otherwise.</returns>
		public abstract bool CanHandle(BaseNode node);

		/// <summary>
		/// Outputs the C++ code for the node to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="node">The node to output.</param>
		/// <param name="defaultWriteNodeFunc">The default implementation of <see cref="CppCodeGenerator.WriteNode"/>.</param>
		/// <param name="logger">The logger.</param>
		/// <returns>True if the code generator has processed the node, false otherwise. If this method returns false, the default implementation is used.</returns>
		public virtual bool WriteNode(IndentedTextWriter writer, BaseNode node, WriteNodeFunc defaultWriteNodeFunc, ILogger logger)
		{
			return false;
		}

		/// <summary>
		/// Transforms the given node if necessary.
		/// </summary>
		/// <param name="node">The node to transform.</param>
		/// <returns>The transformed node.</returns>
		public virtual BaseNode TransformNode(BaseNode node)
		{
			return node;
		}

		/// <summary>
		/// Gets the type definition for the node. If the node is not a simple node <c>null</c> is returned.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="defaultGetTypeDefinitionFunc">The default implementation of <see cref="CppCodeGenerator.GetTypeDefinition"/>.</param>
		/// <param name="defaultResolveWrappedTypeFunc">The default implementation of <see cref="CppCodeGenerator.ResolveWrappedType"/>.</param>
		/// <param name="logger">The logger.</param>
		/// <returns>The type definition for the node or null if no simple type is available.</returns>
		public virtual string GetTypeDefinition(BaseNode node, GetTypeDefinitionFunc defaultGetTypeDefinitionFunc, ResolveWrappedTypeFunc defaultResolveWrappedTypeFunc, ILogger logger)
		{
			return null;
		}
	}

	public class CppCodeGenerator : ICodeGenerator
	{
		#region Custom Code Generators

		private static readonly ISet<CustomCppCodeGenerator> customGenerators = new HashSet<CustomCppCodeGenerator>();

		public static void Add(CustomCppCodeGenerator generator)
		{
			customGenerators.Add(generator);
		}

		public static void Remove(CustomCppCodeGenerator generator)
		{
			customGenerators.Remove(generator);
		}

		private static CustomCppCodeGenerator GetCustomCodeGeneratorForNode(BaseNode node)
		{
			return customGenerators.FirstOrDefault(g => g.CanHandle(node));
		}

		#endregion

		private readonly Dictionary<Type, string> nodeTypeToTypeDefinationMap;

		#region HelperNodes

		private class Utf8CharacterNode : BaseNode
		{
			public override int MemorySize => throw new NotImplementedException();
			public override void GetUserInterfaceInfo(out string name, out Image icon) => throw new NotImplementedException();
			public override Size Draw(DrawContext context, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(DrawContext context) => throw new NotImplementedException();
		}

		private class Utf16CharacterNode : BaseNode
		{
			public override int MemorySize => throw new NotImplementedException();
			public override void GetUserInterfaceInfo(out string name, out Image icon) => throw new NotImplementedException();
			public override Size Draw(DrawContext context, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(DrawContext context) => throw new NotImplementedException();
		}

		private class Utf32CharacterNode : BaseNode
		{
			public override int MemorySize => throw new NotImplementedException();
			public override void GetUserInterfaceInfo(out string name, out Image icon) => throw new NotImplementedException();
			public override Size Draw(DrawContext context, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(DrawContext context) => throw new NotImplementedException();
		}

		#endregion

		public Language Language => Language.Cpp;

		public CppCodeGenerator(CppTypeMapping typeMapping)
		{
			nodeTypeToTypeDefinationMap = new Dictionary<Type, string>
			{
				[typeof(BoolNode)] = typeMapping.TypeBool,
				[typeof(DoubleNode)] = typeMapping.TypeDouble,
				[typeof(FloatNode)] = typeMapping.TypeFloat,
				[typeof(FunctionPtrNode)] = typeMapping.TypeFunctionPtr,
				[typeof(Int8Node)] = typeMapping.TypeInt8,
				[typeof(Int16Node)] = typeMapping.TypeInt16,
				[typeof(Int32Node)] = typeMapping.TypeInt32,
				[typeof(Int64Node)] = typeMapping.TypeInt64,
				[typeof(NIntNode)] = typeMapping.TypeNInt,
				[typeof(Matrix3x3Node)] = typeMapping.TypeMatrix3x3,
				[typeof(Matrix3x4Node)] = typeMapping.TypeMatrix3x4,
				[typeof(Matrix4x4Node)] = typeMapping.TypeMatrix4x4,
				[typeof(UInt8Node)] = typeMapping.TypeUInt8,
				[typeof(UInt16Node)] = typeMapping.TypeUInt16,
				[typeof(UInt32Node)] = typeMapping.TypeUInt32,
				[typeof(UInt64Node)] = typeMapping.TypeUInt64,
				[typeof(NUIntNode)] = typeMapping.TypeNUInt,
				[typeof(Utf8CharacterNode)] = typeMapping.TypeUtf8Text,
				[typeof(Utf16CharacterNode)] = typeMapping.TypeUtf16Text,
				[typeof(Utf32CharacterNode)] = typeMapping.TypeUtf32Text,
				[typeof(Vector2Node)] = typeMapping.TypeVector2,
				[typeof(Vector3Node)] = typeMapping.TypeVector3,
				[typeof(Vector4Node)] = typeMapping.TypeVector4
			};
		}

		public string GenerateCode(IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ILogger logger)
		{
			using var sw = new StringWriter();
			using var iw = new IndentedTextWriter(sw, "\t");

			iw.WriteLine($"// Created with {Constants.ApplicationName} {Constants.ApplicationVersion} by {Constants.Author}");
			iw.WriteLine();

			using (var en = enums.GetEnumerator())
			{
				if (en.MoveNext())
				{
					WriteEnum(iw, en.Current);

					while (en.MoveNext())
					{
						iw.WriteLine();

						WriteEnum(iw, en.Current);
					}

					iw.WriteLine();
				}
			}

			var alreadySeen = new HashSet<ClassNode>();

			IEnumerable<ClassNode> GetReversedClassHierarchy(ClassNode node)
			{
				Contract.Requires(node != null);
				Contract.Ensures(Contract.Result<IEnumerable<ClassNode>>() != null);

				if (!alreadySeen.Add(node))
				{
					return Enumerable.Empty<ClassNode>();
				}

				var classNodes = node.Nodes
					.OfType<BaseWrapperNode>()
					.Where(w => !w.IsNodePresentInChain<PointerNode>()) // Pointers are forward declared
					.Select(w => w.ResolveMostInnerNode() as ClassNode)
					.Where(n => n != null);

				return classNodes
					.SelectMany(GetReversedClassHierarchy)
					.Append(node);
			}

			var classesToWrite = classes
				.Where(c => c.Nodes.None(n => n is FunctionNode)) // Skip class which contains FunctionNodes because these are not data classes.
				.SelectMany(GetReversedClassHierarchy) // Order the classes by their use hierarchy.
				.Distinct();

			using (var en = classesToWrite.GetEnumerator())
			{
				if (en.MoveNext())
				{
					WriteClass(iw, en.Current, classes, logger);

					while (en.MoveNext())
					{
						iw.WriteLine();

						WriteClass(iw, en.Current, classes, logger);
					}
				}
			}

			return sw.ToString();
		}

		/// <summary>
		/// Outputs the C++ code for the given enum to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="enum">The enum to output.</param>
		private void WriteEnum(IndentedTextWriter writer, EnumDescription @enum)
		{
			Contract.Requires(writer != null);
			Contract.Requires(@enum != null);

			writer.Write($"enum class {@enum.Name} : ");
			switch (@enum.Size)
			{
				case EnumDescription.UnderlyingTypeSize.OneByte:
					writer.WriteLine(nodeTypeToTypeDefinationMap[typeof(Int8Node)]);
					break;
				case EnumDescription.UnderlyingTypeSize.TwoBytes:
					writer.WriteLine(nodeTypeToTypeDefinationMap[typeof(Int16Node)]);
					break;
				case EnumDescription.UnderlyingTypeSize.FourBytes:
					writer.WriteLine(nodeTypeToTypeDefinationMap[typeof(Int32Node)]);
					break;
				case EnumDescription.UnderlyingTypeSize.EightBytes:
					writer.WriteLine(nodeTypeToTypeDefinationMap[typeof(Int64Node)]);
					break;
			}
			writer.WriteLine("{");
			writer.Indent++;
			for (var j = 0; j < @enum.Values.Count; ++j)
			{
				var kv = @enum.Values[j];

				writer.Write(kv.Key);
				writer.Write(" = ");
				writer.Write(kv.Value);
				if (j < @enum.Values.Count - 1)
				{
					writer.Write(",");
				}
				writer.WriteLine();
			}
			writer.Indent--;
			writer.WriteLine("};");
		}

		/// <summary>
		/// Outputs the C++ code for the given class to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="class">The class to output.</param>
		/// <param name="classes">The list of all available classes.</param>
		/// <param name="logger">The logger.</param>
		private void WriteClass(IndentedTextWriter writer, ClassNode @class, IEnumerable<ClassNode> classes, ILogger logger)
		{
			Contract.Requires(writer != null);
			Contract.Requires(@class != null);
			Contract.Requires(classes != null);

			writer.Write("class ");
			writer.Write(@class.Name);

			var skipFirstMember = false;
			if (@class.Nodes.FirstOrDefault() is ClassInstanceNode inheritedFromNode)
			{
				skipFirstMember = true;

				writer.Write(" : public ");
				writer.Write(inheritedFromNode.InnerNode.Name);
			}

			if (!string.IsNullOrEmpty(@class.Comment))
			{
				writer.Write(" // ");
				writer.Write(@class.Comment);
			}

			writer.WriteLine();
			
			writer.WriteLine("{");
			writer.WriteLine("public:");
			writer.Indent++;

			var nodes = @class.Nodes
				.Skip(skipFirstMember ? 1 : 0)
				.WhereNot(n => n is FunctionNode);
			WriteNodes(writer, nodes, logger);

			var vTableNodes = @class.Nodes.OfType<VirtualMethodTableNode>().ToList();
			if (vTableNodes.Any())
			{
				writer.WriteLine();

				var virtualMethodNodes = vTableNodes
					.SelectMany(vt => vt.Nodes)
					.OfType<VirtualMethodNode>();
				foreach (var node in virtualMethodNodes)
				{
					writer.Write("virtual void ");
					writer.Write(node.MethodName);
					writer.WriteLine("();");
				}
			}

			var functionNodes = classes
				.SelectMany(c2 => c2.Nodes)
				.OfType<FunctionNode>()
				.Where(f => f.BelongsToClass == @class)
				.ToList();
			if (functionNodes.Any())
			{
				writer.WriteLine();

				foreach (var node in functionNodes)
				{
					writer.Write(node.Signature);
					writer.WriteLine("{ }");
				}
			}

			writer.Indent--;
			writer.Write("}; //Size: 0x");
			writer.WriteLine($"{@class.MemorySize:X04}");

			writer.WriteLine($"static_assert(sizeof({@class.Name}) == 0x{@class.MemorySize:X});");
		}

		/// <summary>
		/// Outputs the C++ code for the given nodes to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="nodes">The nodes to output.</param>
		/// <param name="logger">The logger.</param>
		private void WriteNodes(IndentedTextWriter writer, IEnumerable<BaseNode> nodes, ILogger logger)
		{
			Contract.Requires(writer != null);
			Contract.Requires(nodes != null);

			var fill = 0;
			var fillStart = 0;

			static BaseNode CreatePaddingMember(int offset, int count)
			{
				var node = new ArrayNode
				{
					Offset = offset,
					Count = count,
					Name = $"pad_{offset:X04}"
				};

				node.ChangeInnerNode(new Utf8CharacterNode());

				return node;
			}

			foreach (var member in nodes.WhereNot(m => m is VirtualMethodTableNode))
			{
				if (member is BaseHexNode)
				{
					if (fill == 0)
					{
						fillStart = member.Offset;
					}
					fill += member.MemorySize;

					continue;
				}

				if (fill != 0)
				{
					WriteNode(writer, CreatePaddingMember(fillStart, fill), logger);

					fill = 0;
				}

				WriteNode(writer, member, logger);
			}

			if (fill != 0)
			{
				WriteNode(writer, CreatePaddingMember(fillStart, fill), logger);
			}
		}

		/// <summary>
		/// Outputs the C++ code for the given node to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="node">The node to output.</param>
		/// <param name="logger">The logger.</param>
		private void WriteNode(IndentedTextWriter writer, BaseNode node, ILogger logger)
		{
			Contract.Requires(writer != null);
			Contract.Requires(node != null);

			var custom = GetCustomCodeGeneratorForNode(node);
			if (custom != null)
			{
				if (custom.WriteNode(writer, node, WriteNode, logger))
				{
					return;
				}
			}

			node = TransformNode(node);

			var simpleType = GetTypeDefinition(node, logger);
			if (simpleType != null)
			{
				//$"{type} {node.Name}; //0x{node.Offset.ToInt32():X04} {node.Comment}".Trim();
				writer.Write(simpleType);
				writer.Write(" ");
				writer.Write(node.Name);
				writer.Write("; //0x");
				writer.Write($"{node.Offset:X04}");
				if (!string.IsNullOrEmpty(node.Comment))
				{
					writer.Write(" ");
					writer.Write(node.Comment);
				}
				writer.WriteLine();
			}
			else if (node is BaseWrapperNode)
			{
				writer.Write(ResolveWrappedType(node, false, logger));
				writer.Write("; //0x");
				writer.Write($"{node.Offset:X04}");
				if (!string.IsNullOrEmpty(node.Comment))
				{
					writer.Write(" ");
					writer.Write(node.Comment);
				}
				writer.WriteLine();
			}
			else if (node is UnionNode unionNode)
			{
				writer.Write("union //0x");
				writer.Write($"{node.Offset:X04}");
				if (!string.IsNullOrEmpty(node.Comment))
				{
					writer.Write(" ");
					writer.Write(node.Comment);
				}
				writer.WriteLine();
				writer.WriteLine("{");
				writer.Indent++;

				WriteNodes(writer, unionNode.Nodes, logger);

				writer.Indent--;

				writer.WriteLine($"}} {node.Name};");
			}
			else
			{
				logger.Log(LogLevel.Error, $"Skipping node with unhandled type: {node.GetType()}");
			}
		}

		/// <summary>
		/// Transforms the given node into some other node if necessary.
		/// </summary>
		/// <param name="node">The node to transform.</param>
		/// <returns>The transformed node.</returns>
		private static BaseNode TransformNode(BaseNode node)
		{
			var custom = GetCustomCodeGeneratorForNode(node);
			if (custom != null)
			{
				return custom.TransformNode(node);
			}

			static BaseNode GetCharacterNodeForEncoding(Encoding encoding)
			{
				if (encoding.IsSameCodePage(Encoding.Unicode))
				{
					return new Utf16CharacterNode();
				}
				if (encoding.IsSameCodePage(Encoding.UTF32))
				{
					return new Utf32CharacterNode();
				}
				return new Utf8CharacterNode();
			}

			switch (node)
			{
				case BaseTextNode textNode:
				{
					var arrayNode = new ArrayNode { Count = textNode.Length };
					arrayNode.CopyFromNode(node);
					arrayNode.ChangeInnerNode(GetCharacterNodeForEncoding(textNode.Encoding));
					return arrayNode;
				}
				case BaseTextPtrNode textPtrNode:
				{
					var pointerNode = new PointerNode();
					pointerNode.CopyFromNode(node);
					pointerNode.ChangeInnerNode(GetCharacterNodeForEncoding(textPtrNode.Encoding));
					return pointerNode;
				}
				case BitFieldNode bitFieldNode:
				{
					var underlayingNode = bitFieldNode.GetUnderlayingNode();
					underlayingNode.CopyFromNode(node);
					return underlayingNode;
				}
				case BaseHexNode hexNode:
				{
					var arrayNode = new ArrayNode { Count = hexNode.MemorySize };
					arrayNode.CopyFromNode(node);
					arrayNode.ChangeInnerNode(new Utf8CharacterNode());
					return arrayNode;
				}
			}

			return node;
		}

		/// <summary>
		/// Gets the type definition for the given node. If the node is not a simple node <c>null</c> is returned.
		/// </summary>
		/// <param name="node">The target node.</param>
		/// <param name="logger">The logger.</param>
		/// <returns>The type definition for the node or null if no simple type is available.</returns>
		private string GetTypeDefinition(BaseNode node, ILogger logger)
		{
			Contract.Requires(node != null);

			var custom = GetCustomCodeGeneratorForNode(node);
			if (custom != null)
			{
				return custom.GetTypeDefinition(node, GetTypeDefinition, ResolveWrappedType, logger);
			}

			if (nodeTypeToTypeDefinationMap.TryGetValue(node.GetType(), out var type))
			{
				return type;
			}

			switch (node)
			{
				case ClassInstanceNode classInstanceNode:
					return $"class {classInstanceNode.InnerNode.Name}";
				case EnumNode enumNode:
					return enumNode.Enum.Name;
			}

			return null;
		}

		/// <summary>
		/// Resolves the type of a <see cref="BaseWrapperNode"/> node (<see cref="PointerNode"/> and <see cref="ArrayNode"/>).
		/// </summary>
		/// <param name="node">The node to resolve.</param>
		/// <param name="isAnonymousExpression">Specify if the expression should be anonymous.</param>
		/// <param name="logger">The logger.</param>
		/// <returns>The resolved type of the node.</returns>
		private string ResolveWrappedType(BaseNode node, bool isAnonymousExpression, ILogger logger)
		{
			Contract.Requires(node != null);

			var sb = new StringBuilder();
			if (!isAnonymousExpression)
			{
				sb.Append(node.Name);
			}

			BaseNode lastWrapperNode = null;
			var currentNode = node;

			while (true)
			{
				currentNode = TransformNode(currentNode);

				if (currentNode is PointerNode pointerNode)
				{
					sb.Prepend('*');

					if (pointerNode.InnerNode == null) // void*
					{
						if (!isAnonymousExpression)
						{
							sb.Prepend(' ');
						}
						sb.Prepend("void");
						break;
					}

					lastWrapperNode = pointerNode;
					currentNode = pointerNode.InnerNode;
				}
				else if (currentNode is ArrayNode arrayNode)
				{
					if (lastWrapperNode is PointerNode)
					{
						sb.Prepend('(');
						sb.Append(')');
					}

					sb.Append($"[{arrayNode.Count}]");

					lastWrapperNode = arrayNode;
					currentNode = arrayNode.InnerNode;
				}
				else
				{
					var simpleType = GetTypeDefinition(currentNode, logger);

					if (!isAnonymousExpression)
					{
						sb.Prepend(' ');
					}

					sb.Prepend(simpleType);
					break;
				}
			}

			return sb.ToString().Trim();
		}
	}
}
