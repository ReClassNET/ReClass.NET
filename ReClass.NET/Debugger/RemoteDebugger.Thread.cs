using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace ReClassNET.Debugger
{
	public partial class RemoteDebugger
	{
		private readonly object syncThread = new object();

		private Thread thread;

		private volatile bool running = true;
		private volatile bool isAttached;

		public bool IsAttached => isAttached;

		public bool StartDebuggerIfNeeded(Func<bool> queryAttach)
		{
			Contract.Requires(queryAttach != null);

			if (!process.IsValid)
			{
				return false;
			}

			lock (syncThread)
			{
				if (thread != null && IsAttached)
				{
					return true;
				}

				if (queryAttach())
				{
					thread = new Thread(Run)
					{
						IsBackground = true
					};
					thread.Start();

					return true;
				}
			}

			return false;
		}

		private void Run()
		{
			try
			{
				if (!process.CoreFunctions.AttachDebuggerToProcess(process.UnderlayingProcess.Id))
				{
					return;
				}

				isAttached = true;

				var evt = new DebugEvent();

				running = true;
				while (running)
				{
					if (process.CoreFunctions.AwaitDebugEvent(ref evt, 100))
					{
						evt.ContinueStatus = DebugContinueStatus.Handled;

						HandleExceptionEvent(ref evt);

						process.CoreFunctions.HandleDebugEvent(ref evt);
					}
					else
					{
						if (!process.IsValid)
						{
							Terminate(false);
						}
					}
				}

				process.CoreFunctions.DetachDebuggerFromProcess(process.UnderlayingProcess.Id);
			}
			finally
			{
				isAttached = false;
			}
		}

		public void Terminate()
		{
			Terminate(true);
		}

		private void Terminate(bool join)
		{
			lock (syncBreakpoint)
			{
				foreach (var bp in breakpoints)
				{
					bp.Remove(process);
				}
				breakpoints.Clear();
			}

			running = false;

			if (join)
			{
				lock (syncThread)
				{
					thread?.Join();
				}
			}
		}
	}
}
