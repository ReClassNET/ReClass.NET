using System;
using System.Drawing;
using System.Runtime.InteropServices;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class Matrix4x4Node : BaseMatrixNode
	{
		[StructLayout(LayoutKind.Explicit)]
		private struct Matrix4x4Data
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
			[FieldOffset(48)]
			public readonly float _41;
			[FieldOffset(52)]
			public readonly float _42;
			[FieldOffset(56)]
			public readonly float _43;
			[FieldOffset(60)]
			public readonly float _44;
		}

		public override int ValueTypeSize => sizeof(float);

		public override int MemorySize => 16 * ValueTypeSize;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Matrix 4x4";
			icon = Properties.Resources.B16x16_Button_Matrix_4x4;
		}

		public override Size Draw(ViewInfo view, int x2, int y2)
		{
			return DrawMatrixType(view, x2, y2, "Matrix (4x4)", (int defaultX, ref int maxX, ref int y) =>
			{
				var value = view.Memory.ReadObject<Matrix4x4Data>(Offset);

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

				y += view.Font.Height;
				x = defaultX;
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				x = AddText(view, x, y, view.Settings.ValueColor, 12, $"{value._41,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 13, $"{value._42,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 14, $"{value._43,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.ValueColor, 15, $"{value._44,14:0.000}");
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NoneId, "|");
				maxX = Math.Max(x, maxX);
			});
		}

		protected override int CalculateValuesHeight(ViewInfo view)
		{
			return 4 * view.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 16);
		}
	}
}
