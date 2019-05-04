using System.IO;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class StringReaderExtensionTest
	{
		[Theory]
		[InlineData("", -1)]
		[InlineData(" ", -1)]
		[InlineData("\t", -1)]
		[InlineData("\r", -1)]
		[InlineData("\n", -1)]
		[InlineData("x", (int)'x')]
		[InlineData("x ", (int)'x')]
		[InlineData(" x", (int)'x')]
		[InlineData("  x", (int)'x')]
		[InlineData("\tx ", (int)'x')]
		[InlineData("\rx ", (int)'x')]
		[InlineData("\nx ", (int)'x')]
		public void TestReadSkipWhitespaces(string input, int expected)
		{
			using (var sut = new StringReader(input))
			{
				Check.That(sut.ReadSkipWhitespaces()).IsEqualTo(expected);
			}
		}
	}
}
