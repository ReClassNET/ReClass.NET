using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Int8Node : BaseNumericNode
	{
		public override int MemorySize => 1;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			var value = ReadValueFromMemory(view.Memory);
			return DrawNumeric(view, x, y, Icons.Signed, "Int8", $"{value} (0x{value:X})");
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (sbyte.TryParse(spot.Text, out var val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public sbyte ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadInt8(Offset);
		}
	}
}
