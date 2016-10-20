using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class FloatNode : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Float, "Float", view.Memory.ReadObject<float>(Offset).ToString("0.000"));
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				float val;
				if (float.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
