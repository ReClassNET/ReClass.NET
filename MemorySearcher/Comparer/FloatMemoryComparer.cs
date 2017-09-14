using System;
using System.Diagnostics;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher.Comparer
{
	public class FloatMemoryComparer : IMemoryComparer
	{
		public SearchCompareType CompareType { get; }
		public SearchRoundMode RoundType { get; }
		public float Value1 { get; }
		public float Value2 { get; }
		public int ValueSize => sizeof(float);

		private readonly int significantDigits;
		private readonly float minValue;
		private readonly float maxValue;

		public FloatMemoryComparer(SearchCompareType compareType, SearchRoundMode roundType, int significantDigits, float value1, float value2)
		{
			CompareType = compareType;
			RoundType = roundType;
			this.significantDigits = significantDigits;
			Value1 = (float)Math.Round(value1, significantDigits, MidpointRounding.AwayFromZero);
			Value2 = (float)Math.Round(value2, significantDigits, MidpointRounding.AwayFromZero);

			var factor = (int)Math.Pow(10.0, significantDigits);

			minValue = value1 - 1.0f / factor;
			maxValue = value1 + 1.0f / factor;
		}

		private bool CheckRoundedEquality(float value)
		{
			switch (RoundType)
			{
				case SearchRoundMode.Strict:
					return Value1.IsNearlyEqual((float)Math.Round(value, significantDigits, MidpointRounding.AwayFromZero));
				case SearchRoundMode.Normal:
					return minValue < value && value < maxValue;
				case SearchRoundMode.Truncate:
					return (int)value == (int)Value1;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public bool Compare(byte[] data, int index, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToSingle(data, index);

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

			result = new FloatSearchResult(value);

			return true;
		}

		public bool Compare(byte[] data, int index, SearchResult previous, out SearchResult result)
		{
#if DEBUG
			Debug.Assert(previous is FloatSearchResult);
#endif

			return Compare(data, index, (FloatSearchResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, FloatSearchResult previous, out SearchResult result)
		{
			result = null;

			var value = BitConverter.ToSingle(data, index);

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

			result = new FloatSearchResult(value);

			return true;
		}
	}
}
