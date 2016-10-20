using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UInt32Node : BaseNumericNode
	{
		public override int MemorySize => 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt32", view.Memory.ReadObject<uint>(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				uint val;
				if (uint.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
