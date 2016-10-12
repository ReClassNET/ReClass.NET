using System;
using System.Globalization;

namespace ReClassNET.Nodes
{
	abstract class BaseHexNode : BaseNode
	{
		protected byte[] buffer;
		private DateTime highlightUntil;

		public static DateTime CurrentHighlightTime;

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

			var color = view.Settings.HighlightChangedValues && highlightUntil > CurrentHighlightTime ? view.Settings.HighlightColor : view.Settings.Hex;
			for (var i = 0; i < length; ++i)
			{
				var b = view.Memory.ReadByte(Offset + i);
				if (buffer[i] != b)
				{
					highlightUntil = CurrentHighlightTime.AddSeconds(1);
					buffer[i] = b;
				}

				x = AddText(view, x, y, color, i, $"{b:X02}") + view.Font.Width;
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
