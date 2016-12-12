using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;
using ReClassNET.Native;

namespace ReClassNET.Debugger
{
	public class RemoteDebugger
	{
		private readonly object sync = new object();

		private readonly RemoteProcess process;

		private readonly HashSet<IBreakpoint> breakpoints = new HashSet<IBreakpoint>();

		public bool IsAttached { get; private set; }

		public RemoteDebugger(RemoteProcess process)
		{
			Contract.Requires(process != null);

			this.process = process;
		}

		public bool Attach()
		{
			if (!process.IsValid)
			{
				return false;
			}

			lock (sync)
			{
				if (IsAttached)
				{
					throw new InvalidOperationException();
				}

				IsAttached = process.NativeHelper.DebuggerAttachToProcess(process.UnderlayingProcess.Id);

				return IsAttached;
			}
		}

		public void Detach()
		{
			lock (sync)
			{
				if (!IsAttached)
				{
					return;
				}

				foreach (var bp in breakpoints)
				{
					bp.Remove(process);
				}
				breakpoints.Clear();

				process.NativeHelper.DebuggerDetachFromProcess(process.UnderlayingProcess.Id);

				IsAttached = false;
			}
		}

		public bool WaitForDebugEvent(ref DebugEvent e)
		{
			return process.NativeHelper.DebuggerWaitForDebugEvent(ref e);
		}

		public void ContinueEvent(ref DebugEvent e)
		{
			process.NativeHelper.DebuggerContinueEvent(ref e);
		}

		public bool SetBreakpoint(IBreakpoint bp)
		{
			Contract.Requires(bp != null);

			lock (sync)
			{
				if (!breakpoints.Add(bp))
				{
					return false;
				}

				bp.Set(process);
			}

			return true;
		}

		public void RemoveBreakpoint(IBreakpoint bp)
		{
			Contract.Requires(bp != null);

			lock (sync)
			{
				if (breakpoints.Remove(bp))
				{
					bp.Remove(process);
				}
			}
		}
	}
}
