using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Util;

namespace ReClassNET.Memory
{
	public class MemoryBuffer
	{
		private byte[] data;
		private byte[] historyData;
		private bool hasHistory;

		public RemoteProcess Process { get; set; }

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
					historyData = new byte[value];

					hasHistory = false;
				}
			}
		}

		public int Offset { get; set; }

		public bool ContainsValidData { get; private set; }

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(data != null);
			Contract.Invariant(historyData != null);
		}

		public MemoryBuffer()
		{
			Contract.Ensures(data != null);
			Contract.Ensures(historyData != null);

			data = new byte[0];
			historyData = new byte[0];

			ContainsValidData = true;
		}

		public MemoryBuffer(MemoryBuffer other)
		{
			Contract.Requires(other != null);
			Contract.Ensures(data != null);
			Contract.Ensures(historyData != null);

			data = other.data;
			historyData = other.historyData;
			hasHistory = other.hasHistory;

			ContainsValidData = other.ContainsValidData;
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
			Update(address, true);
		}

		public void Update(IntPtr address, bool setHistory)
		{
			Contract.Requires(Process != null);

			if (setHistory)
			{
				Array.Copy(data, historyData, data.Length);

				hasHistory = true;
			}

			ContainsValidData = Process.ReadRemoteMemoryIntoBuffer(address, ref data);
			if (!ContainsValidData)
			{
				data.FillWithZero();
			}
		}

		public byte ReadByte(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadByte(offset.ToInt32());
		}

		public byte ReadByte(int offset)
		{
			Contract.Requires(offset >= 0);

			if (Offset + offset >= data.Length)
			{
				return 0;
			}

			return data[Offset + offset];
		}

		public byte[] ReadBytes(int offset, int length)
		{
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);

			var buffer = new byte[length];

			ReadBytes(offset, buffer);

			return buffer;
		}

		public void ReadBytes(IntPtr offset, byte[] buffer)
		{
			Contract.Requires(buffer != null);

			ReadBytes(offset.ToInt32(), buffer);
		}

		public void ReadBytes(int offset, byte[] buffer)
		{
			Contract.Requires(offset >= 0);
			Contract.Requires(buffer != null);

			if (Offset + offset + buffer.Length > data.Length)
			{
				return;
			}

			Array.Copy(data, Offset + offset, buffer, 0, buffer.Length);
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

			if (length <= 0)
			{
				return string.Empty;
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

			var sb = new StringBuilder(encoding.GetString(data, Offset + offset, length));
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

		public bool HasChanged(IntPtr offset, int length)
		{
			return HasChanged(offset.ToInt32(), length);
		}

		public bool HasChanged(int offset, int length)
		{
			if (hasHistory)
			{
				return false;
			}

			if (Offset + offset + length > data.Length)
			{
				return false;
			}

			var end = Offset + offset + length;

			for (var i = Offset + offset; i < end; ++i)
			{
				if (data[i] != historyData[i])
				{
					return true;
				}
			}

			return false;
		}
	}
}
