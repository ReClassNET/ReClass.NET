using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Int64Node : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Signed, "Int64", ReadValueFromMemory(view.Memory).ToString());
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (long.TryParse(spot.Text, out var val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public long ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadInt64(Offset);
		}
	}
}
