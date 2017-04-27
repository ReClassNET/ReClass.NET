using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	[ContractClass(typeof(BaseArrayNodeContract))]
	public abstract class BaseArrayNode : BaseReferenceNode
	{
		public int CurrentIndex { get; set; }
		public int Count { get; set; } = 1;

		protected Size Draw(ViewInfo view, int x, int y, string type, HotSpotType exchange)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Array, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "[");
			x = AddText(view, x, y, view.Settings.IndexColor, 0, Count.ToString());
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "]");

			x = AddIcon(view, x, y, Icons.LeftArrow, 2, HotSpotType.Click);
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, "(");
			x = AddText(view, x, y, view.Settings.IndexColor, 1, CurrentIndex.ToString());
			x = AddText(view, x, y, view.Settings.IndexColor, HotSpot.NoneId, ")");
			x = AddIcon(view, x, y, Icons.RightArrow, 3, HotSpotType.Click) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name} Size={MemorySize}>") + view.Font.Width;
			x = AddIcon(view, x + 2, y, Icons.Change, 4, exchange);

			x += view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (levelsOpen[view.Level])
			{
				var childSize = DrawChild(view, tx, y);

				size.Width = Math.Max(size.Width, childSize.Width + tx - origX);
				size.Height += childSize.Height;
			}

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return size;
		}

		protected abstract Size DrawChild(ViewInfo view, int x, int y);

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (levelsOpen[view.Level])
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
				int value;
				if (int.TryParse(spot.Text, out value))
				{
					if (spot.Id == 0)
					{
						if (value != 0)
						{
							Count = value;

							ParentNode.ChildHasChanged(this);
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

	[ContractClassFor(typeof(BaseArrayNode))]
	internal abstract class BaseArrayNodeContract : BaseArrayNode
	{
		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}
	}
}
