using System;
using ReClassNET.Memory;
using System.Diagnostics.Contracts;

namespace ReClassNET.Debugger
{
	public sealed class SoftwareBreakpoint : IBreakpoint
	{
		public IntPtr Address { get; }

		private byte orig;

		private readonly BreakpointHandler handler;

		public SoftwareBreakpoint(IntPtr address, BreakpointHandler handler)
		{
			Contract.Requires(handler != null);

			Address = address;

			this.handler = handler;
		}

		public bool Set(RemoteProcess process)
		{
			var temp = new byte[1];
			if (!process.ReadRemoteMemoryIntoBuffer(Address, ref temp))
			{
				return false;
			}
			orig = temp[0];

			return process.WriteRemoteMemory(Address, new byte[] { 0xCC });
		}

		public void Remove(RemoteProcess process)
		{
			process.WriteRemoteMemory(Address, new[] { orig });
		}

		public void Handler(ref DebugEvent evt)
		{
			handler?.Invoke(this, ref evt);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is SoftwareBreakpoint bp))
			{
				return false;
			}

			return Address == bp.Address;
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode();
		}
	}
}
