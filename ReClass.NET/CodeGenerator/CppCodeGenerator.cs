using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;
using ReClassNET.UI;

namespace ReClassNET.CodeGenerator
{
	public delegate string GetTypeDefinitionFunc(BaseNode node, ILogger logger);

	public delegate string ResolveWrappedTypeFunc(BaseNode node, bool isAnonymousExpression, ILogger logger);

	public interface ICustomCppCodeGenerator
	{
		bool CanHandle(BaseNode node);

		BaseNode TransformNode(BaseNode node);

		string GetTypeDefinition(BaseNode node, GetTypeDefinitionFunc defaultGetTypeDefinitionFunc, ResolveWrappedTypeFunc defaultResolveWrappedTypeFunc, ILogger logger);
	}

	public class CppCodeGenerator : ICodeGenerator
	{
		#region Custom Code Generators

		private static readonly ISet<ICustomCppCodeGenerator> customGenerators = new HashSet<ICustomCppCodeGenerator>();

		public static void Add(ICustomCppCodeGenerator generator)
		{
			customGenerators.Add(generator);
		}

		public static void Remove(ICustomCppCodeGenerator generator)
		{
			customGenerators.Remove(generator);
		}

		private static ICustomCppCodeGenerator GetCustomCodeGeneratorForNode(BaseNode node)
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
			public override Size Draw(ViewInfo view, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(ViewInfo view) => throw new NotImplementedException();
		}

		private class Utf16CharacterNode : BaseNode
		{
			public override int MemorySize => throw new NotImplementedException();
			public override void GetUserInterfaceInfo(out string name, out Image icon) => throw new NotImplementedException();
			public override Size Draw(ViewInfo view, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(ViewInfo view) => throw new NotImplementedException();
		}

		private class Utf32CharacterNode : BaseNode
		{
			public override int MemorySize => throw new NotImplementedException();
			public override void GetUserInterfaceInfo(out string name, out Image icon) => throw new NotImplementedException();
			public override Size Draw(ViewInfo view, int x, int y) => throw new NotImplementedException();
			public override int CalculateDrawnHeight(ViewInfo view) => throw new NotImplementedException();
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
				[typeof(Matrix3x3Node)] = typeMapping.TypeMatrix3x3,
				[typeof(Matrix3x4Node)] = typeMapping.TypeMatrix3x4,
				[typeof(Matrix4x4Node)] = typeMapping.TypeMatrix4x4,
				[typeof(UInt8Node)] = typeMapping.TypeUInt8,
				[typeof(UInt16Node)] = typeMapping.TypeUInt16,
				[typeof(UInt32Node)] = typeMapping.TypeUInt32,
				[typeof(UInt64Node)] = typeMapping.TypeUInt64,
				[typeof(Utf8CharacterNode)] = typeMapping.TypeUtf8Text,
				[typeof(Utf16CharacterNode)] = typeMapping.TypeUtf16Text,
				[typeof(Utf32CharacterNode)] = typeMapping.TypeUtf32Text,
				[typeof(Vector2Node)] = typeMapping.TypeVector2,
				[typeof(Vector3Node)] = typeMapping.TypeVector3,
				[typeof(Vector4Node)] = typeMapping.TypeVector4
			};
		}

		public string GenerateCode(IEnumerable<ClassNode> classes, ILogger logger)
		{
			var classNodes = classes as IList<ClassNode> ?? classes.ToList();

			var sb = new StringBuilder();
			sb.AppendLine($"// Created with {Constants.ApplicationName} {Constants.ApplicationVersion} by {Constants.Author}");
			sb.AppendLine();
			sb.AppendLine(
				string.Join(
					Environment.NewLine + Environment.NewLine,
					// Skip class which contains FunctionNodes because these are not data classes.
					OrderByInheritance(classNodes.Where(c => c.Nodes.None(n => n is FunctionNode))).Select(c =>
					{
						var csb = new StringBuilder();
						csb.Append($"class {c.Name}");

						bool skipFirstMember = false;
						if (c.Nodes.FirstOrDefault() is ClassInstanceNode inheritedFromNode)
						{
							skipFirstMember = true;

							csb.Append(" : public ");
							csb.Append(inheritedFromNode.InnerNode.Name);
						}

						if (!string.IsNullOrEmpty(c.Comment))
						{
							csb.Append($" // {c.Comment}");
						}
						csb.AppendLine();

						csb.AppendLine("{");
						csb.AppendLine("public:");
						csb.AppendLine(
							string.Join(
								Environment.NewLine,
								GetTypeDeclerationsForNodes(c.Nodes.Skip(skipFirstMember ? 1 : 0).WhereNot(n => n is FunctionNode), logger)
									.Select(s => "\t" + s)
							)
						);

						var vTableNodes = c.Nodes.OfType<VirtualMethodTableNode>().ToList();
						if (vTableNodes.Any())
						{
							csb.AppendLine();
							csb.AppendLine(
								string.Join(
									Environment.NewLine,
									vTableNodes.SelectMany(vt => vt.Nodes).OfType<VirtualMethodNode>().Select(m => $"\tvirtual void {m.MethodName}();")
								)
							);
						}

						var functionNodes = classNodes.SelectMany(c2 => c2.Nodes).OfType<FunctionNode>().Where(f => f.BelongsToClass == c).ToList();
						if (functionNodes.Any())
						{
							csb.AppendLine();
							csb.AppendLine(
								string.Join(
									Environment.NewLine,
									functionNodes.Select(f => $"\t{f.Signature} {{ }}")
								)
							);
						}

						csb.Append($"}}; //Size: 0x{c.MemorySize:X04}");
						return csb.ToString();
					})
				)
			);

			return sb.ToString();
		}

