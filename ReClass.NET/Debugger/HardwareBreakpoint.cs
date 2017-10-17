using System;
using System.Diagnostics.Contracts;
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

	public enum HardwareBreakpointTrigger
	{
		Execute,
		Access,
		Write,
	}

	public enum HardwareBreakpointSize
	{
		Size1 = 1,
		Size2 = 2,
		Size4 = 4,
		Size8 = 8
	}

	public sealed class HardwareBreakpoint : IBreakpoint
	{
		public IntPtr Address { get; }
		public HardwareBreakpointRegister Register { get; }
		public HardwareBreakpointTrigger Trigger { get; }
		public HardwareBreakpointSize Size { get; }

		private readonly BreakpointHandler handler;

		public HardwareBreakpoint(IntPtr address, HardwareBreakpointRegister register, HardwareBreakpointTrigger trigger, HardwareBreakpointSize size, BreakpointHandler handler)
		{
			Contract.Requires(handler != null);

			if (register == HardwareBreakpointRegister.InvalidRegister)
			{
				throw new InvalidOperationException();
			}

			Address = address;
			Register = register;
			Trigger = trigger;
			Size = size;

			this.handler = handler;
		}

		public bool Set(RemoteProcess process)
		{
			return process.CoreFunctions.SetHardwareBreakpoint(process.UnderlayingProcess.Id, Address, Register, Trigger, Size, true);
		}

		public void Remove(RemoteProcess process)
		{
			process.CoreFunctions.SetHardwareBreakpoint(process.UnderlayingProcess.Id, Address, Register, Trigger, Size, false);
		}

		public void Handler(ref DebugEvent evt)
		{
			handler?.Invoke(this, ref evt);
		}

		public override bool Equals(object obj)
		{
			var hwbp = obj as HardwareBreakpoint;

			// Two hardware breakpoints are equal if they use the same register.
			return hwbp?.Register == Register;
		}

		public override int GetHashCode()
		{
			return Register.GetHashCode();
		}
	}
}
