using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Logger
{
	class NullLogger : ILogger
	{
		public event NewLogEntryEventHandler NewLogEntry { add { throw new NotSupportedException(); } remove { } }

		public void Log(Exception ex)
		{
			Contract.Requires(ex != null);
		}

		public void Log(LogLevel level, string message)
		{
			Contract.Requires(message != null);
		}
	}
}
