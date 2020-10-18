using System;
using System.Drawing;
using System.Globalization;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class IntNativeNode : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "NInt";
			icon = Properties.Resources.B16x16_Button_NInt;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			var value = ReadValueFromMemory(context.Memory);
			return DrawNumeric(context, x, y, context.IconProvider.Signed, "NInt", value.ToInt64().ToString(), "0x" + value.ToString(Constants.AddressHexFormat));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
#if RECLASSNET64
				if (long.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && long.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
#else
				if (int.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && int.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
#endif
			}
		}

		public IntPtr ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadIntPtr(Offset);
		}
	}
}
