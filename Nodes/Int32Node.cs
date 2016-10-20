using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class Int32Node : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Signed, "Int32", view.Memory.ReadObject<int>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				int val;
				if (int.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
