using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using ReClassNET.Native;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class Disassembler
	{
		private readonly NativeHelper nativeHelper;

		public Disassembler(NativeHelper nativeHelper)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;
		}

		public List<DisassembledInstruction> RemoteDisassembleCode(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);

			var buffer = process.ReadRemoteMemory(address, length);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				return DisassembleCode(handle.AddrOfPinnedObject(), length, address).ToList();
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		public IEnumerable<DisassembledInstruction> DisassembleCode(IntPtr address, int length, IntPtr virtualAddress)
		{
			var instructions = new List<DisassembledInstruction>();

			var eip = address;
			var end = address + length;

			var instruction = new InstructionData();
			while (eip.CompareTo(end) == -1)
			{
				if (!nativeHelper.DisassembleCode(eip, end.Sub(eip).ToInt32(), virtualAddress, out instruction))
				{
					break;
				}

				yield return new DisassembledInstruction
				{
					Address = virtualAddress,
					Length = instruction.Length,
					Data = instruction.Data,
					Instruction = instruction.Instruction
				};

				eip = eip + instruction.Length;
				virtualAddress = virtualAddress + instruction.Length;
			}
		}

		public List<DisassembledInstruction> RemoteDisassembleFunction(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);

			var buffer = process.ReadRemoteMemory(address, length);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				return DisassembleFunction(handle.AddrOfPinnedObject(), length, address).ToList();
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		public IEnumerable<DisassembledInstruction> DisassembleFunction(IntPtr address, int length, IntPtr virtualAddress)
		{
			// Read until first CC.
			return DisassembleCode(address, length, virtualAddress)
				.TakeUntil(i => i.Length == 1 && i.Data[0] == 0xCC);
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
		public byte[] Data;
		public string Instruction;

		public override string ToString() => $"{Address.ToString(Constants.StringHexFormat)} - {Instruction}";
	}
}
