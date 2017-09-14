using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using ReClassNET.Memory;
using ReClassNET.MemorySearcher;

namespace ReClassNET.UI
{
	public partial class MemorySearchResultControl
	{
		private class ResultData : INotifyPropertyChanged
		{
			public string Description { get; set; } = string.Empty;
			public string Address => Result.Address.ToString(Constants.StringHexFormat);
			public string ValueType => Result.ValueType.ToString();
			public string Value { get; private set; }
			public string Previous { get; }

			public bool ShowValueHexadecimal { get; set; }

			public SearchResult Result { get; }

			public event PropertyChangedEventHandler PropertyChanged;

			public ResultData(SearchResult result)
			{
				Contract.Requires(result != null);

				Result = result;
				Previous = Value = FormatValue();
			}

			private void NotifyPropertyChanged(string propertyName = null)
			{
				if (propertyName != null)
				{
					var propertyChanged = PropertyChanged;
					propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
				}
			}

			public void UpdateValue(RemoteProcess process)
			{
				Contract.Requires(process != null);

				var address = Result.Address;

				switch (Result.ValueType)
				{
					case SearchValueType.Byte:
						Value = FormatValue(process.ReadRemoteMemory(address, 1)[0], ShowValueHexadecimal);
						break;
					case SearchValueType.Short:
						Value = FormatValue(BitConverter.ToInt16(process.ReadRemoteMemory(address, 2), 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Integer:
						Value = FormatValue(BitConverter.ToInt32(process.ReadRemoteMemory(address, 4), 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Long:
						Value = FormatValue(BitConverter.ToInt64(process.ReadRemoteMemory(address, 8), 0), ShowValueHexadecimal);
						break;
					case SearchValueType.Float:
						Value = FormatValue(BitConverter.ToSingle(process.ReadRemoteMemory(address, 4), 0));
						break;
					case SearchValueType.Double:
						Value = FormatValue(BitConverter.ToDouble(process.ReadRemoteMemory(address, 8), 0));
						break;
					case SearchValueType.ArrayOfBytes:
					case SearchValueType.String:
						return;
				}

				NotifyPropertyChanged(nameof(Value));
			}

			private string FormatValue()
			{
				Contract.Requires(Result != null);

				switch (Result.ValueType)
				{
					case SearchValueType.Byte:
						return FormatValue(((ByteSearchResult)Result).Value, ShowValueHexadecimal);
					case SearchValueType.Short:
						return FormatValue(((ShortSearchResult)Result).Value, ShowValueHexadecimal);
					case SearchValueType.Integer:
						return FormatValue(((IntegerSearchResult)Result).Value, ShowValueHexadecimal);
					case SearchValueType.Long:
						return FormatValue(((LongSearchResult)Result).Value, ShowValueHexadecimal);
					case SearchValueType.Float:
						return FormatValue(((FloatSearchResult)Result).Value);
					case SearchValueType.Double:
						return FormatValue(((DoubleSearchResult)Result).Value);
					case SearchValueType.ArrayOfBytes:
						return FormatValue((byte[])null);
					case SearchValueType.String:
						return FormatValue(((StringSearchResult)Result).Value);
					default:
						throw new InvalidOperationException();
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
}
