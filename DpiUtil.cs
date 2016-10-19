using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ReClassNET
{
	public static class DpiUtil
	{
		private const int StdDpi = 96;

		private static bool m_bInitialized = false;

		private static int m_nDpiX = StdDpi;
		private static int m_nDpiY = StdDpi;

		private static double m_dScaleX = 1.0;
		private static double m_dScaleY = 1.0;

		public static bool ScalingRequired
		{
			get
			{
				if (Program.DesignMode)
				{
					return false;
				}

				EnsureInitialized();

				return m_nDpiX != StdDpi || m_nDpiY != StdDpi;
			}
		}

		private static void EnsureInitialized()
		{
			if (m_bInitialized)
			{
				return;
			}

			try
			{
				using (var g = Graphics.FromHwnd(IntPtr.Zero))
				{
					m_nDpiX = (int)g.DpiX;
					m_nDpiY = (int)g.DpiY;

					if (m_nDpiX <= 0 || m_nDpiY <= 0)
					{
						m_nDpiX = StdDpi;
						m_nDpiY = StdDpi;
					}
				}
			}
			catch
			{

			}

			m_dScaleX = (double)m_nDpiX / (double)StdDpi;
			m_dScaleY = (double)m_nDpiY / (double)StdDpi;

			m_bInitialized = true;
		}

		public static void ConfigureProcess()
		{
			try
			{
				if (WinUtil.IsAtLeastWindows10)
				{
					Natives.SetProcessDpiAwareness(Natives.ProcessDpiAwareness.SystemAware);
				}
				else if (WinUtil.IsAtLeastWindowsVista)
				{
					Natives.SetProcessDPIAware();
				}
			}
			catch
			{

			}
		}

		public static int ScaleIntX(int i)
		{
			EnsureInitialized();

			return (int)Math.Round(i * m_dScaleX);
		}

		public static int ScaleIntY(int i)
		{
			EnsureInitialized();

			return (int)Math.Round(i * m_dScaleY);
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
