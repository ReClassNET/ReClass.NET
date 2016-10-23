using System;

namespace ReClassNET.Logger
{
	public delegate void NewLogEntryEventHandler(LogLevel level, string message);

	public interface ILogger
	{
		event NewLogEntryEventHandler NewLogEntry;

		void Log(Exception ex);
		void Log(LogLevel level, string message);
	}
}
