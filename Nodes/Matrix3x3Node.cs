using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Matrix3x3Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Matrix3x3Data
		{
			[FieldOffset(0)]
			public float _11;
			[FieldOffset(4)]
			public float _12;
			[FieldOffset(8)]
			public float _13;
			[FieldOffset(12)]
			public float _21;
			[FieldOffset(16)]
			public float _22;
			[FieldOffset(20)]
			public float _23;
			[FieldOffset(24)]
			public float _31;
			[FieldOffset(28)]
			public float _32;
			[FieldOffset(32)]
			public float _33;
		}

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => 9 * 4;

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x2, int y2)
		{
			Contract.Requires(view != null);

			return DrawMatrixType(view, x2, y2, "Matrix (3x3)", (ref int x, ref int y, int defaultX) =>
			{
				var value = view.Memory.ReadObject<Matrix3x3Data>(Offset);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 0, $"{value._11,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 1, $"{value._12,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 2, $"{value._13,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 3, $"{value._21,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 4, $"{value._22,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 5, $"{value._23,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, Program.Settings.ValueColor, 6, $"{value._31,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 7, $"{value._32,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, Program.Settings.ValueColor, 8, $"{value._33,14:0.000}");
				x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NoneId, "|");
			});
		}

		public override void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			base.Update(spot);

			Update(spot, 9);
		}
	}
}
