using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class UInt8Node : BaseNumericNode
	{
		public override int MemorySize => 1;

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return DrawNumeric(view, x, y, Icons.Unsigned, "UInt8", view.Memory.ReadByte(Offset).ToString());
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			if (spot.Id == 0)
			{
				byte val;
				if (byte.TryParse(spot.Text, out val))
				{
					spot.Memory.Process.WriteRemoteMemory(spot.Address, val);
				}
			}
		}
	}
}
