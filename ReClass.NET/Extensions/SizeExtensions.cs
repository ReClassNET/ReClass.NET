using System.Drawing;

namespace ReClassNET.Extensions
{
	public static class SizeExtension
	{
		public static Size Extend(this Size size, int width, int height)
		{
			return new Size(size.Width + width, size.Height + height);
		}
	}
}
