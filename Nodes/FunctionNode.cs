using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class FunctionNode : BaseFunctionNode
	{
		public string Signature { get; set; } = "void function()";

		public ClassNode BelongsToClass { get; set; }

		private int memorySize = IntPtr.Size;
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => memorySize;

		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			DisassembleRemoteCode(memory, spot.Address);

			return string.Join("\n", instructions.Select(i => i.Instruction));
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Function") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			var ptr = view.Address.Add(Offset);

			DisassembleRemoteCode(view.Memory, ptr);

			if (levelsOpen[view.Level])
			{
				y += view.Font.Height;
				var x2 = AddText(view, tx, y, view.Settings.TypeColor, HotSpot.NoneId, "Signature:") + view.Font.Width;
				x2 = AddText(view, x2, y, view.Settings.ValueColor, 0, Signature);
				x = Math.Max(x, x2);

				y += view.Font.Height;
				x2 = AddText(view, tx, y, view.Settings.TextColor, HotSpot.NoneId, "Belongs to: ");
				x2 = AddText(view, x2, y, view.Settings.ValueColor, HotSpot.NoneId, BelongsToClass == null ? "<None>" : $"<{BelongsToClass.Name}>");
				x2 = AddIcon(view, x2, y, Icons.Change, 1, HotSpotType.ChangeType);
				x = Math.Max(x, x2);

				var instructionSize = DrawInstructions(view, tx, y + 4);
				x = Math.Max(x, instructionSize.Width);
				y = instructionSize.Height + 4;
			}

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x, y + view.Font.Height);
		}

		public override Size CalculateSize(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenSize;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				h += instructions.Count * view.Font.Height;
			}
			return new Size(0, h);
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0) // Signature
			{
				Signature = spot.Text;
			}
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
					DisassembleRemoteCode(memory, address, out memorySize);
				}

				ParentNode?.ChildHasChanged(this);
			}
		}
	}
}
