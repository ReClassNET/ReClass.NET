using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace ReClassNET.UI
{
	public static class BannerFactory
	{
		private const int StdHeight = 48; // 96 DPI
		private const int StdIconDim = 32;

		private const int MaxCacheEntries = 20;
		private static readonly Dictionary<string, Image> imageCache = new Dictionary<string, Image>();

		/// <summary>Creates a banner with the given <paramref name="icon"/>, <paramref name="title"/> and <paramref name="text"/>.</summary>
		/// <param name="bannerWidth">Width of the banner.</param>
		/// <param name="bannerHeight">Height of the banner.</param>
		/// <param name="icon">The icon of the banner.</param>
		/// <param name="title">The title of the banner.</param>
		/// <param name="text">The text of the banner.</param>
		/// <param name="skipCache">True to skip cache.</param>
		/// <returns>The created banner.</returns>
		public static Image CreateBanner(int bannerWidth, int bannerHeight, Image icon, string title, string text, bool skipCache)
		{
			Contract.Requires(title != null);
			Contract.Requires(text != null);

			var bannerId = $"{bannerWidth}x{bannerHeight}:{title}:{text}";

			if (skipCache || !imageCache.TryGetValue(bannerId, out var image))
			{
				image = new Bitmap(bannerWidth, bannerHeight, PixelFormat.Format24bppRgb);
				using (var g = Graphics.FromImage(image))
				{
					int xIcon = DpiScaleInt(10, bannerHeight);

					var rect = new Rectangle(0, 0, bannerWidth, bannerHeight);
					using (var brush = new LinearGradientBrush(rect, Color.FromArgb(151, 154, 173), Color.FromArgb(27, 27, 37), LinearGradientMode.Vertical))
					{
						g.FillRectangle(brush, rect);
					}

					int wIconScaled = StdIconDim;
					if (icon != null)
					{
						var iconRel = icon.Width / (float)icon.Height;
						wIconScaled = (int)Math.Round(DpiScaleFloat(iconRel * StdIconDim, bannerHeight));
						var hIconScaled = DpiScaleInt(StdIconDim, bannerHeight);

						int yIcon = (bannerHeight - hIconScaled) / 2;
						if (hIconScaled == icon.Height)
						{
							g.DrawImageUnscaled(icon, xIcon, yIcon);
						}
						else
						{
							g.DrawImage(icon, xIcon, yIcon, wIconScaled, hIconScaled);
						}

						var attributes = new ImageAttributes();
						attributes.SetColorMatrix(new ColorMatrix
						{
							Matrix33 = 0.1f
						});

						int w = wIconScaled * 2;
						int h = hIconScaled * 2;
						int x = bannerWidth - w - xIcon;
						int y = (bannerHeight - h) / 2;
						g.DrawImage(icon, new Rectangle(x, y, w, h), 0, 0, icon.Width, icon.Height, GraphicsUnit.Pixel, attributes);
					}

					int tx = 2 * xIcon;
					int ty = DpiScaleInt(4, bannerHeight);
					if (icon != null)
					{
						tx += wIconScaled;
					}

					float fontSize = DpiScaleFloat((12.0f * 96.0f) / g.DpiY, bannerHeight);
					using (var font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold))
					{
						DrawText(g, title, tx, ty, font, Color.White);
					}

					tx += xIcon;
					ty += xIcon * 2 + 2;

					float fontSizeSmall = DpiScaleFloat((9.0f * 96.0f) / g.DpiY, bannerHeight);
					using (var fontSmall = new Font(FontFamily.GenericSansSerif, fontSizeSmall, FontStyle.Regular))
					{
						DrawText(g, text, tx, ty, fontSmall, Color.White);
					}
				}

				if (!skipCache)
				{
					while (imageCache.Count > MaxCacheEntries)
					{
						imageCache.Remove(imageCache.Keys.First());
					}

					imageCache[bannerId] = image;
				}
			}

			return image;
		}

		private static void DrawText(Graphics g, string text, int x, int y, Font font, Color color)
		{
			using (var brush = new SolidBrush(color))
			{
				using (var format = new StringFormat(StringFormatFlags.FitBlackBox | StringFormatFlags.NoClip))
				{
					g.DrawString(text, font, brush, x, y, format);
				}
			}
		}

		private static int DpiScaleInt(int x, int height) => (int)Math.Round((x * height) / (double)StdHeight);

		private static float DpiScaleFloat(float x, int height) => (x * height) / StdHeight;
	}
}
