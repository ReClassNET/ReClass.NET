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
		public const int DefalutDpi = 96;

		private static int dpiX = DefalutDpi;
		private static int dpiY = DefalutDpi;

		private static double scaleX = 1.0;
		private static double scaleY = 1.0;

		public static void ConfigureProcess()
		{
			NativeMethods.SetProcessDpiAwareness();
		}

		public static void SetDpi(int x, int y)
		{
			dpiX = x;
			dpiY = y;

			if (dpiX <= 0 || dpiY <= 0)
			{
				dpiX = DefalutDpi;
				dpiY = DefalutDpi;
			}

			scaleX = dpiX / (double)DefalutDpi;
			scaleY = dpiY / (double)DefalutDpi;
		}

		public static void TrySetDpiFromCurrentDesktop()
		{
			try
			{
				using var g = Graphics.FromHwnd(IntPtr.Zero);

				SetDpi((int)g.DpiX, (int)g.DpiY);
			}
			catch
			{
				// ignored
			}
		}

		public static int ScaleIntX(int i)
		{
			return (int)Math.Round(i * scaleX);
		}

		public static int ScaleIntY(int i)
		{
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
