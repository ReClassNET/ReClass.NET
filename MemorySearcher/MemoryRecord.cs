using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
{
	public class MemoryRecord
	{
		private readonly RemoteProcess process;

		private IntPtr address;
		private SearchValueType valueType;

		public IntPtr Address
		{
			get => HasScanResultAttached ? Result.Address : address;
			set { if (!HasScanResultAttached) { address = value; } }
		}
		public string AddressStr => Result.Address.ToString(Constants.StringHexFormat);
		public string Description { get; set; } = string.Empty;
		public SearchValueType ValueType
		{
			get => HasScanResultAttached ? Result.ValueType : valueType;
			set { if (!HasScanResultAttached) { valueType = value; } }
		}

		public string ValueStr { get; private set; }
		public string PreviousValueStr { get; }

		public bool ShowValueHexadecimal { get; set; }

		public bool HasScanResultAttached => Result != null;

		public SearchResult Result { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public MemoryRecord(RemoteProcess process)
		{
			Contract.Requires(process != null);

			this.process = process;
		}

		public MemoryRecord(SearchResult result, RemoteProcess process)
			: this(process)
		{
			Contract.Requires(result != null);
			Contract.Requires(process != null);

			Result = result;

			switch (ValueType)
			{
				case SearchValueType.Byte:
					ValueStr = FormatValue(((ByteSearchResult)result).Value, false);
					break;
				case SearchValueType.Short:
					ValueStr = FormatValue(((ShortSearchResult)result).Value, false);
					break;
				case SearchValueType.Integer:
					ValueStr = FormatValue(((IntegerSearchResult)result).Value, false);
					break;
				case SearchValueType.Long:
					ValueStr = FormatValue(((LongSearchResult)result).Value, false);
					break;
				case SearchValueType.Float:
					ValueStr = FormatValue(((FloatSearchResult)result).Value);
					break;
				case SearchValueType.Double:
					ValueStr = FormatValue(((DoubleSearchResult)result).Value);
					break;
				case SearchValueType.ArrayOfBytes:
					ValueStr = FormatValue((byte[])null);
					break;
				case SearchValueType.String:
					ValueStr = FormatValue(((StringSearchResult)result).Value);
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

		public void RefreshValue()
		{
			switch (ValueType)
			{
				case SearchValueType.Byte:
					ValueStr = FormatValue(process.ReadRemoteMemory(Address, 1)[0], ShowValueHexadecimal);
					break;
				case SearchValueType.Short:
					ValueStr = FormatValue(BitConverter.ToInt16(process.ReadRemoteMemory(Address, 2), 0), ShowValueHexadecimal);
					break;
				case SearchValueType.Integer:
					ValueStr = FormatValue(BitConverter.ToInt32(process.ReadRemoteMemory(Address, 4), 0), ShowValueHexadecimal);
					break;
				case SearchValueType.Long:
					ValueStr = FormatValue(BitConverter.ToInt64(process.ReadRemoteMemory(Address, 8), 0), ShowValueHexadecimal);
					break;
				case SearchValueType.Float:
					ValueStr = FormatValue(BitConverter.ToSingle(process.ReadRemoteMemory(Address, 4), 0));
					break;
				case SearchValueType.Double:
					ValueStr = FormatValue(BitConverter.ToDouble(process.ReadRemoteMemory(Address, 8), 0));
					break;
				case SearchValueType.ArrayOfBytes:
				case SearchValueType.String:
					return;
			}

			NotifyPropertyChanged(nameof(ValueStr));
		}

		public void SetValue(string input, bool isHex)
		{
			byte[] data = null;

			if (ValueType == SearchValueType.Byte || ValueType == SearchValueType.Short || ValueType == SearchValueType.Integer || ValueType == SearchValueType.Long)
			{
				var numberStyle = isHex ? NumberStyles.HexNumber : NumberStyles.Integer;
				long.TryParse(input, numberStyle, null, out var value);

				switch (ValueType)
				{
					case SearchValueType.Byte:
						data = BitConverter.GetBytes((byte)value);
						break;
					case SearchValueType.Short:
						data = BitConverter.GetBytes((short)value);
						break;
					case SearchValueType.Integer:
						data = BitConverter.GetBytes((int)value);
						break;
					case SearchValueType.Long:
						data = BitConverter.GetBytes(value);
						break;
				}
			}
			else if (ValueType == SearchValueType.Float || ValueType == SearchValueType.Double)
			{
				var nf = Utils.GuessNumberFormat(input);
				double.TryParse(input, NumberStyles.Float, nf, out var value);

				switch (ValueType)
				{
					case SearchValueType.Float:
						data = BitConverter.GetBytes((float)value);
						break;
					case SearchValueType.Double:
						data = BitConverter.GetBytes(value);
						break;
				}
			}

			if (data != null)
			{
				process.WriteRemoteMemory(Address, data);

				RefreshValue();
			}
		}

		private static string FormatValue(byte value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(short value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(int value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(long value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(float value) => value.ToString(CultureInfo.InvariantCulture);
		private static string FormatValue(double value) => value.ToString(CultureInfo.InvariantCulture);
		private static string FormatValue(byte[] value) => "[...]";
		private static string FormatValue(string value) => $"\"{value}\"";
	}
}
