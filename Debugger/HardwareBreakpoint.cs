using System;
using ReClassNET.Memory;

namespace ReClassNET.Debugger
{
	public enum HardwareBreakpointRegister
	{
		InvalidRegister,

		Dr0,
		Dr1,
		Dr2,
		Dr3
	}

	public enum HardwareBreakpointType
	{
		Access,
		ReadWrite,
		Write,
	}

	public enum HardwareBreakpointSize
	{
		Size1,
		Size2,
		Size4,
		Size8
	}

	public sealed class HardwareBreakpoint : IBreakpoint
	{
		public IntPtr Address { get; }
		public HardwareBreakpointRegister Register { get; }
		public HardwareBreakpointType Type { get; }
		public HardwareBreakpointSize Size { get; }

		public HardwareBreakpoint(IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointType type, HardwareBreakpointSize size)
		{
			if (register == HardwareBreakpointRegister.InvalidRegister)
			{
				throw new InvalidOperationException();
			}

			Address = address;
			Register = register;
			Type = type;
			Size = size;
		}

		public bool Set(RemoteProcess process)
		{
			return process.NativeHelper.DebuggerSetHardwareBreakpoint(process.UnderlayingProcess.Id, this, true);
		}

		public void Remove(RemoteProcess process)
		{
			process.NativeHelper.DebuggerSetHardwareBreakpoint(process.UnderlayingProcess.Id, this, false);
		}

		public override bool Equals(object obj)
		{
			var hwbp = obj as HardwareBreakpoint;
			if (hwbp == null)
			{
				return false;
			}

			// Two hardware breakpoints are equal if the address and type are equal.
			return Address == hwbp.Address && Type == hwbp.Type;
		}

		public override int GetHashCode()
		{
			return (Address.GetHashCode() * 17) ^ Type.GetHashCode();
		}
	}
}
