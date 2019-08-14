using System;
using NFluent;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using Xunit;

namespace ReClass.NET_Tests.MemoryScanner.Comparer
{
	public class LongMemoryComparerTest
	{
		[Theory]
		[InlineData(ScanCompareType.Equal, 0L, 0L)]
		[InlineData(ScanCompareType.Equal, 1L, 2L)]
		[InlineData(ScanCompareType.Equal, 2L, 1L)]
		[InlineData(ScanCompareType.Between, 2L, 4L)]
		[InlineData(ScanCompareType.BetweenOrEqual, 4L, 2L)]
		[InlineData(ScanCompareType.NotEqual, 0L, 0L)]
		public void TestConstructor(ScanCompareType compareType, long value1, long value2)
		{
			var sut = new LongMemoryComparer(compareType, value1, value2);

			Check.That(sut.CompareType).IsEqualTo(compareType);
			Check.That(sut.ValueSize).IsEqualTo(sizeof(long));
			Check.That(sut.Value1).IsOneOf(value1, value2);
			Check.That(sut.Value2).IsOneOf(value1, value2);
		}

		public static TheoryData<ScanCompareType, long, long, byte[], bool, ScanResult> GetTestCompareBasicData() => new TheoryData<ScanCompareType, long, long, byte[], bool, ScanResult>
		{
			{ ScanCompareType.Equal, 0L, 0L, BitConverter.GetBytes(0L), true, new LongScanResult(0L) },
			{ ScanCompareType.Equal, 0L, 0L, BitConverter.GetBytes(1L), false, null },
			{ ScanCompareType.Equal, 1L, 0L, BitConverter.GetBytes(1L), true, new LongScanResult(1L) },
			{ ScanCompareType.Equal, 1L, 0L, BitConverter.GetBytes(0L), false, null },
			{ ScanCompareType.NotEqual, 1L, 0L, BitConverter.GetBytes(0L), true, new LongScanResult(0L) },
			{ ScanCompareType.NotEqual, 1L, 0L, BitConverter.GetBytes(1L), false, null },
			{ ScanCompareType.GreaterThan, 1L, 0L, BitConverter.GetBytes(0L), false, null },
			{ ScanCompareType.GreaterThan, 1L, 0L, BitConverter.GetBytes(2L), true, new LongScanResult(2L) },
			{ ScanCompareType.GreaterThanOrEqual, 1L, 0L, BitConverter.GetBytes(0L), false, null },
			{ ScanCompareType.GreaterThanOrEqual, 1L, 0L, BitConverter.GetBytes(1L), true, new LongScanResult(1L) },
			{ ScanCompareType.GreaterThanOrEqual, 1L, 0L, BitConverter.GetBytes(2L), true, new LongScanResult(2L) },
			{ ScanCompareType.LessThan, 1L, 0L, BitConverter.GetBytes(1L), false, null },
			{ ScanCompareType.LessThan, 1L, 0L, BitConverter.GetBytes(0L), true, new LongScanResult(0L) },
			{ ScanCompareType.LessThanOrEqual, 1L, 0L, BitConverter.GetBytes(2L), false, null },
			{ ScanCompareType.LessThanOrEqual, 1L, 0L, BitConverter.GetBytes(1L), true, new LongScanResult(1L) },
			{ ScanCompareType.LessThanOrEqual, 1L, 0L, BitConverter.GetBytes(0L), true, new LongScanResult(0L) },
			{ ScanCompareType.Between, 1L, 2L, BitConverter.GetBytes(0L), false, null },
			{ ScanCompareType.Between, 1L, 2L, BitConverter.GetBytes(1L), false, null },
			{ ScanCompareType.Between, 1L, 2L, BitConverter.GetBytes(2L), false, null },
			{ ScanCompareType.Between, 1L, 2L, BitConverter.GetBytes(3L), false, null },
			{ ScanCompareType.BetweenOrEqual, 1L, 2L, BitConverter.GetBytes(0L), false, null },
			{ ScanCompareType.BetweenOrEqual, 1L, 2L, BitConverter.GetBytes(1L), true, new LongScanResult(1L) },
			{ ScanCompareType.BetweenOrEqual, 1L, 2L, BitConverter.GetBytes(2L), true, new LongScanResult(2L) },
			{ ScanCompareType.BetweenOrEqual, 1L, 2L, BitConverter.GetBytes(3L), false, null }
		};

