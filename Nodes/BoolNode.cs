using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class BoolNode : BaseNumericNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 1;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height th node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x = x + TextPadding + 16;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Bool") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "=") + view.Font.Width;

			var value = view.Memory.ReadByte(Offset);
			x = AddText(view, x, y, view.Settings.ValueColor, 0, value == 0 ? "false" : "true") + view.Font.Width;

			AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			return IsHidden ? HiddenHeight : view.Font.Height;
		}

		/// <summary>Updates the node from the given spot and sets the value.</summary>
		/// <param name="spot">The spot.</param>
		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				bool val;
				if (bool.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, (byte)(val ? 1 : 0));
				}
			}
		}
	}
}
