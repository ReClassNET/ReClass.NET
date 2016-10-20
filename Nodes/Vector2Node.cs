using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Vector2Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Vector2Data
		{
			[FieldOffset(0)]
			public float X;
			[FieldOffset(4)]
			public float Y;
		}

		public override int MemorySize => 2 * 4;

		public override int Draw(ViewInfo view, int x2, int y2)
		{
			Contract.Requires(view != null);

			return DrawVectorType(view, x2, y2, "Vector2", (ref int x, ref int y) =>
			{
				var value = view.Memory.ReadObject<Vector2Data>(Offset);

				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "(");
				x = AddText(view, x, y, Program.Settings.ValueColor, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ")");
			});
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			Update(spot, 2);
		}
	}
}
