using System;
using System.Drawing;
using System.Globalization;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class NUIntNode : BaseNumericNode
	{
		public override int MemorySize => UIntPtr.Size;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "NUInt";
			icon = Properties.Resources.B16x16_Button_NUInt;
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			var value = ReadValueFromMemory(context.Memory)
#if RECLASSNET64
				.ToUInt64();
#else
				.ToUInt32();
#endif
			return DrawNumeric(context, x, y, context.IconProvider.Unsigned, "NUInt", value.ToString(), $"0x{value:X}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
#if RECLASSNET64
				if (ulong.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && ulong.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
#else
				if (uint.TryParse(spot.Text, out var val) || spot.Text.TryGetHexString(out var hexValue) && uint.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
#endif
			}
		}

		public UIntPtr ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadUIntPtr(Offset);
		}
	}
}
