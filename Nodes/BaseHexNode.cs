using System;
using System.Globalization;

namespace ReClassNET.Nodes
{
	abstract class BaseHexNode : BaseNode
	{
		public int Draw(ViewInfo view, int x, int y, string text, int length)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TXOFFSET + 16;
			x = AddAddressOffset(view, x, y);

			if (view.Settings.ShowText)
			{
				x = AddText(view, x, y, view.Settings.Text, HotSpot.NoneId, text);
			}

			for (var i = 0; i < length; ++i)
			{
				x = AddText(view, x, y, view.Settings.Hex, i, $"{view.Memory.ReadByte(Offset + i):X02}") + view.Font.Width;
			}

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public void Update(HotSpot spot, int length)
		{
			base.Update(spot);

			if (spot.Id >= 0 && spot.Id < length)
			{
				byte val;
				if (byte.TryParse(spot.Text, NumberStyles.HexNumber, null, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address + spot.Id, val);
				}
			}
		}
	}
}
