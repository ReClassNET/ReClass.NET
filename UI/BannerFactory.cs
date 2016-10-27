using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics.Contracts;

namespace ReClassNET.UI
{
	public static class BannerFactory
	{
		private const int StdHeight = 48; // Standard height for 96 DPI
		private const int StdIconDim = 32;

		private static readonly Dictionary<string, Image> imageCache = new Dictionary<string, Image>();

		public static Image CreateBanner(int nWidth, int nHeight, Image imgIcon, string strTitle, string strLine)
		{
			Contract.Requires(strTitle != null);
			Contract.Requires(strLine != null);

			//Debug.Assert((nHeight == StdHeight) || DpiUtil.ScalingRequired);

			string strImageID = $"{nWidth}x{nHeight}:{strTitle}:{strLine}";

			Image img = null;
			if (imageCache.TryGetValue(strImageID, out img))
			{
				return img;
			}

			const float fVert = 90.0f;

			if (img == null)
			{
				img = new Bitmap(nWidth, nHeight, PixelFormat.Format24bppRgb);
				using (var g = Graphics.FromImage(img))
				{
					int xIcon = DpiScaleInt(10, nHeight);

					var clrStart = Color.FromArgb(151, 154, 173);
					var clrEnd = Color.FromArgb(27, 27, 37);

					var fAngle = fVert;

					var rect = new Rectangle(0, 0, nWidth, nHeight);
					using (var brush = new LinearGradientBrush(rect, clrStart, clrEnd, LinearGradientMode.Vertical/*, fAngle, true*/))
					{
						g.FillRectangle(brush, rect);
					}

					int wIconScaled = StdIconDim;
					int hIconScaled = StdIconDim;
					if (imgIcon != null)
					{
						float fIconRel = (float)imgIcon.Width / (float)imgIcon.Height;
						wIconScaled = (int)Math.Round(DpiScaleFloat(fIconRel * (float)StdIconDim, nHeight));
						hIconScaled = DpiScaleInt(StdIconDim, nHeight);

						int yIcon = (nHeight - hIconScaled) / 2;
						if (hIconScaled == imgIcon.Height)
						{
							g.DrawImageUnscaled(imgIcon, xIcon, yIcon);
						}
						else
						{
							g.DrawImage(imgIcon, xIcon, yIcon, wIconScaled, hIconScaled);
						}

						ColorMatrix cm = new ColorMatrix();
						cm.Matrix33 = 0.1f;
						ImageAttributes ia = new ImageAttributes();
						ia.SetColorMatrix(cm);

						int w = wIconScaled * 2, h = hIconScaled * 2;
						int x = nWidth - w - xIcon, y = (nHeight - h) / 2;
						var rectDest = new Rectangle(x, y, w, h);
						g.DrawImage(imgIcon, rectDest, 0, 0, imgIcon.Width, imgIcon.Height, GraphicsUnit.Pixel, ia);
					}

					int tx = 2 * xIcon;
					int ty = DpiScaleInt(4, nHeight);
					if (imgIcon != null)
					{
						tx += wIconScaled;
					}

					float fFontSize = DpiScaleFloat((12.0f * 96.0f) / g.DpiY, nHeight);
					using (Font font = new Font(FontFamily.GenericSansSerif, fFontSize, FontStyle.Bold))
					{
						// - TextRenderer.MeasureText(g, strTitle, font).Width));
						// g.DrawString(strTitle, font, brush, fx, fy);
						DrawText(g, strTitle, tx, ty, font, Color.White);
					}

					tx += xIcon; // fx
					ty += xIcon * 2 + 2; // fy

					float fFontSizeSm = DpiScaleFloat((9.0f * 96.0f) / g.DpiY, nHeight);
					using (Font fontSmall = new Font(FontFamily.GenericSansSerif, fFontSizeSm, FontStyle.Regular))
					{
						// - TextRenderer.MeasureText(g, strLine, fontSmall).Width));
						// g.DrawString(strLine, fontSmall, brush, fx, fy);
						DrawText(g, strLine, tx, ty, fontSmall, Color.White);
					}
				}
			}

			imageCache[strImageID] = img;

			return img;
		}

		private static void DrawText(Graphics g, string strText, int x, int y, Font font, Color clrForeground)
		{
			using (var brush = new SolidBrush(clrForeground))
			{
				using (var sf = new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip))
				{
					g.DrawString(strText, font, brush, x, y, sf);
				}
			}
		}

		private static int DpiScaleInt(int x, int nBaseHeight)
		{
			return (int)Math.Round((double)(x * nBaseHeight) / (double)StdHeight);
		}

		private static float DpiScaleFloat(float x, int nBaseHeight)
		{
			return (x * (float)nBaseHeight) / (float)StdHeight;
		}

		public static void CreateBannerEx(PictureBox picBox, Image imgIcon, string strTitle, string strLine)
		{
			if (picBox == null)
			{
				return;
			}

			try
			{
				picBox.Image = CreateBanner(picBox.Width, picBox.Height, imgIcon, strTitle, strLine);
			}
			catch (Exception)
			{

			}
		}
	}
}
