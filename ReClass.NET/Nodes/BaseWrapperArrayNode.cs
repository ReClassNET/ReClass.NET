using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseWrapperArrayNode : BaseWrapperNode
	{
		public override int MemorySize => InnerNode.MemorySize * Count;

		public int CurrentIndex { get; set; }
		public int Count { get; set; } = 1;
		public bool IsReadOnly { get; protected set; }

		protected override bool PerformCycleCheck => true;

		public override bool CanChangeInnerNodeTo(BaseNode node)
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

		protected Size Draw(DrawContext context, int x, int y, string type)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddOpenCloseIcon(context, x, y);
			x = AddIcon(context, x, y, context.IconProvider.Array, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, "[");
			x = AddText(context, x, y, context.Settings.IndexColor, IsReadOnly ? HotSpot.NoneId : 0, Count.ToString());
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, "]");

			x = AddIcon(context, x, y, context.IconProvider.LeftArrow, 2, HotSpotType.Click);
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, "(");
			x = AddText(context, x, y, context.Settings.IndexColor, 1, CurrentIndex.ToString());
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, ")");
			x = AddIcon(context, x, y, context.IconProvider.RightArrow, 3, HotSpotType.Click) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, $"<Size={MemorySize}>") + context.Font.Width;
			x = AddIcon(context, x + 2, y, context.IconProvider.Change, 4, HotSpotType.ChangeWrappedType);

			x += context.Font.Width;
			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, context.Font.Height);

			if (LevelsOpen[context.Level])
			{
				var childSize = DrawChild(context, tx, y);

				size.Width = Math.Max(size.Width, childSize.Width + tx - origX);
				size.Height += childSize.Height;
			}

			return size;
		}

		protected abstract Size DrawChild(DrawContext context, int x, int y);

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

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				if (int.TryParse(spot.Text, out var value))
				{
					if (spot.Id == 0 && !IsReadOnly)
					{
						if (value != 0)
						{
							Count = value;

							if (CurrentIndex >= value)
							{
								CurrentIndex = value - 1;
							}

							GetParentContainer()?.ChildHasChanged(this);
						}
					}
					else
					{
						if (value < Count)
						{
							CurrentIndex = value;
						}
					}
				}
			}
			else if (spot.Id == 2)
			{
				if (CurrentIndex > 0)
				{
					--CurrentIndex;
				}
			}
			else if (spot.Id == 3)
			{
				if (CurrentIndex < Count - 1)
				{
					++CurrentIndex;
				}
			}
		}
	}
}
