using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.MemorySearcher;
using ReClassNET.MemorySearcher.Comparer;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class MemorySearchForm : IconForm
	{
		private const int MaxVisibleResults = 10000;

		private readonly RemoteProcess process;

		private bool isFirstScan;

		private Searcher searcher;

		private SearchCompareType SelectedCompareType => (scanTypeComboBox.SelectedItem as EnumDescriptionDisplay<SearchCompareType>)?.Value ?? throw new InvalidOperationException();
		private SearchValueType SelectedValueType => (valueTypeComboBox.SelectedItem as EnumDescriptionDisplay<SearchValueType>)?.Value ?? throw new InvalidOperationException();

		public MemorySearchForm(RemoteProcess process)
		{
			Contract.Requires(process != null);

			this.process = process;

			InitializeComponent();

			resultDataGridView.AutoGenerateColumns = false;

			startAddressTextBox.Text = 0.ToString(Constants.StringHexFormat);
			endAddressTextBox.Text =
#if RECLASSNET64
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

		private void ShowResults()
		{
			Contract.Requires(searcher != null);

			SetResultCount(searcher.TotalResultCount);

			var dt = new DataTable();
			var addressColumn = dt.Columns.Add("address", typeof(string));
			var valueColumn = dt.Columns.Add("value", typeof(string));
			var previousColumn = dt.Columns.Add("previous", typeof(string));
			var dataColumn = dt.Columns.Add("data", typeof(SearchResult));

			foreach (var result in searcher.GetResults().Take(MaxVisibleResults).OrderBy(r => r.Address, IntPtrComparer.Instance))
			{
				var row = dt.NewRow();
				row[addressColumn] = result.Address.ToString(Constants.StringHexFormat);
				row[valueColumn] = row[previousColumn] = FormatResultValue(result, false);
				row[dataColumn] = result;
				dt.Rows.Add(row);
			}

			resultDataGridView.DataSource = dt;
		}

		private static string FormatResultValue(SearchResult result, bool showAsHex)
		{
			switch (result)
			{
				case ByteSearchResult sr:
					return showAsHex ? sr.Value.ToString("X") : sr.Value.ToString();
				case ShortSearchResult sr:
					return showAsHex ? sr.Value.ToString("X") : sr.Value.ToString();
				case IntegerSearchResult sr:
					return showAsHex ? sr.Value.ToString("X") : sr.Value.ToString();
				case LongSearchResult sr:
					return showAsHex ? sr.Value.ToString("X") : sr.Value.ToString();
				case FloatSearchResult sr:
					return sr.Value.ToString(CultureInfo.InvariantCulture);
				case DoubleSearchResult sr:
					return sr.Value.ToString(CultureInfo.InvariantCulture);
				case ArrayOfBytesSearchResult sr:
					return "[...]";
				case StringSearchResult sr:
					return "[...]";
				default:
					throw new InvalidOperationException();
			}
		}

		private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			OnValueTypeChanged();
		}

		private void OnValueTypeChanged()
		{
			SetValidCompareTypes();

			var valueType = SelectedValueType;

			int alignment = 1;
			switch (valueType)
			{
				case SearchValueType.Short:
					alignment = 2;
					break;
				case SearchValueType.Integer:
				case SearchValueType.Long:
				case SearchValueType.Float:
				case SearchValueType.Double:
					alignment = 4;
					break;
			}
			fastScanAlignmentTextBox.Text = alignment.ToString();

			floatingOptionsGroupBox.Visible = valueType == SearchValueType.Float || valueType == SearchValueType.Double;
			stringOptionsGroupBox.Visible = valueType == SearchValueType.String;
		}

		private void SetValidCompareTypes()
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
			searcher?.Dispose();
			searcher = null;

			SetResultCount(0);
			resultDataGridView.DataSource = null;

			nextScanButton.Enabled = false;
			valueTypeComboBox.Enabled = true;

			floatingOptionsGroupBox.Enabled = true;
			stringOptionsGroupBox.Enabled = true;
			scanOptionsGroupBox.Enabled = true;

			isFirstScan = true;

			SetValidCompareTypes();
		}

		private async void firstScanButton_Click(object sender, EventArgs e)
		{
			await OnStartFirstScan();
		}

		private async void nextScanButton_Click(object sender, EventArgs e)
		{
			await OnStartSecondScan();
		}

		private async Task OnStartFirstScan()
		{
			if (isFirstScan)
			{
				firstScanButton.Enabled = false;

				var settings = CreateSearchSettings();
				var comparer = CreateComparer(settings);
				searcher = new Searcher(process, settings);

				var report = new Progress<int>(i => scanProgressBar.Value = i);
				var completed = await searcher.Search(comparer, CancellationToken.None, report);

				if (completed)
				{
					ShowResults();
				}

				scanProgressBar.Value = 0;
				firstScanButton.Enabled = true;
				nextScanButton.Enabled = true;
				valueTypeComboBox.Enabled = false;

				floatingOptionsGroupBox.Enabled = false;
				stringOptionsGroupBox.Enabled = false;
				scanOptionsGroupBox.Enabled = false;

				isFirstScan = false;

				SetValidCompareTypes();
			}
			else
			{
				Reset();
			}
		}

		private async Task OnStartSecondScan()
		{
			if (!isFirstScan)
			{
				firstScanButton.Enabled = false;
				nextScanButton.Enabled = false;

				var comparer = CreateComparer(searcher.Settings);

				var report = new Progress<int>(i => scanProgressBar.Value = i);
				var completed = await searcher.Search(comparer, CancellationToken.None, report);

				if (completed)
				{
					ShowResults();
				}

				scanProgressBar.Value = 0;
				firstScanButton.Enabled = true;
				nextScanButton.Enabled = true;
			}
		}

		private SearchSettings CreateSearchSettings()
		{
			var settings = new SearchSettings
			{
				ValueType = SelectedValueType
			};

			long.TryParse(startAddressTextBox.Text, NumberStyles.HexNumber, null, out var startAddressVar);
			long.TryParse(endAddressTextBox.Text, NumberStyles.HexNumber, null, out var endAddressVar);
#if RECLASSNET64
			settings.StartAddress = unchecked((IntPtr)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)endAddressVar);
