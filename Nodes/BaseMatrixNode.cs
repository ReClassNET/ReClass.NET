using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseMatrixNode : BaseNode
	{
		protected BaseMatrixNode()
		{
			levelsOpen.DefaultValue = true;
		}

		protected delegate void DrawMatrixValues(int x, ref int maxX, ref int y);

		protected Size DrawMatrixType(ViewInfo view, int x, int y, string type, DrawMatrixValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Matrix, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			x += view.Font.Width;

			x = AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				drawValues(tx, ref x, ref y);
			}

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x - origX, y - origY + view.Font.Height);
		}

		protected delegate void DrawVectorValues(ref int x, ref int y);
		protected Size DrawVectorType(ViewInfo view, int x, int y, string type, DrawVectorValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Vector, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			if (levelsOpen[view.Level])
			{
				drawValues(ref x, ref y);
			}

			x += view.Font.Width;

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x - origX, y - origY + view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (levelsOpen[view.Level])
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
				float val;
				if (float.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
