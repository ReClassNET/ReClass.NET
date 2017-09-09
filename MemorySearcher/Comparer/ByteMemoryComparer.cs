using System;
using System.Diagnostics;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class ByteMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType { get; }
		public byte Value1 { get; }
		public byte Value2 { get; }
		public int ValueSize => sizeof(byte);

		public ByteMemoryComparer(SearchCompareType compareType, byte value1, byte value2)
		{
			CompareType = compareType;
			Value1 = value1;
			Value2 = value2;
		}

		public bool Compare(byte[] data, int index)
		{
			var value = data[index];

			switch (CompareType)
			{
				case SearchCompareType.Equal:
					return value == Value1;
				case SearchCompareType.NotEqual:
					return value != Value1;
				case SearchCompareType.GreaterThan:
					return value > Value1;
				case SearchCompareType.GreaterThanOrEqual:
					return value >= Value1;
				case SearchCompareType.LessThan:
					return value < Value1;
				case SearchCompareType.LessThanOrEqual:
					return value <= Value1;
				case SearchCompareType.Between:
					return Value1 < value && value < Value2;
				case SearchCompareType.BetweenOrEqual:
					return Value1 <= value && value <= Value2;
				case SearchCompareType.Unknown:
					return true;
				default:
					throw new InvalidCompareTypeException(CompareType);
			}
		}

		public bool Compare(byte[] data, int index, SearchResult other)
		{
#if DEBUG
			Debug.Assert(other is ByteSearchResult);
#endif

			return Compare(data, index, (ByteSearchResult)other);
		}

		public bool Compare(byte[] data, int index, ByteSearchResult other)
		{
			var value = data[index];

			switch (CompareType)
			{
				case SearchCompareType.Equal:
					return value == Value1;
				case SearchCompareType.NotEqual:
					return value != Value1;
				case SearchCompareType.Changed:
					return value != other.Value;
				case SearchCompareType.NotChanged:
					return value == other.Value;
				case SearchCompareType.GreaterThan:
					return value > Value1;
				case SearchCompareType.GreaterThanOrEqual:
					return value >= Value1;
				case SearchCompareType.Increased:
					return value > other.Value;
				case SearchCompareType.IncreasedOrEqual:
					return value >= other.Value;
				case SearchCompareType.LessThan:
					return value < Value1;
				case SearchCompareType.LessThanOrEqual:
					return value <= Value1;
				case SearchCompareType.Decreased:
					return value < other.Value;
				case SearchCompareType.DecreasedOrEqual:
					return value <= other.Value;
				case SearchCompareType.Between:
					return Value1 < value && value < Value2;
				case SearchCompareType.BetweenOrEqual:
					return Value1 <= value && value <= Value2;
				default:
					throw new InvalidCompareTypeException(CompareType);
			}
		}
	}
}
