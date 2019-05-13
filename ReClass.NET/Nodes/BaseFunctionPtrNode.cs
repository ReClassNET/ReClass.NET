using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseFunctionPtrNode : BaseFunctionNode
	{
		public override int MemorySize => IntPtr.Size;

		public override string GetToolTipText(HotSpot spot)
		{
			var ptr = spot.Memory.ReadIntPtr(Offset);

			DisassembleRemoteCode(spot.Process, ptr);

			return string.Join("\n", instructions.Select(i => i.Instruction));
		}

		protected Size Draw(ViewInfo view, int x, int y, string type, string name)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(name != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, name) + view.Font.Width;
			}

			x = AddOpenCloseIcon(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			if (view.Settings.ShowCommentSymbol)
			{
				var value = view.Memory.ReadIntPtr(Offset);

				var module = view.Process.GetModuleToPointer(value);
				if (module != null)
				{
					var symbols = view.Process.Symbols.GetSymbolsForModule(module);
					var symbol = symbols?.GetSymbolString(value, module);
					if (!string.IsNullOrEmpty(symbol))
					{
						x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.ReadOnlyId, symbol);
					}
				}
			}

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			var size = new Size(x - origX, view.Font.Height);

			if (LevelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadIntPtr(Offset);

				DisassembleRemoteCode(view.Process, ptr);

				var instructionSize = DrawInstructions(view, tx, y);

				size.Width = Math.Max(size.Width, instructionSize.Width + tx - origX);
				size.Height += instructionSize.Height;
			}

			return size;
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
			{
				height += instructions.Count * view.Font.Height;
			}
			return height;
		}

		private void DisassembleRemoteCode(RemoteProcess process, IntPtr address)
		{
			Contract.Requires(process != null);

			if (this.address != address)
			{
				instructions.Clear();

				this.address = address;

				if (!address.IsNull() && process.IsValid)
				{
					DisassembleRemoteCode(process, address, out _);
				}
			}
		}
	}
}
