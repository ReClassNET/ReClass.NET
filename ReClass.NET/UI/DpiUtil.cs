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
				using var g = Graphics.FromHwnd(IntPtr.Zero);
				dpiX = (int)g.DpiX;
				dpiY = (int)g.DpiY;

				if (dpiX <= 0 || dpiY <= 0)
				{
					dpiX = StdDpi;
					dpiY = StdDpi;
				}
			}
			catch
			{
				// ignored
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

		public static Image ScaleImage(Image sourceImage)
		{
			if (sourceImage == null)
			{
				return null;
			}

			var width = sourceImage.Width;
			var height = sourceImage.Height;
			var scaledWidth = ScaleIntX(width);
			var scaledHeight = ScaleIntY(height);

			if (width == scaledWidth && height == scaledHeight)
			{
				return sourceImage;
			}

			return ScaleImage(sourceImage, scaledWidth, scaledHeight);
		}

		private static Image ScaleImage(Image sourceImage, int width, int height)
		{
			Contract.Requires(sourceImage != null);
			Contract.Requires(width >= 0);
			Contract.Requires(height >= 0);

			var scaledImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

			using var g = Graphics.FromImage(scaledImage);
			g.Clear(Color.Transparent);

			g.SmoothingMode = SmoothingMode.HighQuality;
			g.CompositingQuality = CompositingQuality.HighQuality;

			var sourceWidth = sourceImage.Width;
			var sourceHeight = sourceImage.Height;

			var interpolationMode = InterpolationMode.HighQualityBicubic;
			if (sourceWidth > 0 && sourceHeight > 0)
			{
				if ((width % sourceWidth) == 0 && (height % sourceHeight) == 0)
				{
					interpolationMode = InterpolationMode.NearestNeighbor;
				}
			}

			g.InterpolationMode = interpolationMode;

			var srcRect = new RectangleF(0.0f, 0.0f, sourceWidth, sourceHeight);
			var destRect = new RectangleF(0.0f, 0.0f, width, height);
			AdjustScaleRects(ref srcRect, ref destRect);

			g.DrawImage(sourceImage, destRect, srcRect, GraphicsUnit.Pixel);

			return scaledImage;
		}

		private static void AdjustScaleRects(ref RectangleF srcRect, ref RectangleF destRect)
		{
			if (destRect.Width > srcRect.Width)
				srcRect.X -= 0.5f;
			if (destRect.Height > srcRect.Height)
				srcRect.Y -= 0.5f;

			if (destRect.Width < srcRect.Width)
				srcRect.X += 0.5f;
			if (destRect.Height < srcRect.Height)
				srcRect.Y += 0.5f;
		}
	}
}
