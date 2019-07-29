using System;
using System.Diagnostics;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class ShortMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType { get; }
		public short Value1 { get; }
		public short Value2 { get; }
		public int ValueSize => sizeof(short);

		public ShortMemoryComparer(ScanCompareType compareType, short value1, short value2)
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
				value =>
				{
					switch (CompareType)
					{
						case ScanCompareType.Equal:
							return value == Value1;
						case ScanCompareType.NotEqual:
							return value != Value1;
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
				},
				out result
			);
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is ShortScanResult);
#endif

			return Compare(data, index, (ShortScanResult)previous, out result);
		}

		public bool Compare(byte[] data, int index, ShortScanResult previous, out ScanResult result)
		{
			return CompareInternal(
				data,
				index,
				value =>
				{
					switch (CompareType)
					{
						case ScanCompareType.Equal:
							return value == Value1;
						case ScanCompareType.NotEqual:
							return value != Value1;
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

						case ScanCompareType.Changed:
							return value != previous.Value;
						case ScanCompareType.NotChanged:
							return value == previous.Value;
						case ScanCompareType.Increased:
							return value > previous.Value;
						case ScanCompareType.IncreasedOrEqual:
							return value >= previous.Value;
						case ScanCompareType.Decreased:
							return value < previous.Value;
						case ScanCompareType.DecreasedOrEqual:
							return value <= previous.Value;
						default:
							throw new InvalidCompareTypeException(CompareType);
					}
				},
				out result
			);
		}

		private static bool CompareInternal(byte[] data, int index, Func<short, bool> matcher, out ScanResult result)
		{
			result = null;

			var value = BitConverter.ToInt16(data, index);

			if (!matcher(value))
			{
				return false;
			}

			result = new ShortScanResult(value);

			return true;
		}
	}
}
