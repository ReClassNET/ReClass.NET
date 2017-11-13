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

		public byte[] RawData => data;

		public int Size
		{
			get => data.Length;
			set
			{
				if (value != data.Length)
				{
					data = new byte[value];
					historyData = new byte[value];
					
					hasHistory = false;

					ContainsValidData = false;
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
			: this(0)
		{
			Contract.Ensures(data != null);
			Contract.Ensures(historyData != null);
		}

		public MemoryBuffer(int size)
		{
			Contract.Requires(size >= 0);
			Contract.Ensures(data != null);
			Contract.Ensures(historyData != null);

			data = new byte[size];
			historyData = new byte[size];
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
			if (Process == null)
			{
				data.FillWithZero();

				hasHistory = false;

				return;
			}

			if (setHistory)
			{
				Array.Copy(data, historyData, data.Length);

				hasHistory = ContainsValidData;
			}

			ContainsValidData = Process.ReadRemoteMemoryIntoBuffer(address, ref data);
			if (!ContainsValidData)
			{
				data.FillWithZero();

				hasHistory = false;
			}
		}

		public byte[] ReadBytes(IntPtr offset, int length)
		{
			return ReadBytes(offset.ToInt32(), length);
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

			offset = Offset + offset;
			if (offset + buffer.Length > data.Length)
			{
				return;
			}

			Array.Copy(data, offset, buffer, 0, buffer.Length);
		}

		public T ReadObject<T>(IntPtr offset) where T : struct
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadObject<T>(offset.ToInt32());
		}

		public T ReadObject<T>(int offset) where T : struct
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + Marshal.SizeOf(typeof(T)) > data.Length)
			{
				return default(T);
			}

			var handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject() + offset);
			handle.Free();

			return obj;
		}

		#region Read Primitive Types

		/// <summary>Reads a <see cref="sbyte"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="sbyte"/> or 0 if the offset is outside the data.</returns>
		public sbyte ReadInt8(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadInt8(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="sbyte"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="sbyte"/> or 0 if the offset is outside the data.</returns>
		public sbyte ReadInt8(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(sbyte) > data.Length)
			{
				return default(sbyte);
			}

			return (sbyte)data[offset];
		}

		/// <summary>Reads a <see cref="byte"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="byte"/> or 0 if the offset is outside the data.</returns>
		public byte ReadUInt8(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadUInt8(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="byte"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="byte"/> or 0 if the offset is outside the data.</returns>
		public byte ReadUInt8(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(byte) > data.Length)
			{
				return default(byte);
			}

			return data[offset];
		}

		/// <summary>Reads a <see cref="short"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="short"/> or 0 if the offset is outside the data.</returns>
		public short ReadInt16(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadInt16(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="short"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="short"/> or 0 if the offset is outside the data.</returns>
		public short ReadInt16(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(short) > data.Length)
			{
				return default(short);
			}

			return BitConverter.ToInt16(data, offset);
		}

		/// <summary>Reads a <see cref="ushort"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="ushort"/> or 0 if the offset is outside the data.</returns>
		public ushort ReadUInt16(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadUInt16(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="ushort"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="ushort"/> or 0 if the offset is outside the data.</returns>
		public ushort ReadUInt16(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(ushort) > data.Length)
			{
				return default(ushort);
			}

			return BitConverter.ToUInt16(data, offset);
		}

		/// <summary>Reads a <see cref="int"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="int"/> or 0 if the offset is outside the data.</returns>
		public int ReadInt32(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadInt32(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="int"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="int"/> or 0 if the offset is outside the data.</returns>
		public int ReadInt32(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(int) > data.Length)
			{
				return default(int);
			}

			return BitConverter.ToInt32(data, offset);
		}

		/// <summary>Reads a <see cref="uint"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="uint"/> or 0 if the offset is outside the data.</returns>
		public uint ReadUInt32(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadUInt32(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="uint"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="uint"/> or 0 if the offset is outside the data.</returns>
		public uint ReadUInt32(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(uint) > data.Length)
			{
				return default(uint);
			}

			return BitConverter.ToUInt32(data, offset);
		}

		/// <summary>Reads a <see cref="long"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="long"/> or 0 if the offset is outside the data.</returns>
		public long ReadInt64(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadInt64(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="long"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="long"/> or 0 if the offset is outside the data.</returns>
		public long ReadInt64(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(long) > data.Length)
			{
				return default(long);
			}

			return BitConverter.ToInt64(data, offset);
		}

		/// <summary>Reads a <see cref="ulong"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="ulong"/> or 0 if the offset is outside the data.</returns>
		public ulong ReadUInt64(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadUInt64(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="ulong"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="ulong"/> or 0 if the offset is outside the data.</returns>
		public ulong ReadUInt64(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(ulong) > data.Length)
			{
				return default(ulong);
			}

			return BitConverter.ToUInt64(data, offset);
		}

		/// <summary>Reads a <see cref="float"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="float"/> or 0 if the offset is outside the data.</returns>
		public float ReadFloat(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadFloat(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="float"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="float"/> or 0 if the offset is outside the data.</returns>
		public float ReadFloat(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(float) > data.Length)
			{
				return default(float);
			}

			return BitConverter.ToSingle(data, offset);
		}

		/// <summary>Reads a <see cref="double"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="double"/> or 0 if the offset is outside the data.</returns>
		public double ReadDouble(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadDouble(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="double"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="double"/> or 0 if the offset is outside the data.</returns>
		public double ReadDouble(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(double) > data.Length)
			{
				return default(double);
			}

			return BitConverter.ToDouble(data, offset);
		}

		/// <summary>Reads a <see cref="IntPtr"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="IntPtr"/> or 0 if the offset is outside the data.</returns>
		public IntPtr ReadIntPtr(IntPtr offset)
		{
			Contract.Requires(offset.ToInt32() >= 0);

			return ReadIntPtr(offset.ToInt32());
		}

		/// <summary>Reads a <see cref="IntPtr"/> from the specific offset.</summary>
		/// <param name="offset">The offset into the data.</param>
		/// <returns>The data read as <see cref="IntPtr"/> or 0 if the offset is outside the data.</returns>
		public IntPtr ReadIntPtr(int offset)
		{
			Contract.Requires(offset >= 0);

#if RECLASSNET64
			return (IntPtr)ReadInt64(offset);
#else
			return (IntPtr)ReadInt32(offset);
#endif
		}

		#endregion

		public string ReadPrintableAsciiString(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadPrintableAsciiString(offset.ToInt32(), length);
		}

		public string ReadPrintableAsciiString(int offset, int length)
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

		public string ReadUtf8String(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.UTF8, offset.ToInt32(), length);
		}

		public string ReadUtf16String(IntPtr offset, int length)
		{
			Contract.Requires(offset.ToInt32() >= 0);
			Contract.Requires(length >= 0);
			Contract.Ensures(Contract.Result<string>() != null);

			return ReadString(Encoding.Unicode, offset.ToInt32(), length);
		}

		public string ReadUtf32String(IntPtr offset, int length)
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
			if (!hasHistory)
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
