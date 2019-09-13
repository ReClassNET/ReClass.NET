using System;
using System.Diagnostics;
using ReClassNET.Extensions;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class FloatMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType { get; }
		public ScanRoundMode RoundType { get; }
		public float Value1 { get; }
		public float Value2 { get; }
		public int ValueSize => sizeof(float);

		private readonly int significantDigits;
		private readonly float minValue;
		private readonly float maxValue;

		public FloatMemoryComparer(ScanCompareType compareType, ScanRoundMode roundType, int significantDigits, float value1, float value2)
		{
			CompareType = compareType;

			RoundType = roundType;
			this.significantDigits = Math.Max(significantDigits, 1);
			Value1 = (float)Math.Round(value1, this.significantDigits, MidpointRounding.AwayFromZero);
			Value2 = (float)Math.Round(value2, this.significantDigits, MidpointRounding.AwayFromZero);

			var factor = (int)Math.Pow(10.0, this.significantDigits);

			minValue = value1 - 1.0f / factor;
			maxValue = value1 + 1.0f / factor;
		}

		private bool CheckRoundedEquality(float value)
		{
			switch (RoundType)
			{
				case ScanRoundMode.Strict:
					return Value1.IsNearlyEqual((float)Math.Round(value, significantDigits, MidpointRounding.AwayFromZero), 0.0001f);
				case ScanRoundMode.Normal:
					return minValue < value && value < maxValue;
				case ScanRoundMode.Truncate:
					return (int)value == (int)Value1;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			result = null;

			var value = BitConverter.ToSingle(data, index);

			bool IsMatch()
			{
				switch (CompareType)
				{
					case ScanCompareType.Equal:
						return CheckRoundedEquality(value);
					case ScanCompareType.NotEqual:
						return !CheckRoundedEquality(value);
					case ScanCompareType.GreaterThan:
						return value > Value1;
					case ScanCompareType.GreaterThanOrEqual:
						return value >= Value1;
					case ScanCompareType.LessThan:
						return value < Value1;
					case ScanCompareType.LessThanOrEqual:
						return value <= Value1;
					case ScanCompareType.Between:
						return Value1 < value && value < Value2;
					case ScanCompareType.BetweenOrEqual:
						return Value1 <= value && value <= Value2;
					case ScanCompareType.Unknown:
						return true;
					default:
						throw new InvalidCompareTypeException(CompareType);
				}
			}

			if (!IsMatch())
			{
				return false;
			}

			result = new FloatScanResult(value);

			return true;
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is FloatScanResult);
#endif

			return Compare(data, index, (FloatScanResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, FloatScanResult previous, out ScanResult result)
		{
			result = null;

			var value = BitConverter.ToSingle(data, index);

			bool IsMatch()
			{
				switch (CompareType)
				{
					case ScanCompareType.Equal:
						return CheckRoundedEquality(value);
					case ScanCompareType.NotEqual:
						return !CheckRoundedEquality(value);
					case ScanCompareType.Changed:
						return value != previous.Value;
					case ScanCompareType.NotChanged:
						return value == previous.Value;
					case ScanCompareType.GreaterThan:
						return value > Value1;
					case ScanCompareType.GreaterThanOrEqual:
						return value >= Value1;
					case ScanCompareType.Increased:
						return value > previous.Value;
					case ScanCompareType.IncreasedOrEqual:
						return value >= previous.Value;
					case ScanCompareType.LessThan:
						return value < Value1;
					case ScanCompareType.LessThanOrEqual:
						return value <= Value1;
					case ScanCompareType.Decreased:
						return value < previous.Value;
					case ScanCompareType.DecreasedOrEqual:
						return value <= previous.Value;
					case ScanCompareType.Between:
						return Value1 < value && value < Value2;
					case ScanCompareType.BetweenOrEqual:
						return Value1 <= value && value <= Value2;
					default:
						throw new InvalidCompareTypeException(CompareType);
				}
			}

			if (!IsMatch())
			{
				return false;
			}

			result = new FloatScanResult(value);

			return true;
		}
	}
}
