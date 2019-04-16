using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NFluent;
using ReClassNET.AddressParser;
using ReClassNET.Memory;
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
			new object[] { "0x123 + 0x234 * 0x345", (IntPtr)0x73527 }
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
			new object[] { "<test.module> * 2", (IntPtr)0x200 }
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
	}
}
