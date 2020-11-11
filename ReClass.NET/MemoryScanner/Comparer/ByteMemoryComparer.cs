using System;
using System.Diagnostics;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class ByteMemoryComparer : ISimpleScanComparer
	{
		public ScanCompareType CompareType { get; }
		public byte Value1 { get; }
		public byte Value2 { get; }
		public int ValueSize => sizeof(byte);

		public ByteMemoryComparer(ScanCompareType compareType, byte value1, byte value2)
		{
			CompareType = compareType;

			Value1 = value1;
			Value2 = value2;
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
			Debug.Assert(previous is ByteScanResult);
#endif

			return Compare(data, index, (ByteScanResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, ByteScanResult previous, out ScanResult result)
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

		private static bool CompareInternal(byte[] data, int index, Func<short, bool> matcher, out ScanResult result)
		{
			result = null;

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}
			if ((uint)index >= data.Length)
			{
				throw new ArgumentOutOfRangeException(nameof(index));
			}
			if (index > data.Length - 1)
			{
				throw new ArgumentException();
			}

			var value = data[index];

			if (!matcher(value))
			{
				return false;
			}

			result = new ByteScanResult(value);

			return true;
		}
	}
}
