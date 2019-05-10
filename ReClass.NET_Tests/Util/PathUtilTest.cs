using System.Collections.Generic;
using System.IO;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class PathUtilTest
	{
		public static IEnumerable<object[]> GetTestConversionData() => new List<object[]>
		{
			new object[] { string.Empty, string.Empty },
			new object[] { "C:/", "C:" + Path.DirectorySeparatorChar },
			new object[] { @"C:\", "C:" + Path.DirectorySeparatorChar },
			new object[] { "C:/test.test", Path.Combine("C:" + Path.DirectorySeparatorChar, "test.test") },
			new object[] { "file:///C:/test.test", Path.Combine("C:" + Path.DirectorySeparatorChar, "test.test") },
		};

		[Theory]
		[MemberData(nameof(GetTestConversionData))]
		public void TestConversion(string input, string expected)
		{
			Check.That(PathUtil.FileUrlToPath(input)).IsEqualTo(expected);
		}
	}
}
