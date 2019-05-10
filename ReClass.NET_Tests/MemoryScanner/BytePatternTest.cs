using System;
using System.Collections.Generic;
using NFluent;
using ReClassNET.MemoryScanner;
using Xunit;

namespace ReClass.NET_Tests.MemoryScanner
{
	public class BytePatternTest
	{
		[Theory]
		[InlineData("", 0, false)]
		[InlineData("0", 1, true)]
		[InlineData("a", 1, true)]
		[InlineData("A", 1, true)]
		[InlineData("00", 1, false)]
		[InlineData("aa", 1, false)]
		[InlineData("AA", 1, false)]
		[InlineData("0000", 2, false)]
		[InlineData("00aa", 2, false)]
		[InlineData("00 aa", 2, false)]
		[InlineData("00\taa", 2, false)]
		[InlineData("?", 1, true)]
		[InlineData("??", 1, true)]
		[InlineData("????", 2, true)]
		[InlineData("?? ??", 2, true)]
		[InlineData("a?", 1, true)]
		[InlineData("?a", 1, true)]
		[InlineData("aa ?a", 2, true)]
		[InlineData("aa ?? 00", 3, true)]
		public void TestParse(string input, int expectedLength, bool expectedHasWildcards)
		{
			var sut = BytePattern.Parse(input);

			Check.That(sut.Length).IsEqualTo(expectedLength);
			Check.That(sut.HasWildcards).IsEqualTo(expectedHasWildcards);
		}

		[Fact]
		public void TestParseThrows()
		{
			Check.ThatCode(() => BytePattern.Parse("aa bb zz")).Throws<ArgumentException>();
		}

		public static TheoryData<IEnumerable<byte>, int> GetTestFromByteEnumerationData() => new TheoryData<IEnumerable<byte>, int>
		{
			{ new byte[0], 0 },
			{ new byte[] { 0x00, 0x11, 0xaa }, 3 }
		};

		[Theory]
		[MemberData(nameof(GetTestFromByteEnumerationData))]
		public void TestFromByteEnumeration(IEnumerable<byte> input, int expectedLength)
		{
			var sut = BytePattern.From(input);

			Check.That(sut.Length).IsEqualTo(expectedLength);
			Check.That(sut.HasWildcards).IsFalse();
		}

		public static TheoryData<IEnumerable<Tuple<byte, bool>>, int, bool> GetTestFromByteEnumerationWithWildcardsData() => new TheoryData<IEnumerable<Tuple<byte, bool>>, int, bool>
		{
			{ new Tuple<byte, bool>[0], 0, false },
			{ new [] { Tuple.Create((byte)0, false) }, 1, false },
			{ new [] { Tuple.Create((byte)0, true) }, 1, true },
			{ new [] { Tuple.Create((byte)0xaa, false), Tuple.Create((byte)0, true) }, 2, true }
		};

		[Theory]
		[MemberData(nameof(GetTestFromByteEnumerationWithWildcardsData))]
		public void TestFromByteEnumerationWithWildcards(IEnumerable<Tuple<byte, bool>> input, int expectedLength, bool expectedHasWildcards)
		{
			var sut = BytePattern.From(input);

			Check.That(sut.Length).IsEqualTo(expectedLength);
			Check.That(sut.HasWildcards).IsEqualTo(expectedHasWildcards);
		}

		[Fact]
		public void TestToArrayWithWildcardsThrows()
		{
			var sut = BytePattern.Parse("0?");

			Check.ThatCode(() => sut.ToByteArray()).Throws<InvalidOperationException>();
		}

		[Theory]
		[InlineData("")]
		[InlineData("00AA", (byte)0x00, (byte)0xAA)]
		[InlineData("00 aa bb 99", (byte)0x00, (byte)0xAA, (byte)0xBB, (byte)0x99)]
		public void TestToArray(string input, params byte[] expected)
		{
			var sut = BytePattern.Parse(input);

			Check.That(sut.ToByteArray()).ContainsExactly(expected);
		}

		public static TheoryData<string, PatternMaskFormat, string, string> GetTestToStringData() => new TheoryData<string, PatternMaskFormat, string, string>
		{
			{ string.Empty, PatternMaskFormat.Separated, string.Empty, string.Empty },
			{ string.Empty, PatternMaskFormat.Combined, string.Empty, null },
			{ "aa bb 00", PatternMaskFormat.Separated, @"\xAA\xBB\x00", "xxx" },
			{ "aa bb 00", PatternMaskFormat.Combined, "AA BB 00", null },
			{ "aa ?? 00", PatternMaskFormat.Separated, @"\xAA\x00\x00", "x?x" },
			{ "aa ?? 00", PatternMaskFormat.Combined, "AA ?? 00", null },
			{ "a? ?? ?0", PatternMaskFormat.Separated, @"\x00\x00\x00", "???" },
			{ "a? ?? ?0", PatternMaskFormat.Combined, "A? ?? ?0", null },
		};

		[Theory]
		[MemberData(nameof(GetTestToStringData))]
		public void TestToString(string input, PatternMaskFormat format, string expectedPattern, string expectedMask)
		{
			var sut = BytePattern.Parse(input);

			var (pattern, mask) = sut.ToString(format);

			Check.That(pattern).IsEqualTo(expectedPattern);
			Check.That(mask).IsEqualTo(expectedMask);
		}
	}
}
