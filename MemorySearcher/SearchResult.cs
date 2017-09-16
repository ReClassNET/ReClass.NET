using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace ReClassNET.MemorySearcher
{
	public abstract class SearchResult
	{
		public abstract SearchValueType ValueType { get; }

		public IntPtr Address { get; set; }
	}

	public class ByteSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Byte;

		public byte Value { get; }

		public ByteSearchResult(byte value)
		{
			Value = value;
		}
	}

	public class ShortSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Short;

		public short Value { get; }

		public ShortSearchResult(short value)
		{
			Value = value;
		}
	}

	public class IntegerSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Integer;

		public int Value { get; }

		public IntegerSearchResult(int value)
		{
			Value = value;
		}
	}

	public class LongSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Long;

		public long Value { get; }

		public LongSearchResult(long value)
		{
			Value = value;
		}
	}

	public class FloatSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Float;

		public float Value { get; }

		public FloatSearchResult(float value)
		{
			Value = value;
		}
	}

	public class DoubleSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.Double;

		public double Value { get; }

		public DoubleSearchResult(double value)
		{
			Value = value;
		}
	}

	public class ArrayOfBytesSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.ArrayOfBytes;

		public byte[] Value { get; }

		public ArrayOfBytesSearchResult(byte[] value)
		{
			Contract.Requires(value != null);

			Value = value;
		}
	}

	public class StringSearchResult : SearchResult
	{
		public override SearchValueType ValueType => SearchValueType.String;

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
