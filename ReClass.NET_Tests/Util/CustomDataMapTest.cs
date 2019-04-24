using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class CustomDataMapTest
	{
		[Fact]
		public void TestNullKeyNotAllowed()
		{
			var sut = new CustomDataMap();

			Check.ThatCode(() => sut.SetString(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.SetBool(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.SetLong(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.SetULong(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.SetXElement(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.GetString(null)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.GetBool(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.GetLong(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.GetULong(null, default)).Throws<ArgumentNullException>();
			Check.ThatCode(() => sut.GetXElement(null, default)).Throws<ArgumentNullException>();
		}

		[Theory]
		[InlineData("key", "")]
		[InlineData("key", "value")]
		public void TestSetGetString(string key, string value)
		{
			var sut = new CustomDataMap();

			sut.SetString(key, value);

			Check.That(sut.GetString(key)).IsEqualTo(value);
		}

		[Theory]
		[InlineData("key", "")]
		[InlineData("key", "value")]
		public void TestIndexString(string key, string value)
		{
			var sut = new CustomDataMap();

			sut.SetString(key, value);

			Check.That(sut[key]).IsEqualTo(value);
		}

		[Fact]
		public void TestItemEnumeration()
		{
			var data = new Dictionary<string, string>
			{
				{ "key1", "value1" },
				{ "key2", "value2" },
				{ "key3", "value3" }
			};

			var sut = new CustomDataMap();

			foreach (var kv in data)
			{
				sut.SetString(kv.Key, kv.Value);
			}

			Check.That(sut).IsEquivalentTo(data);
		}

		[Fact]
		public void TestRemoveItem()
		{
			const string KeyToRemove = "key2";

			var data = new Dictionary<string, string>
			{
				{ "key1", "value1" },
				{ KeyToRemove, "value2" },
				{ "key3", "value3" }
			};

			var sut = new CustomDataMap();

			foreach (var kv in data)
			{
				sut.SetString(kv.Key, kv.Value);
			}

			sut.RemoveValue(KeyToRemove);

			Check.That(sut).IsEquivalentTo(data.Where(kv => kv.Key != KeyToRemove));
		}

		[Theory]
		[InlineData("key", true)]
		[InlineData("key", false)]
		public void TestSetGetBool(string key, bool value)
		{
			var sut = new CustomDataMap();

			sut.SetBool(key, value);

			Check.That(sut.GetBool(key, !value)).IsEqualTo(value);
		}

		[Theory]
		[InlineData("key", -1)]
		[InlineData("key", 0)]
		[InlineData("key", 1)]
		[InlineData("key", long.MaxValue)]
		[InlineData("key", long.MinValue)]
		public void TestSetGetLong(string key, long value)
		{
			var sut = new CustomDataMap();

			sut.SetLong(key, value);

			Check.That(sut.GetLong(key, 0)).IsEqualTo(value);
		}

		[Theory]
		[InlineData("key", 0)]
		[InlineData("key", 1)]
		[InlineData("key", ulong.MaxValue)]
		[InlineData("key", ulong.MinValue)]
		public void TestSetGetULong(string key, ulong value)
		{
			var sut = new CustomDataMap();

			sut.SetULong(key, value);

			Check.That(sut.GetULong(key, 0)).IsEqualTo(value);
		}

		public static IEnumerable<object[]> GetTestSetGetXElementData() => new List<object[]>
		{
			new object[] { "key", null },
			new object[] { "key", new XElement("name") },
			new object[] { "key", new XElement("name", new XAttribute("attr", "test")) },
			new object[] { "key", new XElement("name", new XElement("value", "test")) }
		};

		[Theory]
		[MemberData(nameof(GetTestSetGetXElementData))]
		public void TestSetGetXElement(string key, XElement value)
		{
			var sut = new CustomDataMap();

			sut.SetXElement(key, value);

			Check.That(XNode.DeepEquals(sut.GetXElement(key, null), value)).IsTrue();
		}
	}
}
