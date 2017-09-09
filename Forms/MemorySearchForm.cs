using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.MemorySearcher;
using ReClassNET.MemorySearcher.Comparer;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class MemorySearchForm : IconForm
	{
		private const int MaxVisibleResults = 1000;

		private bool isFirstScan;

		private SearchCompareType SelectedCompareType => (scanTypeComboBox.SelectedItem as EnumDescriptionDisplay<SearchCompareType>)?.Value ?? throw new InvalidOperationException();
		private SearchValueType SelectedValueType => (valueTypeComboBox.SelectedItem as EnumDescriptionDisplay<SearchValueType>)?.Value ?? throw new InvalidOperationException();

		public MemorySearchForm()
		{
			InitializeComponent();

			startAddressTextBox.Text = 0.ToString(Constants.StringHexFormat);
			endAddressTextBox.Text =
#if WIN64
				long.MaxValue.ToString(Constants.StringHexFormat);
#else
				int.MaxValue.ToString(Constants.StringHexFormat);
#endif

			valueTypeComboBox.DataSource = EnumDescriptionDisplay<SearchValueType>.Create();
			OnValueTypeChanged();

			Reset();
		}

		private void SetResultCount(int count)
		{
			resultCountLabel.Text = count > MaxVisibleResults ? $"Found: {count} (only {MaxVisibleResults} shown)" : $"Found: {count}";
		}

		private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			OnValueTypeChanged();
		}

		private void OnValueTypeChanged()
		{
			var valueType = SelectedValueType;
			if (valueType == SearchValueType.ArrayOfBytes || valueType == SearchValueType.String)
			{
				scanTypeComboBox.DataSource = EnumDescriptionDisplay<SearchCompareType>.CreateExact(SearchCompareType.Equal);
			}
			else
			{
				scanTypeComboBox.DataSource = isFirstScan
					? EnumDescriptionDisplay<SearchCompareType>.CreateExclude(
						SearchCompareType.Changed, SearchCompareType.NotChanged, SearchCompareType.Decreased, SearchCompareType.DecreasedOrEqual,
						SearchCompareType.Increased, SearchCompareType.IncreasedOrEqual
					)
					: EnumDescriptionDisplay<SearchCompareType>.CreateExclude(SearchCompareType.Unknown);
			}
		}

		private void Reset()
		{
			SetResultCount(0);
			resultDataGridView.DataSource = null;

			nextScanButton.Enabled = false;

			isFirstScan = true;
		}

		private void firstScanButton_Click(object sender, EventArgs e)
		{
			OnStartFirstScan();
		}

		private void OnStartFirstScan()
		{
			if (isFirstScan)
			{
				var settings = CreateSearchSettings();

				nextScanButton.Enabled = true;
				isFirstScan = false;
			}
			else
			{
				Reset();
			}
		}

		private void ParseInput(out long value1, out long value2, bool isHex)
		{
			value1 = value2 = 0;

			long.TryParse(valueDualValueControl.Value1, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out value1);
			long.TryParse(valueDualValueControl.Value2, isHex ? NumberStyles.HexNumber : NumberStyles.Integer, null, out value2);
		}

		private void ParseInput(out double value1, out double value2)
		{
			value1 = value2 = 0;

			double.TryParse(valueDualValueControl.Value1, out value1);
			double.TryParse(valueDualValueControl.Value2, out value2);
		}

		private SearchSettings CreateSearchSettings()
		{
			var valueType = SelectedValueType;
			var compareType = SelectedCompareType;

			var settings = new SearchSettings();

			if (valueType == SearchValueType.Byte || valueType == SearchValueType.Short || valueType == SearchValueType.Integer || valueType == SearchValueType.Long)
			{
				ParseInput(out var value1, out var value2, isHexCheckBox.Checked);

				switch (valueType)
				{
					case SearchValueType.Byte:
						settings.Comparer = new ByteMemoryComparer(compareType, (byte)value1, (byte)value2);
						break;
					case SearchValueType.Short:
						settings.Comparer = new ShortMemoryComparer(compareType, (short)value1, (short)value2);
						break;
					case SearchValueType.Integer:
						settings.Comparer = new IntegerMemoryComparer(compareType, (int)value1, (int)value2);
						break;
					case SearchValueType.Long:
						settings.Comparer = new LongMemoryComparer(compareType, value1, value2);
						break;
				}
			}
			else if (valueType == SearchValueType.Float || valueType == SearchValueType.Double)
			{
				ParseInput(out var value1, out var value2);

				var roundMode = roundStrictRadioButton.Checked ? SearchRoundMode.Strict : roundLooseRadioButton.Checked ? SearchRoundMode.Normal : SearchRoundMode.Truncate;

				switch (valueType)
				{
					case SearchValueType.Float:
						settings.Comparer = new FloatMemoryComparer(compareType, roundMode, (float)value1, (float)value2);
						break;
					case SearchValueType.Double:
						settings.Comparer = new DoubleMemoryComparer(compareType, roundMode, value1, value2);
						break;
				}
			}
			else if (valueType == SearchValueType.ArrayOfBytes)
			{
				var pattern = BytePattern.Parse(valueDualValueControl.Value1);

				settings.Comparer = new ArrayOfBytesMemoryComparer(pattern);
			}
			else if (valueType == SearchValueType.String)
			{
				var encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;

				settings.Comparer = new StringMemoryComparer(valueDualValueControl.Value1, encoding, caseSensitiveCheckBox.Checked);
			}

			if (settings.Comparer == null)
			{
				throw new Exception();
			}

			long.TryParse(startAddressTextBox.Text, NumberStyles.HexNumber, null, out var startAddressVar);
			long.TryParse(endAddressTextBox.Text, NumberStyles.HexNumber, null, out var endAddressVar);
#if WIN64
			settings.StartAddress = unchecked((IntPtr)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)endAddressVar);
#else
			settings.StartAddress = unchecked((IntPtr)(int)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)(int)endAddressVar);
#endif
			settings.FastScan = fastScanCheckBox.Checked;
			int.TryParse(fastScanAlignmentTextBox.Text, out var alignment);
			settings.FastScanAlignment = Math.Max(1, alignment);

			settings.SearchWritableMemory = CheckStateToSettingState(scanWritableCheckBox.CheckState);
			settings.SearchExecutableMemory = CheckStateToSettingState(scanExecutableCheckBox.CheckState);
			settings.SearchCopyOnWriteMemory = CheckStateToSettingState(scanCopyOnWriteCheckBox.CheckState);

			return settings;
		}

		private static SettingState CheckStateToSettingState(CheckState state)
		{
			switch (state)
			{
				case CheckState.Checked:
					return SettingState.Yes;
				case CheckState.Unchecked:
					return SettingState.No;
				default:
					return SettingState.Indeterminate;
			}
		}
	}
}
