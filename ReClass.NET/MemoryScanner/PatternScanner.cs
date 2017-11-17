using System;
using System.Diagnostics.Contracts;
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
		private static int FindPattern(BytePattern pattern, byte[] data)
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
	}
}
