using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FloatNode : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Float, "Float", ReadValueFromMemory(view.Memory).ToString("0.000"));
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (float.TryParse(spot.Text, out var val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public float ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadFloat(Offset);
		}
	}
}
