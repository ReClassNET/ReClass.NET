namespace ReClassNET.Nodes
{
	abstract class BaseArrayNode : BaseReferenceNode
	{
		public int CurrentIndex { get; set; }
		public int Count { get; set; } = 1;

		public int Draw(ViewInfo view, int x, int y, string name)
		{
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

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, name);
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name);
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, "[");
			x = AddText(view, x, y, view.Settings.Index, 0, Count.ToString());
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, "]");

			x = AddIcon(view, x, y, Icons.LeftBracket, 2, HotSpotType.Click);
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, "(");
			x = AddText(view, x, y, view.Settings.Index, 1, CurrentIndex.ToString());
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, ")");
			x = AddIcon(view, x, y, Icons.RightBracket, 3, HotSpotType.Click);

			x = AddText(view, x, y, view.Settings.Value, HotSpot.NoneId, $"<{InnerNode.Name} Size={MemorySize}>");
			x = AddIcon(view, x + 2, y, Icons.Change, 4, HotSpotType.ChangeX);

			x += view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;

			if (levelsOpen[view.Level])
			{
				y = DrawChild(view, tx, y);
			}

			return y;
		}

		protected abstract int DrawChild(ViewInfo view, int x, int y);

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

							ParentNode.NotifyMemorySizeChanged();
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
