using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace ReClassNET.MemoryScanner
{
	public abstract class ScanResult
	{
		public abstract ScanValueType ValueType { get; }

		public IntPtr Address { get; set; }
	}

	public class ByteScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Byte;

		public byte Value { get; }

		public ByteScanResult(byte value)
		{
			Value = value;
		}
	}

	public class ShortScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Short;

		public short Value { get; }

		public ShortScanResult(short value)
		{
			Value = value;
		}
	}

	public class IntegerScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Integer;

		public int Value { get; }

		public IntegerScanResult(int value)
		{
			Value = value;
		}
	}

	public class LongScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Long;

		public long Value { get; }

		public LongScanResult(long value)
		{
			Value = value;
		}
	}

	public class FloatScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Float;

		public float Value { get; }

		public FloatScanResult(float value)
		{
			Value = value;
		}
	}

	public class DoubleScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.Double;

		public double Value { get; }

		public DoubleScanResult(double value)
		{
			Value = value;
		}
	}

	public class ArrayOfBytesScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.ArrayOfBytes;

		public byte[] Value { get; }

		public ArrayOfBytesScanResult(byte[] value)
		{
			Contract.Requires(value != null);

			Value = value;
		}
	}

	public class StringScanResult : ScanResult
	{
		public override ScanValueType ValueType => ScanValueType.String;

		public string Value { get; }

		public Encoding Encoding { get; }

		public StringScanResult(string value, Encoding encoding)
		{
			Contract.Requires(value != null);
			Contract.Requires(encoding != null);

			Value = value;
			Encoding = encoding;
		}
	}
}
