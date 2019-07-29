using System;
using NFluent;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using Xunit;

namespace ReClass.NET_Tests.MemoryScanner.Comparer
{
	public class ShortMemoryComparerTest
	{
		[Theory]
		[InlineData(ScanCompareType.Equal, 0, 0)]
		[InlineData(ScanCompareType.Equal, 1, 2)]
		[InlineData(ScanCompareType.Equal, 2, 1)]
		[InlineData(ScanCompareType.Between, 2, 4)]
		[InlineData(ScanCompareType.BetweenOrEqual, 4, 2)]
		[InlineData(ScanCompareType.NotEqual, 0, 0)]
		public void TestConstructor(ScanCompareType compareType, short value1, short value2)
		{
			var sut = new ShortMemoryComparer(compareType, value1, value2);

			Check.That(sut.CompareType).IsEqualTo(compareType);
			Check.That(sut.ValueSize).IsEqualTo(sizeof(short));
			Check.That(sut.Value1).IsOneOf(value1, value2);
			Check.That(sut.Value2).IsOneOf(value1, value2);
		}

		public static TheoryData<ScanCompareType, short, short, byte[], bool, ScanResult> GetTestCompareBasicData() => new TheoryData<ScanCompareType, short, short, byte[], bool, ScanResult>
		{
			{ ScanCompareType.Equal, 0, 0, BitConverter.GetBytes((short)0), true, new ShortScanResult(0) },
			{ ScanCompareType.Equal, 0, 0, BitConverter.GetBytes((short)1), false, null },
			{ ScanCompareType.Equal, 1, 0, BitConverter.GetBytes((short)1), true, new ShortScanResult(1) },
			{ ScanCompareType.Equal, 1, 0, BitConverter.GetBytes((short)0), false, null },
			{ ScanCompareType.NotEqual, 1, 0, BitConverter.GetBytes((short)0), true, new ShortScanResult(0) },
			{ ScanCompareType.NotEqual, 1, 0, BitConverter.GetBytes((short)1), false, null },
			{ ScanCompareType.GreaterThan, 1, 0, BitConverter.GetBytes((short)0), false, null },
			{ ScanCompareType.GreaterThan, 1, 0, BitConverter.GetBytes((short)2), true, new ShortScanResult(2) },
			{ ScanCompareType.GreaterThanOrEqual, 1, 0, BitConverter.GetBytes((short)0), false, null },
			{ ScanCompareType.GreaterThanOrEqual, 1, 0, BitConverter.GetBytes((short)1), true, new ShortScanResult(1) },
			{ ScanCompareType.GreaterThanOrEqual, 1, 0, BitConverter.GetBytes((short)2), true, new ShortScanResult(2) },
			{ ScanCompareType.LessThan, 1, 0, BitConverter.GetBytes((short)1), false, null },
			{ ScanCompareType.LessThan, 1, 0, BitConverter.GetBytes((short)0), true, new ShortScanResult(0) },
			{ ScanCompareType.LessThanOrEqual, 1, 0, BitConverter.GetBytes((short)2), false, null },
			{ ScanCompareType.LessThanOrEqual, 1, 0, BitConverter.GetBytes((short)1), true, new ShortScanResult(1) },
			{ ScanCompareType.LessThanOrEqual, 1, 0, BitConverter.GetBytes((short)0), true, new ShortScanResult(0) },
			{ ScanCompareType.Between, 1, 2, BitConverter.GetBytes((short)0), false, null },
			{ ScanCompareType.Between, 1, 2, BitConverter.GetBytes((short)1), false, null },
			{ ScanCompareType.Between, 1, 2, BitConverter.GetBytes((short)2), false, null },
			{ ScanCompareType.Between, 1, 2, BitConverter.GetBytes((short)3), false, null },
			{ ScanCompareType.BetweenOrEqual, 1, 2, BitConverter.GetBytes((short)0), false, null },
			{ ScanCompareType.BetweenOrEqual, 1, 2, BitConverter.GetBytes((short)1), true, new ShortScanResult(1) },
			{ ScanCompareType.BetweenOrEqual, 1, 2, BitConverter.GetBytes((short)2), true, new ShortScanResult(2) },
			{ ScanCompareType.BetweenOrEqual, 1, 2, BitConverter.GetBytes((short)3), false, null }
		};

