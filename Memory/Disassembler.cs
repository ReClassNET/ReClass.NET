using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using ReClassNET.Core;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class Disassembler
	{
		private readonly CoreFunctionsManager coreFunctions;

		public Disassembler(CoreFunctionsManager coreFunctions)
		{
			Contract.Requires(coreFunctions != null);

			this.coreFunctions = coreFunctions;
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> RemoteDisassembleCode(RemoteProcess process, IntPtr address, int length)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, length);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				return DisassembleCode(handle.AddrOfPinnedObject(), length, address);
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>).</summary>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> DisassembleCode(IntPtr address, int length, IntPtr virtualAddress)
		{
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var eip = address;
			var end = address + length;

			while (eip.CompareTo(end) == -1)
			{
				InstructionData instruction;
				var res = coreFunctions.DisassembleCode(eip, end.Sub(eip).ToInt32() + 1, virtualAddress, out instruction);
				if (!res)
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

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="maxLength"/>) in the remote process until the first 0xCC instruction.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="maxLength">The maximum maxLength of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> RemoteDisassembleFunction(RemoteProcess process, IntPtr address, int maxLength)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, maxLength);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				return DisassembleFunction(handle.AddrOfPinnedObject(), maxLength, address);
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="maxLength"/>) until the first 0xCC instruction.</summary>
		/// <param name="address">The address of the code.</param>
		/// <param name="maxLength">The maxLength of the code.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IEnumerable<DisassembledInstruction> DisassembleFunction(IntPtr address, int maxLength, IntPtr virtualAddress)
		{
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			// Read until first CC.
			return DisassembleCode(address, maxLength, virtualAddress)
				.TakeWhile(i => !(i.Length == 1 && i.Data[0] == 0xCC));
		}

		/// <summary>Tries to find and disassembles the instruction prior to the given address.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <returns>The prior instruction.</returns>
		public DisassembledInstruction RemoteGetPreviousInstruction(RemoteProcess process, IntPtr address)
		{
			var buffer = process.ReadRemoteMemory(address - 80, 95);

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

		/// <summary>Gets the previous instruction.</summary>
		/// <param name="address">The address of the code.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>The previous instruction.</returns>
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
							for (var i = 1; i < 15; ++i)
							{
								x = address + 65 + i;
								if (coreFunctions.DisassembleCode(x, end.Sub(x).ToInt32() + 1, virtualAddress, out instruction))
								{
									if (x + instruction.Length == end)
									{
										break;
									}
								}
							}
						}
					}
				}
			}

			return new DisassembledInstruction
			{
				Address = virtualAddress - instruction.Length,
				Length = instruction.Length,
				Data = instruction.Data,
				Instruction = instruction.Instruction
			};
		}

		private IntPtr GetPreviousInstructionHelper(IntPtr address, int distance, IntPtr virtualAddress, ref InstructionData instruction)
		{
			var x = address - distance;
			var y = virtualAddress - distance;
			while (x.CompareTo(address) == -1) // aka x < address
			{
				if (coreFunctions.DisassembleCode(x, address.Sub(x).ToInt32() + 1, y, out instruction))
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

		/// <summary>Tries to find the start address of the function <paramref name="address"/> points into.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address inside the function.</param>
		/// <returns>The start address of the function (maybe) or <see cref="IntPtr.Zero"/> if no start address could be found.</returns>
		public IntPtr RemoteGetFunctionStartAddress(RemoteProcess process, IntPtr address)
		{
			const int BufferLength = 512;

			var buffer = new byte[2 + BufferLength + 2 + 1];

			for (var i = 1; i <= 10; ++i)
			{
				if (!process.ReadRemoteMemoryIntoBuffer(address - i * BufferLength - 2, ref buffer))
				{
					return IntPtr.Zero;
				}

				for (var o = BufferLength + 4; o > 0; --o)
				{
					// Search for two CC in a row.
					if (buffer[o] == 0xCC && buffer[o - 1] == 0xCC)
					{
						var start = address - i * BufferLength + o - 1;

						// Check if the two previous instructions are really a CC.
						var prevInstruction = RemoteGetPreviousInstruction(process, start);
						if (prevInstruction.Length == 1 && prevInstruction.Data[0] == 0xCC)
						{
							prevInstruction = RemoteGetPreviousInstruction(process, start - 1);
							if (prevInstruction.Length == 1 && prevInstruction.Data[0] == 0xCC)
							{
								// Disassemble the code from the start and check if the instructions sum up to address.
								var length = RemoteDisassembleCode(process, start, address.Sub(start).ToInt32())
									.Select(inst => inst.Length)
									.Sum();

								if (start + length == address)
								{
									return start;
								}
							}
							else
							{
								o -= prevInstruction.Length;
							}
						}
						else
						{
							o -= prevInstruction.Length;
						}
					}
				}
			}

			return IntPtr.Zero;
		}
	}

	public class DisassembledInstruction
	{
		public IntPtr Address;
		public int Length;
		public byte[] Data;
		public string Instruction;

		public bool IsValid => Length > 0;

		public override string ToString() => $"{Address.ToString(Constants.StringHexFormat)} - {Instruction}";
	}
}
