using System;
using System.Diagnostics;
using ReClassNET.Extensions;
using ReClassNET.Util.Conversion;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class DoubleMemoryComparer : ISimpleScanComparer
	{
		public ScanCompareType CompareType { get; }
		public ScanRoundMode RoundType { get; }
		public double Value1 { get; }
		public double Value2 { get; }
		public int ValueSize => sizeof(double);

		private readonly int significantDigits;
		private readonly double minValue;
		private readonly double maxValue;

		private readonly EndianBitConverter bitConverter;

		public DoubleMemoryComparer(ScanCompareType compareType, ScanRoundMode roundType, int significantDigits, double value1, double value2, EndianBitConverter bitConverter)
		{
			CompareType = compareType;

			RoundType = roundType;
			this.significantDigits = Math.Max(significantDigits, 1);
			Value1 = Math.Round(value1, this.significantDigits, MidpointRounding.AwayFromZero);
			Value2 = Math.Round(value2, this.significantDigits, MidpointRounding.AwayFromZero);

			var factor = (int)Math.Pow(10.0, this.significantDigits);

			minValue = value1 - 1.0 / factor;
			maxValue = value1 + 1.0 / factor;

			this.bitConverter = bitConverter;
		}

		private bool CheckRoundedEquality(double value) =>
			RoundType switch
			{
				ScanRoundMode.Strict => Value1.IsNearlyEqual(Math.Round(value, significantDigits, MidpointRounding.AwayFromZero), 0.0001),
				ScanRoundMode.Normal => minValue < value && value < maxValue,
				ScanRoundMode.Truncate => (long)value == (long)Value1,
				_ => throw new ArgumentOutOfRangeException()
			};

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			return CompareInternal(
				data,
				index,
				value => CompareType switch
				{
					ScanCompareType.Equal => CheckRoundedEquality(value),
					ScanCompareType.NotEqual => !CheckRoundedEquality(value),
					ScanCompareType.GreaterThan => value > Value1,
					ScanCompareType.GreaterThanOrEqual => value >= Value1,
					ScanCompareType.LessThan => value < Value1,
					ScanCompareType.LessThanOrEqual => value <= Value1,
					ScanCompareType.Between => Value1 < value && value < Value2,
					ScanCompareType.BetweenOrEqual => Value1 <= value && value <= Value2,
					ScanCompareType.Unknown => true,
					_ => throw new InvalidCompareTypeException(CompareType)
				},
				out result
			);
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
			return CompareInternal(
				data,
				index,
				value => CompareType switch
				{
					ScanCompareType.Equal => CheckRoundedEquality(value),
					ScanCompareType.NotEqual => !CheckRoundedEquality(value),
					ScanCompareType.Changed => value != previous.Value,
					ScanCompareType.NotChanged => value == previous.Value,
					ScanCompareType.GreaterThan => value > Value1,
					ScanCompareType.GreaterThanOrEqual => value >= Value1,
					ScanCompareType.Increased => value > previous.Value,
					ScanCompareType.IncreasedOrEqual => value >= previous.Value,
					ScanCompareType.LessThan => value < Value1,
					ScanCompareType.LessThanOrEqual => value <= Value1,
					ScanCompareType.Decreased => value < previous.Value,
					ScanCompareType.DecreasedOrEqual => value <= previous.Value,
					ScanCompareType.Between => Value1 < value && value < Value2,
					ScanCompareType.BetweenOrEqual => Value1 <= value && value <= Value2,
					_ => throw new InvalidCompareTypeException(CompareType)
				},
				out result
			);
		}

		private bool CompareInternal(byte[] data, int index, Func<double, bool> matcher, out ScanResult result)
		{
			result = null;

			var value = bitConverter.ToDouble(data, index);

			if (!matcher(value))
			{
				return false;
			}

			result = new DoubleScanResult(value);

			return true;
		}
	}
}
