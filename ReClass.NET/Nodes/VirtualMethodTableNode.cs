using System;
using System.Drawing;
using System.Linq;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class VirtualMethodTableNode : BaseContainerNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size;

		protected override bool ShouldCompensateSizeChanges => false;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "VTable Pointer";
			icon = Properties.Resources.B16x16_Button_VTable;
		}

		public override bool CanHandleChildNode(BaseNode node)
		{
			return node is VirtualMethodNode;
		}

		public override void Initialize()
		{
			for (var i = 0; i < 10; ++i)
			{
				AddNode(CreateDefaultNodeForSize(IntPtr.Size));
			}
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenCloseIcon(view, x, y);
			x = AddIcon(view, x, y, Icons.VTable, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.VTableColor, HotSpot.NoneId, $"VTable[{Nodes.Count}]") + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			}

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[view.Level])
			{
				var ptr = view.Memory.ReadIntPtr(Offset);

				memory.Size = Nodes.Count * IntPtr.Size;
				memory.UpdateFrom(view.Process, ptr);

				var v = view.Clone();
				v.Address = ptr;
				v.Memory = memory;

				foreach (var node in Nodes)
				{
					var innerSize = node.Draw(v, tx, y);

					size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
					size.Height += innerSize.Height;

					y += innerSize.Height;
				}
			}

			return size;
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
			{
				height += Nodes.Sum(n => n.CalculateDrawnHeight(view));
			}
			return height;
		}

		protected override BaseNode CreateDefaultNodeForSize(int size)
		{
			// ignore the size parameter
			return new VirtualMethodNode();
		}
	}
}
