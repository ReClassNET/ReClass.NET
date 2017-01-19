using System.Diagnostics.Contracts;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseMatrixNode : BaseNode
	{
		protected BaseMatrixNode()
		{
			levelsOpen.DefaultValue = true;
		}

		protected delegate void DrawMatrixValues(int x, ref int y);

		protected int DrawMatrixType(ViewInfo view, int x, int y, string type, DrawMatrixValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TextPadding;

			x = AddIcon(view, x, y, Icons.Matrix, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			x += view.Font.Width;

			AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				drawValues(tx, ref y);
			}

			return y + view.Font.Height;
		}

		protected delegate void DrawVectorValues(ref int x, ref int y);
		protected int DrawVectorType(ViewInfo view, int x, int y, string type, DrawVectorValues drawValues)
		{
			Contract.Requires(view != null);
			Contract.Requires(type != null);
			Contract.Requires(drawValues != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

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

			AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				h += CalculateValuesHeight(view);
			}
			return h;
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
