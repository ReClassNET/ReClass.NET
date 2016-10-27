using System;

namespace ReClassNET.Logger
{
	public class NullLogger : ILogger
	{
		public event NewLogEntryEventHandler NewLogEntry { add { } remove { } }

		public void Log(Exception ex)
		{

		}

		public void Log(LogLevel level, string message)
		{

		}
	}
}
