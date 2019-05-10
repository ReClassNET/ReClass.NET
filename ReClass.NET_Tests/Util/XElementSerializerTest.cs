using System.Collections.Generic;
using System.Drawing;
using NFluent;
using ReClassNET.Extensions;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class XElementSerializerTest
	{
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestBoolSerialization(bool value)
		{
			const string Name = "BoolValue";

			var element = XElementSerializer.ToXml(Name, value);

			Check.That(element.Name.LocalName).IsEqualTo(Name);

			Check.That(XElementSerializer.ToBool(element)).IsEqualTo(value);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(-1)]
		[InlineData(int.MaxValue)]
		[InlineData(int.MinValue)]
		public void TestIntSerialization(int value)
		{
			const string Name = "IntValue";

			var element = XElementSerializer.ToXml(Name, value);

			Check.That(element.Name.LocalName).IsEqualTo(Name);

			Check.That(XElementSerializer.ToInt(element)).IsEqualTo(value);
		}

		[Theory]
		[InlineData("")]
		[InlineData("test")]
		public void TestStringSerialization(string value)
		{
			const string Name = "StringValue";

			var element = XElementSerializer.ToXml(Name, value);

			Check.That(element.Name.LocalName).IsEqualTo(Name);

			Check.That(XElementSerializer.ToString(element)).IsEqualTo(value);
		}

		public static IEnumerable<object[]> GetTestColorSerializationData() => new List<object[]>
		{
			new object[] { Color.Empty },
			new object[] { Color.Red },
			new object[] { Color.Blue },
			new object[] { Color.FromArgb(123, 123, 123) }
		};

		[Theory]
		[MemberData(nameof(GetTestColorSerializationData))]
		public void TestColorSerialization(Color value)
		{
			const string Name = "ColorValue";

			var element = XElementSerializer.ToXml(Name, value);

			Check.That(element.Name.LocalName).IsEqualTo(Name);

			Check.That(XElementSerializer.ToColor(element).ToRgb()).IsEqualTo(value.ToRgb());
		}

		public static IEnumerable<object[]> GetTestDictionarySerializationData() => new List<object[]>
		{
			new object[] { new Dictionary<string, string>() },
			new object[] { new Dictionary<string, string> { { "test", "test" }, { "test2", "test2" } } }
		};

		[Theory]
		[MemberData(nameof(GetTestDictionarySerializationData))]
		public void TestDictionarySerialization(Dictionary<string, string> value)
		{
			const string Name = "DictionaryValue";

			var element = XElementSerializer.ToXml(Name, value);

			Check.That(element.Name.LocalName).IsEqualTo(Name);

			Check.That(XElementSerializer.ToDictionary(element)).IsEquivalentTo(value);
		}
	}
}
