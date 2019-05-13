using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Hex16Node : BaseHexNode
	{
		public override int MemorySize => 2;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Hex16";
			icon = Properties.Resources.B16x16_Button_Hex_16;
		}

		public override string GetToolTipText(HotSpot spot)
		{
			var value = spot.Memory.ReadObject<UInt16Data>(Offset);

			return $"Int16: {value.ShortValue}\nUInt16: 0x{value.UShortValue:X04}";
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowNodeText ? view.Memory.ReadString(view.Settings.RawDataEncoding, Offset, 2) + "       " : null, 2);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 2);
		}
	}
}
