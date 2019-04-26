using System.Drawing;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class ColorExtensionTest
	{
		public static TheoryData<Color, Color> GetTestInvertedColorData() => new TheoryData<Color, Color>
		{
			{ Color.White, Color.Black },
			{ Color.Black, Color.White },
			{ Color.Red, Color.Cyan },
			{ Color.Cyan, Color.Red },
			{ Color.Blue, Color.Yellow },
			{ Color.Yellow, Color.Blue },
			{ Color.Lime, Color.Fuchsia },
			{ Color.Fuchsia, Color.Lime },
			{ Color.FromArgb(100, 100, 100), Color.FromArgb(155, 155, 155) },
			{ Color.FromArgb(50, 100, 150), Color.FromArgb(205, 155, 105) }
		};

		[Theory]
		[MemberData(nameof(GetTestInvertedColorData))]
		public void TestInvertedColor(Color sut, Color expected)
		{
			Check.That(sut.Invert().ToArgb()).IsEqualTo(expected.ToArgb());
		}

		public static TheoryData<Color, int> GetTestToRgbData() => new TheoryData<Color, int>
		{
			{ Color.White, 0xFF_FF_FF },
			{ Color.Black, 0x00_00_00 },
			{ Color.Red, 0xFF_00_00 },
			{ Color.Cyan, 0x00_FF_FF },
			{ Color.Blue, 0x00_00_FF },
			{ Color.Yellow, 0xFF_FF_00 },
			{ Color.Lime, 0x00_FF_00 },
			{ Color.Fuchsia, 0xFF_00_FF },
			{ Color.FromArgb(100, 100, 100), 100 << 16 | 100 << 8 | 100 },
			{ Color.FromArgb(50, 100, 150), 50 << 16 | 100 << 8 | 150 }
		};

		[Theory]
		[MemberData(nameof(GetTestToRgbData))]
		public void TestToRgb(Color sut, int expected)
		{
			Check.That(sut.ToRgb()).IsEqualTo(expected);
		}
	}
}
