using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.Memory
{
	public class SimplePeHeader
	{
		private readonly byte[] data;

		private int e_lfanew => BitConverter.ToInt32(data, 60);

		private int FileHeader => e_lfanew + 4;

		public int NumberOfSections => BitConverter.ToInt16(data, FileHeader + 2);

		private int SizeOfOptionalHeader => BitConverter.ToInt16(data, FileHeader + 16);

		private int FirstSectionOffset => e_lfanew + 24 + SizeOfOptionalHeader;

		public int SectionOffset(int index) => FirstSectionOffset + index * 40;

		private SimplePeHeader(byte[] data)
		{
			this.data = data;
		}

		/// <summary>
		/// Rewrites the section headers to build a valid pe file.
		/// </summary>
		/// <param name="data">The memory of a dumped module.</param>
		public static void FixSectionHeaders(byte[] data)
		{
			var pe = new SimplePeHeader(data);

			using (var ms = new MemoryStream(data))
			{
				using (var bw = new BinaryWriter(ms))
				{
					for (var i = 0; i < pe.NumberOfSections; ++i)
					{
						var offset = pe.SectionOffset(i);
						bw.Seek(offset + 16, SeekOrigin.Begin);
						bw.Write(BitConverter.ToUInt32(data, offset + 8)); // SizeOfRawData = VirtualSize
						bw.Write(BitConverter.ToUInt32(data, offset + 12)); // PointerToRawData = VirtualAddress
					}
				}
			}
		}
	}
}
