using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Vector3Node : BaseMatrixNode
	{
		public override int ValueTypeSize => sizeof(float);

		public override int MemorySize => 3 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Vector3";
			icon = Properties.Resources.B16x16_Button_Vector_3;
		}

		public override Size Draw(DrawContext context, int x2, int y2)
		{
			return DrawVectorType(context, x2, y2, "Vector3", 3);
		}

		protected override int CalculateValuesHeight(DrawContext context)
		{
			return 0;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 3);
		}
	}
}
