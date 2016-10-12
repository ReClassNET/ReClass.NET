namespace ReClassNET.Nodes
{
	class ClassInstanceNode : BaseReferenceNode
	{
		public override int MemorySize => InnerNode.MemorySize;

		public override void Intialize()
		{
			InnerNode = new ClassNode();
			InnerNode.Intialize();
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Class, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Instance ");
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name}>");
			x = AddIcon(view, x, y, Icons.Change, 4, HotSpotType.ChangeX);

			x += view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;

			if (levelsOpen[view.Level])
			{
				var v = view.Clone();
				v.Address = view.Address.Add(Offset);
				v.Memory = view.Memory.Clone();
				v.Memory.Offset = Offset.ToInt32();

				y = InnerNode.Draw(v, tx, y);
			}

			return y;
		}
	}
}
