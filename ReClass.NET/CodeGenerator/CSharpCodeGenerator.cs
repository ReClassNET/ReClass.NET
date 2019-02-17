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
	public class CSharpCodeGenerator : ICodeGenerator
	{
		private static readonly Dictionary<Type, string> typeToTypedefMap = new Dictionary<Type, string>
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
			[typeof(Utf8TextPtrNode)] = "IntPtr",
			[typeof(Utf16TextPtrNode)] = "IntPtr",
			[typeof(Utf32TextPtrNode)] = "IntPtr",
			[typeof(PointerNode)] = "IntPtr",
			[typeof(VirtualMethodTableNode)] = "IntPtr"
		};

		public Language Language => Language.CSharp;

		public string GenerateCode(IEnumerable<ClassNode> classes, ILogger logger)
		{
			var sb = new StringBuilder();
			sb.AppendLine($"// Created with {Constants.ApplicationName} by {Constants.Author}");
			sb.AppendLine();
			sb.AppendLine("// Warning: The code generator doesn't support all node types!");
			sb.AppendLine();
			sb.AppendLine("using System.Runtime.InteropServices;");
			sb.AppendLine();

			sb.Append(
				string.Join(
					Environment.NewLine + Environment.NewLine,
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
								Environment.NewLine + Environment.NewLine,
								GetTypeDeclerationsForNodes(c.Nodes, logger)
									.Select(t => $"\t{t.Item1}{Environment.NewLine}\t{t.Item2}")
							)
						);
						csb.Append("}");
						return csb.ToString();
					})
				)
			);

			return sb.ToString();
		}

		private static IEnumerable<Tuple<string, string>> GetTypeDeclerationsForNodes(IEnumerable<BaseNode> members, ILogger logger)
		{
			Contract.Requires(members != null);
			Contract.Requires(Contract.ForAll(members, m => m != null));
			Contract.Requires(logger != null);
			Contract.Ensures(Contract.Result<IEnumerable<Tuple<string, string>>>() != null);
			Contract.Ensures(Contract.ForAll(Contract.Result<IEnumerable<Tuple<string, string>>>(), t => t != null));

			foreach (var member in members.WhereNot(n => n is BaseHexNode))
			{
				var type = GetTypeForNode(member, logger);
				if (type != null)
				{
					yield return Tuple.Create(
						$"[FieldOffset({member.Offset.ToInt32()})]",
						$"public {type} {member.Name}; //0x{member.Offset.ToInt32():X04} {member.Comment}".Trim()
					);
				}
				else
				{
					logger.Log(LogLevel.Warning, $"Skipping node with unhandled type: {member.GetType()}");
				}
			}
		}

		private static string GetTypeForNode(BaseNode node, ILogger logger)
		{
			Contract.Requires(node != null);
			Contract.Requires(logger != null);

			if (node is BitFieldNode bitFieldNode)
			{
				var underlayingNode = bitFieldNode.GetUnderlayingNode();
				underlayingNode.CopyFromNode(node);
				node = underlayingNode;
			}

			if (typeToTypedefMap.TryGetValue(node.GetType(), out var type))
			{
				return type;
			}

			return null;
		}
	}
}
