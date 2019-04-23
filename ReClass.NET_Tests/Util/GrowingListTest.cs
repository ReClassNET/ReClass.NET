using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class GrowingListTest
	{
		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(10)]
		[InlineData(100)]
		public void CheckSetCount(int index)
		{
			var gl = new GrowingList<int>
			{
				[index] = default
			};

			Check.That(gl.Count).IsEqualTo(index + 1);
		}

		[Theory]
		[InlineData(0, 1, 2)]
		[InlineData(1, 2, 3)]
		[InlineData(2, 10, 11)]
		[InlineData(10, 8, 11)]
		[InlineData(100, 200, 201)]
		[InlineData(0, 0, 1)]
		[InlineData(10, 1, 11)]
		[InlineData(2, 1, 3)]
		public void CheckMultipleSetCount(int index1, int index2, int expected)
		{
			var gl = new GrowingList<int>
			{
				[index1] = default,
				[index2] = default
			};

			Check.That(gl.Count).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(10)]
		[InlineData(100)]
		public void CheckGetCount(int index)
		{
			var gl = new GrowingList<int>();

			var _ = gl[index];

			Check.That(gl.Count).IsEqualTo(index + 1);
		}

		[Theory]
		[InlineData(0, 1, 2)]
		[InlineData(1, 2, 3)]
		[InlineData(2, 10, 11)]
		[InlineData(10, 8, 11)]
		[InlineData(100, 200, 201)]
		[InlineData(0, 0, 1)]
		[InlineData(10, 1, 11)]
		[InlineData(2, 1, 3)]
		public void CheckMultipleGetCount(int index1, int index2, int expected)
		{
			var gl = new GrowingList<int>();

			var _ = gl[index1];
			_ = gl[index2];

			Check.That(gl.Count).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(1, 0)]
		[InlineData(1, 10)]
		[InlineData(-1, 0)]
		[InlineData(-1, 20)]
		public void CheckDefaultValue(int value, int index)
		{
			var gl = new GrowingList<int>(value);

			Check.That(gl[index]).IsEqualTo(value);
		}
	}
}
