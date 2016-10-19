using System;
using System.Globalization;

namespace ReClassNET.Nodes
{
	abstract class BaseHexNode : BaseNode
	{
		protected byte[] buffer;
		private DateTime highlightUntil;

		public static DateTime CurrentHighlightTime;
		public static TimeSpan HightlightDuration = TimeSpan.FromSeconds(1);

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

			if (Program.Settings.ShowText)
			{
				x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.NoneId, text);
			}

			var color = Program.Settings.HighlightChangedValues && highlightUntil > CurrentHighlightTime ? Program.Settings.HighlightColor : Program.Settings.HexColor;
			var changed = false;
			for (var i = 0; i < length; ++i)
			{
				var b = view.Memory.ReadByte(Offset + i);
				if (buffer[i] != b)
				{
					changed = true;

					buffer[i] = b;
				}

				x = AddText(view, x, y, color, i, $"{b:X02}") + view.Font.Width;
			}

			if (changed)
			{
				highlightUntil = CurrentHighlightTime.Add(HightlightDuration);
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
