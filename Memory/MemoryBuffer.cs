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

		private byte[] data;

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

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(data != null);
		}

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
			Contract.Requires(Process != null);

			Process.ReadRemoteMemoryIntoBuffer(address, ref data);
		}

		public byte ReadByte(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadByte(offset.ToInt32());
		}

		public byte ReadByte(int offset)
		{
			Contract.Requires(offset >= 0);

			if (Offset + offset > data.Length)
			{
				return 0;
			}

			return data[Offset + offset];
		}

		public byte[] ReadBytes(int offset, int length)
		{
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);

			var bytes = new byte[length];

			if (Offset + offset + length > data.Length)
			{
				return bytes;
			}

			Array.Copy(data, Offset + offset, bytes, 0, length);
			return bytes;
		}

		public T ReadObject<T>(IntPtr offset) where T : struct
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadObject<T>(offset.ToInt32());
		}

		public T ReadObject<T>(int offset) where T : struct
		{
			Contract.Requires(offset >= 0);

			if (Offset + offset + Marshal.SizeOf(typeof(T)) > data.Length)
			{
				return default(T);
			}

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = Marshal.PtrToStructure(handle.AddrOfPinnedObject() + Offset + offset, typeof(T));
			handle.Free();

			if (obj == null)
			{
				return default(T);
			}
			return (T)obj;
		}

		public string ReadPrintableASCIIString(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadPrintableASCIIString(offset.ToInt32(), length);
		}

		public string ReadPrintableASCIIString(int offset, int length)
		{
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			if (Offset + offset + length > data.Length)
			{
				length = Math.Max(data.Length - Offset - offset, 0);
			}

			var sb = new StringBuilder(length);
			for (var i = 0; i < length; ++i)
			{
				var c = (char)data[Offset + offset + i];
				sb.Append(c.IsPrintable() ? c : '.');
			}
			return sb.ToString();
		}

		private string ReadString(Encoding encoding, int offset, int length)
		{
			Contract.Requires(encoding != null);
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			if (Offset + offset + length > data.Length)
			{
				length = data.Length - Offset - offset;
			}

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
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.UTF8, offset.ToInt32(), length);
		}

		public string ReadUTF16String(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.Unicode, offset.ToInt32(), length);
		}

		public string ReadUTF32String(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.UTF32, offset.ToInt32(), length);
		}
	}
}
