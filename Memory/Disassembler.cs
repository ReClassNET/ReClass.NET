using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class Disassembler
	{
		public List<DisassembledInstruction> DisassembleRemoteCode(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);

			var instructions = new List<DisassembledInstruction>();

			var buffer = process.ReadRemoteMemory(address, length);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var eip = handle.AddrOfPinnedObject();
				var end = eip + length;
				var virtualAddress = address;

				var instruction = new NativeHelper.InstructionData();
				while (true)
				{
					if (!process.NativeHelper.DisassembleCode(eip, end.Sub(eip).ToInt32(), virtualAddress, out instruction))
					{
						break;
					}

					instructions.Add(new DisassembledInstruction
					{
						Address = virtualAddress,
						Length = instruction.Length,
						Instruction = instruction.Instruction
					});

					eip = eip + instruction.Length;
					if (eip.CompareTo(end) >= 0 || buffer[eip.Sub(handle.AddrOfPinnedObject()).ToInt32()] == 0xCC)
					{
						break;
					}
					virtualAddress = virtualAddress + instruction.Length;
				}
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}

			return instructions;
		}
	}

	public class DisassembledInstruction
	{
		public IntPtr Address;
		public int Length;
		public string Instruction;
	}
}
