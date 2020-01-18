using System;
using System.Drawing;
using System.Globalization;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class IntPtrNode : BaseNumericNode
	{
		public override int MemorySize => IntPtr.Size;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "IntPtr";
			icon = Properties.Resources.B16x16_Button_Int_Ptr;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Signed, "IntPtr", value.ToString(), $"0x{value.ToString("X")}");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0 || spot.Id == 1) {
#if RECLASSNET64
				if (long.TryParse(spot.Text, out var val) 
					|| spot.Text.TryGetHexString(out var hexValue) 
					&& long.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, (IntPtr)val);
				}
#else
				if (int.TryParse(spot.Text, out var val) 
					|| spot.Text.TryGetHexString(out var hexValue) 
					&& int.TryParse(hexValue, NumberStyles.HexNumber, null, out val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, (UIntPtr)val);
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
