using System;
using System.Diagnostics;
using ReClassNET.Util.Conversion;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class LongMemoryComparer : ISimpleScanComparer
	{
		public ScanCompareType CompareType { get; }
		public long Value1 { get; }
		public long Value2 { get; }
		public int ValueSize => sizeof(long);

		private readonly EndianBitConverter bitConverter;

		public LongMemoryComparer(ScanCompareType compareType, long value1, long value2, EndianBitConverter bitConverter)
		{
			CompareType = compareType;

			Value1 = value1;
			Value2 = value2;

			this.bitConverter = bitConverter;
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			return CompareInternal(
				data,
				index,
				value => CompareType switch
				{
					ScanCompareType.Equal => value == Value1,
					ScanCompareType.NotEqual => value != Value1,
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
			Debug.Assert(previous is LongScanResult);
#endif

			return Compare(data, index, (LongScanResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, LongScanResult previous, out ScanResult result)
		{
			return CompareInternal(
				data,
				index,
				value => CompareType switch
				{
					ScanCompareType.Equal => value == Value1,
					ScanCompareType.NotEqual => value != Value1,
					ScanCompareType.GreaterThan => value > Value1,
					ScanCompareType.GreaterThanOrEqual => value >= Value1,
					ScanCompareType.LessThan => value < Value1,
					ScanCompareType.LessThanOrEqual => value <= Value1,
					ScanCompareType.Between => Value1 < value && value < Value2,
					ScanCompareType.BetweenOrEqual => Value1 <= value && value <= Value2,
					ScanCompareType.Changed => value != previous.Value,
					ScanCompareType.NotChanged => value == previous.Value,
					ScanCompareType.Increased => value > previous.Value,
					ScanCompareType.IncreasedOrEqual => value >= previous.Value,
					ScanCompareType.Decreased => value < previous.Value,
					ScanCompareType.DecreasedOrEqual => value <= previous.Value,
					_ => throw new InvalidCompareTypeException(CompareType)
				},
				out result
			);
		}

		private bool CompareInternal(byte[] data, int index, Func<long, bool> matcher, out ScanResult result)
		{
			result = null;

			var value = bitConverter.ToInt64(data, index);

			if (!matcher(value))
			{
				return false;
			}

			result = new LongScanResult(value);

			return true;
		}
	}
}
