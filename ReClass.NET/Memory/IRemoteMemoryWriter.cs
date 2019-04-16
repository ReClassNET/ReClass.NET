using System;

namespace ReClassNET.Memory
{
	public interface IRemoteMemoryWriter
	{
		/// <summary>Writes the given <paramref name="data"/> to the <paramref name="address"/> in the remote process.</summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="data">The data to write.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		bool WriteRemoteMemory(IntPtr address, byte[] data);

		/// <summary>Writes the given <paramref name="value"/> to the <paramref name="address"/> in the remote process.</summary>
		/// <typeparam name="T">Type of the value to write.</typeparam>
		/// <param name="address">The address to write to.</param>
		/// <param name="value">The value to write.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		bool WriteRemoteMemory<T>(IntPtr address, T value) where T : struct;
	}
}
