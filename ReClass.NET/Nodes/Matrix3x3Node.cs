using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Matrix3x3Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct Matrix3x3Data
		{
			[FieldOffset(0)]
			public readonly float _11;
			[FieldOffset(4)]
			public readonly float _12;
			[FieldOffset(8)]
			public readonly float _13;
			[FieldOffset(12)]
			public readonly float _21;
			[FieldOffset(16)]
			public readonly float _22;
			[FieldOffset(20)]
			public readonly float _23;
			[FieldOffset(24)]
			public readonly float _31;
			[FieldOffset(28)]
			public readonly float _32;
			[FieldOffset(32)]
			public readonly float _33;
		}

		public override int ValueTypeSize => sizeof(float);

		public override int MemorySize => 9 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Matrix 3x3";
			icon = Properties.Resources.B16x16_Button_Matrix_3x3;
		}

		public override Size Draw(ViewInfo view, int x2, int y2)
		{
			return DrawMatrixType(view, x2, y2, "Matrix (3x3)", (int defaultX, ref int maxX, ref int y) =>
			{
				var value = view.Memory.ReadObject<Matrix3x3Data>(Offset);

				y += view.Font.Height;
				var x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 0, $"{value._11,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 1, $"{value._12,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 2, $"{value._13,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 3, $"{value._21,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 4, $"{value._22,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 5, $"{value._23,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 6, $"{value._31,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 7, $"{value._32,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 8, $"{value._33,14:0.000}");
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

			Update(spot, 9);
		}
	}
}
