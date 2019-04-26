using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.MemoryScanner
{
	public enum MemoryRecordAddressMode
	{
		Absolute,
		Relative,
		Unknown
	}

	public class MemoryRecord : INotifyPropertyChanged
	{
		private IntPtr addressOrOffset;
		private string moduleName;

		public MemoryRecordAddressMode AddressMode { get; set; }

		public IntPtr AddressOrOffset
		{
			get => addressOrOffset;
			set
			{
				addressOrOffset = value;
				AddressMode = MemoryRecordAddressMode.Unknown;
			}
		}

		public IntPtr RealAddress { get; private set; }
	
		public string AddressStr => RealAddress.ToString(Constants.AddressHexFormat);

		public string ModuleName
		{
			get => moduleName;
			set
			{
				moduleName = value;
				AddressMode = MemoryRecordAddressMode.Relative;
			}
		}
		public bool IsRelativeAddress => !string.IsNullOrEmpty(ModuleName);

		public string Description { get; set; } = string.Empty;
		public ScanValueType ValueType { get; set; }

		public string ValueStr { get; private set; }
		public string PreviousValueStr { get; }
		public bool HasChangedValue { get; private set; }

		public int ValueLength { get; set; }

		public Encoding Encoding { get; set; }

		public bool ShowValueHexadecimal { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public MemoryRecord()
		{

		}

		public MemoryRecord(ScanResult result)
		{
			Contract.Requires(result != null);

			addressOrOffset = result.Address;
			AddressMode = MemoryRecordAddressMode.Unknown;
			ValueType = result.ValueType;

			switch (ValueType)
			{
				case ScanValueType.Byte:
					ValueStr = FormatValue(((ByteScanResult)result).Value, false);
					break;
				case ScanValueType.Short:
					ValueStr = FormatValue(((ShortScanResult)result).Value, false);
					break;
				case ScanValueType.Integer:
					ValueStr = FormatValue(((IntegerScanResult)result).Value, false);
					break;
				case ScanValueType.Long:
					ValueStr = FormatValue(((LongScanResult)result).Value, false);
					break;
				case ScanValueType.Float:
					ValueStr = FormatValue(((FloatScanResult)result).Value);
					break;
				case ScanValueType.Double:
					ValueStr = FormatValue(((DoubleScanResult)result).Value);
					break;
				case ScanValueType.ArrayOfBytes:
					var byteData = ((ArrayOfBytesScanResult)result).Value;
					ValueLength = byteData.Length;
					ValueStr = FormatValue(byteData);
					break;
				case ScanValueType.String:
					var strResult = (StringScanResult)result;
					ValueLength = strResult.Value.Length;
					Encoding = strResult.Encoding;
					ValueStr = FormatValue(strResult.Value);
					break;
				default:
					throw new InvalidOperationException();
			}

			PreviousValueStr = ValueStr;
		}

		private void NotifyPropertyChanged(string propertyName)
		{
			var propertyChanged = PropertyChanged;
			propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void ResolveAddress(RemoteProcess process)
		{
			Contract.Requires(process != null);

			if (AddressMode == MemoryRecordAddressMode.Unknown)
			{
				RealAddress = addressOrOffset;

				var module = process.GetModuleToPointer(addressOrOffset);
				if (module != null)
				{
					addressOrOffset = addressOrOffset.Sub(module.Start);
					ModuleName = module.Name;

					AddressMode = MemoryRecordAddressMode.Relative;
				}
				else
				{
					AddressMode = MemoryRecordAddressMode.Absolute;
				}
			}
			else if (AddressMode == MemoryRecordAddressMode.Relative)
			{
				var module = process.GetModuleByName(ModuleName);
				if (module != null)
				{
					RealAddress = module.Start.Add(addressOrOffset);
				}
			}
		}

		public void RefreshValue(RemoteProcess process)
		{
			Contract.Requires(process != null);

			byte[] buffer;

			switch (ValueType)
			{
				case ScanValueType.Byte:
					buffer = new byte[1];
					break;
				case ScanValueType.Short:
					buffer = new byte[2];
					break;
				case ScanValueType.Integer:
				case ScanValueType.Float:
					buffer = new byte[4];
					break;
				case ScanValueType.Long:
				case ScanValueType.Double:
					buffer = new byte[8];
					break;
				case ScanValueType.ArrayOfBytes:
					buffer = new byte[ValueLength];
					break;
				case ScanValueType.String:
					buffer = new byte[ValueLength * Encoding.GuessByteCountPerChar()];
					break;
				default:
					throw new InvalidOperationException();
			}

			if (process.ReadRemoteMemoryIntoBuffer(RealAddress, ref buffer))
			{
				switch (ValueType)
				{
					case ScanValueType.Byte:
						ValueStr = FormatValue(buffer[0], ShowValueHexadecimal);
						break;
					case ScanValueType.Short:
						ValueStr = FormatValue(BitConverter.ToInt16(buffer, 0), ShowValueHexadecimal);
						break;
					case ScanValueType.Integer:
						ValueStr = FormatValue(BitConverter.ToInt32(buffer, 0), ShowValueHexadecimal);
						break;
					case ScanValueType.Long:
						ValueStr = FormatValue(BitConverter.ToInt64(buffer, 0), ShowValueHexadecimal);
						break;
					case ScanValueType.Float:
						ValueStr = FormatValue(BitConverter.ToSingle(buffer, 0));
						break;
					case ScanValueType.Double:
						ValueStr = FormatValue(BitConverter.ToDouble(buffer, 0));
						break;
					case ScanValueType.ArrayOfBytes:
						ValueStr = FormatValue(buffer);
						break;
					case ScanValueType.String:
						ValueStr = FormatValue(Encoding.GetString(buffer));
						break;
				}
			}
			else
			{
				ValueStr = "???";
			}

			HasChangedValue = ValueStr != PreviousValueStr;

			NotifyPropertyChanged(nameof(ValueStr));
		}

		public void SetValue(RemoteProcess process, string input, bool isHex)
		{
			Contract.Requires(process != null);
			Contract.Requires(input != null);

			byte[] data = null;

			if (ValueType == ScanValueType.Byte || ValueType == ScanValueType.Short || ValueType == ScanValueType.Integer || ValueType == ScanValueType.Long)
			{
				var numberStyle = isHex ? NumberStyles.HexNumber : NumberStyles.Integer;
				long.TryParse(input, numberStyle, null, out var value);

				switch (ValueType)
				{
					case ScanValueType.Byte:
						data = BitConverter.GetBytes((byte)value);
						break;
					case ScanValueType.Short:
						data = BitConverter.GetBytes((short)value);
						break;
					case ScanValueType.Integer:
						data = BitConverter.GetBytes((int)value);
						break;
					case ScanValueType.Long:
						data = BitConverter.GetBytes(value);
						break;
				}
			}
			else if (ValueType == ScanValueType.Float || ValueType == ScanValueType.Double)
			{
				var nf = NumberFormat.GuessNumberFormat(input);
				double.TryParse(input, NumberStyles.Float, nf, out var value);

				switch (ValueType)
				{
					case ScanValueType.Float:
						data = BitConverter.GetBytes((float)value);
						break;
					case ScanValueType.Double:
						data = BitConverter.GetBytes(value);
						break;
				}
			}

			if (data != null)
			{
				process.WriteRemoteMemory(RealAddress, data);

				RefreshValue(process);
			}
		}

		private static string FormatValue(byte value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(short value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(int value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(long value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(float value) => value.ToString("0.0000");
		private static string FormatValue(double value) => value.ToString("0.0000");
		private static string FormatValue(byte[] value) => HexadecimalFormatter.ToString(value);
		private static string FormatValue(string value) => value;
	}
}
