using ReClassNET.AddressParser;

namespace ReClass.NET_Tests.AddressParser
{
	public class InterpreterTest : ExecutorTest
	{
		protected override IExecutor CreateExecutor()
		{
			return new Interpreter();
		}
	}
}
