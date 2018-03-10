using System.Drawing;
using System.Globalization;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class UInt16Node : BaseNumericNode
	{
		public override int MemorySize => 2;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt16", value.ToString(), $"0x{value:X}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
				if (ushort.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && ushort.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public ushort ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadUInt16(Offset);
		}
	}
}
