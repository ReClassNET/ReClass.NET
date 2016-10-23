using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColorCode;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	class CSharpCodeGenerator : ICodeGenerator
	{
		public ILanguage Language => Languages.CSharp;

		public string GetCodeFromClasses(IEnumerable<ClassNode> classes)
		{
			var sb = new StringBuilder();
			sb.AppendLine("// Created with ReClass.NET");
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
						csb.AppendLine($"struct {c.Name}");
						csb.AppendLine("{");
						csb.AppendLine(
							string.Join(
								"\n\n",
								c.Nodes.Where(n => !(n is BaseHexNode)).Select(n =>
								{
									var nsb = new StringBuilder();
									nsb.AppendLine($"\t[FieldOffset({n.Offset.ToInt64()})]");
									var decorator = GetFieldDecorator(n);
									if (!string.IsNullOrEmpty(decorator))
									{
										nsb.AppendLine(decorator);
									}
									nsb.Append($"\tpublic {"int"} {n.Name};");
									if (!string.IsNullOrEmpty(n.Comment))
									{
										nsb.Append($" // {n.Comment}");
									}
									return nsb.ToString();
								})
							)
						);
						csb.Append("}");
						return csb.ToString();
					})
				)
			);

			return sb.ToString();
		}

		private string GetFieldDecorator(BaseNode node)
		{
			var arrayNode = node as BaseArrayNode;
			if (arrayNode != null)
			{
				return $"[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]";
			}

			return null;
		}

		private string GetNodeType(BaseNode node)
		{
			if (node is ClassInstanceNode)
			{
				return ((ClassInstanceNode)node).InnerNode.Name;
			}

			throw new Exception();
		}
	}
}
