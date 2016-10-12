namespace ReClassNET.Nodes
{
	abstract class BaseTextNode : BaseNode
	{
		private int memorySize;

		public override int MemorySize => memorySize;

		public abstract int CharacterSize { get; }

		public BaseTextNode()
		{

		}

		public override void CopyFromNode(BaseNode node)
		{
			memorySize = node.MemorySize;
		}

		public int DrawText(ViewInfo view, int x, int y, string type, int length, string text)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TXOFFSET;
			x = AddIcon(view, x, y, Icons.Text, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, type);
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name);
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, "[");
			x = AddText(view, x, y, view.Settings.Index, 0, length.ToString());
			x = AddText(view, x, y, view.Settings.Index, HotSpot.NoneId, "]") + view.Font.Width;

			x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, "= '");
			x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, text.LimitLength(150));
			x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, "'") + view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				int val;
				if (int.TryParse(spot.Text, out val))
				{
					memorySize = val * CharacterSize;

					ParentNode.NotifyMemorySizeChanged();
				}
			}
		}
	}
}
