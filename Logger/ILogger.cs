using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Logger
{
	public delegate void NewLogEntryEventHandler(LogLevel level, string message);

	[ContractClass(typeof(ILoggerContract))]
	public interface ILogger
	{
		event NewLogEntryEventHandler NewLogEntry;

		void Log(Exception ex);
		void Log(LogLevel level, string message);
	}

	[ContractClassFor(typeof(ILogger))]
	internal abstract class ILoggerContract : ILogger
	{
		public event NewLogEntryEventHandler NewLogEntry;

		public void Log(Exception ex)
		{
			Contract.Requires(ex != null);

			throw new NotImplementedException();
		}

		public void Log(LogLevel level, string message)
		{
			Contract.Requires(message != null);

			throw new NotImplementedException();
		}
	}
}
