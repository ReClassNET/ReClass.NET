using System.Drawing;
using System.Globalization;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Int64Node : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Int64";
			icon = Properties.Resources.B16x16_Button_Int_64;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Signed, "Int64", value.ToString(), $"0x{value:X}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				if (long.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && long.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public long ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadInt64(Offset);
		}
	}
}
