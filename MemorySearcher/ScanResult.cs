using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace ReClassNET.MemorySearcher
{
	public abstract class ScanResult
	{
		public abstract ScanValueType ValueType { get; }

		public IntPtr Address { get; set; }
	}

	public class ByteSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Byte;

		public byte Value { get; }

		public ByteSearchResult(byte value)
		{
			Value = value;
		}
	}

	public class ShortSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Short;

		public short Value { get; }

		public ShortSearchResult(short value)
		{
			Value = value;
		}
	}

	public class IntegerSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Integer;

		public int Value { get; }

		public IntegerSearchResult(int value)
		{
			Value = value;
		}
	}

	public class LongSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Long;

		public long Value { get; }

		public LongSearchResult(long value)
		{
			Value = value;
		}
	}

	public class FloatSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Float;

		public float Value { get; }

		public FloatSearchResult(float value)
		{
			Value = value;
		}
	}

	public class DoubleSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Double;

		public double Value { get; }

		public DoubleSearchResult(double value)
		{
			Value = value;
		}
	}

	public class ArrayOfBytesSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.ArrayOfBytes;

		public byte[] Value { get; }

		public ArrayOfBytesSearchResult(byte[] value)
		{
			Contract.Requires(value != null);

			Value = value;
		}
	}

	public class StringSearchResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.String;

		public string Value { get; }

		public Encoding Encoding { get; }

		public StringSearchResult(string value, Encoding encoding)
		{
			Contract.Requires(value != null);
			Contract.Requires(encoding != null);

			Value = value;
			Encoding = encoding;
		}
	}
}
