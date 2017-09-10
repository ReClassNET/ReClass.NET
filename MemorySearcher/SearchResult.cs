using System;

namespace ReClassNET.MemorySearcher
{
	public abstract class SearchResult
	{
		public IntPtr Address { get; set; }
	}

	public class ByteSearchResult : SearchResult
	{
		public byte Value { get; }

		public ByteSearchResult(byte value)
		{
			Value = value;
		}
	}

	public class ShortSearchResult : SearchResult
	{
		public short Value { get; }

		public ShortSearchResult(short value)
		{
			Value = value;
		}
	}

	public class IntegerSearchResult : SearchResult
	{
		public int Value { get; }

		public IntegerSearchResult(int value)
		{
			Value = value;
		}
	}

	public class LongSearchResult : SearchResult
	{
		public long Value { get; }

		public LongSearchResult(long value)
		{
			Value = value;
		}
	}

	public class FloatSearchResult : SearchResult
	{
		public float Value { get; }

		public FloatSearchResult(float value)
		{
			Value = value;
		}
	}

	public class DoubleSearchResult : SearchResult
	{
		public double Value { get; }

		public DoubleSearchResult(double value)
		{
			Value = value;
		}
	}

	public class ArrayOfBytesSearchResult : SearchResult
	{

	}

	public class StringSearchResult : SearchResult
	{

	}
}
