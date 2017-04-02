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

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Function, -1, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Function") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			var size = new Size(x - origX, view.Font.Height);

			var ptr = view.Address.Add(Offset);
			DisassembleRemoteCode(view.Memory, ptr);

			if (levelsOpen[view.Level])
			{
				y += view.Font.Height;
				x = AddText(view, tx, y, view.Settings.TypeColor, HotSpot.NoneId, "Signature:") + view.Font.Width;
				x = AddText(view, x, y, view.Settings.ValueColor, 0, Signature);
				size.Width = Math.Max(size.Width, x - origX);
				size.Height += view.Font.Height;

				y += view.Font.Height;
				x = AddText(view, tx, y, view.Settings.TextColor, HotSpot.NoneId, "Belongs to: ");
				x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, BelongsToClass == null ? "<None>" : $"<{BelongsToClass.Name}>");
				x = AddIcon(view, x, y, Icons.Change, 1, HotSpotType.ChangeType);
				size.Width = Math.Max(size.Width, x - origX);
				size.Height += view.Font.Height;

				var instructionSize = DrawInstructions(view, tx, y + 4);
				size.Width = Math.Max(size.Width, instructionSize.Width + tx - origX);
				size.Height += instructionSize.Height + 4;
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
			if (levelsOpen[view.Level])
			{
				height += instructions.Count * view.Font.Height;
			}
			return height;
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
