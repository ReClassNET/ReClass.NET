using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class BitStringTest
	{
		[Theory]
		[InlineData(0, "0000 0000")]
		[InlineData(1, "0000 0001")]
		[InlineData(127, "0111 1111")]
		[InlineData(128, "1000 0000")]
		[InlineData(255, "1111 1111")]
		[InlineData(0b1010_1010, "1010 1010")]
		public void TestToStringByte(byte value, string expected)
		{
			Check.That(BitString.ToString(value)).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(0, "0000 0000 0000 0000")]
		[InlineData(1, "0000 0000 0000 0001")]
		[InlineData(127, "0000 0000 0111 1111")]
		[InlineData(128, "0000 0000 1000 0000")]
		[InlineData(255, "0000 0000 1111 1111")]
		[InlineData(short.MaxValue, "0111 1111 1111 1111")]
		[InlineData(short.MinValue, "1000 0000 0000 0000")]
		[InlineData(unchecked((short)0b1010_1010_1010_1010), "1010 1010 1010 1010")]
		public void TestToStringShort(short value, string expected)
		{
			Check.That(BitString.ToString(value)).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(0, "0000 0000 0000 0000 0000 0000 0000 0000")]
		[InlineData(1, "0000 0000 0000 0000 0000 0000 0000 0001")]
		[InlineData(127, "0000 0000 0000 0000 0000 0000 0111 1111")]
		[InlineData(128, "0000 0000 0000 0000 0000 0000 1000 0000")]
		[InlineData(255, "0000 0000 0000 0000 0000 0000 1111 1111")]
		[InlineData(short.MaxValue, "0000 0000 0000 0000 0111 1111 1111 1111")]
		[InlineData(short.MinValue, "1111 1111 1111 1111 1000 0000 0000 0000")]
		[InlineData(int.MaxValue, "0111 1111 1111 1111 1111 1111 1111 1111")]
		[InlineData(int.MinValue, "1000 0000 0000 0000 0000 0000 0000 0000")]
		[InlineData(unchecked((int)0b1010_1010_1010_1010_1010_1010_1010_1010), "1010 1010 1010 1010 1010 1010 1010 1010")]
		public void TestToStringInt(int value, string expected)
		{
			Check.That(BitString.ToString(value)).IsEqualTo(expected);
		}

		[Theory]
		[InlineData(0, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000")]
		[InlineData(1, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0001")]
		[InlineData(127, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0111 1111")]
		[InlineData(128, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 1000 0000")]
		[InlineData(255, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 1111 1111")]
		[InlineData(short.MaxValue, "0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0111 1111 1111 1111")]
		[InlineData(short.MinValue, "1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1000 0000 0000 0000")]
		[InlineData(int.MaxValue, "0000 0000 0000 0000 0000 0000 0000 0000 0111 1111 1111 1111 1111 1111 1111 1111")]
		[InlineData(int.MinValue, "1111 1111 1111 1111 1111 1111 1111 1111 1000 0000 0000 0000 0000 0000 0000 0000")]
		[InlineData(long.MaxValue, "0111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111 1111")]
		[InlineData(long.MinValue, "1000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000 0000")]
		[InlineData(unchecked((long)0b1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010_1010), "1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010 1010")]
		public void TestToStringLong(long value, string expected)
		{
			Check.That(BitString.ToString(value)).IsEqualTo(expected);
		}
	}
}
