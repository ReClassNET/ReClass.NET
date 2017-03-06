using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseTextPtrNode : BaseNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="type">The name of the type.</param>
		/// <param name="text">The text.</param>
		/// <returns>The height the node occupies.</returns>
		public Size DrawText(ViewInfo view, int x, int y, string type, string text)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(text != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TextPadding;
			x = AddIcon(view, x, y, Icons.Text, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, "= '");
			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, text);
			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, "'") + view.Font.Width;

			x = AddComment(view, x, y);

			return new Size(x, y + view.Font.Height);
		}

		public override Size CalculateSize(ViewInfo view)
		{
			return IsHidden ? HiddenSize : new Size(0, view.Font.Height);
		}
	}
}
