using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;

namespace ReClassNET.Debugger
{
	public delegate void BreakpointHandler(IBreakpoint breakpoint, ref DebugEvent evt);

	[ContractClass(typeof(BreakpointContract))]
	public interface IBreakpoint
	{
		IntPtr Address { get; }

		bool Set(RemoteProcess process);
		void Remove(RemoteProcess process);

		void Handler(ref DebugEvent evt);
	}

	[ContractClassFor(typeof(IBreakpoint))]
	internal abstract class BreakpointContract : IBreakpoint
	{
		public IntPtr Address => throw new NotImplementedException();

		public void Handler(ref DebugEvent evt)
		{
			throw new NotImplementedException();
		}

		public void Remove(RemoteProcess process)
		{
			Contract.Requires(process != null);

			throw new NotImplementedException();
		}

		public bool Set(RemoteProcess process)
		{
			Contract.Requires(process != null);

			throw new NotImplementedException();
		}
	}
}
