using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Extensions
{
	public static class ExtensionColor
	{
		[Pure]
		[DebuggerStepThrough]
		public static int ToRgb(this Color color)
		{
			return 0xFFFFFF & color.ToArgb();
		}

		[Pure]
		[DebuggerStepThrough]
		public static Color Invert(this Color color)
		{
			return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
		}
	}
}
