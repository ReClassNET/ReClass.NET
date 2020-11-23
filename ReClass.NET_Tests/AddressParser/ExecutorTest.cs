using System;
using System.Collections.Generic;
using Moq;
using NFluent;
using ReClassNET.AddressParser;
using ReClassNET.Memory;
using ReClassNET.Util.Conversion;
using Xunit;

namespace ReClass.NET_Tests.AddressParser
{
	public abstract class ExecutorTest
	{
		protected abstract IExecutor CreateExecutor();

		public static IEnumerable<object[]> GetSimpleExpressionTestData() => new List<object[]>
		{
			new object[] { "0", (IntPtr)0x0 },
			new object[] { "0 + 0", (IntPtr)0x0 },
			new object[] { "+0", (IntPtr)0x0 },
			new object[] { "-0", (IntPtr)0x0 },
			new object[] { "-1", (IntPtr)(-1) },
			new object[] { "+0 + 0", (IntPtr)0x0 },
			new object[] { "-0 - 0", (IntPtr)0x0 },
			new object[] { "0 + 1", (IntPtr)0x1 },
			new object[] { "0 - 1", (IntPtr)(-1) },
			new object[] { "1 + 2 * 3", (IntPtr)0x7 },
			new object[] { "0x123 + 0x234 * 0x345", (IntPtr)0x73527 },
			new object[] { "4 / 0x2", (IntPtr)0x2 }
		};

		[Theory]
		[MemberData(nameof(GetSimpleExpressionTestData))]
		public void SimpleExpressionTest(string expression, IntPtr expected)
		{
			var mock = new Mock<IProcessReader>();

			var executor = CreateExecutor();

			Check.That(executor.Execute(Parser.Parse(expression), mock.Object)).IsEqualTo(expected);
		}

		public static IEnumerable<object[]> GetModuleExpressionTestData() => new List<object[]>
		{
			new object[] { "<test.module>", (IntPtr)0x100 },
			new object[] { "<test.module> + 0", (IntPtr)0x100 },
			new object[] { "<test.module> + 10", (IntPtr)0x110 },
			new object[] { "<test.module> * 2", (IntPtr)0x200 },
			new object[] { "<not.found>", (IntPtr)0x0 },
		};

		[Theory]
		[MemberData(nameof(GetModuleExpressionTestData))]
		public void ModuleExpressionTest(string expression, IntPtr expected)
		{
			var mock = new Mock<IProcessReader>();
			mock.Setup(p => p.GetModuleByName("test.module"))
				.Returns(new Module { Start = (IntPtr)0x100 });

			var executor = CreateExecutor();

			Check.That(executor.Execute(Parser.Parse(expression), mock.Object)).IsEqualTo(expected);
		}

		public static IEnumerable<object[]> GetReadMemoryExpressionTestData(int bytesToRead) => new List<object[]>
		{
			new object[] { $"[0,{bytesToRead}]", (IntPtr)0x0 },
			new object[] { $"[0,{bytesToRead}] + 10", (IntPtr)0x10 },
			new object[] { $"[10,{bytesToRead}]", (IntPtr)0x10 },
			new object[] { $"[10 + 10,{bytesToRead}]", (IntPtr)0x20 },
			new object[] { $"[[10,{bytesToRead}] + 10,{bytesToRead}]", (IntPtr)0x20 },
			new object[] { $"[[10,{bytesToRead}] + [10,{bytesToRead}],{bytesToRead}] + [10,{bytesToRead}]", (IntPtr)0x30 }
		};

		[Theory]
		[MemberData(nameof(GetReadMemoryExpressionTestData), 4)]
		public void ReadMemoryExpression32Test(string expression, IntPtr expected)
		{
			var converter = EndianBitConverter.System;

			var mock = new Mock<IProcessReader>();
			mock.SetupProperty(p => p.BitConverter)
				.SetupGet(p => p.BitConverter)
				.Returns(converter);
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0, sizeof(int)))
				.Returns(converter.GetBytes(0));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x10, sizeof(int)))
				.Returns(converter.GetBytes(0x10));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x20, sizeof(int)))
				.Returns(converter.GetBytes(0x20));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x30, sizeof(int)))
				.Returns(converter.GetBytes(0x30));

			var executor = CreateExecutor();

			Check.That(executor.Execute(Parser.Parse(expression), mock.Object)).IsEqualTo(expected);
		}

		[Theory]
		[MemberData(nameof(GetReadMemoryExpressionTestData), 8)]
		public void ReadMemoryExpression64Test(string expression, IntPtr expected)
		{
			var converter = EndianBitConverter.System;

			var mock = new Mock<IProcessReader>();
			mock.SetupProperty(p => p.BitConverter)
				.SetupGet(p => p.BitConverter)
				.Returns(converter);
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0, sizeof(long)))
				.Returns(converter.GetBytes(0L));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x10, sizeof(long)))
				.Returns(converter.GetBytes(0x10L));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x20, sizeof(long)))
				.Returns(converter.GetBytes(0x20L));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x30, sizeof(long)))
				.Returns(converter.GetBytes(0x30L));

			var executor = CreateExecutor();

			Check.That(executor.Execute(Parser.Parse(expression), mock.Object)).IsEqualTo(expected);
		}

		[Fact]
		public void ReadMemoryExpressionInvariantTest()
		{
			var converter = EndianBitConverter.System;

			var mock = new Mock<IProcessReader>();
			mock.SetupProperty(p => p.BitConverter)
				.SetupGet(p => p.BitConverter)
				.Returns(converter);
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x10, sizeof(int)))
				.Returns(converter.GetBytes(0x10));
			mock.Setup(p => p.ReadRemoteMemory((IntPtr)0x10, sizeof(long)))
				.Returns(converter.GetBytes(0x10L));

			var executor = CreateExecutor();

			Check.That(executor.Execute(Parser.Parse("[10]"), mock.Object)).IsEqualTo((IntPtr)0x10);
		}
	}
}
