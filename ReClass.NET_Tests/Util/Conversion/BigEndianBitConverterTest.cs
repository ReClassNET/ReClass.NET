using System;
using NFluent;
using ReClassNET.Util.Conversion;
using Xunit;

namespace ReClass.NET_Tests.Util.Conversion
{
	public class BigEndianBitConverterTest
	{
		[Fact]
		public void ToXXX_ThrowsOnNull()
		{
			var sut = new BigEndianBitConverter();

			Check.ThatCode(() => sut.ToInt32(null, 0)).Throws<ArgumentNullException>();
		}

		[Fact]
		public void ToXXX_ThrowsOnInvalidIndexOrSize()
		{
			var sut = new BigEndianBitConverter();

			var data = new byte[3];
			Check.ThatCode(() => sut.ToInt32(data, 0)).Throws<ArgumentOutOfRangeException>();

			data = new byte[4];
			Check.ThatCode(() => sut.ToInt32(data, 1)).Throws<ArgumentOutOfRangeException>();
		}

		[Fact]
		public void GetBytes()
		{
			var sut = new BigEndianBitConverter();

			Check.That(new byte[] { 0 }).ContainsExactly(sut.GetBytes(false));
			Check.That(new byte[] { 1 }).ContainsExactly(sut.GetBytes(true));

			Check.That(new byte[] { 0, 0 }).ContainsExactly(sut.GetBytes((short)0));
			Check.That(new byte[] { 0, 1 }).ContainsExactly(sut.GetBytes((short)1));
			Check.That(new byte[] { 1, 0 }).ContainsExactly(sut.GetBytes((short)256));
			Check.That(new byte[] { 255, 255 }).ContainsExactly(sut.GetBytes((short)-1));

			Check.That(new byte[] { 0, 0 }).ContainsExactly(sut.GetBytes((ushort)0));
			Check.That(new byte[] { 0, 1 }).ContainsExactly(sut.GetBytes((ushort)1));
			Check.That(new byte[] { 1, 0 }).ContainsExactly(sut.GetBytes((ushort)256));
			Check.That(new byte[] { 255, 255 }).ContainsExactly(sut.GetBytes(ushort.MaxValue));

			Check.That(new byte[] { 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(0));
			Check.That(new byte[] { 0, 0, 0, 1 }).ContainsExactly(sut.GetBytes(1));
			Check.That(new byte[] { 0, 0, 1, 0 }).ContainsExactly(sut.GetBytes(256));
			Check.That(new byte[] { 0, 1, 0, 0 }).ContainsExactly(sut.GetBytes(65536));
			Check.That(new byte[] { 1, 0, 0, 0 }).ContainsExactly(sut.GetBytes(16777216));
			Check.That(new byte[] { 255, 255, 255, 255 }).ContainsExactly(sut.GetBytes(-1));

			Check.That(new byte[] { 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(0u));
			Check.That(new byte[] { 0, 0, 0, 1 }).ContainsExactly(sut.GetBytes(1u));
			Check.That(new byte[] { 0, 0, 1, 0 }).ContainsExactly(sut.GetBytes(256u));
			Check.That(new byte[] { 0, 1, 0, 0 }).ContainsExactly(sut.GetBytes(65536u));
			Check.That(new byte[] { 1, 0, 0, 0 }).ContainsExactly(sut.GetBytes(16777216u));
			Check.That(new byte[] { 255, 255, 255, 255 }).ContainsExactly(sut.GetBytes(uint.MaxValue));

			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(0L));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }).ContainsExactly(sut.GetBytes(1L));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 1, 0 }).ContainsExactly(sut.GetBytes(256L));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 1, 0, 0 }).ContainsExactly(sut.GetBytes(65536L));
			Check.That(new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 }).ContainsExactly(sut.GetBytes(16777216L));
			Check.That(new byte[] { 0, 0, 0, 1, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(4294967296L));
			Check.That(new byte[] { 0, 0, 1, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(1099511627776L));
			Check.That(new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(281474976710656L));
			Check.That(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(72057594037927936L));
			Check.That(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }).ContainsExactly(sut.GetBytes(-1L));

			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(0UL));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 0, 1 }).ContainsExactly(sut.GetBytes(1UL));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 0, 1, 0 }).ContainsExactly(sut.GetBytes(256UL));
			Check.That(new byte[] { 0, 0, 0, 0, 0, 1, 0, 0 }).ContainsExactly(sut.GetBytes(65536UL));
			Check.That(new byte[] { 0, 0, 0, 0, 1, 0, 0, 0 }).ContainsExactly(sut.GetBytes(16777216UL));
			Check.That(new byte[] { 0, 0, 0, 1, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(4294967296UL));
			Check.That(new byte[] { 0, 0, 1, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(1099511627776UL));
			Check.That(new byte[] { 0, 1, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(281474976710656UL));
			Check.That(new byte[] { 1, 0, 0, 0, 0, 0, 0, 0 }).ContainsExactly(sut.GetBytes(72057594037927936UL));
			Check.That(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 }).ContainsExactly(sut.GetBytes(ulong.MaxValue));
		}

		[Fact]
		public void ToXXX()
		{
			var sut = new BigEndianBitConverter();

			var data = new byte[] { 0, 0, 0, 0, 0, 0, 0, 3 };
			Check.That(sut.ToBoolean(data, 0)).IsFalse();
			Check.That(sut.ToBoolean(data, 7)).IsTrue();
			Check.That(sut.ToChar(data, 0)).IsEqualTo('\0');
			Check.That(sut.ToChar(data, 6)).IsEqualTo('\u0003');
			Check.That(sut.ToInt16(data, 0)).IsEqualTo(0);
			Check.That(sut.ToInt16(data, 6)).IsEqualTo(3);
			Check.That(sut.ToUInt16(data, 0)).IsEqualTo(0u);
			Check.That(sut.ToUInt16(data, 6)).IsEqualTo(3u);
			Check.That(sut.ToInt32(data, 0)).IsEqualTo(0);
			Check.That(sut.ToInt32(data, 4)).IsEqualTo(3);
			Check.That(sut.ToUInt32(data, 0)).IsEqualTo(0u);
			Check.That(sut.ToUInt32(data, 4)).IsEqualTo(3u);
			Check.That(sut.ToInt64(data, 0)).IsEqualTo(3L);
			Check.That(sut.ToUInt64(data, 0)).IsEqualTo(3UL);

			data = new byte[] { 0x41, 0x20, 0, 0, 0, 0, 0, 0 };
			Check.That(sut.ToSingle(data, 0)).IsEqualTo(10.0f);
			Check.That(sut.ToSingle(data, 4)).IsEqualTo(0.0f);

			data = new byte[] { 0x40, 0x24, 0, 0, 0, 0, 0, 0 };
			Check.That(sut.ToDouble(data, 0)).IsEqualTo(10.0);
		}
	}
}
