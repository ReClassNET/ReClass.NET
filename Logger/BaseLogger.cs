using Microsoft.SqlServer.MessageBox;
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
			Log(LogLevel.Error, ExceptionMessageBox.GetMessageText(ex), ex);
		}

		public void Log(LogLevel level, string message)
		{
			Log(level, message, null);
		}

		private void Log(LogLevel level, string message, Exception ex)
		{
			Contract.Requires(message != null);

			lock (sync)
			{
				NewLogEntry?.Invoke(level, message, ex);
			}
		}
	}
}
