using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class DoubleNode : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Double, "Double", view.Memory.ReadObject<double>(Offset).ToString("0.000"));
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				double val;
				if (double.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
