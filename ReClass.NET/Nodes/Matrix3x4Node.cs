using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Matrix3x4Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct Matrix3x4Data
		{
			[FieldOffset(0)]
			public readonly float _11;
			[FieldOffset(4)]
			public readonly float _12;
			[FieldOffset(8)]
			public readonly float _13;
			[FieldOffset(12)]
			public readonly float _14;
			[FieldOffset(16)]
			public readonly float _21;
			[FieldOffset(20)]
			public readonly float _22;
			[FieldOffset(24)]
			public readonly float _23;
			[FieldOffset(28)]
			public readonly float _24;
			[FieldOffset(32)]
			public readonly float _31;
			[FieldOffset(36)]
			public readonly float _32;
			[FieldOffset(40)]
			public readonly float _33;
			[FieldOffset(44)]
			public readonly float _34;
		}

		public override int ValueTypeSize => sizeof(float);

		public override int MemorySize => 12 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Matrix 3x4";
			icon = Properties.Resources.B16x16_Button_Matrix_3x4;
		}

		public override Size Draw(ViewInfo view, int x2, int y2)
		{
			return DrawMatrixType(view, x2, y2, "Matrix (3x4)", (int defaultX, ref int maxX, ref int y) =>
			{
				var value = view.Memory.ReadObject<Matrix3x4Data>(Offset);

				y += view.Font.Height;
				var x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 0, $"{value._11,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 1, $"{value._12,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 2, $"{value._13,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 3, $"{value._14,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 4, $"{value._21,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 5, $"{value._22,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 6, $"{value._23,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 7, $"{value._24,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 8, $"{value._31,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 9, $"{value._32,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 10, $"{value._33,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 11, $"{value._34,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);
			});
		}

		protected override int CalculateValuesHeight(ViewInfo view)
		{
			return 3 * view.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 12);
		}
	}
}
