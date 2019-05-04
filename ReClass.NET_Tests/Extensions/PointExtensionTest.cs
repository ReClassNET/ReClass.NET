using System.Drawing;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class PointExtensionTest
	{
		public static TheoryData<Point, int, int, Point> GetTestRelocateData() => new TheoryData<Point, int, int, Point>
		{
			{ Point.Empty, 0, 0, Point.Empty },
			{ Point.Empty, 1, 1, new Point(1, 1) },
			{ Point.Empty, -1, -1, new Point(-1, -1) },
			{ new Point(-1, -1), 1, 1, Point.Empty },
		};

		[Theory]
		[MemberData(nameof(GetTestRelocateData))]
		public void TestRelocate(Point sut, int offsetX, int offsetY, Point expected)
		{
			Check.That(sut.Relocate(offsetX, offsetY)).IsEqualTo(expected);
		}
	}
}
