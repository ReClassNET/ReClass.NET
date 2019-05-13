using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class FloatNode : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Float";
			icon = Properties.Resources.B16x16_Button_Float;
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return DrawNumeric(view, x, y, Icons.Float, "Float", ReadValueFromMemory(view.Memory).ToString("0.000"), null);
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (float.TryParse(spot.Text, out var val))
				{
					spot.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}

		public float ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadFloat(Offset);
		}
	}
}
