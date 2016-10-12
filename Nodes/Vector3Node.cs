using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Vector3Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Vector3Data
		{
			[FieldOffset(0)]
			public float X;
			[FieldOffset(4)]
			public float Y;
			[FieldOffset(8)]
			public float Z;
		}

		public override int MemorySize => 3 * 4;

		public override int Draw(ViewInfo view, int x2, int y2)
		{
			return DrawVectorType(view, x2, y2, "Vector3", (ref int x, ref int y) =>
			{
				var value = view.Memory.ReadObject<Vector3Data>(Offset);

				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, "(");
				x = AddText(view, x, y, view.Settings.Value, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 2, $"{value.Z:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ")");
			});
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 3);
		}
	}
}
