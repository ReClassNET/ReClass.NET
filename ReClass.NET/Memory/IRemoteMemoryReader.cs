using System;
using System.Text;

namespace ReClassNET.Memory
{
	public interface IRemoteMemoryReader
	{
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

		/// <summary>Reads the object from the address in the remote process.</summary>
		/// <typeparam name="T">Type of the value to read.</typeparam>
		/// <param name="address">The address to read from.</param>
		/// <returns>The remote object.</returns>
		T ReadRemoteObject<T>(IntPtr address) where T : struct;

		/// <summary>Reads a <see cref="sbyte"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="sbyte"/> or 0 if the read fails.</returns>
		sbyte ReadRemoteInt8(IntPtr address);

		/// <summary>Reads a <see cref="byte"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="byte"/> or 0 if the read fails.</returns>
		byte ReadRemoteUInt8(IntPtr address);

		/// <summary>Reads a <see cref="short"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="short"/> or 0 if the read fails.</returns>
		short ReadRemoteInt16(IntPtr address);

		/// <summary>Reads a <see cref="ushort"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="ushort"/> or 0 if the read fails.</returns>
		ushort ReadRemoteUInt16(IntPtr address);

		/// <summary>Reads a <see cref="int"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="int"/> or 0 if the read fails.</returns>
		int ReadRemoteInt32(IntPtr address);

		/// <summary>Reads a <see cref="uint"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="uint"/> or 0 if the read fails.</returns>
		uint ReadRemoteUInt32(IntPtr address);

		/// <summary>Reads a <see cref="long"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="long"/> or 0 if the read fails.</returns>
		long ReadRemoteInt64(IntPtr address);

		/// <summary>Reads a <see cref="ulong"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="ulong"/> or 0 if the read fails.</returns>
		ulong ReadRemoteUInt64(IntPtr address);

		/// <summary>Reads a <see cref="float"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="float"/> or 0 if the read fails.</returns>
		float ReadRemoteFloat(IntPtr address);

		/// <summary>Reads a <see cref="double"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="double"/> or 0 if the read fails.</returns>
		double ReadRemoteDouble(IntPtr address);

		/// <summary>Reads a <see cref="IntPtr"/> from the address in the remote process.</summary>
		/// <param name="address">The address to read from.</param>
		/// <returns>The data read as <see cref="IntPtr"/> or 0 if the read fails.</returns>
		IntPtr ReadRemoteIntPtr(IntPtr address);

		/// <summary>Reads a string from the address in the remote process with the given length using the provided encoding.</summary>
		/// <param name="encoding">The encoding used by the string.</param>
		/// <param name="address">The address of the string.</param>
		/// <param name="length">The length of the string.</param>
		/// <returns>The string.</returns>
		string ReadRemoteString(Encoding encoding, IntPtr address, int length);

		/// <summary>Reads a string from the address in the remote process with the given length and encoding. The string gets truncated at the first zero character.</summary>
		/// <param name="encoding">The encoding used by the string.</param>
		/// <param name="address">The address of the string.</param>
		/// <param name="length">The maximum length of the string.</param>
		/// <returns>The string.</returns>
		string ReadRemoteStringUntilFirstNullCharacter(Encoding encoding, IntPtr address, int length);

		/// <summary>Reads remote runtime type information for the given address from the remote process.</summary>
		/// <param name="address">The address.</param>
		/// <returns>A string containing the runtime type information or null if no information could get found.</returns>
		string ReadRemoteRuntimeTypeInformation(IntPtr address);
	}
}
