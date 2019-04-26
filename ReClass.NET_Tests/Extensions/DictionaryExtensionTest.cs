using System.Collections.Generic;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class DictionaryExtensionTest
	{
		[Fact]
		public void TestRemoveWhere()
		{
			var sut = new Dictionary<int, string>
			{
				{ 0, "val0" },
				{ 1, "val1" },
				{ 2, "val2" },
				{ 3, "val3" }
			};

			sut.RemoveWhere(kv => kv.Key % 2 == 1);

			Check.That(sut.Keys).IsEquivalentTo(0, 2);

			sut.RemoveWhere(kv => kv.Key == 2);

			Check.That(sut.Keys).IsEquivalentTo(0);
		}
	}
}
