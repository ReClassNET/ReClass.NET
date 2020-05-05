using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.CodeGenerator
{
	public class CSharpCodeGenerator : ICodeGenerator
	{
		private static readonly Dictionary<Type, string> nodeTypeToTypeDefinationMap = new Dictionary<Type, string>
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
			[typeof(VirtualMethodTableNode)] = "IntPtr",

			[typeof(Vector2Node)] = "Vector2",
			[typeof(Vector3Node)] = "Vector3",
			[typeof(Vector4Node)] = "Vector4"
        };

		public Language Language => Language.CSharp;

		public string GenerateCode(IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ILogger logger)
		{
			using (var sw = new StringWriter())
			{
				using (var iw = new IndentedTextWriter(sw, "\t"))
				{
					iw.WriteLine($"// Created with {Constants.ApplicationName} {Constants.ApplicationVersion} by {Constants.Author}");
					iw.WriteLine();
					iw.WriteLine("// Warning: The C# code generator doesn't support all node types!");
					iw.WriteLine();
					iw.WriteLine("using System.Runtime.InteropServices;");

					iw.WriteLine("// optional namespace, only for vectors");
					iw.WriteLine("using System.Numerics;");
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

					var classesToWrite = classes
						.Where(c => c.Nodes.None(n => n is FunctionNode)) // Skip class which contains FunctionNodes because these are not data classes.
						.Distinct();

					using (var en = classesToWrite.GetEnumerator())
					{
						if (en.MoveNext())
						{
							WriteClass(iw, en.Current, logger);

							while (en.MoveNext())
							{
								iw.WriteLine();

								WriteClass(iw, en.Current, logger);
							}
						}
					}
				}

				return sw.ToString();
			}
		}

		/// <summary>
		/// Outputs the C# code for the given enum to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="enum">The enum to output.</param>
		private static void WriteEnum(IndentedTextWriter writer, EnumDescription @enum)
		{
			Contract.Requires(writer != null);
			Contract.Requires(@enum != null);

			writer.Write($"enum {@enum.Name} : ");
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
		/// Outputs the C# code for the given class to the <see cref="TextWriter"/> instance.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="class">The class to output.</param>
		/// <param name="logger">The logger.</param>
		private static void WriteClass(IndentedTextWriter writer, ClassNode @class, ILogger logger)
		{
			Contract.Requires(writer != null);
			Contract.Requires(@class != null);
			Contract.Requires(logger != null);

			writer.WriteLine("[StructLayout(LayoutKind.Explicit)]");
			writer.Write("public struct ");
			writer.Write(@class.Name);

			if (!string.IsNullOrEmpty(@class.Comment))
			{
				writer.Write(" // ");
				writer.Write(@class.Comment);
			}

			writer.WriteLine();

			writer.WriteLine("{");
			writer.Indent++;

			var nodes = @class.Nodes
				.WhereNot(n => n is FunctionNode || n is BaseHexNode);
			foreach (var node in nodes)
			{
				var type = GetTypeDefinition(node);
				if (type != null)
				{
					writer.Write($"[FieldOffset(0x{node.Offset:X})] public readonly {type} {node.Name};");
					if (!string.IsNullOrEmpty(node.Comment))
					{
						writer.Write(" ");
						writer.Write(node.Comment);
					}
					writer.WriteLine();
				}
				else
				{
					logger.Log(LogLevel.Warning, $"Skipping node with unhandled type: {node.GetType()}");
				}
			}

			writer.Indent--;
			writer.WriteLine("}");
		}

		/// <summary>
		/// Gets the type definition for the given node. If the node is not a simple node <c>null</c> is returned.
		/// </summary>
		/// <param name="node">The target node.</param>
		/// <returns>The type definition for the node or null if no simple type is available.</returns>
		private static string GetTypeDefinition(BaseNode node)
		{
			Contract.Requires(node != null);

			if (node is BitFieldNode bitFieldNode)
			{
				var underlayingNode = bitFieldNode.GetUnderlayingNode();
				underlayingNode.CopyFromNode(node);
				node = underlayingNode;
			}

			if (nodeTypeToTypeDefinationMap.TryGetValue(node.GetType(), out var type))
			{
				return type;
			}

			if (node is EnumNode enumNode)
			{
				return enumNode.Enum.Name;
			}

			return null;
		}
	}
}
