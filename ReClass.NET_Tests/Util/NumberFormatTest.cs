using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class NumberFormatTest
	{
		[Theory]
		[InlineData("123,34", ",", ".")]
		[InlineData("123.34", ".", ",")]
		[InlineData("1.123,34", ",", ".")]
		[InlineData("1,123.34", ".", ",")]
		public void TestGuess(string input, string expectedDecimalSeparator, string expectedGroupSeparator)
		{
			var nf = NumberFormat.GuessNumberFormat(input);

			Check.That(nf.NumberDecimalSeparator).IsEqualTo(expectedDecimalSeparator);
			Check.That(nf.NumberGroupSeparator).IsEqualTo(expectedGroupSeparator);
		}
	}
}
