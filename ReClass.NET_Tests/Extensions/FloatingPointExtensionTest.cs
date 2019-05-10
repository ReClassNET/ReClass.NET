using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class FloatingPointExtensionTest
	{
		[Theory]
		[InlineData(0.0f, 0.0f, 0.0f, true)]
		[InlineData(0.0f, 1.0f, 0.0f, false)]
		[InlineData(0.0f, 1.0f, 1.0f, true)]
		[InlineData(1.0f, 0.0f, 1.0f, true)]
		[InlineData(-1.0f, 0.0f, 1.0f, true)]
		[InlineData(-1.0f, 1.0f, 1.0f, false)]
		[InlineData(-0.5f, 0.5f, 1.0f, true)]
		[InlineData(-0.25f, 0.75f, 1.0f, true)]
		[InlineData(-0.25f, 0.25f, 0.5f, true)]
		[InlineData(0.9999f, 1.0f, 0.0f, false)]
		[InlineData(0.9999f, 1.0f, 0.1f, true)]
		[InlineData(0.9999f, 1.0f, 0.01f, true)]
		[InlineData(0.9999f, 1.0f, 0.001f, true)]
		[InlineData(0.9999f, 1.0f, 0.0001f, true, Skip = "Should work but float can't hold these values")]
		[InlineData(0.9999f, 1.0f, 0.00001f, false, Skip = "Should work but float can't hold these values")]
		public void TestNearlyEqualFloat(float value1, float value2, float epsilon, bool expected)
		{
			Check.That(value1.IsNearlyEqual(value2, epsilon)).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(0.0, 0.0, 0.0, true)]
		[InlineData(0.0, 1.0, 0.0, false)]
		[InlineData(0.0, 1.0, 1.0, true)]
		[InlineData(1.0, 0.0, 1.0, true)]
		[InlineData(-1.0, 0.0, 1.0, true)]
		[InlineData(-1.0, 1.0, 1.0, false)]
		[InlineData(-0.5, 0.5, 1.0, true)]
		[InlineData(-0.25, 0.75, 1.0, true)]
		[InlineData(-0.25, 0.25, 0.5, true)]
		[InlineData(0.9999, 1.0, 0.0, false)]
		[InlineData(0.9999, 1.0, 0.1, true)]
		[InlineData(0.9999, 1.0, 0.01, true)]
		[InlineData(0.9999, 1.0, 0.001, true)]
		[InlineData(0.9999, 1.0, 0.0001, true)]
		[InlineData(0.9999, 1.0, 0.00001, false)]
		public void TestNearlyEqualDouble(double value1, double value2, double epsilon, bool expected)
		{
			Check.That(value1.IsNearlyEqual(value2, epsilon)).IsEqualTo(expected);
		}
	}
}
