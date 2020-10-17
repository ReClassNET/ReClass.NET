using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseMatrixNode : BaseNode
	{
		/// <summary>Size of the value type in bytes.</summary>
		public abstract int ValueTypeSize { get; }

		protected BaseMatrixNode()
		{
			LevelsOpen.DefaultValue = true;
		}

		protected delegate void DrawMatrixValues(int x, ref int maxX, ref int y);

		protected Size DrawMatrixType(DrawContext context, int x, int y, string type, DrawMatrixValues drawValues)
		{
			Contract.Requires(context != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, context.IconProvider.Matrix, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddOpenCloseIcon(context, x, y);

			x += context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			if (LevelsOpen[context.Level])
			{
				drawValues(tx, ref x, ref y);
			}

			return new Size(x - origX, y - origY + context.Font.Height);
		}

		protected delegate void DrawVectorValues(ref int x, ref int y);
		protected Size DrawVectorType(DrawContext context, int x, int y, string type, DrawVectorValues drawValues)
		{
			Contract.Requires(context != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			DrawInvalidMemoryIndicatorIcon(context, y);

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, context.IconProvider.Vector, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddOpenCloseIcon(context, x, y);

			if (LevelsOpen[context.Level])
			{
				drawValues(ref x, ref y);
			}

			x += context.Font.Width;

			x = AddComment(context, x, y);

			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			return new Size(x - origX, y - origY + context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level])
			{
				height += CalculateValuesHeight(context);
			}
			return height;
		}

		protected abstract int CalculateValuesHeight(DrawContext context);

		public void Update(HotSpot spot, int max)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id >= 0 && spot.Id < max)
			{
				if (float.TryParse(spot.Text, out var val))
				{
					spot.Process.WriteRemoteMemory(spot.Address + spot.Id * ValueTypeSize, val);
				}
			}
		}
	}
}
