using System;
using System.Text;
using NFluent;
using ReClassNET.Memory;
using Xunit;

namespace ReClass.NET_Tests.Memory
{
	public class MemoryBufferTest
	{
		private static MemoryBuffer CreateFromBytes(params byte[] data)
		{
			var buffer = new MemoryBuffer(data.Length);
			Array.Copy(data, buffer.RawData, data.Length);
			return buffer;
		}

		public static TheoryData<MemoryBuffer, int, int, byte[]> GetTestReadBytesData() => new TheoryData<MemoryBuffer, int, int, byte[]>
		{
			{ CreateFromBytes(), 0, 0, new byte[0] },
			{ CreateFromBytes(), 0, 4, new byte[] { 0, 0, 0, 0 } },
			{ CreateFromBytes(), 10, 4, new byte[] { 0, 0, 0, 0 } },
			{ CreateFromBytes(1, 2, 3, 4), 0, 0, new byte[0] },
			{ CreateFromBytes(1, 2, 3, 4), 0, 4, new byte[] { 1, 2, 3, 4 } },
			{ CreateFromBytes(1, 2, 3, 4), 2, 4, new byte[] { 0, 0, 0, 0 } },
			{ CreateFromBytes(1, 2, 3, 4), 10, 4, new byte[] { 0, 0, 0, 0 } },
			{ CreateFromBytes(1, 2, 3, 4, 5, 6), 2, 4, new byte[] { 3, 4, 5, 6 } }
		};

		[Theory]
		[MemberData(nameof(GetTestReadBytesData))]
		public void TestReadBytesReturn(MemoryBuffer sut, int offset, int length, byte[] expected)
		{
			Check.That(sut.ReadBytes(offset, length)).ContainsExactly(expected);
		}

		[Theory]
		[MemberData(nameof(GetTestReadBytesData))]
		public void TestReadBytesFill(MemoryBuffer sut, int offset, int length, byte[] expected)
		{
			var data = new byte[length];

			sut.ReadBytes(offset, data);

			Check.That(data).ContainsExactly(expected);
		}

		public static TheoryData<MemoryBuffer, int, sbyte, byte> GetTestReadUInt8Data() => new TheoryData<MemoryBuffer, int, sbyte, byte>
		{
			{ CreateFromBytes(), 0, 0, 0 },
			{ CreateFromBytes(), 4, 0, 0 },
			{ CreateFromBytes(1, 2, 3, 4), 0, 1, 1 },
			{ CreateFromBytes(1, 2, 3, 4), 2, 3, 3 },
			{ CreateFromBytes(1, 2, 3, 0xFF), 3, -1, 255 }
		};

		[Theory]
		[MemberData(nameof(GetTestReadUInt8Data))]
		public void TestReadInt8(MemoryBuffer sut, int offset, sbyte expectedSigned, byte expectedUnsigned)
		{
			Check.That(sut.ReadInt8(offset)).IsEqualTo(expectedSigned);
			Check.That(sut.ReadUInt8(offset)).IsEqualTo(expectedUnsigned);
		}

		public static TheoryData<MemoryBuffer, int, short, ushort> GetTestReadUInt16Data() => new TheoryData<MemoryBuffer, int, short, ushort>
		{
			{ CreateFromBytes(), 0, 0, 0 },
			{ CreateFromBytes(), 4, 0, 0 },
			{ CreateFromBytes(1, 2, 3, 4), 0, 0x0201, 0x0201 },
			{ CreateFromBytes(1, 2, 3, 4), 2, 0x0403, 0x0403 },
			{ CreateFromBytes(1, 2, 3, 0xBF, 0xFF), 3, unchecked((short)0xFFBF), 0xFFBF }
		};

		[Theory]
		[MemberData(nameof(GetTestReadUInt16Data))]
		public void TestReadInt16(MemoryBuffer sut, int offset, short expectedSigned, ushort expectedUnsigned)
		{
			Check.That(sut.ReadInt16(offset)).IsEqualTo(expectedSigned);
			Check.That(sut.ReadUInt16(offset)).IsEqualTo(expectedUnsigned);
		}

		public static TheoryData<MemoryBuffer, int, int, uint> GetTestReadUInt32Data() => new TheoryData<MemoryBuffer, int, int, uint>
		{
			{ CreateFromBytes(), 0, 0, 0 },
			{ CreateFromBytes(), 4, 0, 0 },
			{ CreateFromBytes(1, 2, 3, 4), 0, 0x04030201, 0x04030201 },
			{ CreateFromBytes(1, 2, 3, 4, 0xBF, 0xFF), 2, unchecked((int)0xFFBF0403), 0xFFBF0403 }
		};

		[Theory]
		[MemberData(nameof(GetTestReadUInt32Data))]
		public void TestReadInt32(MemoryBuffer sut, int offset, int expectedSigned, uint expectedUnsigned)
		{
			Check.That(sut.ReadInt32(offset)).IsEqualTo(expectedSigned);
			Check.That(sut.ReadUInt32(offset)).IsEqualTo(expectedUnsigned);
		}

