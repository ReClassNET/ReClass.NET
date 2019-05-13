using System;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Text;
using ReClassNET.Extensions;

namespace ReClassNET.Memory
{
	public class MemoryBuffer
	{
		private byte[] data;
		private byte[] historyData;

		private bool hasHistory;

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
				Offset = Offset
			};
		}

		public void UpdateFrom(IRemoteMemoryReader reader, IntPtr address)
		{
			if (reader == null)
			{
				data.FillWithZero();

				hasHistory = false;

				return;
			}

			Array.Copy(data, historyData, data.Length);

			hasHistory = ContainsValidData;

			ContainsValidData = reader.ReadRemoteMemoryIntoBuffer(address, ref data);
			if (!ContainsValidData)
			{
				data.FillWithZero();

				hasHistory = false;
			}
		}

		public byte[] ReadBytes(int offset, int length)
		{
			Contract.Requires(offset >= 0);
			Contract.Requires(length >= 0);

			var buffer = new byte[length];

			ReadBytes(offset, buffer);

			return buffer;
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

		public T ReadObject<T>(int offset) where T : struct
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + Marshal.SizeOf(typeof(T)) > data.Length)
			{
				return default;
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
		public sbyte ReadInt8(int offset)
		{
			Contract.Requires(offset >= 0);

			offset = Offset + offset;
			if (offset + sizeof(sbyte) > data.Length)
			{
				return default;
			}

			return (sbyte)data[offset];
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
				return default;
			}

			return data[offset];
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
				return default;
			}

			return BitConverter.ToInt16(data, offset);
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
				return default;
			}

			return BitConverter.ToUInt16(data, offset);
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
				return default;
			}

			return BitConverter.ToInt32(data, offset);
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
				return default;
			}

			return BitConverter.ToUInt32(data, offset);
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
				return default;
			}

			return BitConverter.ToInt64(data, offset);
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
				return default;
			}

			return BitConverter.ToUInt64(data, offset);
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
				return default;
			}

			return BitConverter.ToSingle(data, offset);
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
				return default;
			}

			return BitConverter.ToDouble(data, offset);
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

		public string ReadString(Encoding encoding, int offset, int length)
		{
			Contract.Requires(encoding != null);
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
