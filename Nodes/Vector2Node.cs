using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Vector2Node : BaseVecNode
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
			return DrawVectorType(view, x2, y2, "Vector2", (x, y) =>
			{
				var value = view.Memory.ReadObject<Vector2Data>(Offset);

				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, "(");
				x = AddText(view, x, y, view.Settings.Value, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ")");

				return x;
			});
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 2);
		}
	}
}
