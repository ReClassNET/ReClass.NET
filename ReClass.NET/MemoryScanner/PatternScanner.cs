using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using ReClassNET.Core;
using ReClassNET.Memory;

namespace ReClassNET.MemoryScanner
{
	public class PatternScanner
	{
		/// <summary>
		/// Searchs for the <see cref="BytePattern"/> in the specified <see cref="Module"/>.
		/// </summary>
		/// <param name="pattern">The pattern to search.</param>
		/// <param name="process">The process to read from.</param>
		/// <param name="module">The module of the process.</param>
		/// <returns>The address of the pattern or <see cref="IntPtr.Zero"/> if the pattern was not found.</returns>
		public static IntPtr FindPattern(BytePattern pattern, RemoteProcess process, Module module)
		{
			Contract.Requires(pattern != null);
			Contract.Requires(process != null);
			Contract.Requires(module != null);

			return FindPattern(pattern, process, module.Start, module.Size.ToInt32());
		}

		/// <summary>
		/// Searchs for the <see cref="BytePattern"/> in the specified <see cref="Section"/>.
		/// </summary>
		/// <param name="pattern">The pattern to search.</param>
		/// <param name="process">The process to read from.</param>
		/// <param name="section">The section of the process.</param>
		/// <returns>The address of the pattern or <see cref="IntPtr.Zero"/> if the pattern was not found.</returns>
		public static IntPtr FindPattern(BytePattern pattern, RemoteProcess process, Section section)
		{
			Contract.Requires(pattern != null);
			Contract.Requires(process != null);
			Contract.Requires(section != null);

			return FindPattern(pattern, process, section.Start, section.Size.ToInt32());
		}

		/// <summary>
		/// Searchs for the <see cref="BytePattern"/> in the specified address range.
		/// </summary>
		/// <param name="pattern">The pattern to search.</param>
		/// <param name="process">The process to read from.</param>
		/// <param name="start">The start address.</param>
		/// <param name="size">The size of the address range.</param>
		/// <returns>The address of the pattern or <see cref="IntPtr.Zero"/> if the pattern was not found.</returns>
		public static IntPtr FindPattern(BytePattern pattern, RemoteProcess process, IntPtr start, int size)
		{
			Contract.Requires(pattern != null);
			Contract.Requires(process != null);

			var moduleBytes = process.ReadRemoteMemory(start, size);

			var offset = FindPattern(pattern, moduleBytes);
			if (offset == -1)
			{
				return IntPtr.Zero;
			}

			return start + offset;
		}

		/// <summary>
		/// Searchs for the <see cref="BytePattern"/> in the specified data.
		/// </summary>
		/// <param name="pattern">The pattern to search.</param>
		/// <param name="data">The data to scan.</param>
		/// <returns>The index in data where the pattern was found or -1 otherwise.</returns>
		public static int FindPattern(BytePattern pattern, byte[] data)
		{
			Contract.Requires(pattern != null);
			Contract.Requires(data != null);

			var limit = data.Length - pattern.Length;
			for (var i = 0; i < limit; ++i)
			{
				if (pattern.Equals(data, i))
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Creates a <see cref="BytePattern"/> for the given address range.
		/// </summary>
		/// <param name="process">The process to use.</param>
		/// <param name="start">The start of the address range.</param>
		/// <param name="size">The size of the address range.</param>
		/// <returns>A <see cref="BytePattern"/> describing the address range.</returns>
		public static BytePattern CreatePatternFromCode(RemoteProcess process, IntPtr start, int size)
		{
			var data = new List<Tuple<byte, bool>>();

			var buffer = process.ReadRemoteMemory(start, size);

			var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
			try
			{
				var eip = handle.AddrOfPinnedObject();

				process.CoreFunctions.DisassembleCode(eip, size, IntPtr.Zero, true, (ref InstructionData instruction) =>
				{
					for (var i = 0; i < instruction.Length; ++i)
					{
						data.Add(Tuple.Create(instruction.Data[i], i >= instruction.StaticInstructionBytes));
					}
					return true;
				});
			}
			finally
			{
				if (handle.IsAllocated)
				{
					handle.Free();
				}
			}

			return BytePattern.From(data);
		}
	}
}
