using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class EncodingExtensionTest
	{
		public static TheoryData<Encoding, int> GetTestSimpleByteCountData() => new TheoryData<Encoding, int>
		{
			{ Encoding.ASCII, 1 },
			{ Encoding.GetEncoding(1252), 1 },
			{ Encoding.UTF8, 1 },
			{ Encoding.Unicode, 2 },
			{ Encoding.BigEndianUnicode, 2 },
			{ Encoding.UTF32, 4 }
		};

		[Theory]
		[MemberData(nameof(GetTestSimpleByteCountData))]
		public void TestSimpleByteCount(Encoding encoding, int expectedByteCount)
		{
			Check.That(encoding.GuessByteCountPerChar()).IsEqualTo(expectedByteCount);
		}

		public static IEnumerable<object[]> GetTestSimpleByteCountNotImplementedData() => Encoding.GetEncodings()
			.Select(e => e.GetEncoding())
			.WhereNot(e => e.IsSameCodePage(Encoding.ASCII) || e.IsSameCodePage(Encoding.UTF8) || e.IsSameCodePage(Encoding.Unicode) || e.IsSameCodePage(Encoding.BigEndianUnicode) || e.IsSameCodePage(Encoding.UTF32) || e.CodePage == 1252)
			.Select(e => new object[] { e });

		[Theory]
		[MemberData(nameof(GetTestSimpleByteCountNotImplementedData))]
		public void TestSimpleByteCountNotImplemented(Encoding encoding)
		{
			Check.ThatCode(encoding.GuessByteCountPerChar).Throws<NotImplementedException>();
		}

		public static TheoryData<Encoding, Encoding, bool> GetTestIsSameCodePageData() => new TheoryData<Encoding, Encoding, bool>
		{
			{ Encoding.ASCII, Encoding.ASCII, true },
			{ Encoding.UTF8, Encoding.UTF8, true },
			{ Encoding.Unicode, Encoding.Unicode, true },
			{ Encoding.UTF32, Encoding.UTF32, true },
			{ Encoding.ASCII, Encoding.UTF8, false },
			{ Encoding.ASCII, Encoding.Unicode, false },
			{ Encoding.ASCII, Encoding.UTF32, false },
			{ Encoding.UTF8, Encoding.UTF32, false },
			{ Encoding.Unicode, Encoding.UTF32, false },
			{ Encoding.UTF8, Encoding.Unicode, false }
		};

		[Theory]
		[MemberData(nameof(GetTestIsSameCodePageData))]
		public void TestIsSameCodePage(Encoding sut, Encoding other, bool expected)
		{
			Check.That(sut.IsSameCodePage(other)).IsEqualTo(expected);
		}
	}
}
