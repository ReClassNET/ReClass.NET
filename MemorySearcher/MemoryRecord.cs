using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using ReClassNET.Memory;
using ReClassNET.Util;

namespace ReClassNET.MemorySearcher
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
		private IntPtr realAddress;
		private string moduleName;

		public MemoryRecordAddressMode AddressMode { get; set; }

		public IntPtr Address
		{
			get => addressOrOffset;
			set
			{
				addressOrOffset = value;
				AddressMode = MemoryRecordAddressMode.Unknown;
			}
		}
	
		public string AddressStr => realAddress.ToString(Constants.StringHexFormat);

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
		public SearchValueType ValueType { get; set; }

		public string ValueStr { get; private set; }
		public string PreviousValueStr { get; }

		public int ValueLength { get; set; }

		public Encoding Encoding { get; set; }

		public bool ShowValueHexadecimal { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public MemoryRecord()
		{

		}

		public MemoryRecord(SearchResult result)
		{
			Contract.Requires(result != null);

			addressOrOffset = result.Address;
			AddressMode = MemoryRecordAddressMode.Unknown;
			ValueType = result.ValueType;

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
					var byteData = ((ArrayOfBytesSearchResult)result).Value;
					ValueLength = byteData.Length;
					ValueStr = FormatValue(byteData);
					break;
				case SearchValueType.String:
					var strResult = (StringSearchResult)result;
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
				realAddress = addressOrOffset;

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
					realAddress = module.Start.Add(addressOrOffset);
				}
			}
		}

		public void RefreshValue(RemoteProcess process)
		{
			Contract.Requires(process != null);

			byte[] buffer;

			switch (ValueType)
			{
				case SearchValueType.Byte:
					buffer = new byte[1];
					break;
				case SearchValueType.Short:
					buffer = new byte[2];
					break;
				case SearchValueType.Integer:
				case SearchValueType.Float:
					buffer = new byte[4];
					break;
				case SearchValueType.Long:
				case SearchValueType.Double:
					buffer = new byte[8];
					break;
				case SearchValueType.ArrayOfBytes:
					buffer = new byte[ValueLength];
					break;
				case SearchValueType.String:
					buffer = new byte[ValueLength * Encoding.GetSimpleByteCountPerChar()];
					break;
				default:
					throw new InvalidOperationException();
			}

			if (process.ReadRemoteMemoryIntoBuffer(realAddress, ref buffer))
			{
				switch (ValueType)
				{
					case SearchValueType.Byte:
						ValueStr = FormatValue(buffer[0], ShowValueHexadecimal);
						break;
					case SearchValueType.Short:
						ValueStr = FormatValue(BitConverter.ToInt16(buffer, 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Integer:
						ValueStr = FormatValue(BitConverter.ToInt32(buffer, 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Long:
						ValueStr = FormatValue(BitConverter.ToInt64(buffer, 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Float:
						ValueStr = FormatValue(BitConverter.ToSingle(buffer, 0));
						break;
					case SearchValueType.Double:
						ValueStr = FormatValue(BitConverter.ToDouble(buffer, 0));
						break;
					case SearchValueType.ArrayOfBytes:
						ValueStr = FormatValue(buffer);
						break;
					case SearchValueType.String:
						ValueStr = FormatValue(Encoding.GetString(buffer));
						break;
				}
			}
			else
			{
				ValueStr = "???";
			}

			NotifyPropertyChanged(nameof(ValueStr));
		}

		public void SetValue(RemoteProcess process, string input, bool isHex)
		{
			Contract.Requires(process != null);
			Contract.Requires(input != null);

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
				process.WriteRemoteMemory(realAddress, data);

				RefreshValue(process);
			}
		}

		private static string FormatValue(byte value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(short value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(int value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(long value, bool showAsHex) => showAsHex ? value.ToString("X") : value.ToString();
		private static string FormatValue(float value) => value.ToString(CultureInfo.InvariantCulture);
		private static string FormatValue(double value) => value.ToString(CultureInfo.InvariantCulture);
		private static string FormatValue(byte[] value) => Utils.ByteArrayToHexString(value);
		private static string FormatValue(string value) => value;
	}
}
