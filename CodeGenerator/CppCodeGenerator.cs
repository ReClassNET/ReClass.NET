using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using ColorCode;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	class CppCodeGenerator : ICodeGenerator
	{
		private readonly Dictionary<Type, string> typeToTypedefMap = new Dictionary<Type, string>
		{
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
			[typeof(UTF8TextNode)] = Program.Settings.TypeUTF8Text,
			[typeof(UTF8TextPtrNode)] = Program.Settings.TypeUTF8TextPtr,
			[typeof(UTF16TextNode)] = Program.Settings.TypeUTF16Text,
			[typeof(UTF16TextPtrNode)] = Program.Settings.TypeUTF16TextPtr,
			[typeof(UTF32TextNode)] = Program.Settings.TypeUTF32Text,
			[typeof(UTF32TextPtrNode)] = Program.Settings.TypeUTF32PtrText,
			[typeof(Vector2Node)] = Program.Settings.TypeVector2,
			[typeof(Vector3Node)] = Program.Settings.TypeVector3,
			[typeof(Vector4Node)] = Program.Settings.TypeVector4
		};

		public ILanguage Language => Languages.Cpp;

		public string GetCodeFromClasses(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);

			var sb = new StringBuilder();
			sb.AppendLine("// Created with ReClass.NET");
			sb.AppendLine();
			sb.AppendLine(
				string.Join(
					"\n\n",
					OrderByInheritance(classes).Select(c =>
					{
						var csb = new StringBuilder();
						csb.Append($"class {c.Name}");

						bool skipFirstMember = false;
						var inheritedFromNode = c.Nodes.FirstOrDefault() as ClassInstanceNode;
						if (inheritedFromNode != null)
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
								"\n",
								YieldMemberDefinitions(c.Nodes.Skip(skipFirstMember ? 1 : 0))
									.Select(s => "\t" + s)
							)
						);

						var vtables = c.Nodes.OfType<VTableNode>();
						if (vtables.Any())
						{
							csb.AppendLine();
							csb.AppendLine(
								string.Join(
									"\n",
									vtables.SelectMany(vt => vt.Nodes).OfType<VMethodNode>().Select(m => $"\tvirtual void {m.MethodName}();")
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

		private IEnumerable<ClassNode> OrderByInheritance(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);

			var alreadySeen = new HashSet<ClassNode>();

			return classes.SelectMany(c => YieldReversedHierarchy(c, alreadySeen)).Distinct();
		}

		private IEnumerable<ClassNode> YieldReversedHierarchy(ClassNode node, HashSet<ClassNode> alreadySeen)
		{
			Contract.Requires(node != null);
			Contract.Requires(alreadySeen != null);

			if (!alreadySeen.Add(node))
			{
				yield break;
			}

			foreach (var referenceNode in node.Nodes.OfType<BaseReferenceNode>())
			{
				foreach (var referencedNode in YieldReversedHierarchy(referenceNode.InnerNode as ClassNode, alreadySeen))
				{
					yield return referencedNode;
				}
			}

			yield return node;
		}

		private IEnumerable<string> YieldMemberDefinitions(IEnumerable<BaseNode> members)
		{
			Contract.Requires(members != null);

			int fill = 0;
			int fillStart = 0;

			foreach (var member in members)
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
					yield return BuildMemberDefinition(Program.Settings.TypePadding, fill, $"unknown{fillStart:X04}", fillStart, string.Empty);

					fill = 0;
				}

				string type;
				if (typeToTypedefMap.TryGetValue(member.GetType(), out type))
				{
					int count = 0;
					if (member is BaseTextNode)
					{
						count = ((BaseTextNode)member).CharacterCount;
					}

					yield return BuildMemberDefinition(member, type, count);
				}

				if (member is BitFieldNode)
				{
					switch (((BitFieldNode)member).Bits)
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

					yield return BuildMemberDefinition(member, type);
				}
				else if (member is ClassInstanceArrayNode)
				{
					var instanceArray = (ClassInstanceArrayNode)member;

					yield return BuildMemberDefinition(member, instanceArray.InnerNode.Name, instanceArray.Count);
				}
				else if (member is ClassInstanceNode)
				{
					yield return BuildMemberDefinition(member, ((ClassInstanceNode)member).InnerNode.Name);
				}
				else if (member is ClassPtrArrayNode)
				{
					var ptrArray = (ClassPtrArrayNode)member;

					yield return BuildMemberDefinition(member, $"class {ptrArray.InnerNode.Name}*", ptrArray.Count);
				}
				else if (member is ClassPtrNode)
				{
					yield return BuildMemberDefinition(member, $"class {((ClassPtrNode)member).InnerNode.Name}*");
				}
			}

			if (fill != 0)
			{
				yield return BuildMemberDefinition(Program.Settings.TypePadding, fill, $"unknown{fillStart:X04}", fillStart, string.Empty);
			}
		}

		private string BuildMemberDefinition(BaseNode member, string type)
		{
			Contract.Requires(member != null);
			Contract.Requires(type != null);

			return BuildMemberDefinition(member, type, 0);
		}

		private string BuildMemberDefinition(BaseNode member, string type, int count)
		{
			Contract.Requires(member != null);
			Contract.Requires(type != null);

			var comment = string.IsNullOrEmpty(member.Comment) ? string.Empty : " " + member.Comment;

			return BuildMemberDefinition(type, count, member.Name, member.Offset.ToInt32(), comment);
		}

		private string BuildMemberDefinition(string type, int count, string name, int offset, string comment)
		{
			if (count == 0)
			{
				return $"{type} {name}; //0x{offset:X04}{comment}";
			}
			else
			{
				return $"{type} {name}[{count}]; //0x{offset:X04}{comment}";
			}
		}
	}
}
