using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.DataExchange.ReClass.Legacy;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.DataExchange.ReClass
{
	public partial class ReClassNetFile : IReClassImport, IReClassExport
	{
		private readonly ReClassNetProject project;

		public ReClassNetFile(ReClassNetProject project)
		{
			Contract.Requires(project != null);

			this.project = project;
		}

		static ReClassNetFile()
		{
			// Obsolete: The name of the class was changed. Because of this older files can't load this nodes.
			buildInStringToTypeMap["UTF8TextNode"] = typeof(Utf8TextNode);
			buildInStringToTypeMap["UTF8TextPtrNode"] = typeof(Utf8TextPtrNode);
			buildInStringToTypeMap["UTF16TextNode"] = typeof(Utf16TextNode);
			buildInStringToTypeMap["UTF16TextPtrNode"] = typeof(Utf16TextPtrNode);
			buildInStringToTypeMap["UTF32TextNode"] = typeof(Utf32TextNode);
			buildInStringToTypeMap["UTF32TextPtrNode"] = typeof(Utf32TextPtrNode);
			buildInStringToTypeMap["VTableNode"] = typeof(VirtualMethodTableNode);

			// Legacy
			buildInStringToTypeMap["ClassInstanceArrayNode"] = typeof(ClassInstanceArrayNode);
			buildInStringToTypeMap["ClassPtrArrayNode"] = typeof(ClassPointerArrayNode);
			buildInStringToTypeMap["ClassPtrNode"] = typeof(ClassPointerNode);
		}

		private static readonly Dictionary<string, Type> buildInStringToTypeMap = new[]
		{
			typeof(BoolNode),
			typeof(BitFieldNode),
			typeof(EnumNode),
			typeof(ClassInstanceNode),
			typeof(DoubleNode),
			typeof(FloatNode),
			typeof(FunctionNode),
			typeof(FunctionPtrNode),
			typeof(Hex8Node),
			typeof(Hex16Node),
			typeof(Hex32Node),
			typeof(Hex64Node),
			typeof(Int8Node),
			typeof(Int16Node),
			typeof(Int32Node),
			typeof(Int64Node),
			typeof(Matrix3x3Node),
			typeof(Matrix3x4Node),
			typeof(Matrix4x4Node),
			typeof(UInt8Node),
			typeof(UInt16Node),
			typeof(UInt32Node),
			typeof(UInt64Node),
			typeof(Utf8TextNode),
			typeof(Utf8TextPtrNode),
			typeof(Utf16TextNode),
			typeof(Utf16TextPtrNode),
			typeof(Utf32TextNode),
			typeof(Utf32TextPtrNode),
			typeof(Vector2Node),
			typeof(Vector3Node),
			typeof(Vector4Node),
			typeof(VirtualMethodTableNode),
			typeof(ArrayNode),
			typeof(PointerNode),
			typeof(UnionNode)
		}.ToDictionary(t => t.Name, t => t);

		private static readonly Dictionary<Type, string> buildInTypeToStringMap = buildInStringToTypeMap.ToDictionary(kv => kv.Value, kv => kv.Key);
	}
}
