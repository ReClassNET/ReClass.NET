using System;

namespace ReClassNET.MemorySearcher
{
	public abstract class SearchResult
	{
		public IntPtr Address { get; }

		protected SearchResult(IntPtr address)
		{
			Address = address;
		}
	}

	public class ByteSearchResult : SearchResult
	{
		public byte Value { get; }

		public ByteSearchResult(IntPtr address, byte value)
			: base(address)
		{
			Value = value;
		}
	}

	public class ShortSearchResult : SearchResult
	{
		public short Value { get; }

		public ShortSearchResult(IntPtr address, short value)
			: base(address)
		{
			Value = value;
		}
	}

	public class IntegerSearchResult : SearchResult
	{
		public int Value { get; }

		public IntegerSearchResult(IntPtr address, int value)
			: base(address)
		{
			Value = value;
		}
	}

	public class LongSearchResult : SearchResult
	{
		public long Value { get; }

		public LongSearchResult(IntPtr address, long value)
			: base(address)
		{
			Value = value;
		}
	}

	public class FloatSearchResult : SearchResult
	{
		public float Value { get; }

		public FloatSearchResult(IntPtr address, float value)
			: base(address)
		{
			Value = value;
		}
	}

	public class DoubleSearchResult : SearchResult
	{
		public double Value { get; }

		public DoubleSearchResult(IntPtr address, double value)
			: base(address)
		{
			Value = value;
		}
	}

	public class ArrayOfBytesSearchResult : SearchResult
	{
		public ArrayOfBytesSearchResult(IntPtr address)
			: base(address)
		{

		}
	}

	public class StringSearchResult : SearchResult
	{
		public StringSearchResult(IntPtr address)
			: base(address)
		{

		}
	}
}
