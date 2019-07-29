using System;
using System.Diagnostics;
using ReClassNET.Extensions;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class DoubleMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType { get; }
		public ScanRoundMode RoundType { get; }
		public double Value1 { get; }
		public double Value2 { get; }
		public int ValueSize => sizeof(double);

		private readonly int significantDigits;
		private readonly double minValue;
		private readonly double maxValue;

		public DoubleMemoryComparer(ScanCompareType compareType, ScanRoundMode roundType, int significantDigits, double value1, double value2)
		{
			CompareType = compareType;

			RoundType = roundType;
			this.significantDigits = Math.Max(significantDigits, 1);
			Value1 = Math.Round(value1, this.significantDigits, MidpointRounding.AwayFromZero);
			Value2 = Math.Round(value2, this.significantDigits, MidpointRounding.AwayFromZero);

			var factor = (int)Math.Pow(10.0, this.significantDigits);

			minValue = value1 - 1.0 / factor;
			maxValue = value1 + 1.0 / factor;
		}

		private bool CheckRoundedEquality(double value)
		{
			switch (RoundType)
			{
				case ScanRoundMode.Strict:
					return Value1.IsNearlyEqual(Math.Round(value, significantDigits, MidpointRounding.AwayFromZero), 0.0001);
				case ScanRoundMode.Normal:
					return minValue < value && value < maxValue;
				case ScanRoundMode.Truncate:
					return (long)value == (long)Value1;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			result = null;

			var value = BitConverter.ToDouble(data, index);

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

			result = new DoubleScanResult(value);

			return true;
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is DoubleScanResult);
#endif

			return Compare(data, index, (DoubleScanResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, DoubleScanResult previous, out ScanResult result)
		{
			result = null;

			var value = BitConverter.ToDouble(data, index);

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

			result = new DoubleScanResult(value);

			return true;
		}
	}
}
