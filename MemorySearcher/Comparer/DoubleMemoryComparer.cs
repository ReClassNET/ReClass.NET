using System;
using System.Diagnostics;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class DoubleMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType { get; }
		public SearchRoundMode RoundType { get; }
		public double Value1 { get; }
		public double Value2 { get; }
		public int ValueSize => sizeof(double);

		public DoubleMemoryComparer(SearchCompareType compareType, SearchRoundMode roundType, double value1, double value2)
		{
			CompareType = compareType;
			RoundType = roundType;
			Value1 = value1;
			Value2 = value2;
		}

		private bool CheckRoundedEquality(double value)
		{
			switch (RoundType)
			{
				case SearchRoundMode.Strict:
					return Math.Abs(value - Value1) < 0.05f;
				case SearchRoundMode.Normal:
					return Math.Abs(value - Value1) < 0.5f;
				case SearchRoundMode.Truncate:
					return (int)value == (int)Value1;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public bool Compare(byte[] data, int index, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToDouble(data, index);

			bool IsMatch()
			{
				switch (CompareType)
				{
					case SearchCompareType.Equal:
						return CheckRoundedEquality(value);
					case SearchCompareType.NotEqual:
						return !CheckRoundedEquality(value);
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

			result = new DoubleSearchResult(value);

			return true;
		}

		public bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result)
		{
#if DEBUG
			Debug.Assert(previous is DoubleSearchResult);
#endif

			return Compare(data, index, (DoubleSearchResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, DoubleSearchResult previous, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToDouble(data, index);

			bool IsMatch()
			{
				switch (CompareType)
				{
					case SearchCompareType.Equal:
						return CheckRoundedEquality(value);
					case SearchCompareType.NotEqual:
						return !CheckRoundedEquality(value);
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

			result = new DoubleSearchResult(value);

			return true;
		}
	}
}
