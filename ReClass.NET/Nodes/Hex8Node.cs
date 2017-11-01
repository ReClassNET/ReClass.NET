using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Hex8Node : BaseHexNode
	{
		public override int MemorySize => 1;

		public override string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			var b = memory.ReadUInt8(Offset);

			return $"Int8: {(int)b}\nUInt8: 0x{b:X02}";
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowNodeText ? view.Memory.ReadPrintableAsciiString(Offset, 1) + "        " : null, 1);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 1);
		}
	}
}
