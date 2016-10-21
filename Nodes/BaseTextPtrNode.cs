using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	abstract class BaseTextPtrNode : BaseNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public int DrawText(ViewInfo view, int x, int y, string type, int length, string text)
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

			x += TXOFFSET;
			x = AddIcon(view, x, y, Icons.Text, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.NoneId, "= '");
			x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.NoneId, text);
			x = AddText(view, x, y, Program.Settings.TextColor, HotSpot.NoneId, "'") + view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}
	}
}
