using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.Extensions;
using ReClassNET.Nodes;

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
				var type = GuessExplicitNode(node, memory);
				if (type != null)
				{
					node.GetParentContainer()?.ReplaceChildNode(node, type);
				}
			}
		}

		public static BaseNode GuessExplicitNode(BaseHexNode node, MemoryBuffer memory)
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
			if (raw.InterpretAsUtf8().IsLikelyPrintableData() >= 0.75f)
			{
				return new Utf8TextNode();
			}
			if (raw.InterpretAsUtf16().IsLikelyPrintableData() >= 0.75f)
			{
				return new Utf16TextNode();
			}

			if (is8ByteAligned)
			{
#if RECLASSNET64
				var pointerType = GuessPointerNode(data64.IntPtr, memory);
				if (pointerType != null)
				{
					return pointerType;
				}
#endif
			}

			{
#if RECLASSNET32
				var pointerNode = GuessPointerNode(data32.IntPtr, memory);
				if (pointerNode != null)
				{
					return pointerNode;
				}
#endif

				// 0 could be anything.
				if (data32.IntValue != 0)
				{
					// If the data represents a reasonable range, it could be a float.
					if (-999999.0f <= data32.FloatValue && data32.FloatValue <= 999999.0f && !data32.FloatValue.IsNearlyEqual(0.0f, 0.001f))
					{
						return new FloatNode();
					}

					if (-999999 <= data32.IntValue && data32.IntValue <= 999999)
					{
						return new Int32Node();
					}
				}
			}

			if (is8ByteAligned)
			{
				if (data64.LongValue != 0)
				{
					// If the data represents a reasonable range, it could be a double.
					if (-999999.0 <= data64.DoubleValue && data64.DoubleValue <= 999999.0 && !data64.DoubleValue.IsNearlyEqual(0.0, 0.001))
					{
						return new DoubleNode();
					}
				}
			}

			return null;
		}

		private static BaseNode GuessPointerNode(IntPtr address, MemoryBuffer memory)
		{
			Contract.Requires(memory != null);

			if (address.IsNull())
			{
				return null;
			}

			var section = memory.Process.GetSectionToPointer(address);
			if (section == null)
			{
				return null;
			}

			if (section.Category == SectionCategory.CODE) // If the section contains code, it should be a function pointer.
			{
				return new FunctionPtrNode();
			}
			if (section.Category == SectionCategory.DATA || section.Category == SectionCategory.HEAP) // If the section contains data, it is at least a pointer to a class or something.
			{
				// Check if it is a vtable. Check if the first 3 values are pointers to a code section.
				var possibleVmt = memory.Process.ReadRemoteObject<ThreePointersData>(address);
				if (memory.Process.GetSectionToPointer(possibleVmt.Pointer1)?.Category == SectionCategory.CODE
					&& memory.Process.GetSectionToPointer(possibleVmt.Pointer2)?.Category == SectionCategory.CODE
					&& memory.Process.GetSectionToPointer(possibleVmt.Pointer3)?.Category == SectionCategory.CODE)
				{
					return new VirtualMethodTableNode();
				}

				// Check if it is a string.
				var data = memory.Process.ReadRemoteMemory(address, IntPtr.Size * 2);
				if (data.Take(IntPtr.Size).InterpretAsUtf8().IsLikelyPrintableData() >= 07.5f)
				{
					return new Utf8TextPtrNode();
				}
				if (data.InterpretAsUtf16().IsLikelyPrintableData() >= 0.75f)
				{
					return new Utf16TextPtrNode();
				}

				// Now it could be a pointer to something else but we can't tell. :(
				return new PointerNode();
			}

			return null;
		}
	}
}
