using System.Drawing;

namespace ReClassNET
{
	class FontEx
	{
		public Font Font { get; set; }
		public Size CharSize { get; set; }
		public int Width => CharSize.Width;
		public int Height => CharSize.Height;
	}
}
