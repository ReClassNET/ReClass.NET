using System.Diagnostics;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class ByteMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType { get; }
		public byte Value1 { get; }
		public byte Value2 { get; }
		public int ValueSize => sizeof(byte);

		public ByteMemoryComparer(ScanCompareType compareType, byte value1, byte value2)
		{
			CompareType = compareType;

			if (compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual)
			{
				if (value1 > value2)
				{
					Utils.Swap(ref value1, ref value2);
				}
			}

			Value1 = value1;
			Value2 = value2;
		}

		public unsafe bool Compare(byte* data, out ScanResult result)
		{
			result = null;

			var value = *data;

			bool IsMatch()
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
			}

			if (!IsMatch())
			{
				return false;
			}

			result = new ByteScanResult(value);

			return true;
		}

		public unsafe bool Compare(byte* data, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is ByteScanResult);
#endif

			return Compare(data, (ByteScanResult)previous, out result);
		}

		public unsafe bool Compare(byte* data, ByteScanResult previous, out ScanResult result)
		{
			result = null;

			var value = *data;

			bool IsMatch()
			{
				switch (CompareType)
				{
					case ScanCompareType.Equal:
						return value == Value1;
					case ScanCompareType.NotEqual:
						return value != Value1;
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

			result = new ByteScanResult(value);

			return true;
		}
	}
}
