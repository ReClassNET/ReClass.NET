using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UInt64Node : BaseNumericNode
	{
		public override int MemorySize => 8;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt64", view.Memory.ReadObject<ulong>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				ulong val;
				if (ulong.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
