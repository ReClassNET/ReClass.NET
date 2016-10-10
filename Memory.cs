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

		public Memory()
		{
			data = new byte[]
			{
				1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,
			};
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
			Process.ReadMemoryIntoBuffer(address, ref data);
		}

		public byte ReadByte(int offset)
		{
			return data[Offset + offset];
		}

		public T ReadObject<T>(int offset)
		{
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			var obj = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject().Add(offset), typeof(T));
			handle.Free();

			return obj;
		}

		public string ReadPrintableASCIIString(int offset, int length)
		{
			var sb = new StringBuilder(length);
			for (var i = 0; i < length; ++i)
			{
				var b = data[offset + i];
				if ((b >= ' ' && b <= '~'))
				{
					sb.Append((char)b);
				}
				else
				{
					sb.Append('.');
				}
			}
			return sb.ToString();
		}

		private string ReadString(Encoding encoding, int offset, int length)
		{
			var sb = new StringBuilder(encoding.GetString(data, offset, length));
			for (var i = 0; i < sb.Length; ++i)
			{
				if (!char.IsLetterOrDigit(sb[i]))
				{
					sb[i] = '.';
				}
			}
			return sb.ToString();
		}

		public string ReadUTF8String(int offset, int length)
		{
			var sb = new StringBuilder();
			for (var i = 0; i < length; ++i)
			{
				var c = (char)data[offset + i];
				//(((c)>=32 && (c)<=126) || ((c)>=161 && (c)<=254))
				if (!char.IsLetterOrDigit(c))
				{
					c = '.';
				}
				sb.Append(c);
			}
			return sb.ToString();

			//return ReadString(Encoding.ASCII, offset, length);
		}

		public string ReadUTF16String(int offset, int length)
		{
			return ReadString(Encoding.Unicode, offset, length);
		}

		public string ReadUTF32String(int offset, int length)
		{
			return ReadString(Encoding.UTF32, offset, length);
		}

		public string GetModuleName(IntPtr address)
		{
			return null;
		}

		public Symbols GetSymbolsForModule(string name)
		{
			return null;
		}

		public string GetNamedAddress(IntPtr address)
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
