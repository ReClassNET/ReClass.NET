using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ReClassNET.Native;

namespace ReClassNET.UI
{
	public static class DpiUtil
	{
		private const int StdDpi = 96;

		private static bool initialized;

		private static int dpiX = StdDpi;
		private static int dpiY = StdDpi;

		private static double scaleX = 1.0;
		private static double scaleY = 1.0;

		public static bool ScalingRequired
		{
			get
			{
				if (Program.DesignMode)
				{
					return false;
				}

				EnsureInitialized();

				return dpiX != StdDpi || dpiY != StdDpi;
			}
		}

		private static void EnsureInitialized()
		{
			if (initialized)
			{
				return;
			}

			try
			{
				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					dpiX = (int)g.DpiX;
					dpiY = (int)g.DpiY;

					if (dpiX <= 0 || dpiY <= 0)
					{
						dpiX = StdDpi;
						dpiY = StdDpi;
					}
				}
			}
			catch
			{

			}

			scaleX = dpiX / (double)StdDpi;
			scaleY = dpiY / (double)StdDpi;

			initialized = true;
		}

		public static void ConfigureProcess()
		{
			NativeMethods.SetProcessDpiAwareness();
		}

		public static int ScaleIntX(int i)
		{
			EnsureInitialized();

			return (int)Math.Round(i * scaleX);
		}

		public static int ScaleIntY(int i)
		{
			EnsureInitialized();

			return (int)Math.Round(i * scaleY);
		}

		public static Image ScaleImage(Image img)
		{
			if (img == null)
			{
				return null;
			}

			int w = img.Width;
			int h = img.Height;
			int sw = ScaleIntX(w);
			int sh = ScaleIntY(h);

			if (w == sw && h == sh)
			{
				return img;
			}

			return ScaleImage(img, sw, sh);
		}

		private static Image ScaleImage(Image img, int w, int h)
		{
			Contract.Requires(img != null);
			Contract.Requires(w >= 0);
			Contract.Requires(h >= 0);

			var bmp = new Bitmap(w, h, PixelFormat.Format32bppArgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.Clear(Color.Transparent);

				g.SmoothingMode = SmoothingMode.HighQuality;
				g.CompositingQuality = CompositingQuality.HighQuality;

				var wSrc = img.Width;
				var hSrc = img.Height;

				InterpolationMode im = InterpolationMode.HighQualityBicubic;
				if (wSrc > 0 && hSrc > 0)
				{
					if ((w % wSrc) == 0 && (h % hSrc) == 0)
					{
						im = InterpolationMode.NearestNeighbor;
					}
				}

				g.InterpolationMode = im;

				var rSource = new RectangleF(0.0f, 0.0f, wSrc, hSrc);
				var rDest = new RectangleF(0.0f, 0.0f, w, h);
				AdjustScaleRects(ref rSource, ref rDest);

				g.DrawImage(img, rDest, rSource, GraphicsUnit.Pixel);
			}

			return bmp;
		}

		private static void AdjustScaleRects(ref RectangleF rSource, ref RectangleF rDest)
		{
			if (rDest.Width > rSource.Width)
				rSource.X = rSource.X - 0.5f;
			if (rDest.Height > rSource.Height)
				rSource.Y = rSource.Y - 0.5f;

			if (rDest.Width < rSource.Width)
				rSource.X = rSource.X + 0.5f;
			if (rDest.Height < rSource.Height)
				rSource.Y = rSource.Y + 0.5f;
		}
	}
}
