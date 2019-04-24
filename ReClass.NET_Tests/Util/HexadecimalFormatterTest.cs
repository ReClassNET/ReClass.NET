using System.Collections.Generic;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class HexadecimalFormatterTest
	{
		public static IEnumerable<object[]> GetTestData() => new List<object[]>
		{
			new object[] { new byte[0], string.Empty },
			new object[] { new byte[] { 0x12 }, "12" },
			new object[] { new byte[] { 0x12, 0x23, 0x34, 0x45 }, "12 23 34 45" }
		};

		[Theory]
		[MemberData(nameof(GetTestData))]
		public void Test(byte[] data, string expected)
		{
			Check.That(HexadecimalFormatter.ToString(data)).IsEqualTo(expected);
		}
	}
}
