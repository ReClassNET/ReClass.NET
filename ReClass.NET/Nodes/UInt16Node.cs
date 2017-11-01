using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class UInt16Node : BaseNumericNode
	{
		public override int MemorySize => 2;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt16", ReadValueFromMemory(view.Memory).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (ushort.TryParse(spot.Text, out var val))
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
