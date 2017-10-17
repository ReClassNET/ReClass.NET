using System;

namespace ReClassNET.Debugger
{
	public class NoHardwareBreakpointAvailableException : Exception
	{
		public NoHardwareBreakpointAvailableException()
			: base("All available hardware breakpoints are already set.")
		{

		}
	}
}
