using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace ReClassNET.Memory
{
	public class Dumper
	{
		private readonly RemoteProcess process;

		public Dumper(RemoteProcess process)
		{
			Contract.Requires(process != null);
			Contract.Ensures(this.process != null);

			this.process = process;
		}

		/// <summary>Dumps a section to the given stream.</summary>
		/// <param name="address">The begin of the section.</param>
		/// <param name="size">The size of the section.</param>
		/// <param name="stream">The stream to dump to.</param>
		public void DumpSection(IntPtr address, int size, Stream stream)
		{
			Contract.Requires(size >= 0);
			Contract.Requires(stream != null);

			var data = process.ReadRemoteMemory(address, size);

			stream.Write(data, 0, data.Length);
		}

		/// <summary>Dumps a module to the given stream. The section headers of the pe header get fixed to make a valid pe file.</summary>
		/// <param name="address">The begin of the module.</param>
		/// <param name="size">The size of the module.</param>
		/// <param name="stream">The stream to dump to.</param>
		public void DumpModule(IntPtr address, int size, Stream stream)
		{
			Contract.Requires(size >= 0);
			Contract.Requires(stream != null);

			var data = process.ReadRemoteMemory(address, size);

			var pe = new SimplePeHeader(data);

			// Fix the section headers.
			using (var bw = new BinaryWriter(new MemoryStream(data)))
			{
				for (var i = 0; i < pe.NumberOfSections; ++i)
				{
					var offset = pe.SectionOffset(i);
					bw.Seek(offset + 16, SeekOrigin.Begin);
					bw.Write(BitConverter.ToUInt32(data, offset + 8)); // SizeOfRawData = VirtualSize
					bw.Write(BitConverter.ToUInt32(data, offset + 12)); // PointerToRawData = VirtualAddress
				}
			}

			stream.Write(data, 0, data.Length);
		}

		private class SimplePeHeader
		{
			private readonly byte[] data;

			private int e_lfanew => BitConverter.ToInt32(data, 60);

			private int FileHeader => e_lfanew + 4;

			public int NumberOfSections => BitConverter.ToInt16(data, FileHeader + 2);

			private int SizeOfOptionalHeader => BitConverter.ToInt16(data, FileHeader + 16);

			private int FirstSectionOffset => e_lfanew + 24 + SizeOfOptionalHeader;

			public int SectionOffset(int index) => FirstSectionOffset + index * 40;

			public SimplePeHeader(byte[] data)
			{
				this.data = data;
			}
		}
	}
}
