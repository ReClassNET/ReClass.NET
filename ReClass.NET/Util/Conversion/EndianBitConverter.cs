using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Util.Conversion
{
	public abstract class EndianBitConverter
	{
		public static LittleEndianBitConverter Little { get; } = new LittleEndianBitConverter();

		public static BigEndianBitConverter Big { get; } = new BigEndianBitConverter();

		public static EndianBitConverter System { get; } = BitConverter.IsLittleEndian ? (EndianBitConverter)Little : Big;


		public bool ToBoolean(byte[] value, int startIndex) => BitConverter.ToBoolean(value, startIndex);

		public char ToChar(byte[] value, int startIndex) => unchecked((char)FromBytes(value, startIndex, 2));

		public double ToDouble(byte[] value, int startIndex) => BitConverter.Int64BitsToDouble(ToInt64(value, startIndex));

		public float ToSingle(byte[] value, int startIndex) => new Int32FloatUnion(ToInt32(value, startIndex)).FloatValue;

		public short ToInt16(byte[] value, int startIndex) => unchecked((short)FromBytes(value, startIndex, 2));

		public int ToInt32(byte[] value, int startIndex) => unchecked((int)FromBytes(value, startIndex, 4));

		public long ToInt64(byte[] value, int startIndex) => FromBytes(value, startIndex, 8);

		public ushort ToUInt16(byte[] value, int startIndex) => unchecked((ushort)FromBytes(value, startIndex, 2));

		public uint ToUInt32(byte[] value, int startIndex) => unchecked((uint)FromBytes(value, startIndex, 4));

		public ulong ToUInt64(byte[] value, int startIndex) => unchecked((ulong)FromBytes(value, startIndex, 8));

		protected abstract long FromBytes(byte[] value, int index, int bytesToConvert);


		public byte[] GetBytes(bool value) => BitConverter.GetBytes(value);

		public byte[] GetBytes(char value) => ToBytes(value, 2);

		public byte[] GetBytes(double value) => ToBytes(BitConverter.DoubleToInt64Bits(value), 8);

		public byte[] GetBytes(short value) => ToBytes(value, 2);

		public byte[] GetBytes(int value) => ToBytes(value, 4);

		public byte[] GetBytes(long value) => ToBytes(value, 8);

		public byte[] GetBytes(float value) => ToBytes(new Int32FloatUnion(value).IntValue, 4);

		public byte[] GetBytes(ushort value) => ToBytes(value, 2);

		public byte[] GetBytes(uint value) => ToBytes(value, 4);

		public byte[] GetBytes(ulong value) => ToBytes(unchecked((long)value), 8);

		protected abstract byte[] ToBytes(long value, int bytes);


		[StructLayout(LayoutKind.Explicit)]
		private readonly struct Int32FloatUnion
		{
			[FieldOffset(0)]
			public readonly int IntValue;

			[FieldOffset(0)]
			public readonly float FloatValue;

			internal Int32FloatUnion(int value)
			{
				FloatValue = 0.0f;
				IntValue = value;
			}

			internal Int32FloatUnion(float value)
			{
				IntValue = 0;
				FloatValue = value;
			}
		}
	}
}
