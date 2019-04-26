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
			.WhereNot(e => e.Equals(Encoding.ASCII) || e.Equals(Encoding.UTF8) || e.Equals(Encoding.Unicode) || e.Equals(Encoding.BigEndianUnicode) || e.Equals(Encoding.UTF32))
			.Select(e => new object[] { e });

		[Theory]
		[MemberData(nameof(GetTestSimpleByteCountNotImplementedData))]
		public void TestSimpleByteCountNotImplemented(Encoding encoding)
		{
			Check.ThatCode(encoding.GuessByteCountPerChar).Throws<NotImplementedException>();
		}
	}
}
