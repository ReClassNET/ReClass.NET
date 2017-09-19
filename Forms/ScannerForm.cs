using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.DataExchange.Scanner;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.MemorySearcher;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class ScannerForm : IconForm
	{
		private const int MaxVisibleResults = 10000;

		private bool isFirstScan;

		private Scanner searcher;

		private ScanCompareType SelectedCompareType => (scanTypeComboBox.SelectedItem as EnumDescriptionDisplay<ScanCompareType>)?.Value ?? throw new InvalidOperationException();
		private ScanValueType SelectedValueType => (valueTypeComboBox.SelectedItem as EnumDescriptionDisplay<ScanValueType>)?.Value ?? throw new InvalidOperationException();

		public ScannerForm()
		{
			InitializeComponent();

			toolStripPanel.RenderMode = ToolStripRenderMode.Professional;
			toolStripPanel.Renderer = new CustomToolStripProfessionalRenderer(true, false);
			menuToolStrip.Renderer = new CustomToolStripProfessionalRenderer(false, false);

			startAddressTextBox.Text = 0.ToString(Constants.StringHexFormat);
			endAddressTextBox.Text =
#if RECLASSNET64
				long.MaxValue.ToString(Constants.StringHexFormat);
#else
				int.MaxValue.ToString(Constants.StringHexFormat);
#endif

			valueTypeComboBox.DataSource = EnumDescriptionDisplay<ScanValueType>.Create();
			OnValueTypeChanged();

			Reset();

			firstScanButton.Enabled = flowLayoutPanel.Enabled = Program.RemoteProcess.IsValid;

			Program.RemoteProcess.ProcessAttached += RemoteProcessOnProcessAttached;
			Program.RemoteProcess.ProcessClosing += RemoteProcessOnProcessClosing;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			base.OnFormClosed(e);

			GlobalWindowManager.RemoveWindow(this);
		}

		#region Event Handler

		private void RemoteProcessOnProcessAttached(RemoteProcess remoteProcess)
		{
			firstScanButton.Enabled = nextScanButton.Enabled = flowLayoutPanel.Enabled = true;

			Reset();

			if (addressListMemoryRecordList.Records.Any())
			{
				if (MessageBox.Show("Keep the current address list?", "Process has changed", MessageBoxButtons.YesNo) != DialogResult.Yes)
				{
					addressListMemoryRecordList.Clear();
				}
				else
				{
					foreach (var record in addressListMemoryRecordList.Records)
					{
						record.ResolveAddress(Program.RemoteProcess);
						record.RefreshValue(Program.RemoteProcess);
					}
				}
			}
		}

		private void RemoteProcessOnProcessClosing(RemoteProcess remoteProcess)
		{
			Reset();

			firstScanButton.Enabled = nextScanButton.Enabled = flowLayoutPanel.Enabled = false;
		}

		private void MemorySearchForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			searcher?.Dispose();

			Program.RemoteProcess.ProcessAttached -= RemoteProcessOnProcessAttached;
			Program.RemoteProcess.ProcessClosing -= RemoteProcessOnProcessClosing;
		}

		private void updateValuesTimer_Tick(object sender, EventArgs e)
		{
			resultMemoryRecordList.RefreshValues(Program.RemoteProcess);
			addressListMemoryRecordList.RefreshValues(Program.RemoteProcess);
		}

		private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			OnValueTypeChanged();
		}

		private async void firstScanButton_Click(object sender, EventArgs e)
		{
			await OnStartFirstScan();
		}

		private async void nextScanButton_Click(object sender, EventArgs e)
		{
			await OnStartSecondScan();
		}

		#endregion

		private void SetResultCount(int count)
		{
			resultCountLabel.Text = count > MaxVisibleResults ? $"Found: {count} (only {MaxVisibleResults} shown)" : $"Found: {count}";
		}

		private void ShowScannerResults()
		{
			Contract.Requires(searcher != null);

			SetResultCount(searcher.TotalResultCount);

			resultMemoryRecordList.SetRecords(
				searcher.GetResults()
					.Take(MaxVisibleResults)
					.OrderBy(r => r.Address, IntPtrComparer.Instance)
					.Select(r =>
					{
						var record = new MemoryRecord(r);
						record.ResolveAddress(Program.RemoteProcess);
						return record;
					})
			);
		}

		private void OnValueTypeChanged()
		{
			SetValidCompareTypes();

			isHexCheckBox.Enabled = true;

			var valueType = SelectedValueType;

			switch (valueType)
			{
				case ScanValueType.Byte:
				case ScanValueType.Short:
				case ScanValueType.Integer:
				case ScanValueType.Long:
					isHexCheckBox.Enabled = true;
					break;
				case ScanValueType.Float:
				case ScanValueType.Double:
				case ScanValueType.ArrayOfBytes:
				case ScanValueType.String:
					isHexCheckBox.Checked = false;
					isHexCheckBox.Enabled = false;
					break;
			}

			var alignment = 1;
			switch (valueType)
			{
				case ScanValueType.Short:
					alignment = 2;
					break;
				case ScanValueType.Float:
				case ScanValueType.Double:
				case ScanValueType.Integer:
				case ScanValueType.Long:
					alignment = 4;
					break;
			}
			fastScanAlignmentTextBox.Text = alignment.ToString();

			floatingOptionsGroupBox.Visible = valueType == ScanValueType.Float || valueType == ScanValueType.Double;
			stringOptionsGroupBox.Visible = valueType == ScanValueType.String;
		}

		private void SetValidCompareTypes()
		{
			var valueType = SelectedValueType;
			if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String)
			{
				scanTypeComboBox.DataSource = EnumDescriptionDisplay<ScanCompareType>.CreateExact(ScanCompareType.Equal);
			}
			else
			{
				scanTypeComboBox.DataSource = isFirstScan
					? EnumDescriptionDisplay<ScanCompareType>.CreateExclude(
						ScanCompareType.Changed, ScanCompareType.NotChanged, ScanCompareType.Decreased, ScanCompareType.DecreasedOrEqual,
						ScanCompareType.Increased, ScanCompareType.IncreasedOrEqual
					)
					: EnumDescriptionDisplay<ScanCompareType>.CreateExclude(ScanCompareType.Unknown);
			}
		}

		private void Reset()
		{
			searcher?.Dispose();
			searcher = null;

			SetResultCount(0);
			resultMemoryRecordList.SetRecords(null);

			firstScanButton.Enabled = true;
			nextScanButton.Enabled = false;

			isHexCheckBox.Enabled = true;
			isHexCheckBox.Checked = false;

			valueTypeComboBox.Enabled = true;
			valueTypeComboBox.SelectedItem = valueTypeComboBox.Items.Cast<EnumDescriptionDisplay<ScanValueType>>().FirstOrDefault(e => e.Value == ScanValueType.Integer);
			OnValueTypeChanged();

			floatingOptionsGroupBox.Enabled = true;
			stringOptionsGroupBox.Enabled = true;
			scanOptionsGroupBox.Enabled = true;

			isFirstScan = true;

			SetValidCompareTypes();
		}

		private async Task OnStartFirstScan()
		{
			if (isFirstScan)
			{
				firstScanButton.Enabled = false;

				var settings = CreateSearchSettings();
				var comparer = CreateComparer(settings);
				searcher = new Scanner(Program.RemoteProcess, settings);

				var report = new Progress<int>(i => scanProgressBar.Value = i);
				var completed = await searcher.Search(comparer, CancellationToken.None, report);

				if (completed)
				{
					ShowScannerResults();

					firstScanButton.Enabled = true;
					nextScanButton.Enabled = true;
					valueTypeComboBox.Enabled = false;

					floatingOptionsGroupBox.Enabled = false;
					stringOptionsGroupBox.Enabled = false;
					scanOptionsGroupBox.Enabled = false;

					isFirstScan = false;

					SetValidCompareTypes();
				}

				scanProgressBar.Value = 0;

				return;
			}

			Reset();
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
					ShowScannerResults();
				}

				scanProgressBar.Value = 0;
				firstScanButton.Enabled = true;
				nextScanButton.Enabled = true;
			}
		}

		private ScanSettings CreateSearchSettings()
		{
			var settings = new ScanSettings
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

			settings.ScanWritableMemory = CheckStateToSettingState(scanWritableCheckBox.CheckState);
			settings.ScanExecutableMemory = CheckStateToSettingState(scanExecutableCheckBox.CheckState);
			settings.ScanCopyOnWriteMemory = CheckStateToSettingState(scanCopyOnWriteCheckBox.CheckState);

			return settings;
		}

		private IScanComparer CreateComparer(ScanSettings settings)
		{
			Contract.Requires(settings != null);

			var compareType = SelectedCompareType;

			if (settings.ValueType == ScanValueType.Byte || settings.ValueType == ScanValueType.Short || settings.ValueType == ScanValueType.Integer || settings.ValueType == ScanValueType.Long)
			{
				var numberStyle = isHexCheckBox.Checked ? NumberStyles.HexNumber : NumberStyles.Integer;
				long.TryParse(dualValueBox.Value1, numberStyle, null, out var value1);
				long.TryParse(dualValueBox.Value2, numberStyle, null, out var value2);

				switch (settings.ValueType)
				{
					case ScanValueType.Byte:
						return new ByteMemoryComparer(compareType, (byte)value1, (byte)value2);
					case ScanValueType.Short:
						return new ShortMemoryComparer(compareType, (short)value1, (short)value2);
					case ScanValueType.Integer:
						return new IntegerMemoryComparer(compareType, (int)value1, (int)value2);
					case ScanValueType.Long:
						return new LongMemoryComparer(compareType, value1, value2);
				}
			}
			else if (settings.ValueType == ScanValueType.Float || settings.ValueType == ScanValueType.Double)
			{
				int CalculateSignificantDigits(string input, NumberFormatInfo numberFormat)
				{
					Contract.Requires(input != null);
					Contract.Requires(numberFormat != null);

					var digits = 0;

					var decimalIndex = input.IndexOf(numberFormat.NumberDecimalSeparator, StringComparison.Ordinal);
					if (decimalIndex != -1)
					{
						digits = input.Length - 1 - decimalIndex;
					}

					return digits;
				}

				var nf1 = Utils.GuessNumberFormat(dualValueBox.Value1);
				double.TryParse(dualValueBox.Value1, NumberStyles.Float, nf1, out var value1);
				var nf2 = Utils.GuessNumberFormat(dualValueBox.Value2);
				double.TryParse(dualValueBox.Value2, NumberStyles.Float, nf2, out var value2);

				var significantDigits = Math.Max(
					CalculateSignificantDigits(dualValueBox.Value1, nf1),
					CalculateSignificantDigits(dualValueBox.Value2, nf2)
				);

				var roundMode = roundStrictRadioButton.Checked ? ScanRoundMode.Strict : roundLooseRadioButton.Checked ? ScanRoundMode.Normal : ScanRoundMode.Truncate;

				switch (settings.ValueType)
				{
					case ScanValueType.Float:
						return new FloatMemoryComparer(compareType, roundMode, significantDigits, (float)value1, (float)value2);
					case ScanValueType.Double:
						return new DoubleMemoryComparer(compareType, roundMode, significantDigits, value1, value2);
				}
			}
			else if (settings.ValueType == ScanValueType.ArrayOfBytes)
			{
				var pattern = BytePattern.Parse(dualValueBox.Value1);

				return new ArrayOfBytesMemoryComparer(pattern);
			}
			else if (settings.ValueType == ScanValueType.String)
			{
				var encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;

				return new StringMemoryComparer(dualValueBox.Value1, encoding, caseSensitiveCheckBox.Checked);
			}

			throw new Exception();
		}

		private void clearAddressListToolStripButton_Click(object sender, EventArgs e)
		{
			addressListMemoryRecordList.Clear();
		}

		private void memorySearchResultControl_ResultDoubleClick(object sender, MemoryRecord record)
		{
			addressListMemoryRecordList.Records.Add(record);
		}

		private string addressFilePath;

		private void openAddressFileToolStripButton_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.CheckFileExists = true;
				ofd.Filter = $"All Scanner Types |*{ReClassScanFile.FileExtension};*{CheatEngineFile.FileExtension};*{CrySearchFile.FileExtension}"
							+ $"|{ReClassScanFile.FormatName} (*{ReClassScanFile.FileExtension})|*{ReClassScanFile.FileExtension}"
							+ $"|{CheatEngineFile.FormatName} (*{CheatEngineFile.FileExtension})|*{CheatEngineFile.FileExtension}"
							+ $"|{CrySearchFile.FormatName} (*{CrySearchFile.FileExtension})|*{CrySearchFile.FileExtension}";

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					IScannerImport import = null;
					switch (Path.GetExtension(ofd.FileName)?.ToLower())
					{
						case ReClassScanFile.FileExtension:
							import = new ReClassScanFile();
							break;
						case CheatEngineFile.FileExtension:
							import = new CheatEngineFile();
							break;
						case CrySearchFile.FileExtension:
							import = new CrySearchFile();
							break;
						default:
							Program.Logger.Log(LogLevel.Error, $"The file '{ofd.FileName}' has an unknown type.");
							break;
					}
					if (import != null)
					{
						if (addressListMemoryRecordList.Records.Any())
						{
							if (MessageBox.Show("The address list contains addresses. Do you really want to open the file?", $"{Constants.ApplicationName} Scanner", MessageBoxButtons.YesNo) != DialogResult.Yes)
							{
								return;
							}
						}

						if (import is ReClassScanFile)
						{
							addressFilePath = ofd.FileName;
						}

						addressListMemoryRecordList.SetRecords(
							import.Load(ofd.FileName, Program.Logger)
								.Select(r =>
								{
									r.ResolveAddress(Program.RemoteProcess);
									r.RefreshValue(Program.RemoteProcess);
									return r;
								})
						);
					}
				}
			}
		}

		private void saveAddressFileToolStripButton_Click(object sender, EventArgs e)
		{
			if (addressListMemoryRecordList.Records.None())
			{
				return;
			}

			if (string.IsNullOrEmpty(addressFilePath))
			{
				saveAsToolStripButton_Click(sender, e);

				return;
			}

			var file = new ReClassScanFile();
			file.Save(addressListMemoryRecordList.Records, addressFilePath, Program.Logger);
		}

		private void saveAsToolStripButton_Click(object sender, EventArgs e)
		{
			if (addressListMemoryRecordList.Records.None())
			{
				return;
			}

			using (var sfd = new SaveFileDialog())
			{
				sfd.DefaultExt = ReClassScanFile.FileExtension;
				sfd.Filter = $"{ReClassScanFile.FormatName} (*{ReClassScanFile.FileExtension})|*{ReClassScanFile.FileExtension}";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					addressFilePath = sfd.FileName;

					saveAddressFileToolStripButton_Click(sender, e);
				}
			}
		}

		private void resultListContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			var cms = (ContextMenuStrip)sender;

			var isResultList = cms.SourceControl.Parent == resultMemoryRecordList;

			addSelectedResultsToAddressListToolStripMenuItem.Visible = isResultList;
			changeToolStripMenuItem.Visible = !isResultList;
			removeSelectedRecordsToolStripMenuItem.Visible = !isResultList;

			// Hide all other items if multiple records are selected.
			var multipleRecordsSelected = (isResultList ? resultMemoryRecordList.SelectedRecords.Count : addressListMemoryRecordList.SelectedRecords.Count) > 1;
			for (var i = 3; i < cms.Items.Count; ++i)
			{
				cms.Items[i].Visible = !multipleRecordsSelected;
			}
		}

		private static MemoryRecordList GetMemoryRecordListFromMenuItem(object sender) =>
			(MemoryRecordList)((ContextMenuStrip)((ToolStripMenuItem)sender).Owner).SourceControl.Parent;

		private void removeSelectedRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			addressListMemoryRecordList.Records.Remove(addressListMemoryRecordList.SelectedRecord);
		}

		private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var record = GetMemoryRecordListFromMenuItem(sender)?.SelectedRecord;
			if (record != null)
			{
				Clipboard.SetText(record.RealAddress.ToString("X"));
			}
		}

		private void addSelectedResultsToAddressListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var record in resultMemoryRecordList.SelectedRecords)
			{
				addressListMemoryRecordList.Records.Add(record);
			}
		}

		private void setCurrentClassAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LinkedWindowFeatures.SetCurrentClassAddress(GetMemoryRecordListFromMenuItem(sender).SelectedRecord.RealAddress);
		}

		private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LinkedWindowFeatures.CreateClassAtAddress(GetMemoryRecordListFromMenuItem(sender).SelectedRecord.RealAddress, true);
		}

		private void findOutWhatAccessesThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedRecord(
				GetMemoryRecordListFromMenuItem(sender).SelectedRecord,
				false
			);
		}

		private void findOutWhatWritesToThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedRecord(
				GetMemoryRecordListFromMenuItem(sender).SelectedRecord,
				true
			);
		}

		private void FindWhatInteractsWithSelectedRecord(MemoryRecord record,bool writeOnly)
		{
			int size;
			switch (record.ValueType)
			{
				case ScanValueType.Byte:
					size = 1;
					break;
				case ScanValueType.Short:
					size = 2;
					break;
				case ScanValueType.Integer:
				case ScanValueType.Float:
					size = 4;
					break;
				case ScanValueType.Long:
				case ScanValueType.Double:
					size = 8;
					break;
				case ScanValueType.ArrayOfBytes:
					size = record.ValueLength;
					break;
				case ScanValueType.String:
					size = record.ValueLength;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			LinkedWindowFeatures.FindWhatInteractsWithAddress(record.RealAddress, size, writeOnly);
		}
	}
}
