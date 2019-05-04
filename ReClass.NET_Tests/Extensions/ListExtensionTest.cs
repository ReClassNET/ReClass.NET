using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using ReClassNET.Extensions;
using Xunit;

namespace ReClass.NET_Tests.Extensions
{
	public class ListExtensionTest
	{
		public static TheoryData<IList<int>, Func<int, int>, int> GetTestBinarySearchData() => new TheoryData<IList<int>, Func<int, int>, int>
		{

		};

		public void TestBinarySearch(IList<int> sut, Func<int, int> comparer, int expected)
		{
			Check.That(sut.BinarySearch(comparer)).IsEqualTo(expected);
		}
	}
}
