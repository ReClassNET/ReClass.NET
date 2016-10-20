using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;

namespace ReClassNET
{
	class Memory
	{
		public RemoteProcess Process { get; set; }

		private byte[] data = new byte[0];

		public int Size
		{
			get
			{
				return data.Length;
			}
			set
			{
				if (value != data.Length)
				{
					data = new byte[value];
				}
			}
		}

		public int Offset { get; set; }

		public Memory()
		{
			data = new byte[0];
		}

		public Memory(Memory other)
		{
			Contract.Requires(other != null);

			data = other.data;
		}

		public Memory Clone()
		{
			return new Memory(this)
			{
				Offset = Offset,
				Process = Process
			};
		}

		public void Update(IntPtr address)
		{
			Process.ReadRemoteMemoryIntoBuffer(address, ref data);
		}

		public byte ReadByte(IntPtr offset)
		{
			return ReadByte(offset.ToInt32());
		}

		public byte ReadByte(int offset)
		{
			return data[Offset + offset];
		}

		public T ReadObject<T>(IntPtr offset)
		{
			return ReadObject<T>(offset.ToInt32());
		}

		public T ReadObject<T>(int offset)
		{
			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + offset, typeof(T));
			handle.Free();

			return obj;
		}

		public string ReadPrintableASCIIString(IntPtr offset, int length)
		{
			return ReadPrintableASCIIString(offset.ToInt32(), length);
		}

		public string ReadPrintableASCIIString(int offset, int length)
		{
			var sb = new StringBuilder(length);
			for (var i = 0; i < length; ++i)
			{
				var c = (char)data[offset + i];
				sb.Append(c.IsPrintable() ? c : '.');
			}
			return sb.ToString();
		}

		private string ReadString(Encoding encoding, int offset, int length)
		{
			Contract.Requires(encoding != null);

			var sb = new StringBuilder(encoding.GetString(data, offset, length));
			for (var i = 0; i < sb.Length; ++i)
			{
				if (!sb[i].IsPrintable())
				{
					sb[i] = '.';
				}
			}
			return sb.ToString();
		}

		public string ReadUTF8String(IntPtr offset, int length)
		{
			return ReadString(Encoding.UTF8, offset.ToInt32(), length);
		}

		public string ReadUTF16String(IntPtr offset, int length)
		{
			return ReadString(Encoding.Unicode, offset.ToInt32(), length);
		}

		public string ReadUTF32String(IntPtr offset, int length)
		{
			return ReadString(Encoding.UTF32, offset.ToInt32(), length);
		}
	}
}
