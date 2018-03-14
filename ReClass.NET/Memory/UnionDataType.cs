using System;
using System.Runtime.InteropServices;

namespace ReClassNET.Memory
{
	[StructLayout(LayoutKind.Explicit)]
	public struct UInt8Data
	{
		[FieldOffset(0)]
		public sbyte SByteValue;

		[FieldOffset(0)]
		public byte ByteValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UInt16Data
	{
		[FieldOffset(0)]
		public short ShortValue;

		[FieldOffset(0)]
		public ushort UShortValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UInt32FloatData
	{
		[FieldOffset(0)]
		public int IntValue;

		public IntPtr IntPtr => (IntPtr)IntValue;

		[FieldOffset(0)]
		public uint UIntValue;

		public UIntPtr UIntPtr => (UIntPtr)UIntValue;

		[FieldOffset(0)]
		public float FloatValue;
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct UInt64FloatDoubleData
	{
		[FieldOffset(0)]
		public long LongValue;

		public IntPtr IntPtr =>
#if RECLASSNET32
			unchecked((IntPtr)(int)LongValue);
#else
			(IntPtr)LongValue;
#endif

		[FieldOffset(0)]
		public ulong ULongValue;

		public UIntPtr UIntPtr =>
#if RECLASSNET32
			unchecked((UIntPtr)(uint)ULongValue);
#else
			(UIntPtr)ULongValue;
#endif

		[FieldOffset(0)]
		public float FloatValue;

		[FieldOffset(0)]
		public double DoubleValue;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ThreePointersData
	{
		public IntPtr Pointer1;
		public IntPtr Pointer2;
		public IntPtr Pointer3;
	}
}