		public static TheoryData<MemoryBuffer, int, long, ulong> GetTestReadUInt64Data() => new TheoryData<MemoryBuffer, int, long, ulong>
		{
			{ CreateFromBytes(), 0, 0, 0 },
			{ CreateFromBytes(), 4, 0, 0 },
			{ CreateFromBytes(1, 2, 3, 4, 5, 6, 7, 8), 0, 0x0807060504030201, 0x0807060504030201 },
			{ CreateFromBytes(1, 2, 3, 4, 5, 6, 0xBF, 0xFF, 0xBF, 0xFF), 2, unchecked((long)0xFFBFFFBF06050403), 0xFFBFFFBF06050403 }
		};

		[Theory]
		[MemberData(nameof(GetTestReadUInt64Data))]
		public void TestReadInt64(MemoryBuffer sut, int offset, long expectedSigned, ulong expectedUnsigned)
		{
			Check.That(sut.ReadInt64(offset)).IsEqualTo(expectedSigned);
			Check.That(sut.ReadUInt64(offset)).IsEqualTo(expectedUnsigned);
		}

		public static TheoryData<MemoryBuffer, int, float> GetTestReadFloatData() => new TheoryData<MemoryBuffer, int, float>
		{
			{ CreateFromBytes(), 0, 0.0f },
			{ CreateFromBytes(), 4, 0.0f },
			{ CreateFromBytes(0, 0x40, 0x9A, 0x44), 0, 1234.0f },
			{ CreateFromBytes(1, 2, 0, 8, 0x87, 0x45), 2, 4321.0f }
		};

		[Theory]
		[MemberData(nameof(GetTestReadFloatData))]
		public void TestReadFloat(MemoryBuffer sut, int offset, float expected)
		{
			Check.That(sut.ReadFloat(offset)).IsCloseTo(expected, 0.0001);
		}

		public static TheoryData<MemoryBuffer, int, double> GetTestReadDoubleData() => new TheoryData<MemoryBuffer, int, double>
		{
			{ CreateFromBytes(), 0, 0.0 },
			{ CreateFromBytes(), 4, 0.0 },
			{ CreateFromBytes(0x54, 0x74, 0x24, 0x97, 0x1F, 0xE1, 0xB0, 0x40), 0, 4321.1234 },
			{ CreateFromBytes(1, 2, 0x68, 0x22, 0x6C, 0x78, 0xBA, 0x49, 0x93, 0x40), 2, 1234.4321 }
		};

		[Theory]
		[MemberData(nameof(GetTestReadDoubleData))]
		public void TestReadDouble(MemoryBuffer sut, int offset, double expected)
		{
			Check.That(sut.ReadDouble(offset)).IsCloseTo(expected, 0.0001);
		}

		public static TheoryData<MemoryBuffer, Encoding, int, int, string> GetTestReadStringData() => new TheoryData<MemoryBuffer, Encoding, int, int, string>
		{
			{ CreateFromBytes(), Encoding.ASCII, 0, 0, string.Empty },
			{ CreateFromBytes(), Encoding.ASCII, 4, 0, string.Empty },
			{ CreateFromBytes(), Encoding.ASCII, 0, 4, string.Empty },
			{ CreateFromBytes(0x31, 0x32, 0x33, 0x61, 0x62, 0x63), Encoding.ASCII, 0, 6, "123abc" },
			{ CreateFromBytes(0x31, 0x32, 0x33, 0x61, 0x62, 0x63), Encoding.ASCII, 2, 3, "3ab" },
			{ CreateFromBytes(0, 0, 0, 0, 0, 0), Encoding.GetEncoding(1252), 0, 6, "......" },
			{ CreateFromBytes(0, 0, 0, 0, 0, 0), Encoding.UTF8, 0, 6, "......" },
			{ CreateFromBytes(0, 1, 2, 3, 4, 5), Encoding.UTF8, 0, 6, "......" },
			{ CreateFromBytes(0x31, 0x32, 0x33, 0x61, 0x62, 0x63, 0xC4, 0xD6, 0xDC), Encoding.GetEncoding(1252), 0, 9, "123abcÄÖÜ" },
			{ CreateFromBytes(0x31, 0x32, 0x33, 0x61, 0x62, 0x63, 0xC3, 0x84, 0xC3, 0x96, 0xC3, 0x9C), Encoding.UTF8, 0, 12, "123abcÄÖÜ" },
			{ CreateFromBytes(0x61, 0xC3), Encoding.UTF8, 0, 2, "a." },
			{ CreateFromBytes(0x31, 0x00, 0x32, 0x00, 0x33, 0x00, 0x61, 0x00, 0x62, 0x00, 0x63, 0x00, 0xC4, 0x00, 0xD6, 0x00, 0xDC, 0x00), Encoding.Unicode, 0, 18, "123abcÄÖÜ" },
			{ CreateFromBytes(0x31, 0x00, 0x00, 0x00, 0x32, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 0x00, 0x61, 0x00, 0x00, 0x00, 0x62, 0x00, 0x00, 0x00, 0x63, 0x00, 0x00, 0x00, 0xC4, 0x00, 0x00, 0x00, 0xD6, 0x00, 0x00, 0x00, 0xDC, 0x00, 0x00, 0x00), Encoding.UTF32, 0, 36, "123abcÄÖÜ" }
		};

		[Theory]
		[MemberData(nameof(GetTestReadStringData))]
		public void TestReadString(MemoryBuffer sut, Encoding encoding, int offset, int length, string expected)
		{
			Check.That(sut.ReadString(encoding, offset, length)).IsEqualTo(expected);
		}
	}
}
