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

		/// <summary>
		/// Disassembles the code in the given range (<paramref name="address"/>, <paramref name="lenght"/>) in the remote process.
		/// </summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public List<DisassembledInstruction> RemoteDisassembleCode(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<List<DisassembledInstruction>>() != null);

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

		/// <summary>
		/// Disassembles the code in the given range (<paramref name="address"/>, <paramref name="lenght"/>).
		/// </summary>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> DisassembleCode(IntPtr address, int length, IntPtr virtualAddress)
		{
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

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

				eip += instruction.Length;
				virtualAddress += instruction.Length;
			}
		}

		/// <summary>
		/// Disassembles the code in the given range (<paramref name="address"/>, <paramref name="lenght"/>) in the remote process until the first 0xCC instruction.
		/// </summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public List<DisassembledInstruction> RemoteDisassembleFunction(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<List<DisassembledInstruction>>() != null);

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

		/// <summary>
		/// Disassembles the code in the given range (<paramref name="address"/>, <paramref name="lenght"/>) until the first 0xCC instruction.
		/// </summary>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> DisassembleFunction(IntPtr address, int length, IntPtr virtualAddress)
		{
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			// Read until first CC.
			return DisassembleCode(address, length, virtualAddress)
				.TakeUntil(i => i.Length == 1 && i.Data[0] == 0xCC);
		}

		/// <summary>
		/// Disassembles the instruction prior to the given address.
		/// </summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <returns>The prior instruction.</returns>
		public DisassembledInstruction RemoteGetPreviousInstruction(RemoteProcess process, IntPtr address)
		{
			var buffer = process.ReadRemoteMemory(address - 80, 80);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				return GetPreviousInstruction(handle.AddrOfPinnedObject(), address);
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		private DisassembledInstruction GetPreviousInstruction(IntPtr address, IntPtr virtualAddress)
		{
			var end = address + 80;

			var instruction = new InstructionData();

			var x = GetPreviousInstructionHelper(end, 80, virtualAddress, ref instruction);
			if (x != end)
			{
				x = GetPreviousInstructionHelper(end, 40, virtualAddress, ref instruction);
				if (x != end)
				{
					x = GetPreviousInstructionHelper(end, 20, virtualAddress, ref instruction);
					if (x != end)
					{
						x = GetPreviousInstructionHelper(end, 10, virtualAddress, ref instruction);
						if (x != end)
						{
							for (var i = 1; i < 20; ++i)
							{
								x = end - i;
								if (nativeHelper.DisassembleCode(x, end.Sub(x).ToInt32(), virtualAddress, out instruction))
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

		private IntPtr GetPreviousInstructionHelper(IntPtr address, int distance, IntPtr virtualAddress, ref InstructionData instruction)
		{
			var x = address - distance;
			var y = virtualAddress - distance;
			while (x.CompareTo(address) == -1) // aka x < address
			{
				if (nativeHelper.DisassembleCode(x, address.Sub(x).ToInt32(), y, out instruction))
				{
					x += instruction.Length;
					y += instruction.Length;
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
