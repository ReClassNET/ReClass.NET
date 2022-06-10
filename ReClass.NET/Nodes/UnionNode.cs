using System;
using System.Drawing;
using System.Linq;
using ReClassNET.Controls;
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

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			x = AddOpenCloseIcon(context, x, y);
			x = AddIcon(context, x, y, context.IconProvider.Union, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Union") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}

			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, $"[Nodes: {Nodes.Count}, Size: {MemorySize}]") + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level])
			{
				var innerContext = context.Clone();
				innerContext.Settings = Program.Settings.Clone();
				innerContext.Settings.ShowNodeAddress = false;
				innerContext.Address = context.Address + Offset;
				innerContext.Memory = context.Memory.Clone();
				innerContext.Memory.Offset += Offset;

				foreach (var node in Nodes)
				{
					var innerSize = node.Draw(innerContext, tx, y);

					size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
					size.Height += innerSize.Height;

					y += innerSize.Height;
				}
			}

			return size;
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level])
			{
				height += Nodes.Sum(n => n.CalculateDrawnHeight(context));
			}
			return height;
		}
	}
}
