using System;

namespace ReClassNET.Logger
{
	class BaseLogger : ILogger
	{
		private readonly object sync = new object();

		public event NewLogEntryEventHandler NewLogEntry;

		public void Log(Exception ex)
		{
			Log(LogLevel.Error, ex.ToString());
		}

		public void Log(LogLevel level, string message)
		{
			lock (sync)
			{
				NewLogEntry?.Invoke(level, message);
			}
		}
	}
}
