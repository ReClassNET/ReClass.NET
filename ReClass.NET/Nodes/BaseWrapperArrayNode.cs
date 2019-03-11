using System;
using System.Drawing;
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

		protected Size Draw(ViewInfo view, int x, int y, string type)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenCloseIcon(view, x, y);
			x = AddIcon(view, x, y, Icons.Array, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "[");
			x = AddText(view, x, y, view.Settings.IndexColor, IsReadOnly ? HotSpot.NoneId : 0, Count.ToString());
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "]");

			x = AddIcon(view, x, y, Icons.LeftArrow, 2, HotSpotType.Click);
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "(");
			x = AddText(view, x, y, view.Settings.IndexColor, 1, CurrentIndex.ToString());
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, ")");
			x = AddIcon(view, x, y, Icons.RightArrow, 3, HotSpotType.Click) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<Size={MemorySize}>") + view.Font.Width;
			x = AddIcon(view, x + 2, y, Icons.Change, 4, HotSpotType.ChangeWrappedType);

			x += view.Font.Width;
			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, view.Font.Height);

			if (LevelsOpen[view.Level])
			{
				var childSize = DrawChild(view, tx, y);

				size.Width = Math.Max(size.Width, childSize.Width + tx - origX);
				size.Height += childSize.Height;
			}

			return size;
		}

		protected abstract Size DrawChild(ViewInfo view, int x, int y);

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
			{
				height += InnerNode.CalculateDrawnHeight(view);
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
