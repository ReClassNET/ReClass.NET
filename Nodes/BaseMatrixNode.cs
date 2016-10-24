using System.Diagnostics.Contracts;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	abstract class BaseMatrixNode : BaseNode
	{
		public BaseMatrixNode()
		{
			levelsOpen.DefaultValue = true;
		}

		protected delegate void DrawMatrixValues(ref int x, ref int y, int defaultX);

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

			x += TXOFFSET;

			x = AddIcon(view, x, y, Icons.Matrix, HotSpot.NoneId, HotSpotType.None);

			var tx = x;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			x += view.Font.Width;

			x = AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				drawValues(ref x, ref y, tx);
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

			x += TXOFFSET;

			x = AddIcon(view, x, y, Icons.Vector, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			if (levelsOpen[view.Level])
			{
				drawValues(ref x, ref y);
			}

			x += view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

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
