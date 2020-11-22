using System;
using NFluent;
using ReClassNET.Util.Conversion;
using Xunit;

namespace ReClass.NET_Tests.Util.Conversion
{
	public class EndianBitConverterTest
	{
		[Fact]
		public void Properties_AreNotNull()
		{
			Check.That(EndianBitConverter.System).IsNotNull();
			Check.That(EndianBitConverter.Big).IsNotNull();
			Check.That(EndianBitConverter.Little).IsNotNull();
		}

		[Fact]
		public void Types()
		{
			Check.That(EndianBitConverter.Big.GetType()).IsNotEqualTo(EndianBitConverter.Little.GetType());
			Check.That(EndianBitConverter.System.GetType()).IsEqualTo(BitConverter.IsLittleEndian ? EndianBitConverter.Little.GetType() : EndianBitConverter.Big.GetType());
		}
	}
}
