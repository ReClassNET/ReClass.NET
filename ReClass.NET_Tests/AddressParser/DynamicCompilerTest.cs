using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReClassNET.AddressParser;

namespace ReClass.NET_Tests.AddressParser
{
	public class DynamicCompilerTest : ExecutorTest
	{
		protected override IExecuter CreateExecutor()
		{
			return new DynamicCompiler();
		}
	}
}
