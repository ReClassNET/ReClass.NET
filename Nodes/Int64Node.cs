using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class Int64Node : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Signed, "Int64", view.Memory.ReadObject<long>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				long val;
				if (long.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
