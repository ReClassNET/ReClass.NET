using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Logger
{
	class BaseLogger : ILogger
	{
		private readonly object sync = new object();

		public event NewLogEntryEventHandler NewLogEntry;

		public void Log(Exception ex)
		{
			Contract.Requires(ex != null);

			Log(LogLevel.Error, ex.ToString());
		}

		public void Log(LogLevel level, string message)
		{
			Contract.Requires(message != null);

			lock (sync)
			{
				NewLogEntry?.Invoke(level, message);
			}
		}
	}
}
