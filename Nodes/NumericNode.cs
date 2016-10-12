using System.Drawing;

namespace ReClassNET.Nodes
{
	abstract class NumericNode : BaseNode
	{
		public int DrawNumeric(ViewInfo view, int x, int y, Image icon, string type, string value)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x = x + TXOFFSET;

			x = AddIcon(view, x, y, icon, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, "=") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.Value, 0, value) + view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}
	}
}
