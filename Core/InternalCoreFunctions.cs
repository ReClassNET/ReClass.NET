using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Core
{
	internal class InternalCoreFunctions : NativeCoreWrapper
	{
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool DisassembleCodeDelegate(IntPtr address, IntPtr length, IntPtr virtualAddress, out InstructionData instruction);

		private readonly DisassembleCodeDelegate disassembleCodeDelegate;

		public InternalCoreFunctions(IntPtr handle)
			: base(handle)
		{
			disassembleCodeDelegate = GetFunctionDelegate<DisassembleCodeDelegate>(handle, "DisassembleCode");
		}

		public bool DisassembleCode(IntPtr address, int length, IntPtr virtualAddress, out InstructionData instruction)
		{
			return disassembleCodeDelegate(address, (IntPtr)length, virtualAddress, out instruction);
		}
	}
}
