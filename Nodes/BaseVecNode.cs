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

		public void Update(HotSpot spot, int max)
		{
			base.Update(spot);

			if (spot.Id >= 0 && spot.Id < max)
			{
				float val;
				if (float.TryParse(spot.Text, out val))
				{
					//WriteMemory()
				}
			}
		}
	}
}
