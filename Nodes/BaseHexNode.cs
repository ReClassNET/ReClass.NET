using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseHexNode : BaseNode
	{
		public static DateTime CurrentHighlightTime;
		public static readonly TimeSpan HightlightDuration = TimeSpan.FromSeconds(1);

		private static readonly Dictionary<IntPtr, ValueTypeWrapper<DateTime>> HighlightTimer = new Dictionary<IntPtr, ValueTypeWrapper<DateTime>>();

		private readonly byte[] buffer;

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

			view.Memory.ReadBytes(Offset, buffer);

			var color = view.Settings.HexColor;
			if (view.Settings.HighlightChangedValues)
			{
				var address = view.Address.Add(Offset);

				HighlightTimer.RemoveWhere(kv => kv.Value.Value < CurrentHighlightTime);

				ValueTypeWrapper<DateTime> until;
				if (HighlightTimer.TryGetValue(address, out until))
				{
					if (until.Value >= CurrentHighlightTime)
					{
						color = view.Settings.HighlightColor;

						if (view.Memory.HasChanged(Offset, MemorySize))
						{
							until.Value = CurrentHighlightTime.Add(HightlightDuration);
						}
					}
				}
				else if (view.Memory.HasChanged(Offset, MemorySize))
				{
					HighlightTimer.Add(address, CurrentHighlightTime.Add(HightlightDuration));

					color = view.Settings.HighlightColor;
				}
			}

			for (var i = 0; i < length; ++i)
			{
				x = AddText(view, x, y, color, i, $"{buffer[i]:X02}") + view.Font.Width;
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
