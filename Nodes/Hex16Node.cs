using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.Nodes
{
	class Hex16Node : BaseHexNode
	{
		public override int MemorySize => 2;

		public Hex16Node()
		{
			buffer = new byte[2];
		}

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var value = memory.ReadObject<UInt16Data>(Offset);

			return $"Int16: {value.ShortValue}\nWORD: 0x{value.UShortValue:X04}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 2) + "       " : null, 2);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 2);
		}
	}
}
