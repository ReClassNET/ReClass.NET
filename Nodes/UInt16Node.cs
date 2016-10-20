using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UInt16Node : BaseNumericNode
	{
		public override int MemorySize => 2;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt16", view.Memory.ReadObject<ushort>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				ushort val;
				if (ushort.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
