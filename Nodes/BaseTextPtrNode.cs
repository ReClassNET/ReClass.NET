using System;

namespace ReClassNET.Nodes
{
	abstract class BaseTextPtrNode : BaseNode
	{
		public override int MemorySize => IntPtr.Size;

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

			x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, " = '");
			x = AddText(view, x, y, view.Settings.Text, 1, text);
			x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, "' ") + view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				//change address
				
				//WriteMemory(spot.Address
			}
		}
	}
}
