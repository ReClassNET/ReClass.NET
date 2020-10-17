using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseFunctionPtrNode : BaseFunctionNode
	{
		public override int MemorySize => IntPtr.Size;

		public override string GetToolTipText(HotSpot spot)
		{
			var ptr = spot.Memory.ReadIntPtr(Offset);

			DisassembleRemoteCode(spot.Process, ptr);

			return string.Join("\n", Instructions.Select(i => i.Instruction));
		}

		protected Size Draw(DrawContext context, int x, int y, string type, string name)
		{
			Contract.Requires(context != null);
			Contract.Requires(type != null);
			Contract.Requires(name != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, context.IconProvider.Function, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, name) + context.Font.Width;
			}

			x = AddOpenCloseIcon(context, x, y) + context.Font.Width;

			x = AddComment(context, x, y);

			if (context.Settings.ShowCommentSymbol)
			{
				var value = context.Memory.ReadIntPtr(Offset);

				var module = context.Process.GetModuleToPointer(value);
				if (module != null)
				{
					var symbols = context.Process.Symbols.GetSymbolsForModule(module);
					var symbol = symbols?.GetSymbolString(value, module);
					if (!string.IsNullOrEmpty(symbol))
					{
						x = AddText(context, x, y, context.Settings.OffsetColor, HotSpot.ReadOnlyId, symbol);
					}
				}
			}

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			var size = new Size(x - origX, context.Font.Height);

			if (LevelsOpen[context.Level])
			{
				var ptr = context.Memory.ReadIntPtr(Offset);

				DisassembleRemoteCode(context.Process, ptr);

				var instructionSize = DrawInstructions(context, tx, y);

				size.Width = Math.Max(size.Width, instructionSize.Width + tx - origX);
				size.Height += instructionSize.Height;
			}

			return size;
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level])
			{
				height += Instructions.Count * context.Font.Height;
			}
			return height;
		}

		private void DisassembleRemoteCode(RemoteProcess process, IntPtr address)
		{
			Contract.Requires(process != null);

			if (this.Address != address)
			{
				Instructions.Clear();

				this.Address = address;

				if (!address.IsNull() && process.IsValid)
				{
					DisassembleRemoteCode(process, address, out _);
				}
			}
		}
	}
}
