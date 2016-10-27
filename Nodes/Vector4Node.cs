using System.Runtime.InteropServices;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	class Vector4Node : BaseMatrixNode
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

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 4 * 4;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x2, int y2)
		{
			return DrawVectorType(view, x2, y2, "Vector4", (ref int x, ref int y) =>
			{
				var value = view.Memory.ReadObject<Vector4Data>(Offset);

				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "(");
				x = AddText(view, x, y, Program.Settings.ValueColor, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 2, $"{value.Z:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 3, $"{value.W:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ")");
			});
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 4);
		}
	}
}
