using System;

namespace ReClassNET.Logger
{
	delegate void NewLogEntryEventHandler(LogLevel level, string message);

	interface ILogger
	{
		event NewLogEntryEventHandler NewLogEntry;

		void Log(Exception ex);
		void Log(LogLevel level, string message);
	}
}
