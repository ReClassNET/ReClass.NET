using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class NodeDissector
	{
		public static void DissectNodes(IEnumerable<BaseHexNode> nodes, MemoryBuffer memory)
		{
			Contract.Requires(nodes != null);
			Contract.Requires(Contract.ForAll(nodes, n => n != null));
			Contract.Requires(memory != null);

			foreach (var node in nodes)
			{
				var type = GuessType(node, memory);
				if (type != null)
				{
					node.ParentNode.ReplaceChildNode(node, type);
				}
			}
		}

		public static Type GuessType(BaseHexNode node, MemoryBuffer memory)
		{
			Contract.Requires(node != null);
			Contract.Requires(memory != null);

			var offset = node.Offset.ToInt32();
			var is4ByteAligned = offset % 4 == 0;
			var is8ByteAligned = offset % 8 == 0;

			// The node is not aligned, skip it.
			if (!is4ByteAligned)
			{
				return null;
			}

			var data64 = memory.ReadObject<UInt64FloatDoubleData>(offset);
			var data32 = memory.ReadObject<UInt32FloatData>(offset);

			var raw = memory.ReadBytes(offset, node.MemorySize);
			if (raw.InterpretAsUTF8().IsLikelyPrintableData() >= 0.75f)
			{
				return typeof(UTF8TextNode);
			}
			else if (raw.InterpretAsUTF16().IsLikelyPrintableData() >= 0.75f)
			{
				return typeof(UTF16TextNode);
			}

			if (is8ByteAligned)
			{
#if WIN64
				var pointerType = GuessPointerType(data64.IntPtr, memory);
				if (pointerType != null)
				{
					return pointerType;
				}
#endif
			}

			{
#if WIN32
				var pointerType = GuessPointerType(data32.IntPtr, memory);
				if (pointerType != null)
				{
					return pointerType;
				}
#endif

				// 0 could be anything.
				if (data32.IntValue != 0)
				{
					// If the data represents a reasonable range, it could be a float.
					if (-99999.0f <= data32.FloatValue && data32.FloatValue <= 99999.0f && !data32.FloatValue.IsNearlyEqual(0.0f, 0.001f))
					{
						return typeof(FloatNode);
					}

					if (-99999 <= data32.IntValue && data32.IntValue <= 99999)
					{
						return typeof(Int32Node);
					}
				}
			}

			if (is8ByteAligned)
			{
				if (data64.LongValue != 0)
				{
					// If the data represents a reasonable range, it could be a double.
					if (-99999.0 <= data64.DoubleValue && data64.DoubleValue <= 99999.0 && !data64.DoubleValue.IsNearlyEqual(0.0, 0.001))
					{
						return typeof(DoubleNode);
					}
				}
			}

			return null;
		}

		private static Type GuessPointerType(IntPtr address, MemoryBuffer memory)
		{
			Contract.Requires(memory != null);

			if (address.IsNull())
			{
				return null;
			}

			var section = memory.Process.GetSectionToPointer(address);
			if (section != null) // If the address points to a section it's valid memory.
			{
				if (section.Category == SectionCategory.CODE) // If the section contains code, it should be a function pointer.
				{
					return typeof(FunctionPtrNode);
				}
				else if (section.Category == SectionCategory.DATA || section.Category == SectionCategory.HEAP) // If the section contains data, it is at least a pointer to a class or something.
				{
					// Check if it is a vtable. Check if the first 3 values are pointers to a code section.
					bool valid = true;
					for (var i = 0; i < 3; ++i)
					{
						var pointee = memory.Process.ReadRemoteObject<IntPtr>(address);
						if (memory.Process.GetSectionToPointer(pointee)?.Category != SectionCategory.CODE)
						{
							valid = false;
							break;
						}
					}
					if (valid)
					{
						return typeof(VTableNode);
					}

					// Check if it is a string.
					var data = memory.Process.ReadRemoteMemory(address, IntPtr.Size * 2);
					if (data.Take(IntPtr.Size).InterpretAsUTF8().IsLikelyPrintableData() >= 07.5f)
					{
						return typeof(UTF8TextPtrNode);
					}
					else if (data.InterpretAsUTF16().IsLikelyPrintableData() >= 0.75f)
					{
						return typeof(UTF16TextPtrNode);
					}

					// Now it could be a pointer to something else but we can't tell. :(
					//return typeof(ClassPtrNode);
				}
			}

			return null;
		}
	}
}
