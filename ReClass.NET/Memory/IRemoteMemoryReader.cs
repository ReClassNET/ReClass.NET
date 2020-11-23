using System;
using ReClassNET.Util.Conversion;

namespace ReClassNET.Memory
{
	public interface IRemoteMemoryReader
	{
		EndianBitConverter BitConverter { get; set; }

		/// <summary>Reads remote memory from the address into the buffer.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="buffer">[out] The data buffer to fill. If the remote process is not valid, the buffer will get filled with zeros.</param>
		bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer);

		/// <summary>Reads remote memory from the address into the buffer.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="buffer">[out] The data buffer to fill. If the remote process is not valid, the buffer will get filled with zeros.</param>
		/// <param name="offset">The offset in the data.</param>
		/// <param name="length">The number of bytes to read.</param>
		bool ReadRemoteMemoryIntoBuffer(IntPtr address, ref byte[] buffer, int offset, int length);

		/// <summary>Reads <paramref name="size"/> bytes from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <param name="size">The size in bytes to read.</param>
		/// <returns>An array of bytes.</returns>
		byte[] ReadRemoteMemory(IntPtr address, int size);

		/// <summary>Reads remote runtime type information for the given address from the remote process.</summary>
		/// <param name="address">The address.</param>
		/// <returns>A string containing the runtime type information or null if no information could get found.</returns>
		string ReadRemoteRuntimeTypeInformation(IntPtr address);
	}
}
