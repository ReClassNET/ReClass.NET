using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReClassNET.Nodes;

namespace ReClassNET.CodeGenerator
{
	class CppCodeGenerator : ICodeGenerator
	{
		public string GetCodeFromClasses(IList<ClassNode> classes)
		{
			var sb = new StringBuilder();
			sb.AppendLine("// Created with ReClass.NET");
			sb.AppendLine();
			//sb.AppendLine(string.Join("\n", classes.Select(c => $"class {c.Name};")));
			//sb.AppendLine();
			sb.AppendLine(
				string.Join(
					"\n\n",
					classes.Select(c =>
					{
						var csb = new StringBuilder();
						csb.Append($"class {c.Name}");
						if (!string.IsNullOrEmpty(c.Comment))
						{
							csb.Append($" // {c.Comment}");
						}
						csb.AppendLine();
						csb.AppendLine("{");
						csb.AppendLine("public:");
						/*csb.AppendLine(
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
						);*/
						csb.Append("}");
						return csb.ToString();
					})
				)
			);

			throw new NotImplementedException();
		}
	}
}
