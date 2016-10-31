using System.Runtime.InteropServices;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Matrix3x4Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Matrix3x4Data
		{
			[FieldOffset(0)]
			public float _11;
			[FieldOffset(4)]
			public float _12;
			[FieldOffset(8)]
			public float _13;
			[FieldOffset(12)]
			public float _14;
			[FieldOffset(16)]
			public float _21;
			[FieldOffset(20)]
			public float _22;
			[FieldOffset(24)]
			public float _23;
			[FieldOffset(28)]
			public float _24;
			[FieldOffset(32)]
			public float _31;
			[FieldOffset(36)]
			public float _32;
			[FieldOffset(40)]
			public float _33;
			[FieldOffset(44)]
			public float _34;
		}

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 12 * 4;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x2, int y2)
		{
			return DrawMatrixType(view, x2, y2, "Matrix (3x4)", (ref int x, ref int y, int defaultX) =>
			{
				var value = view.Memory.ReadObject<Matrix3x4Data>(Offset);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 0, $"{value._11,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 1, $"{value._12,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 2, $"{value._13,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 3, $"{value._14,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 4, $"{value._21,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 5, $"{value._22,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 6, $"{value._23,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 7, $"{value._24,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 8, $"{value._31,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 9, $"{value._32,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 10, $"{value._33,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 11, $"{value._34,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
			});
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 12);
		}
	}
}
