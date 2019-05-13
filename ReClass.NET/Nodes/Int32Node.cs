using System.Drawing;
using System.Globalization;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Int32Node : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Int32";
			icon = Properties.Resources.B16x16_Button_Int_32;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Signed, "Int32", value.ToString(), $"0x{value:X}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				if (int.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && int.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public int ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadInt32(Offset);
		}
	}
}
