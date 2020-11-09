using System;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Hex64Node : BaseHexCommentNode
	{
		public override int MemorySize => 8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Hex64";
			icon = Properties.Resources.B16x16_Button_Hex_64;
		}

		public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
		{
			var value = ReadFromBuffer(spot.Memory, Offset);

			address = value.IntPtr;

			return spot.Process.GetSectionToPointer(value.IntPtr) != null;
		}

		public override string GetToolTipText(HotSpot spot)
		{
			var value = ReadFromBuffer(spot.Memory, Offset);

			return $"Int64: {value.LongValue}\nUInt64: 0x{value.ULongValue:X016}\nFloat: {value.FloatValue:0.000}\nDouble: {value.DoubleValue:0.000}";
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			return Draw(context, x, y, context.Settings.ShowNodeText ? context.Memory.ReadString(context.Settings.RawDataEncoding, Offset, 8) + " " : null, 8);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 8);
		}

		protected override int AddComment(DrawContext context, int x, int y)
		{
			x = base.AddComment(context, x, y);

			var value = ReadFromBuffer(context.Memory, Offset);

			x = AddComment(context, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}

		private static UInt64FloatDoubleData ReadFromBuffer(MemoryBuffer memory, int offset) => new UInt64FloatDoubleData
		{
			Raw1 = memory.ReadInt32(offset),
			Raw2 = memory.ReadInt32(offset + sizeof(int))
		};
	}
}
