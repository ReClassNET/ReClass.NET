using System;
using System.Drawing;
using System.Linq;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UnionNode : BaseContainerNode
	{
		public override int MemorySize => Nodes.Max(n => n.MemorySize);

		protected override bool ShouldCompensateSizeChanges => false;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Union";
			icon = Properties.Resources.B16x16_Button_Union;
		}

		public override bool CanHandleChildNode(BaseNode node)
		{
			switch (node)
			{
				case null:
				case ClassNode _:
				case VirtualMethodNode _:
					return false;
			}

			return true;
		}

		public override void Initialize()
		{
			AddNode(CreateDefaultNodeForSize(IntPtr.Size));
		}

		public override void UpdateOffsets()
		{
			foreach (var node in Nodes)
			{
				node.Offset = 0;
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
			x = AddIcon(view, x, y, Icons.Union, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Union") + view.Font.Width;

			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"[Nodes: {Nodes.Count}, Size: {MemorySize}]") + view.Font.Width;

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[view.Level])
			{
				var v = view.Clone();
				v.Settings = Program.Settings.Clone();
				v.Settings.ShowNodeAddress = false;
				v.Address = view.Address + Offset;
				v.Memory = view.Memory.Clone();
				v.Memory.Offset += Offset;

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
	}
}
