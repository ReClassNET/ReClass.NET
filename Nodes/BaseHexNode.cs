using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseHexNode : BaseNode
	{
		private readonly byte[] buffer;
		private DateTime highlightUntil;

		public static DateTime CurrentHighlightTime;
		public static readonly TimeSpan HightlightDuration = TimeSpan.FromSeconds(1);

		protected BaseHexNode()
		{
			Contract.Ensures(buffer != null);

			buffer = new byte[MemorySize];
		}

		protected int Draw(ViewInfo view, int x, int y, string text, int length)
		{
			Contract.Requires(view != null);
			Contract.Requires(text != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TextPadding + 16;
			x = AddAddressOffset(view, x, y);

			if (view.Settings.ShowNodeText)
			{
				x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, text);
			}

			var color = view.Settings.HighlightChangedValues && highlightUntil > CurrentHighlightTime ? view.Settings.HighlightColor : view.Settings.HexColor;
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

			AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			return IsHidden ? HiddenHeight : view.Font.Height;
		}

		/// <summary>Updates the node from the given spot. Sets the value of the selected byte.</summary>
		/// <param name="spot">The spot.</param>
		/// <param name="maxId">The highest spot id.</param>
		public void Update(HotSpot spot, int maxId)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id >= 0 && spot.Id < maxId)
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
