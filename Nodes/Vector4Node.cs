using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Vector4Node : BaseVecNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Vector4Data
		{
			[FieldOffset(0)]
			public float X;
			[FieldOffset(4)]
			public float Y;
			[FieldOffset(8)]
			public float Z;
			[FieldOffset(12)]
			public float W;
		}

		public override int MemorySize => 4 * 4;

		public override int Draw(ViewInfo view, int x2, int y2)
		{
			return DrawVectorType(view, x2, y2, "Vector4", (x, y) =>
			{
				var value = view.Memory.ReadObject<Vector4Data>(Offset);

				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, "(");
				x = AddText(view, x, y, view.Settings.Value, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 2, $"{value.Z:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 3, $"{value.W:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ")");

				return x;
			});
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 4);
		}
	}
}
