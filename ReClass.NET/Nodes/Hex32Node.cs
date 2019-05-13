using System;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Hex32Node : BaseHexCommentNode
	{
		public override int MemorySize => 4;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Hex32";
			icon = Properties.Resources.B16x16_Button_Hex_32;
		}

		public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
		{
			var value = spot.Memory.ReadObject<UInt32FloatData>(Offset);

			address = value.IntPtr;

			return spot.Process?.GetNamedAddress(value.IntPtr) != null;
		}

		public override string GetToolTipText(HotSpot spot)
		{
			var value = spot.Memory.ReadObject<UInt32FloatData>(Offset);

			return $"Int32: {value.IntValue}\nUInt32: 0x{value.UIntValue:X08}\nFloat: {value.FloatValue:0.000}";
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowNodeText ? view.Memory.ReadString(view.Settings.RawDataEncoding, Offset, 4) + "     " : null, 4);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 4);
		}

		protected override int AddComment(ViewInfo view, int x, int y)
		{
			x = base.AddComment(view, x, y);

			var value = view.Memory.ReadObject<UInt32FloatData>(Offset);

			x = AddComment(view, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}
	}
}
