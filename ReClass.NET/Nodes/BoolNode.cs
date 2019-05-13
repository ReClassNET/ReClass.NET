using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class BoolNode : BaseNumericNode
	{
		public override int MemorySize => 1;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Bool";
			icon = Properties.Resources.B16x16_Button_Bool;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding + Icons.Dimensions;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Bool") + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			}
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "=") + view.Font.Width;

			var value = view.Memory.ReadUInt8(Offset);
			x = AddText(view, x, y, view.Settings.ValueColor, 0, value == 0 ? "false" : "true") + view.Font.Width;

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			return new Size(x - origX, view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			return IsHidden && !IsWrapped ? HiddenHeight : view.Font.Height;
		}

		/// <summary>Updates the node from the given spot and sets the value.</summary>
		/// <param name="spot">The spot.</param>
		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (bool.TryParse(spot.Text, out var val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, (byte)(val ? 1 : 0));
				}
			}
		}
	}
}
