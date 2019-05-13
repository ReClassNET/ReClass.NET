using System.Drawing;
using System.Globalization;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UInt64Node : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "UInt64 / QWORD";
			icon = Properties.Resources.B16x16_Button_UInt_64;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt64", value.ToString(), $"0x{value:X}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				if (ulong.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && ulong.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public ulong ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadUInt64(Offset);
		}
	}
}
