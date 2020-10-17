using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseNumericNode : BaseNode
	{
		/// <summary>Draws the node.</summary>
		/// <param name="context">The drawing context.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="icon">The icon of the node.</param>
		/// <param name="type">The type of the node.</param>
		/// <param name="value">The value of the node.</param>
		/// <param name="alternativeValue">An alternative value of the node.</param>
		/// <returns>The pixel size the node occupies.</returns>
		protected Size DrawNumeric(DrawContext context, int x, int y, Image icon, string type, string value, string alternativeValue)
		{
			Contract.Requires(context != null);
			Contract.Requires(icon != null);
			Contract.Requires(type != null);
			Contract.Requires(value != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, icon, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}
			x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NoneId, "=") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.ValueColor, 0, value) + context.Font.Width;
			if (alternativeValue != null)
			{
				x = AddText(context, x, y, context.Settings.ValueColor, 1, alternativeValue) + context.Font.Width;
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
	}
}
