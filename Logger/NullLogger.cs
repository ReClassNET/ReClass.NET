using System;

namespace ReClassNET.Logger
{
	class NullLogger : ILogger
	{
		public event NewLogEntryEventHandler NewLogEntry;

		public void Log(Exception ex)
		{
			
		}

		public void Log(LogLevel level, string message)
		{
			
		}
	}
}
