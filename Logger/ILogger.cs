using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Logger
{
	public delegate void NewLogEntryEventHandler(LogLevel level, string message, Exception ex);

	[ContractClass(typeof(LoggerContract))]
	public interface ILogger
	{
		/// <summary>Gets triggered every time a new entry is created.</summary>
		event NewLogEntryEventHandler NewLogEntry;

		/// <summary>Logs the given exception. The <see cref="LogLevel"/> is always set to <see cref="LogLevel.Error"/>.</summary>
		/// <param name="ex">The exception to log.</param>
		void Log(Exception ex);

		/// <summary>Logs the given message.</summary>
		/// <param name="level">The level of the message.</param>
		/// <param name="message">The message to log.</param>
		void Log(LogLevel level, string message);
	}

	[ContractClassFor(typeof(ILogger))]
	internal abstract class LoggerContract : ILogger
	{
		public event NewLogEntryEventHandler NewLogEntry { add { throw new NotImplementedException(); } remove { } }

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
