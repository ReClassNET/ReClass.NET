using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ReClassNET.Memory
{
	public static class Dumper
	{
		/// <summary>Dumps a chunk of memory to the given stream.</summary>
		/// <param name="reader">The memory reader to use.</param>
		/// <param name="address">The begin of the chunk.</param>
		/// <param name="size">The size of the chunk.</param>
		/// <param name="stream">The stream to dump to.</param>
		public static void DumpRaw(IRemoteMemoryReader reader, IntPtr address, int size, Stream stream)
		{
			Contract.Requires(size >= 0);
			Contract.Requires(stream != null);

			var data = reader.ReadRemoteMemory(address, size);

			stream.Write(data, 0, data.Length);
		}

		/// <summary>Dumps a section to the given stream.</summary>
		/// <param name="reader">The memory reader to use.</param>
		/// <param name="section">The section to dump.</param>
		/// <param name="stream">The stream to dump to.</param>
		public static void DumpSection(IRemoteMemoryReader reader, Section section, Stream stream)
		{
			Contract.Requires(section != null);
			Contract.Requires(stream != null);

			DumpRaw(reader, section.Start, section.Size.ToInt32(), stream);
		}

		/// <summary>Dumps a module to the given stream. The section headers of the pe header get fixed to build a valid pe file.</summary>
		/// <param name="reader">The memory reader to use.</param>
		/// <param name="module">The module to dump.</param>
		/// <param name="stream">The stream to dump to.</param>
		public static void DumpModule(IRemoteMemoryReader reader, Module module, Stream stream)
		{
			Contract.Requires(module != null);
			Contract.Requires(stream != null);

			var data = reader.ReadRemoteMemory(module.Start, module.Size.ToInt32());

			SimplePeHeader.FixSectionHeaders(data);

			stream.Write(data, 0, data.Length);
		}
	}
}
