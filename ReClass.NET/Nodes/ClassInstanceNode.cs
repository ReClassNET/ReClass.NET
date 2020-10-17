using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ClassInstanceNode : BaseClassWrapperNode
	{
		public override int MemorySize => InnerNode.MemorySize;

		protected override bool PerformCycleCheck => true;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Class Instance";
			icon = Properties.Resources.B16x16_Button_Class_Instance;
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
			x = AddIcon(context, x, y, context.IconProvider.Class, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Instance") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}
			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name}>") + context.Font.Width;
			x = AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeClassType) + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level])
			{
				var innerContext = context.Clone();
				innerContext.Address = context.Address + Offset;
				innerContext.Memory = context.Memory.Clone();
				innerContext.Memory.Offset += Offset;

				var innerSize = InnerNode.Draw(innerContext, tx, y);
				size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
				size.Height += innerSize.Height;
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
				height += InnerNode.CalculateDrawnHeight(context);
			}
			return height;
		}
	}
}
