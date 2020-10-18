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
			[typeof(NIntNode)] = "IntPtr",
			[typeof(UInt8Node)] = "byte",
			[typeof(UInt16Node)] = "ushort",
			[typeof(UInt32Node)] = "uint",
			[typeof(UInt64Node)] = "ulong",
			[typeof(NUIntNode)] = "UIntPtr",

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
			using var sw = new StringWriter();
			using var iw = new IndentedTextWriter(sw, "\t");

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

			var unicodeStringClassLengthsToGenerate = new HashSet<int>();

			using (var en = classesToWrite.GetEnumerator())
			{
				if (en.MoveNext())
				{
					void FindUnicodeStringClasses(IEnumerable<BaseNode> nodes)
					{
						unicodeStringClassLengthsToGenerate.UnionWith(nodes.OfType<Utf16TextNode>().Select(n => n.Length));
					}

					FindUnicodeStringClasses(en.Current!.Nodes);

					WriteClass(iw, en.Current, logger);

					while (en.MoveNext())
					{
						iw.WriteLine();

						FindUnicodeStringClasses(en.Current!.Nodes);

						WriteClass(iw, en.Current, logger);
					}
				}
			}

			if (unicodeStringClassLengthsToGenerate.Any())
			{
				foreach (var length in unicodeStringClassLengthsToGenerate)
				{
					iw.WriteLine();

					WriteUnicodeStringClass(iw, length);
				}
			}

			return sw.ToString();
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

			writer.WriteLine("[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]");
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
				var (type, attribute) = GetTypeDefinition(node);
				if (type != null)
				{
					if (attribute != null)
					{
						writer.WriteLine(attribute);
					}

					writer.WriteLine($"[FieldOffset(0x{node.Offset:X})]");
					writer.Write($"public readonly {type} {node.Name};");
					if (!string.IsNullOrEmpty(node.Comment))
					{
						writer.Write(" //");
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
		/// Gets the type definition for the given node. If the node is not expressible <c>null</c> as typename is returned.
		/// </summary>
		/// <param name="node">The target node.</param>
		/// <returns>The type definition for the node or null as typename if the node is not expressible.</returns>
		private static (string typeName, string attribute) GetTypeDefinition(BaseNode node)
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
				return (type, null);
			}

			return node switch
			{
				EnumNode enumNode => (enumNode.Enum.Name, null),
				Utf8TextNode utf8TextNode => ("string", $"[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {utf8TextNode.Length})]"),
				Utf16TextNode utf16TextNode => (GetUnicodeStringClassName(utf16TextNode.Length), "[MarshalAs(UnmanagedType.Struct)]"),
				_ => (null, null)
			};
		}

		private static string GetUnicodeStringClassName(int length) => $"__UnicodeString{length}";

		/// <summary>
		/// Writes a helper class for unicode strings with the specific length.
		/// </summary>
		/// <param name="writer">The writer to output to.</param>
		/// <param name="length">The string length for this class.</param>
		private static void WriteUnicodeStringClass(IndentedTextWriter writer, int length)
		{
			var className = GetUnicodeStringClassName(length);

			writer.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]");
			writer.WriteLine($"public struct {className}");
			writer.WriteLine("{");
			writer.Indent++;
			writer.WriteLine($"[MarshalAs(UnmanagedType.ByValTStr, SizeConst = {length})]");
			writer.WriteLine("public string Value;");
			writer.WriteLine();
			writer.WriteLine($"public static implicit operator string({className} value) => value.Value;");
			writer.Indent--;
			writer.WriteLine("}");
		}
	}
}
