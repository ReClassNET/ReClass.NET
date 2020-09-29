using System.Drawing;
using NFluent;
using ReClassNET.UI;
using Xunit;

namespace ReClass.NET_Tests.UI
{
	public class DpiUtilTest
	{
		[Fact]
		public void ScaleImageReturnsNullOnNull()
		{
			Check.That(DpiUtil.ScaleImage(null)).IsNull();
		}

		[Fact]
		public void ScaleImageReturnsOriginalInstanceOnSameSize()
		{
			DpiUtil.SetDpi(DpiUtil.DefalutDpi, DpiUtil.DefalutDpi);

			using var sourceImage = new Bitmap(10, 10);
			var scaledImage = DpiUtil.ScaleImage(sourceImage);

			Check.That(sourceImage).IsSameReferenceAs(scaledImage);
		}

		[Fact]
		public void ScaleImageReturnsScaledImage()
		{
			const int SourceSize = 10;
			const int ScaleFactor = 2;

			DpiUtil.SetDpi(DpiUtil.DefalutDpi * ScaleFactor, DpiUtil.DefalutDpi * ScaleFactor);

			using var sourceImage = new Bitmap(SourceSize, SourceSize);
			using var scaledImage = DpiUtil.ScaleImage(sourceImage);

			Check.That(sourceImage).Not.IsSameReferenceAs(scaledImage);
			Check.That(scaledImage.Width).IsEqualTo(sourceImage.Width * ScaleFactor);
			Check.That(scaledImage.Height).IsEqualTo(sourceImage.Height * ScaleFactor);
		}
	}
}
