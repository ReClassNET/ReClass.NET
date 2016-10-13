using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET
{
	[StructLayout(LayoutKind.Explicit)]
	struct UInt8Data
	{
		[FieldOffset(0)]
		public sbyte SByteValue;

		[FieldOffset(0)]
		public byte ByteValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	struct UInt16Data
	{
		/*[FieldOffset(0)]
		public fixed byte ByteValue[2];*/

		[FieldOffset(0)]
		public short ShortValue;

		[FieldOffset(0)]
		public ushort UShortValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	struct UInt32FloatData
	{
		/*[FieldOffset(0)]
		public fixed byte ByteValue[4];*/

		[FieldOffset(0)]
		public float FloatValue;

		[FieldOffset(0)]
		public int IntValue;

		public IntPtr IntPtr => unchecked((IntPtr)IntValue);

		[FieldOffset(0)]
		public uint UIntValue;

		public UIntPtr UIntPtr => unchecked((UIntPtr)UIntValue);
	}

	[StructLayout(LayoutKind.Explicit)]
	struct UInt64FloatDoubleData
	{
		/*[FieldOffset(0)]
		public fixed byte ByteValue[8];*/

		[FieldOffset(0)]
		public float FloatValue;

		[FieldOffset(0)]
		public double DoubleValue;

		[FieldOffset(0)]
		public long LongValue;

		public IntPtr IntPtr =>
#if WIN32
			unchecked((IntPtr)(int)LongValue);
#else
			unchecked((IntPtr)LongValue);
#endif

		[FieldOffset(0)]
		public ulong ULongValue;

		public UIntPtr UIntPtr =>
#if WIN32
			unchecked((UIntPtr)(uint)ULongValue);
#else
			unchecked((UIntPtr)ULongValue);
#endif
	}

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
			var obj = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject().Add(offset), typeof(T));
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

		public Symbols GetSymbolsForModule(string name)
		{
			return null;
		}
	}

	class Symbols
	{
		public string GetSymbolStringWithVA(IntPtr address)
		{
			return null;
		}
	}
}
