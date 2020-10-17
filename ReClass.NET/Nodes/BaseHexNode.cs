using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public abstract class BaseHexNode : BaseNode
	{
		private static readonly Random highlightRandom = new Random();
		private static readonly Color[] highlightColors = {
			Color.Aqua, Color.Aquamarine, Color.Blue, Color.BlueViolet, Color.Chartreuse, Color.Crimson, Color.LawnGreen, Color.Magenta
		};
		private static Color GetRandomHighlightColor() => highlightColors[highlightRandom.Next(highlightColors.Length)];

		private static readonly TimeSpan hightlightDuration = TimeSpan.FromSeconds(1);

		private static readonly Dictionary<IntPtr, ValueTypeWrapper<DateTime>> highlightTimer = new Dictionary<IntPtr, ValueTypeWrapper<DateTime>>();

		private readonly byte[] buffer;

		protected BaseHexNode()
		{
			Contract.Ensures(buffer != null);

			buffer = new byte[MemorySize];
		}

		protected Size Draw(DrawContext context, int x, int y, string text, int length)
		{
			Contract.Requires(context != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);
			x = AddIconPadding(context, x);

			x = AddAddressOffset(context, x, y);

			if (!string.IsNullOrEmpty(text))
			{
				x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, text);
			}

			context.Memory.ReadBytes(Offset, buffer);

			var color = context.Settings.HexColor;
			if (context.Settings.HighlightChangedValues)
			{
				var address = context.Address + Offset;

				highlightTimer.RemoveWhere(kv => kv.Value.Value < context.CurrentTime);

				if (highlightTimer.TryGetValue(address, out var until))
				{
					if (until.Value >= context.CurrentTime)
					{
						color = GetRandomHighlightColor();

						if (context.Memory.HasChanged(Offset, MemorySize))
						{
							until.Value = context.CurrentTime.Add(hightlightDuration);
						}
					}
				}
				else if (context.Memory.HasChanged(Offset, MemorySize))
				{
					highlightTimer.Add(address, context.CurrentTime.Add(hightlightDuration));

					color = GetRandomHighlightColor();
				}
			}

			for (var i = 0; i < length; ++i)
			{
				x = AddText(context, x, y, color, i, $"{buffer[i]:X02}") + context.Font.Width;
			}

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			return new Size(x - origX, context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			return IsHidden && !IsWrapped ? HiddenHeight : context.Font.Height;
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
				if (byte.TryParse(spot.Text, NumberStyles.HexNumber, null, out var val))
				{
					spot.Process.WriteRemoteMemory(spot.Address + spot.Id, val);
				}
			}
		}

		public byte[] ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadBytes(Offset, MemorySize);
		}
	}
}
