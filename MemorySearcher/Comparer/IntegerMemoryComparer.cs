using System;
using System.Diagnostics;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class IntegerMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType { get; }
		public int Value1 { get; }
		public int Value2 { get; }
		public int ValueSize => sizeof(int);

		public IntegerMemoryComparer(SearchCompareType compareType, int value1, int value2)
		{
			CompareType = compareType;
			Value1 = value1;
			Value2 = value2;
		}

		public bool Compare(byte[] data, int index, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToInt32(data, index);

			bool IsMatch()
			{
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

			if (!IsMatch())
			{
				return false;
			}

			result = new IntegerSearchResult(value);

			return true;
		}

		public bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result)
		{
#if DEBUG
			Debug.Assert(previous is IntegerSearchResult);
#endif

			return Compare(data, index, (IntegerSearchResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, IntegerSearchResult previous, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToInt32(data, index);

			bool IsMatch()
			{
				switch (CompareType)
				{
					case SearchCompareType.Equal:
						return value == Value1;
					case SearchCompareType.NotEqual:
						return value != Value1;
					case SearchCompareType.Changed:
						return value != previous.Value;
					case SearchCompareType.NotChanged:
						return value == previous.Value;
					case SearchCompareType.GreaterThan:
						return value > Value1;
					case SearchCompareType.GreaterThanOrEqual:
						return value >= Value1;
					case SearchCompareType.Increased:
						return value > previous.Value;
					case SearchCompareType.IncreasedOrEqual:
						return value >= previous.Value;
					case SearchCompareType.LessThan:
						return value < Value1;
					case SearchCompareType.LessThanOrEqual:
						return value <= Value1;
					case SearchCompareType.Decreased:
						return value < previous.Value;
					case SearchCompareType.DecreasedOrEqual:
						return value <= previous.Value;
					case SearchCompareType.Between:
						return Value1 < value && value < Value2;
					case SearchCompareType.BetweenOrEqual:
						return Value1 <= value && value <= Value2;
					default:
						throw new InvalidCompareTypeException(CompareType);
				}
			}

			if (!IsMatch())
			{
				return false;
			}

			result = new IntegerSearchResult(value);

			return true;
		}
	}
}
