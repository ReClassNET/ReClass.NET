using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class MemoryBuffer
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

		public MemoryBuffer()
		{
			Contract.Ensures(data != null);

			data = new byte[0];
		}

		public MemoryBuffer(MemoryBuffer other)
		{
			Contract.Requires(other != null);
			Contract.Ensures(data != null);

			data = other.data;
		}

		public MemoryBuffer Clone()
		{
			Contract.Ensures(Contract.Result<MemoryBuffer>() != null);

			return new MemoryBuffer(this)
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
			if (Offset + offset < 0 || Offset + offset > data.Length)
			{
				throw new IndexOutOfRangeException();
			}

			return data[Offset + offset];
		}

		public byte[] ReadBytes(int offset, int length)
		{
			if (Offset + offset < 0 || Offset + offset + length > data.Length)
			{
				throw new IndexOutOfRangeException();
			}

			var b = new byte[length];
			Array.Copy(data, Offset + offset, b, 0, length);
			return b;
		}

		public T ReadObject<T>(IntPtr offset) where T : struct
		{
			return ReadObject<T>(offset.ToInt32());
		}

		public T ReadObject<T>(int offset) where T : struct
		{
			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject() + Offset + offset, typeof(T));
			handle.Free();

			return obj;
		}

		public string ReadPrintableASCIIString(IntPtr offset, int length)
		{
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadPrintableASCIIString(offset.ToInt32(), length);
		}

		public string ReadPrintableASCIIString(int offset, int length)
		{
			Contract.Requires(offset >= 0);
			//Contract.Requires(offset < data.Length);
			Contract.Requires(length >= 0);
			//Contract.Requires(length < data.Length);
			//Contract.Requires(offset + length < data.Length);
			Contract.Ensures(Contract.Result<string>() != null);

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
			Contract.Ensures(Contract.Result<string>() != null);

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
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.UTF8, offset.ToInt32(), length);
		}

		public string ReadUTF16String(IntPtr offset, int length)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.Unicode, offset.ToInt32(), length);
		}

		public string ReadUTF32String(IntPtr offset, int length)
		{
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.UTF32, offset.ToInt32(), length);
		}
	}
}
