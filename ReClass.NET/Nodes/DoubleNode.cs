using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class DoubleNode : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Double";
			icon = Properties.Resources.B16x16_Button_Double;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Double, "Double", ReadValueFromMemory(view.Memory).ToString("0.000"), null);
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (double.TryParse(spot.Text, out var val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public double ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadDouble(Offset);
		}
	}
}
