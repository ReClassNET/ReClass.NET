using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UInt32Node : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt32", ReadValueFromMemory(view.Memory).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (uint.TryParse(spot.Text, out var val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public uint ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadUInt32(Offset);
		}
	}
}
