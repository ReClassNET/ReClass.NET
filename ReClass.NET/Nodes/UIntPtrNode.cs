using System;
using System.Drawing;
using System.Globalization;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UIntPtrNode : BaseNumericNode
	{
		public override int MemorySize => UIntPtr.Size;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "UIntPtr";
			icon = Properties.Resources.B16x16_Button_UInt_Ptr;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
#if RECLASSNET64
			return DrawNumeric(view, x, y, Icons.Unsigned, "UIntPtr", value.ToString(), $"0x{value.ToUInt64():X}");
#else
			return DrawNumeric(view, x, y, Icons.Unsigned, "UIntPtr", value.ToString(), $"0x{value.ToUInt32():X}");
#endif
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1)
			{
#if RECLASSNET64
				if (ulong.TryParse(spot.Text, out var val) 
					|| spot.Text.TryGetHexString(out var hexValue) 
					&& ulong.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, (UIntPtr)val);
				}
#else
				if (uint.TryParse(spot.Text, out var val) 
					|| spot.Text.TryGetHexString(out var hexValue) 
					&& uint.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, (UIntPtr)val);
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
