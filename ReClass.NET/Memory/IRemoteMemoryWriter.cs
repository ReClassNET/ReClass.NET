using System;
using System.Text;
using ReClassNET.Util.Conversion;

namespace ReClassNET.Memory
{
	public interface IRemoteMemoryWriter
	{
		EndianBitConverter BitConverter { get; set; }

		/// <summary>Writes the given <paramref name="data"/> to the <paramref name="address"/> in the remote process.</summary>
		/// <param name="address">The address to write to.</param>
		/// <param name="data">The data to write.</param>
		/// <returns>True if it succeeds, false if it fails.</returns>
		bool WriteRemoteMemory(IntPtr address, byte[] data);
	}
}