#else
			settings.StartAddress = unchecked((IntPtr)(int)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)(int)endAddressVar);
#endif
			settings.FastScan = fastScanCheckBox.Checked;
			int.TryParse(fastScanAlignmentTextBox.Text, out var alignment);
			settings.FastScanAlignment = Math.Max(1, alignment);

			SettingState CheckStateToSettingState(CheckState state)
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

			settings.SearchWritableMemory = CheckStateToSettingState(scanWritableCheckBox.CheckState);
			settings.SearchExecutableMemory = CheckStateToSettingState(scanExecutableCheckBox.CheckState);
			settings.SearchCopyOnWriteMemory = CheckStateToSettingState(scanCopyOnWriteCheckBox.CheckState);

			return settings;
		}

		private IMemoryComparer CreateComparer(SearchSettings settings)
		{
			Contract.Requires(settings != null);

			var compareType = SelectedCompareType;

			if (settings.ValueType == SearchValueType.Byte || settings.ValueType == SearchValueType.Short || settings.ValueType == SearchValueType.Integer || settings.ValueType == SearchValueType.Long)
			{
				var numberStyle = isHexCheckBox.Checked ? NumberStyles.HexNumber : NumberStyles.Integer;
				long.TryParse(valueDualValueControl.Value1, numberStyle, null, out var value1);
				long.TryParse(valueDualValueControl.Value2, numberStyle, null, out var value2);

				switch (settings.ValueType)
				{
					case SearchValueType.Byte:
						return new ByteMemoryComparer(compareType, (byte)value1, (byte)value2);
					case SearchValueType.Short:
						return new ShortMemoryComparer(compareType, (short)value1, (short)value2);
					case SearchValueType.Integer:
						return new IntegerMemoryComparer(compareType, (int)value1, (int)value2);
					case SearchValueType.Long:
						return new LongMemoryComparer(compareType, value1, value2);
				}
			}
			else if (settings.ValueType == SearchValueType.Float || settings.ValueType == SearchValueType.Double)
			{
				double.TryParse(valueDualValueControl.Value1, out var value1);
				double.TryParse(valueDualValueControl.Value2, out var value2);

				var roundMode = roundStrictRadioButton.Checked ? SearchRoundMode.Strict : roundLooseRadioButton.Checked ? SearchRoundMode.Normal : SearchRoundMode.Truncate;

				switch (settings.ValueType)
				{
					case SearchValueType.Float:
						return new FloatMemoryComparer(compareType, roundMode, (float)value1, (float)value2);
					case SearchValueType.Double:
						return new DoubleMemoryComparer(compareType, roundMode, value1, value2);
				}
			}
			else if (settings.ValueType == SearchValueType.ArrayOfBytes)
			{
				var pattern = BytePattern.Parse(valueDualValueControl.Value1);

				return new ArrayOfBytesMemoryComparer(pattern);
			}
			else if (settings.ValueType == SearchValueType.String)
			{
				var encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;

				return new StringMemoryComparer(valueDualValueControl.Value1, encoding, caseSensitiveCheckBox.Checked);
			}

			throw new Exception();
		}

		private void MemorySearchForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			searcher?.Dispose();
		}
	}
}
