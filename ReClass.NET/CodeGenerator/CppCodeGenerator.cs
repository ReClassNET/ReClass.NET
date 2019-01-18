using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	public class CppCodeGenerator : ICodeGenerator
	{
		private readonly Dictionary<Type, string> typeToTypedefMap = new Dictionary<Type, string>
		{
			[typeof(BoolNode)] = Program.Settings.TypeBool,
			[typeof(DoubleNode)] = Program.Settings.TypeDouble,
			[typeof(FloatNode)] = Program.Settings.TypeFloat,
			[typeof(FunctionPtrNode)] = Program.Settings.TypeFunctionPtr,
			[typeof(Int8Node)] = Program.Settings.TypeInt8,
			[typeof(Int16Node)] = Program.Settings.TypeInt16,
			[typeof(Int32Node)] = Program.Settings.TypeInt32,
			[typeof(Int64Node)] = Program.Settings.TypeInt64,
			[typeof(Matrix3x3Node)] = Program.Settings.TypeMatrix3x3,
			[typeof(Matrix3x4Node)] = Program.Settings.TypeMatrix3x4,
			[typeof(Matrix4x4Node)] = Program.Settings.TypeMatrix4x4,
			[typeof(UInt8Node)] = Program.Settings.TypeUInt8,
			[typeof(UInt16Node)] = Program.Settings.TypeUInt16,
			[typeof(UInt32Node)] = Program.Settings.TypeUInt32,
			[typeof(UInt64Node)] = Program.Settings.TypeUInt64,
			[typeof(Utf8TextNode)] = Program.Settings.TypeUTF8Text,
			[typeof(Utf8TextPtrNode)] = Program.Settings.TypeUTF8TextPtr,
			[typeof(Utf16TextNode)] = Program.Settings.TypeUTF16Text,
			[typeof(Utf16TextPtrNode)] = Program.Settings.TypeUTF16TextPtr,
			[typeof(Utf32TextNode)] = Program.Settings.TypeUTF32Text,
			[typeof(Utf32TextPtrNode)] = Program.Settings.TypeUTF32TextPtr,
			[typeof(Vector2Node)] = Program.Settings.TypeVector2,
			[typeof(Vector3Node)] = Program.Settings.TypeVector3,
			[typeof(Vector4Node)] = Program.Settings.TypeVector4
		};

		public Language Language => Language.Cpp;

		public string GenerateCode(IEnumerable<ClassNode> classes, ILogger logger)
		{
			var classNodes = classes as IList<ClassNode> ?? classes.ToList();

			var sb = new StringBuilder();
			sb.AppendLine($"// Created with {Constants.ApplicationName} by {Constants.Author}");
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
								GetMemberDefinitionsForNodes(c.Nodes.Skip(skipFirstMember ? 1 : 0).WhereNot(n => n is FunctionNode), logger)
									.Select(MemberDefinitionToString)
									.Select(s => "\t" + s)
							)
						);

						var vTableNodes = c.Nodes.OfType<VTableNode>().ToList();
						if (vTableNodes.Any())
						{
							csb.AppendLine();
							csb.AppendLine(
								string.Join(
									Environment.NewLine,
									vTableNodes.SelectMany(vt => vt.Nodes).OfType<VMethodNode>().Select(m => $"\tvirtual void {m.MethodName}();")
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
				.Select(w => w.ResolveMostInnerNode() as ClassNode)
				.Where(n => n != null);

			return classNodes
				.SelectMany(c => YieldReversedHierarchy(c, alreadySeen))
				.Append(node);
		}

		private IEnumerable<MemberDefinition> GetMemberDefinitionsForNodes(IEnumerable<BaseNode> members, ILogger logger)
		{
			Contract.Requires(members != null);
			Contract.Requires(Contract.ForAll(members, m => m != null));
			Contract.Ensures(Contract.Result<IEnumerable<MemberDefinition>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<MemberDefinition>>(), d => d != null));

			int fill = 0;
			int fillStart = 0;

			foreach (var member in members.WhereNot(m => m is VTableNode))
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
					yield return new MemberDefinition(Program.Settings.TypePadding, fill, $"pad_{fillStart:X04}", fillStart, string.Empty);

					fill = 0;
				}

				var definition = GetMemberDefinitionForNode(member, logger);
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
				yield return new MemberDefinition(Program.Settings.TypePadding, fill, $"pad_{fillStart:X04}", fillStart, string.Empty);
			}
		}

		private MemberDefinition GetMemberDefinitionForNode(BaseNode member, ILogger logger)
		{
			var generator = CustomCodeGenerator.GetGenerator(member, Language);
			if (generator != null)
			{
				return generator.GetMemberDefinition(member, Language, logger);
			}

			if (typeToTypedefMap.TryGetValue(member.GetType(), out var type))
			{
				var count = (member as BaseTextNode)?.Length ?? 0;

				return new MemberDefinition(member, type, count);
			}
			if (member is BitFieldNode bitFieldNode)
			{
				switch (bitFieldNode.Bits)
				{
					case 8:
						type = Program.Settings.TypeUInt8;
						break;
					case 16:
						type = Program.Settings.TypeUInt16;
						break;
					case 32:
						type = Program.Settings.TypeUInt32;
						break;
					case 64:
						type = Program.Settings.TypeUInt64;
						break;
				}

				return new MemberDefinition(bitFieldNode, type);
			}

			if (member is ClassInstanceNode classInstanceNode)
			{
				return new MemberDefinition(classInstanceNode, $"class {classInstanceNode.InnerNode.Name}");
			}

			if (member is BaseWrapperNode wrapperNode)
			{
				// TODO Support WrapperNode chains
				if (member is PointerNode)
				{
					return new MemberDefinition(member, wrapperNode.InnerNode == null ? "void*" : GetMemberDefinitionForNode(wrapperNode.InnerNode, logger).Type + "*");
				}
				if (member is ArrayNode arrayNode)
				{
					return new MemberDefinition(member, GetMemberDefinitionForNode(wrapperNode.InnerNode, logger).Type, arrayNode.Count);
				}
			}

			return null;
		}

		private static string MemberDefinitionToString(MemberDefinition member)
		{
			Contract.Requires(member != null);

			if (member.IsArray)
			{
				return $"{member.Type} {member.Name}[{member.ArrayCount}]; //0x{member.Offset:X04} {member.Comment}".Trim();
			}
			return $"{member.Type} {member.Name}; //0x{member.Offset:X04} {member.Comment}".Trim();
		}
	}
}
