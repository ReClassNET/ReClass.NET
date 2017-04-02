using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseFunctionPtrNode : BaseFunctionNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size;

		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			var ptr = memory.ReadObject<IntPtr>(Offset);

			DisassembleRemoteCode(memory, ptr);

			return string.Join("\n", instructions.Select(i => i.Instruction));
		}

		protected Size Draw(ViewInfo view, int x, int y, string type, string name)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(name != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			if (view.Settings.ShowCommentSymbol)
			{
				var value = view.Memory.ReadObject<IntPtr>(Offset);

				var module = view.Memory.Process.GetModuleToPointer(value);
				if (module != null)
				{
					var symbols = view.Memory.Process.Symbols.GetSymbolsForModule(module);
					var symbol = symbols?.GetSymbolString(value, module);
					if (!string.IsNullOrEmpty(symbol))
					{
						x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.ReadOnlyId, symbol);
					}
				}
			}

			var size = new Size(x - origX, view.Font.Height);

			if (levelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadObject<IntPtr>(Offset);

				DisassembleRemoteCode(view.Memory, ptr);

				var instructionSize = DrawInstructions(view, tx, y);

				size.Width = Math.Max(size.Width, instructionSize.Width + tx - origX);
				size.Height += instructionSize.Height;
			}

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return size;
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				height += instructions.Count * view.Font.Height;
			}
			return height;
		}

		private void DisassembleRemoteCode(MemoryBuffer memory, IntPtr address)
		{
			Contract.Requires(memory != null);

			if (this.address != address)
			{
				instructions.Clear();

				this.address = address;

				if (!address.IsNull() && memory.Process.IsValid)
				{
					int unused;
					DisassembleRemoteCode(memory, address, out unused);
				}
			}
		}
	}
}
