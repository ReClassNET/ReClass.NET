using System;
using ReClassNET.Memory;

namespace ReClassNET.Debugger
{
	public sealed class SoftwareBreakpoint : IBreakpoint
	{
		public IntPtr Address { get; }

		private byte orig;

		public SoftwareBreakpoint(IntPtr address)
		{
			Address = address;
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
			process.WriteRemoteMemory(Address, new byte[] { orig });
		}

		public override bool Equals(object obj)
		{
			var bp = obj as SoftwareBreakpoint;
			if (bp == null)
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
