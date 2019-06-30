using System;
using System.Collections.Generic;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class StringExtensionTest
	{
		public static TheoryData<char> GetTestIsPrintableData() => new TheoryData<char>
		{
			'0', '9', ' ', 'a', 'A', 'z', 'Z', '-', '_', '°', '^', '"', '\\', '\"', '&', '@', '$', '|', '<', '>', ';', ',', '.', ':', '#', '*', '+', '~', '`', '´', 'ß', '?', '=', '(', ')', '[', ']', '{', '}'
		};

		[Theory]
		[MemberData(nameof(GetTestIsPrintableData))]
		public void TestIsPrintable(char c)
		{
			Check.That(c.IsPrintable()).IsTrue();
		}

		public static TheoryData<char> GetTestIsNotPrintableData() => new TheoryData<char>
		{
			'\u0000','\u0001', '\u0002', '\u009A','\u009B', '\u009C', '\u009D','\u009E', '\u009F'
		};

		[Theory]
		[MemberData(nameof(GetTestIsNotPrintableData))]
		public void TestIsNotPrintable(char c)
		{
			Check.That(c.IsPrintable()).IsFalse();
		}

		public static TheoryData<string, int, string> GetTestLimitLengthData() => new TheoryData<string, int, string>
		{
			{ string.Empty, 0, string.Empty },
			{ string.Empty, 1, string.Empty },
			{ "01234", 0, string.Empty },
			{ "01234", 1, "0" },
			{ "01234", 5, "01234" },
			{ "01234", 10, "01234" }
		};

		[Theory]
		[MemberData(nameof(GetTestLimitLengthData))]
		public void TestLimitLength(string sut, int length, string expected)
		{
			Check.That(sut.LimitLength(length)).IsEqualTo(expected);
		}

		[Fact]
		public void TestLimitLengthThrows()
		{
			Check.ThatCode(() => "".LimitLength(-1)).Throws<ArgumentOutOfRangeException>();
		}

		public static TheoryData<IEnumerable<byte>, IEnumerable<char>> GetTestInterpretAsSingleByteCharacterData() => new TheoryData<IEnumerable<byte>, IEnumerable<char>>
		{
			{ new byte[0], string.Empty },
			{ new [] { (byte)'t', (byte)'e', (byte)'s', (byte)'t' }, "test" }
		};

		[Theory]
		[MemberData(nameof(GetTestInterpretAsSingleByteCharacterData))]
		public void TestInterpretAsSingleByteCharacter(IEnumerable<byte> sut, IEnumerable<char> expected)
		{
			Check.That(sut.InterpretAsSingleByteCharacter()).ContainsExactly(expected);
		}

		public static TheoryData<IEnumerable<byte>, IEnumerable<char>> GetTestInterpretAsDoubleByteCharacterData() => new TheoryData<IEnumerable<byte>, IEnumerable<char>>
		{
			{ new byte[0], string.Empty },
			{ new [] { (byte)'t', (byte)0, (byte)'e', (byte)0, (byte)'s', (byte)0, (byte)'t', (byte)0 }, "test" }
		};

		[Theory]
		[MemberData(nameof(GetTestInterpretAsDoubleByteCharacterData))]
		public void TestInterpretAsDoubleByteCharacter(IEnumerable<byte> sut, IEnumerable<char> expected)
		{
			Check.That(sut.InterpretAsDoubleByteCharacter()).ContainsExactly(expected);
		}

		public static TheoryData<IEnumerable<char>, float> GetTestCalculatePrintableDataThresholdData() => new TheoryData<IEnumerable<char>, float>
		{
			{ new char[0], 0.0f },
			{ new [] { '\0' }, 0.0f },
			{ new [] { 'a' }, 1.0f },
			{ new [] { '\0', 'a' }, 0.0f },
			{ new [] { 'a', '\0' }, 0.5f },
			{ new [] { '\0', 'a', 'a' }, 0.0f },
			{ new [] { 'a', 'a', '\0' }, 2 / 3.0f },
			{ new [] { 'a', 'a', '\0', '\0' }, 0.5f },
			{ new [] { 'a', 'a', '\0', '\0', '\0' }, 2 / 5.0f }
		};

		[Theory]
		[MemberData(nameof(GetTestCalculatePrintableDataThresholdData))]
		public void TestCalculatePrintableDataThreshold(IEnumerable<char> sut, float expected)
		{
			Check.That(sut.CalculatePrintableDataThreshold()).IsCloseTo(expected, 0.001);
		}

		[Theory]
		[InlineData('a')]
		[InlineData('a', 'a')]
		[InlineData('a', 'a', 'f')]
		[InlineData('#', '+', 'r', '?', 'ß', '%', '&', '§', '_', '0', '/', '(', 'ö')]
		public void TestIsPrintableData(params char[] sut)
		{
			Check.That(sut.IsPrintableData()).IsTrue();
		}

		[Theory]
		[InlineData]
		[InlineData('a', '\0')]
		[InlineData('\0', 'a')]
		[InlineData('a', 'a', '\0')]
		[InlineData('a', 'a', 'f', '\0')]
		[InlineData('a', 'a', '\0', 'f')]
		[InlineData('a', '\0', 'a', 'f')]
		[InlineData('\0', 'a', 'a', 'f')]
		public void TestIsNotPrintableData(params char[] sut)
		{
			Check.That(sut.IsPrintableData()).IsFalse();
		}

		[Theory]
		[InlineData('a', 'a', 'f', '\0')]
		[InlineData('1', '2', '3', '4', '5', '6', '7', '8', '\0', '\0')]
		[InlineData('1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'a', 'b', '\0', '\0', '\0', '\0')]
		public void TestIsLikelyPrintableData(params char[] sut)
		{
			Check.That(sut.IsLikelyPrintableData()).IsTrue();
		}

		[Theory]
		[InlineData]
		[InlineData('a', '\0')]
		[InlineData('\0', 'a')]
		[InlineData('a', 'a', '\0')]
		[InlineData('a', 'a', '\0', 'f')]
		[InlineData('a', 'a', '\0', '\0')]
		[InlineData('a', '\0', 'a', 'f')]
		public void TestIsNotLikelyPrintableData(params char[] sut)
		{
			Check.That(sut.IsPrintableData()).IsFalse();
		}

		[Theory]
		[InlineData("", false, null)]
		[InlineData("-", false, null)]
		[InlineData("-0", false, null)]
		[InlineData("-0x0", false, null)]
		[InlineData("-h0", false, null)]
		[InlineData("0", true, "0")]
		[InlineData("h0", true, "0")]
		[InlineData("0x0", true, "0")]
		[InlineData("0123456789abcdef", true, "0123456789abcdef")]
		[InlineData("h0123456789abcdef", true, "0123456789abcdef")]
		[InlineData("0x0123456789abcdef", true, "0123456789abcdef")]
		[InlineData("0123456789ABCDEF", true, "0123456789ABCDEF")]
		public void TestTryGetHexString(string input, bool expectedResult, string expectedValue)
		{
			Check.That(input.TryGetHexString(out var value)).IsEqualTo(expectedResult);
			Check.That(value).IsEqualTo(expectedValue);
		}
	}
}
