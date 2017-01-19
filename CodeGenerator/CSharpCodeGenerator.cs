using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.CodeGenerator
{
	class CSharpCodeGenerator : ICodeGenerator
	{
		private readonly Dictionary<Type, string> typeToTypedefMap = new Dictionary<Type, string>
		{
			[typeof(DoubleNode)] = "double",
			[typeof(FloatNode)] = "float",
			[typeof(BoolNode)] = "bool",
			[typeof(Int8Node)] = "sbyte",
			[typeof(Int16Node)] = "short",
			[typeof(Int32Node)] = "int",
			[typeof(Int64Node)] = "long",
			[typeof(UInt8Node)] = "byte",
			[typeof(UInt16Node)] = "ushort",
			[typeof(UInt32Node)] = "uint",
			[typeof(UInt64Node)] = "ulong",

			[typeof(FunctionPtrNode)] = "IntPtr",
			[typeof(UTF8TextPtrNode)] = "IntPtr",
			[typeof(UTF16TextPtrNode)] = "IntPtr",
			[typeof(UTF32TextPtrNode)] = "IntPtr",
			[typeof(ClassPtrNode)] = "IntPtr",
			[typeof(VTableNode)] = "IntPtr"
		};

		public Language Language => Language.CSharp;

		public string GenerateCode(IEnumerable<ClassNode> classes, ILogger logger)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"// Created with {Constants.ApplicationName} by {Constants.Author}");
			sb.AppendLine();
			sb.AppendLine("// Warning: The code doesn't contain arrays and instances!");
			sb.AppendLine();
			sb.AppendLine("using System.Runtime.InteropServices;");
			sb.AppendLine();

			sb.Append(
				string.Join(
					"\n\n",
					classes.Select(c =>
					{
						var csb = new StringBuilder();

						csb.AppendLine("[StructLayout(LayoutKind.Explicit)]");
						csb.Append($"struct {c.Name}");
						if (!string.IsNullOrEmpty(c.Comment))
						{
							csb.Append($" // {c.Comment}");
						}
						csb.AppendLine();

						csb.AppendLine("{");

						csb.AppendLine(
							string.Join(
								"\n\n",
								YieldMemberDefinitions(c.Nodes, logger)
									.Select(m => $"\t{GetFieldDecorator(m)}\n\t{GetFieldDefinition(m)}")
							)
						);
						csb.Append("}");
						return csb.ToString();
					})
				)
			);

			return sb.ToString();
		}

		private IEnumerable<MemberDefinition> YieldMemberDefinitions(IEnumerable<BaseNode> members, ILogger logger)
		{
			Contract.Requires(members != null);
			Contract.Requires(Contract.ForAll(members, m => m != null));
			Contract.Ensures(Contract.Result<IEnumerable<MemberDefinition>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<MemberDefinition>>(), d => d != null));

			foreach (var member in members.WhereNot(n => n is BaseHexNode))
			{
				var bitFieldNode = member as BitFieldNode;
				if (bitFieldNode != null)
				{
					string type;
					switch (bitFieldNode.Bits)
					{
						default:
							type = typeToTypedefMap[typeof(UInt8Node)];
							break;
						case 16:
							type = typeToTypedefMap[typeof(UInt16Node)];
							break;
						case 32:
							type = typeToTypedefMap[typeof(UInt32Node)];
							break;
						case 64:
							type = typeToTypedefMap[typeof(UInt64Node)];
							break;
					}

					yield return new MemberDefinition(member, type);
				}
				else
				{
					string type;
					if (typeToTypedefMap.TryGetValue(member.GetType(), out type))
					{
						yield return new MemberDefinition(member, type);
					}
					else
					{
						var generator = CustomCodeGenerator.GetGenerator(member, Language);
						if (generator != null)
						{
							yield return generator.GetMemberDefinition(member, Language, logger);

							continue;
						}

						logger.Log(LogLevel.Error, $"Skipping node with unhandled type: {member.GetType()}");
					}
				}
			}
		}

		private string GetFieldDecorator(MemberDefinition member)
		{
			Contract.Requires(member != null);

			return $"[FieldOffset({member.Offset})]";
		}

		private string GetFieldDefinition(MemberDefinition member)
		{
			Contract.Requires(member != null);

			return $"public {member.Type} {member.Name}; //0x{member.Offset:X04} {member.Comment}".Trim();
		}
	}
}
