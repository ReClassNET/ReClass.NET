﻿using System;

namespace ReClassNET.Nodes
{
	class VTableNode : BaseContainerNode
	{
		private Memory memory = new Memory();

		public override int MemorySize => IntPtr.Size;

		public override void Intialize()
		{
			InsertBytes(0, 10 * IntPtr.Size);
		}

		public override void ClearSelection()
		{
			base.ClearSelection();

			foreach (var node in nodes)
			{
				node.ClearSelection();
			}
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.VTable, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.VTableColor, HotSpot.NoneId, $"VTable[{nodes.Count}]") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddComment(view, x, y);

			y += view.Font.Height;

			if (levelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadObject<IntPtr>(Offset);

				memory.Size = nodes.Count * IntPtr.Size;
				memory.Process = view.Memory.Process;
				memory.Update(ptr);

				var v = view.Clone();
				v.Address = ptr;
				v.Memory = memory;

				foreach (var node in nodes)
				{
					y = node.Draw(v, tx, y);
				}
			}

			return y;
		}

		public override bool ReplaceChildNode(int index, BaseNode node)
		{
			return false;
		}

		public override void InsertBytes(int index, int size)
		{
			if (index < 0 || index > nodes.Count || size == 0)
			{
				return;
			}

			var offset = IntPtr.Zero;
			if (index > 0)
			{
				var node = nodes[index - 1];
				offset = node.Offset + node.MemorySize;
			}

			while (size > 0)
			{
				var node = new VirtualFunctionPtrNode
				{
					Offset = offset,
					ParentNode = this
				};

				nodes.Insert(index, node);

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}

			UpdateOffsets();
		}

		public override bool RemoveNode(BaseNode node)
		{
			var removed = base.RemoveNode(node);
			if (removed)
			{
				UpdateOffsets();
			}
			return removed;
		}
	}
}