		private static IEnumerable<ClassNode> OrderByInheritance(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);
			Contract.Requires(Contract.ForAll(classes, c => c != null));
			Contract.Ensures(Contract.Result<IEnumerable<ClassNode>>() != null);

			var alreadySeen = new HashSet<ClassNode>();

			return classes
				.SelectMany(c => YieldReversedHierarchy(c, alreadySeen))
				.Distinct();
		}

		private static IEnumerable<ClassNode> YieldReversedHierarchy(ClassNode node, ISet<ClassNode> alreadySeen)
		{
			Contract.Requires(node != null);
			Contract.Requires(alreadySeen != null);
			Contract.Requires(Contract.ForAll(alreadySeen, c => c != null));
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
				.SelectMany(c => YieldReversedHierarchy(c, alreadySeen))
				.Append(node);
		}

		private IEnumerable<string> GetTypeDeclerationsForNodes(IEnumerable<BaseNode> members, ILogger logger)
		{
			Contract.Requires(members != null);
			Contract.Requires(Contract.ForAll(members, m => m != null));
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<string>>(), d => d != null));

			int fill = 0;
			int fillStart = 0;

			BaseNode CreatePaddingMember(int offset, int count)
			{
				var node = new ArrayNode
				{
					Offset = (IntPtr)offset,
					Count = count,
					Name = $"pad_{offset:X04}"
				};

				node.ChangeInnerNode(new Utf8CharacterNode());

				return node;
			}

			foreach (var member in members.WhereNot(m => m is VirtualMethodTableNode))
			{
				if (member is BaseHexNode)
				{
					if (fill == 0)
					{
						fillStart = member.Offset.ToInt32();
					}
					fill += member.MemorySize;

					continue;
				}

				if (fill != 0)
				{
					yield return GetTypeDeclerationForNode(CreatePaddingMember(fillStart, fill), logger);

					fill = 0;
				}

				var definition = GetTypeDeclerationForNode(member, logger);
				if (definition != null)
				{
					yield return definition;
				}
				else
				{
					logger.Log(LogLevel.Error, $"Skipping node with unhandled type: {member.GetType()}");
				}
			}

			if (fill != 0)
			{
				yield return GetTypeDeclerationForNode(CreatePaddingMember(fillStart, fill), logger);
			}
		}

		private string GetTypeDefinition(BaseNode node, ILogger logger)
		{
			var custom = GetCustomCodeGeneratorForNode(node);
			if (custom != null)
			{
				return custom.GetTypeDefinition(node, GetTypeDefinition, ResolveWrappedType, logger);
			}

			if (nodeTypeToTypeDefinationMap.TryGetValue(node.GetType(), out var type))
			{
				return type;
			}

			if (node is ClassInstanceNode classInstanceNode)
			{
				return $"class {classInstanceNode.InnerNode.Name}";
			}

			return null;
		}

		private string GetTypeDeclerationForNode(BaseNode node, ILogger logger)
		{
			var transformedNode = TransformNode(node);

			var simpleType = GetTypeDefinition(transformedNode, logger);
			if (simpleType != null)
			{
				return NodeToString(node, simpleType);
			}

			if (transformedNode is BaseWrapperNode wrapperNode)
			{
				var sb = new StringBuilder();

				sb.Append(ResolveWrappedType(node, false, logger));

				sb.Append($"; //0x{node.Offset.ToInt32():X04} {node.Comment}".Trim());

				return sb.ToString();
			}

			return null;
		}

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

		private static BaseNode TransformNode(BaseNode node)
		{
			var custom = GetCustomCodeGeneratorForNode(node);
			if (custom != null)
			{
				return custom.TransformNode(node);
			}

			BaseNode GetCharacterNodeForEncoding(Encoding encoding)
			{
				if (encoding.Equals(Encoding.Unicode))
				{
					return new Utf16CharacterNode();
				}
				if (encoding.Equals(Encoding.UTF32))
				{
					return new Utf32CharacterNode();
				}
				return new Utf8CharacterNode();
			}

			if (node is BaseTextNode textNode)
			{
				var arrayNode = new ArrayNode { Count = textNode.Length };
				arrayNode.CopyFromNode(node);
				arrayNode.ChangeInnerNode(GetCharacterNodeForEncoding(textNode.Encoding));
				return arrayNode;
			}

			if (node is BaseTextPtrNode textPtrNode)
			{
				var pointerNode = new PointerNode();
				pointerNode.CopyFromNode(node);
				pointerNode.ChangeInnerNode(GetCharacterNodeForEncoding(textPtrNode.Encoding));
				return pointerNode;
			}

			if (node is BitFieldNode bitFieldNode)
			{
				var underlayingNode = bitFieldNode.GetUnderlayingNode();
				underlayingNode.CopyFromNode(node);
				return underlayingNode;
			}

			if (node is BaseHexNode hexNode)
			{
				var arrayNode = new ArrayNode { Count = hexNode.MemorySize };
				arrayNode.CopyFromNode(node);
				arrayNode.ChangeInnerNode(new Utf8CharacterNode());
				return arrayNode;
			}

			return node;
		}

		private static string NodeToString(BaseNode node, string type)
		{
			Contract.Requires(node != null);
			Contract.Requires(type != null);

			return $"{type} {node.Name}; //0x{node.Offset.ToInt32():X04} {node.Comment}".Trim();
		}
	}
}
