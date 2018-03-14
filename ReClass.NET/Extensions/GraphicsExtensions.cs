using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.Extensions
{
	public static class GraphicsExtension
	{
		/// <summary>
		/// Use GDI to render normal text because GDI+ doesn't work nicely with long texts and the custom width calculation.
		/// But GDI is simple, there is no custom rendering (rotation, scale, ...). So the BitFieldNode uses GDI+ for rendering.
		/// </summary>
		/// <param name="g">The graphics context.</param>
		/// <param name="text">The text to render.</param>
		/// <param name="font">The font to use.</param>
		/// <param name="color">The color to use.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static void DrawStringEx(this Graphics g, string text, Font font, Color color, int x, int y)
		{
			TextRenderer.DrawText(g, text, font, new Point(x, y), color);
		}
	}
}
