using System.Drawing;

namespace ReClassNET.Util
{
	public static class ExtensionColor
	{
		public static Color Invert(this Color color)
		{
			return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
		}
	}
}
