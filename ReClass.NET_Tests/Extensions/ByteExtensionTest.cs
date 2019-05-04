using System.Linq;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class ByteExtensionTest
	{
		public static TheoryData<byte[]> GetTestFillWithZeroData() => new TheoryData<byte[]>
		{
			new byte[0],
			Enumerable.Repeat(1, 1).Select(i => (byte)i).ToArray(),
			Enumerable.Repeat(1, 10).Select(i => (byte)i).ToArray(),
			Enumerable.Repeat(1, 100).Select(i => (byte)i).ToArray(),
			Enumerable.Repeat(1, 1000).Select(i => (byte)i).ToArray()
		};

		[Theory]
		[MemberData(nameof(GetTestFillWithZeroData))]
		public void TestFillWithZero(byte[] sut)
		{
			sut.FillWithZero();

			Check.That(sut.All(b => b == 0)).IsTrue();
		}
	}
}
