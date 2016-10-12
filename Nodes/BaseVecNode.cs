using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.Nodes
{
	abstract class BaseVecNode : BaseNode
	{
		public BaseVecNode()
		{
			levelsOpen.DefaultValue = true;
		}

		public int DrawVectorType(ViewInfo view, int x, int y, string type, Func<int, int, int> drawValues)
		{
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

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, type) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			if (levelsOpen[view.Level])
			{
				x = drawValues(x, y);
			}

			x += view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public void Update(HotSpot spot, int max)
		{
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