		public static TheoryData<ScanCompareType, long, long, byte[], bool, ScanResult> GetTestCompareScanCompareTypeUnknownData() => new TheoryData<ScanCompareType, long, long, byte[], bool, ScanResult>
		{
			{ ScanCompareType.Unknown, 0L, 0L, BitConverter.GetBytes(0L), true, new LongScanResult(0L) },
			{ ScanCompareType.Unknown, 0L, 0L, BitConverter.GetBytes(1L), true, new LongScanResult(1L) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareBasicData))]
		[MemberData(nameof(GetTestCompareScanCompareTypeUnknownData))]
		public void TestCompare(ScanCompareType compareType, long value1, long value2, byte[] data, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new LongMemoryComparer(compareType, value1, value2);

			Check.That(sut.Compare(data, 0, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<LongScanResult>();
			}
		}

		[Theory]
		[InlineData(ScanCompareType.Changed)]
		[InlineData(ScanCompareType.NotChanged)]
		[InlineData(ScanCompareType.Decreased)]
		[InlineData(ScanCompareType.DecreasedOrEqual)]
		[InlineData(ScanCompareType.Increased)]
		[InlineData(ScanCompareType.IncreasedOrEqual)]
		public void TestCompareInvalidCompareTypeThrows(ScanCompareType compareType)
		{
			var sut = new LongMemoryComparer(compareType, 0L, 0L);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes(0L), 0, out _)).Throws<InvalidCompareTypeException>();
		}

		public static TheoryData<byte[], int, Type> GetTestCompareThrowsData() => new TheoryData<byte[], int, Type>
		{
			{ null, 0, typeof(ArgumentNullException) },
			{ new byte[0], 0, typeof(ArgumentOutOfRangeException) },
			{ new byte[8], 8, typeof(ArgumentOutOfRangeException) },
			{ new byte[7], 0, typeof(ArgumentException) },
			{ new byte[8], 1, typeof(ArgumentException) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareThrowsData))]
		public void TestCompareInvalidDataThrows(byte[] data, int index, Type expectedExceptionType)
		{
			var sut = new LongMemoryComparer(ScanCompareType.Equal, 0L, 0L);

			Check.ThatCode(() => sut.Compare(data, index, out _)).ThrowsType(expectedExceptionType);
		}

		public static TheoryData<ScanCompareType, long, long, byte[], ScanResult, bool, ScanResult> GetTestCompareWithPreviousData()
		{
			var data = new TheoryData<ScanCompareType, long, long, byte[], ScanResult, bool, ScanResult>
			{
				{ ScanCompareType.Changed, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), true, new LongScanResult(0) },
				{ ScanCompareType.Changed, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), false, null },
				{ ScanCompareType.NotChanged, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), true, new LongScanResult(1) },
				{ ScanCompareType.NotChanged, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), false, null },
				{ ScanCompareType.Increased, 0L, 0L, BitConverter.GetBytes(2L), new LongScanResult(1L), true, new LongScanResult(2) },
				{ ScanCompareType.Increased, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), false, null },
				{ ScanCompareType.Increased, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), false, null },
				{ ScanCompareType.IncreasedOrEqual, 0L, 0L, BitConverter.GetBytes(2L), new LongScanResult(1L), true, new LongScanResult(2) },
				{ ScanCompareType.IncreasedOrEqual, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), true, new LongScanResult(1) },
				{ ScanCompareType.IncreasedOrEqual, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), false, null },
				{ ScanCompareType.Decreased, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), true, new LongScanResult(0) },
				{ ScanCompareType.Decreased, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), false, null },
				{ ScanCompareType.Decreased, 0L, 0L, BitConverter.GetBytes(2L), new LongScanResult(1L), false, null },
				{ ScanCompareType.DecreasedOrEqual, 0L, 0L, BitConverter.GetBytes(0L), new LongScanResult(1L), true, new LongScanResult(0) },
				{ ScanCompareType.DecreasedOrEqual, 0L, 0L, BitConverter.GetBytes(1L), new LongScanResult(1L), true, new LongScanResult(1) },
				{ ScanCompareType.DecreasedOrEqual, 0L, 0L, BitConverter.GetBytes(2L), new LongScanResult(1L), false, null }
			};

			var basicData = GetTestCompareBasicData();
			foreach (var x in basicData)
			{
				data.Add((ScanCompareType)x[0], (long)x[1], (long)x[2], (byte[])x[3], new LongScanResult(1L), (bool)x[4], (ScanResult)x[5]);
			}

			return data;
		}

		[Theory]
		[MemberData(nameof(GetTestCompareWithPreviousData))]
		public void TestCompareWithPrevious(ScanCompareType compareType, long value1, long value2, byte[] data, ScanResult previousScanResult, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new LongMemoryComparer(compareType, value1, value2);

			Check.That(sut.Compare(data, 0, previousScanResult, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<LongScanResult>();
			}
		}

		[Fact]
		public void TestCompareWithPreviousThrows()
		{
			var sut = new LongMemoryComparer(ScanCompareType.Unknown, 0L, 0L);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes(0L), 0, new LongScanResult(0L), out _)).Throws<InvalidCompareTypeException>();
		}
	}
}
