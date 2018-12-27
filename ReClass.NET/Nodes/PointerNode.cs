using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class PointerNode : BaseWrapperNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size;

		public override bool PerformCycleCheck => false;

		public override bool CanChangeInnerNodeTo(BaseNode node)
		{
			return true;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			//return DrawHidden(view, x, y);
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Pointer, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Ptr") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			if (InnerNode == null)
			{
				x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, "<void>") + view.Font.Width;
			}
			else
			{
				
			}
			x = AddIcon(view, x, y, Icons.Change, 4, HotSpotType.ChangeType) + view.Font.Width;

			var ptr = view.Memory.ReadIntPtr(Offset);

			x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, "->") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.ValueColor, 0, "0x" + ptr.ToString(Constants.AddressHexFormat)) + view.Font.Width;

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (levelsOpen[view.Level] && InnerNode != null)
			{
				memory.Size = InnerNode.MemorySize;
				memory.Process = view.Memory.Process;
				memory.Update(ptr);

				var v = view.Clone();
				v.Address = ptr;
				v.Memory = memory;

				var innerSize = InnerNode.Draw(v, tx, y);

				size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
				size.Height += innerSize.Height;
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
			if (levelsOpen[view.Level] && InnerNode != null)
			{
				height += InnerNode.CalculateDrawnHeight(view);
			}
			return height;
		}
	}
}
