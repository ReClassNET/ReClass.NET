using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using ReClassNET.Core;
using ReClassNET.Extensions;

namespace ReClassNET.Memory
{
	public class Disassembler
	{
		// The maximum number of bytes of a x86-64 instruction.
		public const int MaximumInstructionLength = 15;

		private readonly CoreFunctionsManager coreFunctions;

		public Disassembler(CoreFunctionsManager coreFunctions)
		{
			Contract.Requires(coreFunctions != null);

			this.coreFunctions = coreFunctions;
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code in bytes.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(IRemoteMemoryReader process, IntPtr address, int length)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			return RemoteDisassembleCode(process, address, length, -1);
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="length"/>) in the remote process.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="length">The length of the code in bytes.</param>
		/// <param name="maxInstructions">The maximum number of instructions to disassemble. If <paramref name="maxInstructions"/> is -1, all available instructions get returned.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleCode(IRemoteMemoryReader process, IntPtr address, int length, int maxInstructions)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, length);

			return DisassembleCode(buffer, address, maxInstructions);
		}

		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <param name="maxInstructions">The maximum number of instructions to disassemble. If <paramref name="maxInstructions"/> is -1, all available instructions get returned.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/>.</returns>
		public IReadOnlyList<DisassembledInstruction> DisassembleCode(byte[] data, IntPtr virtualAddress, int maxInstructions)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IList<DisassembledInstruction>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<DisassembledInstruction>();

				coreFunctions.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref InstructionData instruction) =>
				{
					instructions.Add(new DisassembledInstruction(ref instruction));

					return maxInstructions == -1 || instructions.Count < maxInstructions;
				});

				return instructions;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Disassembles the code in the given range (<paramref name="address"/>, <paramref name="maxLength"/>) in the remote process until the first 0xCC instruction.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <param name="maxLength">The maximum maxLength of the code.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/> which belong to the function.</returns>
		public IReadOnlyList<DisassembledInstruction> RemoteDisassembleFunction(IRemoteMemoryReader process, IntPtr address, int maxLength)
		{
			Contract.Requires(process != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var buffer = process.ReadRemoteMemory(address, maxLength);

			return DisassembleFunction(buffer, address);
		}

		/// <summary>Disassembles the code in the given data.</summary>
		/// <param name="data">The data to disassemble.</param>
		/// <param name="virtualAddress">The virtual address of the code. This allows to decode instructions located anywhere in memory even if they are not at their original place.</param>
		/// <returns>A list of <see cref="DisassembledInstruction"/> which belong to the function.</returns>
		public IReadOnlyList<DisassembledInstruction> DisassembleFunction(byte[] data, IntPtr virtualAddress)
		{
			Contract.Requires(data != null);
			Contract.Ensures(Contract.Result<IEnumerable<DisassembledInstruction>>() != null);

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try
			{
				var instructions = new List<DisassembledInstruction>();

				// Read until first CC.
				coreFunctions.DisassembleCode(handle.AddrOfPinnedObject(), data.Length, virtualAddress, false, (ref InstructionData result) =>
				{
					if (result.Length == 1 && result.Data[0] == 0xCC)
					{
						return false;
					}

					instructions.Add(new DisassembledInstruction(ref result));

					return true;
				});

				return instructions;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Tries to find and disassembles the instruction prior to the given address.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address of the code.</param>
		/// <returns>The prior instruction.</returns>
		public DisassembledInstruction RemoteGetPreviousInstruction(IRemoteMemoryReader process, IntPtr address)
		{
			const int TotalBufferSize = 7 * MaximumInstructionLength;
			const int BufferShiftSize = 6 * MaximumInstructionLength;

			var buffer = process.ReadRemoteMemory(address - BufferShiftSize, TotalBufferSize);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var bufferAddress = handle.AddrOfPinnedObject();
				var targetBufferAddress = bufferAddress + BufferShiftSize;

				var instruction = default(InstructionData);

				foreach (var offset in new[]
				{
					6 * MaximumInstructionLength,
					4 * MaximumInstructionLength,
					2 * MaximumInstructionLength,
					1 * MaximumInstructionLength,
					14, 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
				})
				{
					var currentAddress = targetBufferAddress - offset;

					coreFunctions.DisassembleCode(currentAddress, offset + 1, address - offset, false, (ref InstructionData data) =>
					{
						var nextAddress = currentAddress + data.Length;
						if (nextAddress.CompareTo(targetBufferAddress) > 0)
						{
							return false;
						}

						instruction = data;

						currentAddress = nextAddress;

						return true;
					});

					if (currentAddress == targetBufferAddress)
					{
						return new DisassembledInstruction(ref instruction);
					}
				}

				return null;
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}
		}

		/// <summary>Tries to find the start address of the function <paramref name="address"/> points into.</summary>
		/// <param name="process">The process to read from.</param>
		/// <param name="address">The address inside the function.</param>
		/// <returns>The start address of the function (maybe) or <see cref="IntPtr.Zero"/> if no start address could be found.</returns>
		public IntPtr RemoteGetFunctionStartAddress(IRemoteMemoryReader process, IntPtr address)
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
								var totalInstructionLength = RemoteDisassembleCode(process, start, address.Sub(start).ToInt32())
									.Sum(ins => ins.Length);
								if (start + totalInstructionLength == address)
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
		public IntPtr Address { get; set; }
		public int Length { get; set; }
		public byte[] Data { get; set; }
		public string Instruction { get; set; }

		public bool IsValid => Length > 0;

		public DisassembledInstruction(ref InstructionData data)
		{
			Address = data.Address;
			Length = data.Length;
			Data = data.Data;
			Instruction = data.Instruction;
		}

		public override string ToString() => $"{Address.ToString(Constants.AddressHexFormat)} - {Instruction}";
	}
}
