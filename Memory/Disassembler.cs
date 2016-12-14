using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ReClassNET.Native;
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

				var instruction = new InstructionData();
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

		public DisassembledInstruction GetPreviousInstruction(RemoteProcess process, IntPtr address)
		{
			var buffer = process.ReadRemoteMemory(address - 80, 80);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var eip = handle.AddrOfPinnedObject();
				var end = eip + 80;
				var virtualAddress = address;

				var instruction = new InstructionData();

				var x = GetPreviousInstructionHelper(process, end, 80, ref instruction);
				if (x != end)
				{
					x = GetPreviousInstructionHelper(process, end, 40, ref instruction);
					if (x != end)
					{
						x = GetPreviousInstructionHelper(process, end, 20, ref instruction);
						if (x != end)
						{
							x = GetPreviousInstructionHelper(process, end, 10, ref instruction);
							if (x != end)
							{
								for (var i = 1; i < 20; ++i)
								{
									x = end - i;
									if (process.NativeHelper.DisassembleCode(x, end.Sub(x).ToInt32(), virtualAddress, out instruction))
									{
										break;
									}
								}
							}
						}
					}
				}

				return new DisassembledInstruction
				{
					Address = address - instruction.Length,
					Length = instruction.Length,
					Instruction = instruction.Instruction
				};
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		private IntPtr GetPreviousInstructionHelper(RemoteProcess process, IntPtr address, int distance, ref InstructionData instruction)
		{
			var x = address - distance;
			while (x.CompareTo(address) == -1) // aka x < address
			{
				if (process.NativeHelper.DisassembleCode(x, address.Sub(x).ToInt32(), IntPtr.Zero, out instruction))
				{
					x += instruction.Length;
				}
				else
				{
					break;
				}
			}
			return x;
		}
	}

	public class DisassembledInstruction
	{
		public IntPtr Address;
		public int Length;
		public string Instruction;
	}
}
