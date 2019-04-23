using System.Collections.Generic;
using System.Linq;
using NFluent;
using ReClassNET.Util;
using Xunit;

namespace ReClass.NET_Tests.Util
{
	public class CommandLineArgsTest
	{
		[Fact]
		public void TestNullArgs()
		{
			var sut = new CommandLineArgs(null);

			Check.That(sut.FileNames).IsEmpty();
			Check.That(sut.Parameters).IsEmpty();
		}

		[Fact]
		public void TestEmptyArgs()
		{
			var sut = new CommandLineArgs(new string[0]);

			Check.That(sut.FileNames).IsEmpty();
			Check.That(sut.Parameters).IsEmpty();
		}

		[Fact]
		public void TestEmptyStringArgs()
		{
			var sut = new CommandLineArgs(new[] { string.Empty, string.Empty });

			Check.That(sut.FileNames).IsEmpty();
			Check.That(sut.Parameters).IsEmpty();
		}

		[Theory]
		[InlineData("test.test")]
		[InlineData("test.test", "test2.test")]
		[InlineData("C:/test.test", "test2.test")]
		[InlineData("test.test", "C:/test2.test")]
		[InlineData("C:/test.test", "C:/test2.test")]
		[InlineData(@"C:\test.test", "test2.test")]
		[InlineData(@"test.test", @"C:\test2.test")]
		[InlineData(@"C:\test.test", @"C:\test2.test")]
		public void TestFilenames(params string[] args)
		{
			var sut = new CommandLineArgs(args);

			Check.That(sut.FileName).IsEqualTo(args[0]);
			Check.That(sut.FileNames).HasSize(args.Length);
			Check.That(sut.FileNames).IsEquivalentTo(args);

			Check.That(sut.Parameters).IsEmpty();
		}

		[Theory]
		[InlineData("-p")]
		[InlineData("-p=")]
		[InlineData("-p:")]
		[InlineData("-p=123")]
		[InlineData("-p:123")]
		[InlineData("--p")]
		[InlineData("--p=")]
		[InlineData("--p:")]
		[InlineData("--p=123")]
		[InlineData("--p:123")]
		public void TestParameterFormats(string arg)
		{
			var sut = new CommandLineArgs(new [] { arg });

			Check.That(sut.Parameters).HasSize(1);
			Check.That(sut.Parameters.First().Key).IsEqualTo("p");

			Check.That(sut.FileNames).IsEmpty();
		}

		[Theory]
		[InlineData("-p", "")]
		[InlineData("-p=", "")]
		[InlineData("-p:", "")]
		[InlineData("-p=123", "123")]
		[InlineData("-p:123", "123")]
		public void TestParameterValues(string arg, string expectedValue)
		{
			var sut = new CommandLineArgs(new[] { arg });

			Check.That(sut.Parameters.First().Value).IsEqualTo(expectedValue);
		}

		public static IEnumerable<object[]> GetTestFilenamesAndParametersData() => new List<object[]>
		{
			new object[] { new[] { "test.test" }, 1, 0 },
			new object[] { new[] { "-p" }, 0, 1 },
			new object[] { new[] { "test.test", "-p" }, 1, 1 },
			new object[] { new[] { "test.test", "-p", "test2.test" }, 2, 1 },
			new object[] { new[] { "test.test", "-p", "-p2=123", "test2.test" }, 2, 2 },
			new object[] { new[] { "-p3:4", "test.test", "-p", "-p2=123", "test2.test" }, 2, 3 }
		};

		[Theory]
		[MemberData(nameof(GetTestFilenamesAndParametersData))]
		public void TestFilenamesAndParameters(string[] args, int expectedFilenames, int expectedParameters)
		{
			var sut = new CommandLineArgs(args);

			Check.That(sut.FileNames).HasSize(expectedFilenames);
			Check.That(sut.Parameters).HasSize(expectedParameters);
		}
	}
}
