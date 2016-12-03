using System;
using System.Diagnostics.Contracts;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	[ContractClass(typeof(BaseArrayNodeContract))]
	public abstract class BaseArrayNode : BaseReferenceNode
	{
		public int CurrentIndex { get; set; }
		public int Count { get; set; } = 1;

		protected int Draw(ViewInfo view, int x, int y, string type, HotSpotType exchange)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

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
			x = AddIcon(view, x, y, Icons.RightArrow, 3, HotSpotType.Click);

			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name} Size={MemorySize}>");
			x = AddIcon(view, x + 2, y, Icons.Change, 4, exchange);

			x += view.Font.Width;
			AddComment(view, x, y);

			y += view.Font.Height;

			if (levelsOpen[view.Level])
			{
				y = DrawChild(view, tx, y);
			}

			return y;
		}

		protected abstract int DrawChild(ViewInfo view, int x, int y);

		public override int CalculateHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				h += CalculateChildHeight(view);
			}
			return h;
		}

		protected abstract int CalculateChildHeight(ViewInfo view);

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
		protected override int DrawChild(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}
	}
}
