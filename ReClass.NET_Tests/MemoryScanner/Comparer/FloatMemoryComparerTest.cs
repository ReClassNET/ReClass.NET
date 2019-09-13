using System;
using NFluent;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using Xunit;

namespace ReClass.NET_Tests.MemoryScanner.Comparer
{
	public class FloatMemoryComparerTest
	{
		[Theory]
		[InlineData(ScanCompareType.Equal, ScanRoundMode.Normal, 0.0f, 0.0f)]
		[InlineData(ScanCompareType.Equal, ScanRoundMode.Strict, 1.0f, 2.0f)]
		[InlineData(ScanCompareType.Equal, ScanRoundMode.Truncate, 2.0f, 1.0f)]
		[InlineData(ScanCompareType.Between, ScanRoundMode.Normal, 2.0f, 4.0f)]
		[InlineData(ScanCompareType.BetweenOrEqual, ScanRoundMode.Strict, 4.0f, 2.0f)]
		[InlineData(ScanCompareType.NotEqual, ScanRoundMode.Truncate, 0.0f, 0.0f)]
		public void TestConstructor(ScanCompareType compareType, ScanRoundMode roundMode, float value1, float value2)
		{
			var sut = new FloatMemoryComparer(compareType, roundMode, 1, value1, value2);

			Check.That(sut.CompareType).IsEqualTo(compareType);
			Check.That(sut.RoundType).IsEqualTo(roundMode);
			Check.That(sut.ValueSize).IsEqualTo(sizeof(float));
			Check.That(sut.Value1).IsOneOf(value1, value2);
			Check.That(sut.Value2).IsOneOf(value1, value2);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public void TestConstructorSignificantDigits(int significantDigits)
		{
			const float Value1 = 1.234567f;
			const float Value2 = 7.654321f;

			var sut = new FloatMemoryComparer(ScanCompareType.Equal, ScanRoundMode.Normal, significantDigits, Value1, Value2);

			Check.That(sut.Value1).IsEqualTo((float)Math.Round(Value1, significantDigits, MidpointRounding.AwayFromZero));
			Check.That(sut.Value2).IsEqualTo((float)Math.Round(Value2, significantDigits, MidpointRounding.AwayFromZero));
		}

		public static TheoryData<ScanCompareType, float, float, byte[], bool, ScanResult> GetTestCompareBasicData() => new TheoryData<ScanCompareType, float, float, byte[], bool, ScanResult>
		{
			{ ScanCompareType.GreaterThan, 1.0f, 0.0f, BitConverter.GetBytes(0.0f), false, null },
			{ ScanCompareType.GreaterThan, 1.0f, 0.0f, BitConverter.GetBytes(2.0f), true, new FloatScanResult(2.0f) },
			{ ScanCompareType.GreaterThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(0.0f), false, null },
			{ ScanCompareType.GreaterThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(1.0f), true, new FloatScanResult(1.0f) },
			{ ScanCompareType.GreaterThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(2.0f), true, new FloatScanResult(2.0f) },
			{ ScanCompareType.LessThan, 1.0f, 0.0f, BitConverter.GetBytes(1.0f), false, null },
			{ ScanCompareType.LessThan, 1.0f, 0.0f, BitConverter.GetBytes(0.0f), true, new FloatScanResult(0.0f) },
			{ ScanCompareType.LessThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(2.0f), false, null },
			{ ScanCompareType.LessThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(1.0f), true, new FloatScanResult(1.0f) },
			{ ScanCompareType.LessThanOrEqual, 1.0f, 0.0f, BitConverter.GetBytes(0.0f), true, new FloatScanResult(0.0f) },
			{ ScanCompareType.Between, 1.0f, 2.0f, BitConverter.GetBytes(0.0f), false, null },
			{ ScanCompareType.Between, 1.0f, 2.0f, BitConverter.GetBytes(1.0f), false, null },
			{ ScanCompareType.Between, 1.0f, 2.0f, BitConverter.GetBytes(2.0f), false, null },
			{ ScanCompareType.Between, 1.0f, 2.0f, BitConverter.GetBytes(3.0f), false, null },
			{ ScanCompareType.BetweenOrEqual, 1.0f, 2.0f, BitConverter.GetBytes(0.0f), false, null },
			{ ScanCompareType.BetweenOrEqual, 1.0f, 2.0f, BitConverter.GetBytes(1.0f), true, new FloatScanResult(1.0f) },
			{ ScanCompareType.BetweenOrEqual, 1.0f, 2.0f, BitConverter.GetBytes(2.0f), true, new FloatScanResult(2.0f) },
			{ ScanCompareType.BetweenOrEqual, 1.0f, 2.0f, BitConverter.GetBytes(3.0f), false, null }
		};

		public static TheoryData<ScanCompareType, float, float, byte[], bool, ScanResult> GetTestCompareScanCompareTypeUnknownData() => new TheoryData<ScanCompareType, float, float, byte[], bool, ScanResult>
		{
			{ ScanCompareType.Unknown, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), true, new FloatScanResult(0.0f) },
			{ ScanCompareType.Unknown, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), true, new FloatScanResult(1.0f) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareBasicData))]
		[MemberData(nameof(GetTestCompareScanCompareTypeUnknownData))]
		public void TestCompare(ScanCompareType compareType, float value1, float value2, byte[] data, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new FloatMemoryComparer(compareType, ScanRoundMode.Normal, 1, value1, value2);

			Check.That(sut.Compare(data, 0, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<FloatScanResult>();
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
			var sut = new FloatMemoryComparer(compareType, ScanRoundMode.Normal, 1, 0.0f, 0.0f);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes(0.0f), 0, out _)).Throws<InvalidCompareTypeException>();
		}

		public static TheoryData<byte[], int, Type> GetTestCompareThrowsData() => new TheoryData<byte[], int, Type>
		{
			{ null, 0, typeof(ArgumentNullException) },
			{ new byte[0], 0, typeof(ArgumentOutOfRangeException) },
			{ new byte[1], 1, typeof(ArgumentOutOfRangeException) }
		};

		[Theory]
		[MemberData(nameof(GetTestCompareThrowsData))]
		public void TestCompareInvalidDataThrows(byte[] data, int index, Type expectedExceptionType)
		{
			var sut = new FloatMemoryComparer(ScanCompareType.Unknown, ScanRoundMode.Normal, 1, 0.0f, 0.0f);

			Check.ThatCode(() => sut.Compare(data, index, out _)).ThrowsType(expectedExceptionType);
		}

		public static TheoryData<ScanCompareType, float, float, byte[], ScanResult, bool, ScanResult> GetTestCompareWithPreviousData()
		{
			var data = new TheoryData<ScanCompareType, float, float, byte[], ScanResult, bool, ScanResult>
			{
				{ ScanCompareType.Changed, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), true, new FloatScanResult(0.0f) },
				{ ScanCompareType.Changed, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.NotChanged, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), true, new FloatScanResult(1.0f) },
				{ ScanCompareType.NotChanged, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.Increased, 0.0f, 0.0f, BitConverter.GetBytes(2.0f), new FloatScanResult(1.0f), true, new FloatScanResult(2.0f) },
				{ ScanCompareType.Increased, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.Increased, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.IncreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(2.0f), new FloatScanResult(1.0f), true, new FloatScanResult(2.0f) },
				{ ScanCompareType.IncreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), true, new FloatScanResult(1.0f) },
				{ ScanCompareType.IncreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.Decreased, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), true, new FloatScanResult(0.0f) },
				{ ScanCompareType.Decreased, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.Decreased, 0.0f, 0.0f, BitConverter.GetBytes(2.0f), new FloatScanResult(1.0f), false, null },
				{ ScanCompareType.DecreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(0.0f), new FloatScanResult(1.0f), true, new FloatScanResult(0.0f) },
				{ ScanCompareType.DecreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(1.0f), new FloatScanResult(1.0f), true, new FloatScanResult(1.0f) },
				{ ScanCompareType.DecreasedOrEqual, 0.0f, 0.0f, BitConverter.GetBytes(2.0f), new FloatScanResult(1.0f), false, null }
			};

			var basicData = GetTestCompareBasicData();
			foreach (var x in basicData)
			{
				data.Add((ScanCompareType)x[0], (float)x[1], (float)x[2], (byte[])x[3], new FloatScanResult(1.0f), (bool)x[4], (ScanResult)x[5]);
			}

			return data;
		}

		[Theory]
		[MemberData(nameof(GetTestCompareWithPreviousData))]
		public void TestCompareWithPrevious(ScanCompareType compareType, float value1, float value2, byte[] data, ScanResult previousScanResult, bool expectedResult, ScanResult expectedScanResult)
		{
			var sut = new FloatMemoryComparer(compareType, ScanRoundMode.Normal, 1, value1, value2);

			Check.That(sut.Compare(data, 0, previousScanResult, out var scanResult)).IsEqualTo(expectedResult);
			Check.That(scanResult).IsEqualTo(expectedScanResult);
			if (scanResult != null)
			{
				Check.That(scanResult).IsInstanceOf<FloatScanResult>();
			}
		}

		[Fact]
		public void TestCompareWithPreviousThrows()
		{
			var sut = new FloatMemoryComparer(ScanCompareType.Unknown, ScanRoundMode.Normal, 1, 0, 0);

			Check.ThatCode(() => sut.Compare(BitConverter.GetBytes(0.0f), 0, new FloatScanResult(0.0f), out _)).Throws<InvalidCompareTypeException>();
		}
	}
}