		public static TheoryData<ScanCompareType, short, short, byte[], bool, ScanResult> GetTestCompareScanCompareTypeUnknownData() => new TheoryData<ScanCompareType, short, short, byte[], bool, ScanResult>
		{
			{ ScanCompareType.Unknown, 0, 0, BitConverter.GetBytes((short)0), true, new ShortScanResult(0) },
			{ ScanCompareType.Unknown, 0, 0, BitConverter.GetBytes((short)1), true, new ShortScanResult(1) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareBasicData))]
		[MemberData(nameof(GetTestCompareScanCompareTypeUnknownData))]
		public void TestCompare(ScanCompareType compareType, short value1, short value2, byte[] data, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new ShortMemoryComparer(compareType, value1, value2);

			Check.That(sut.Compare(data, 0, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<ShortScanResult>();
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
			var sut = new ShortMemoryComparer(compareType, 0, 0);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes((short)0), 0, out _)).Throws<InvalidCompareTypeException>();
		}

		public static TheoryData<byte[], int, Type> GetTestCompareThrowsData() => new TheoryData<byte[], int, Type>
		{
			{ null, 0, typeof(ArgumentNullException) },
			{ new byte[0], 0, typeof(ArgumentOutOfRangeException) },
			{ new byte[2], 2, typeof(ArgumentOutOfRangeException) },
			{ new byte[1], 0, typeof(ArgumentException) },
			{ new byte[2], 1, typeof(ArgumentException) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareThrowsData))]
		public void TestCompareInvalidDataThrows(byte[] data, int index, Type expectedExceptionType)
		{
			var sut = new ShortMemoryComparer(ScanCompareType.Equal, 0, 0);

			Check.ThatCode(() => sut.Compare(data, index, out _)).ThrowsType(expectedExceptionType);
		}

		public static TheoryData<ScanCompareType, short, short, byte[], ScanResult, bool, ScanResult> GetTestCompareWithPreviousData()
		{
			var data = new TheoryData<ScanCompareType, short, short, byte[], ScanResult, bool, ScanResult>
			{
				{ ScanCompareType.Changed, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), true, new ShortScanResult(0) },
				{ ScanCompareType.Changed, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), false, null },
				{ ScanCompareType.NotChanged, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), true, new ShortScanResult(1) },
				{ ScanCompareType.NotChanged, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), false, null },
				{ ScanCompareType.Increased, 0, 0, BitConverter.GetBytes((short)2), new ShortScanResult(1), true, new ShortScanResult(2) },
				{ ScanCompareType.Increased, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), false, null },
				{ ScanCompareType.Increased, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), false, null },
				{ ScanCompareType.IncreasedOrEqual, 0, 0, BitConverter.GetBytes((short)2), new ShortScanResult(1), true, new ShortScanResult(2) },
				{ ScanCompareType.IncreasedOrEqual, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), true, new ShortScanResult(1) },
				{ ScanCompareType.IncreasedOrEqual, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), false, null },
				{ ScanCompareType.Decreased, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), true, new ShortScanResult(0) },
				{ ScanCompareType.Decreased, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), false, null },
				{ ScanCompareType.Decreased, 0, 0, BitConverter.GetBytes((short)2), new ShortScanResult(1), false, null },
				{ ScanCompareType.DecreasedOrEqual, 0, 0, BitConverter.GetBytes((short)0), new ShortScanResult(1), true, new ShortScanResult(0) },
				{ ScanCompareType.DecreasedOrEqual, 0, 0, BitConverter.GetBytes((short)1), new ShortScanResult(1), true, new ShortScanResult(1) },
				{ ScanCompareType.DecreasedOrEqual, 0, 0, BitConverter.GetBytes((short)2), new ShortScanResult(1), false, null }
			};

			var basicData = GetTestCompareBasicData();
			foreach (var x in basicData)
			{
				data.Add((ScanCompareType)x[0], (short)x[1], (short)x[2], (byte[])x[3], new ShortScanResult(1), (bool)x[4], (ScanResult)x[5]);
			}

			return data;
		}

		[Theory]
		[MemberData(nameof(GetTestCompareWithPreviousData))]
		public void TestCompareWithPrevious(ScanCompareType compareType, short value1, short value2, byte[] data, ScanResult previousScanResult, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new ShortMemoryComparer(compareType, value1, value2);

			Check.That(sut.Compare(data, 0, previousScanResult, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<ShortScanResult>();
			}
		}

		[Fact]
		public void TestCompareWithPreviousThrows()
		{
			var sut = new ShortMemoryComparer(ScanCompareType.Unknown, 0, 0);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes((short)0), 0, new ShortScanResult(0), out _)).Throws<InvalidCompareTypeException>();
		}
	}
}
