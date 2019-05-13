using System.Diagnostics.Contracts;
using System.Drawing;
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

		protected Size DrawMatrixType(ViewInfo view, int x, int y, string type, DrawMatrixValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Matrix, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddOpenCloseIcon(view, x, y);

			x += view.Font.Width;

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			if (LevelsOpen[view.Level])
			{
				drawValues(tx, ref x, ref y);
			}

			return new Size(x - origX, y - origY + view.Font.Height);
		}

		protected delegate void DrawVectorValues(ref int x, ref int y);
		protected Size DrawVectorType(ViewInfo view, int x, int y, string type, DrawVectorValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicatorIcon(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Vector, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddOpenCloseIcon(view, x, y);

			if (LevelsOpen[view.Level])
			{
				drawValues(ref x, ref y);
			}

			x += view.Font.Width;

			x = AddComment(view, x, y);

			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			return new Size(x - origX, y - origY + view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
			{
				height += CalculateValuesHeight(view);
			}
			return height;
		}

		protected abstract int CalculateValuesHeight(ViewInfo view);

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
