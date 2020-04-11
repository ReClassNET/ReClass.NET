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
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class ScannerForm : IconForm
	{
		// The designer can't handle generic controls...
		internal class ScanCompareTypeComboBox : EnumComboBox<ScanCompareType> { }
		internal class ScanValueTypeComboBox : EnumComboBox<ScanValueType> { }

		private const int MaxVisibleResults = 10000;

		private readonly RemoteProcess process;

		private bool isFirstScan;

		private Scanner scanner;
		private CancellationTokenSource cts;

		private string addressFilePath;

		public ScannerForm(RemoteProcess process)
		{
			Contract.Requires(process != null);

			this.process = process;

			InitializeComponent();

			toolStripPanel.Renderer = new CustomToolStripProfessionalRenderer(true, false);
			menuToolStrip.Renderer = new CustomToolStripProfessionalRenderer(false, false);

			SetGuiFromSettings(ScanSettings.Default);

			OnValueTypeChanged();

			Reset();

			firstScanButton.Enabled = flowLayoutPanel.Enabled = process.IsValid;

			process.ProcessAttached += RemoteProcessOnProcessAttached;
			process.ProcessClosing += RemoteProcessOnProcessClosing;
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
				if (MessageBox.Show("Keep the current address list?", "Process has changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					addressListMemoryRecordList.Clear();
				}
				else
				{
					foreach (var record in addressListMemoryRecordList.Records)
					{
						record.ResolveAddress(process);
						record.RefreshValue(process);
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
			scanner?.Dispose();

			process.ProcessAttached -= RemoteProcessOnProcessAttached;
			process.ProcessClosing -= RemoteProcessOnProcessClosing;
		}

		private void updateValuesTimer_Tick(object sender, EventArgs e)
		{
			resultMemoryRecordList.RefreshValues(process);
			addressListMemoryRecordList.RefreshValues(process);
		}

		private void scanTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			OnCompareTypeChanged();
		}

		private void valueTypeComboBox_SelectionChangeCommitted(object sender, EventArgs e)
		{
			OnValueTypeChanged();
		}

		private async void firstScanButton_Click(object sender, EventArgs e)
		{
			if (isFirstScan)
			{
				try
				{
					var settings = CreateSearchSettings();
					var comparer = CreateComparer(settings);

					await StartFirstScanEx(settings, comparer);
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}

				return;
			}

			Reset();
		}

		private async void nextScanButton_Click(object sender, EventArgs e)
		{
			if (!process.IsValid)
			{
				return;
			}

			if (!isFirstScan)
			{
				firstScanButton.Enabled = false;
				nextScanButton.Enabled = false;
				cancelScanIconButton.Visible = true;

				try
				{
					var comparer = CreateComparer(scanner.Settings);

					var report = new Progress<int>(i =>
					{
						scanProgressBar.Value = i;
						SetResultCount(scanner.TotalResultCount);
					});
					cts = new CancellationTokenSource();

					await scanner.Search(comparer, report, cts.Token);

					ShowScannerResults(scanner);

					undoIconButton.Enabled = scanner.CanUndoLastScan;
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}

				firstScanButton.Enabled = true;
				nextScanButton.Enabled = true;
				cancelScanIconButton.Visible = false;

				scanProgressBar.Value = 0;
			}
		}

		private void cancelScanIconButton_Click(object sender, EventArgs e)
		{
			cts?.Cancel();
		}

		private void memorySearchResultControl_ResultDoubleClick(object sender, MemoryRecord record)
		{
			addressListMemoryRecordList.Records.Add(record);
		}

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
							if (MessageBox.Show("The address list contains addresses. Do you really want to open the file?", $"{Constants.ApplicationName} Scanner", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
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
									r.ResolveAddress(process);
									r.RefreshValue(process);
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

		private void clearAddressListToolStripButton_Click(object sender, EventArgs e)
		{
			addressListMemoryRecordList.Clear();
		}

		private void showInputCorrelatorIconButton_Click(object sender, EventArgs e)
		{
			new InputCorrelatorForm(this, process).Show();
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

		private void addSelectedResultsToAddressListToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var record in resultMemoryRecordList.SelectedRecords)
			{
				addressListMemoryRecordList.Records.Add(record);
			}
		}

		private void removeSelectedRecordsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var record in addressListMemoryRecordList.SelectedRecords)
			{
				addressListMemoryRecordList.Records.Remove(record);
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

		private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var record = GetMemoryRecordListFromMenuItem(sender)?.SelectedRecord;
			if (record != null)
			{
				Clipboard.SetText(record.RealAddress.ToString("X"));
			}
		}

		private void undoIconButton_Click(object sender, EventArgs e)
		{
			if (scanner.CanUndoLastScan)
			{
				scanner.UndoLastScan();

				ShowScannerResults(scanner);
			}

			undoIconButton.Enabled = scanner.CanUndoLastScan;
		}

		#endregion

		/// <summary>
		/// Displays the total result count.
		/// </summary>
		/// <param name="count">Number of.</param>
		private void SetResultCount(int count)
		{
			resultCountLabel.Text = count > MaxVisibleResults ? $"Found: {count} (only {MaxVisibleResults} shown)" : $"Found: {count}";
		}

		/// <summary>
		/// Shows some of the scanner results.
		/// </summary>
		public void ShowScannerResults(Scanner scanner)
		{
			Contract.Requires(scanner != null);

			SetResultCount(scanner.TotalResultCount);

			resultMemoryRecordList.SetRecords(
				scanner.GetResults()
					.Take(MaxVisibleResults)
					.OrderBy(r => r.Address, IntPtrComparer.Instance)
					.Select(r =>
					{
						var record = new MemoryRecord(r);
						record.ResolveAddress(process);
						return record;
					})
			);
		}

		/// <summary>
		/// Set input elements according to the selected compare type.
		/// </summary>
		private void OnCompareTypeChanged()
		{
			var enableHexCheckBox = true;
			var enableValueBox = true;
			var enableDualInput = false;

			switch (compareTypeComboBox.SelectedValue)
			{
				case ScanCompareType.Unknown:
					enableHexCheckBox = false;
					enableValueBox = false;
					break;
				case ScanCompareType.Between:
				case ScanCompareType.BetweenOrEqual:
					enableDualInput = true;
					break;
			}

			switch (valueTypeComboBox.SelectedValue)
			{
				case ScanValueType.Float:
				case ScanValueType.Double:
				case ScanValueType.ArrayOfBytes:
				case ScanValueType.String:
					isHexCheckBox.Checked = false;
					enableHexCheckBox = false;
					break;
			}

			isHexCheckBox.Enabled = enableHexCheckBox;
			dualValueBox.Enabled = enableValueBox;
			dualValueBox.ShowSecondInputField = enableDualInput;
		}

		/// <summary>
		/// Hide gui elements after the value type has changed.
		/// </summary>
		private void OnValueTypeChanged()
		{
			SetValidCompareTypes();

			var valueType = valueTypeComboBox.SelectedValue;

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

		/// <summary>
		/// Sets valid compare types dependend on the selected value type.
		/// </summary>
		private void SetValidCompareTypes()
		{
			var compareType = compareTypeComboBox.SelectedValue;
			var valueType = valueTypeComboBox.SelectedValue;
			if (valueType == ScanValueType.ArrayOfBytes || valueType == ScanValueType.String)
			{
				compareTypeComboBox.SetAvailableValues(ScanCompareType.Equal);
			}
			else if (isFirstScan)
			{
				compareTypeComboBox.SetAvailableValuesExclude(
					ScanCompareType.Changed, ScanCompareType.NotChanged, ScanCompareType.Decreased,
					ScanCompareType.DecreasedOrEqual, ScanCompareType.Increased, ScanCompareType.IncreasedOrEqual
				);
			}
			else
			{
				compareTypeComboBox.SetAvailableValuesExclude(ScanCompareType.Unknown);
			}

			compareTypeComboBox.SelectedValue = compareType;
		}

		/// <summary>
		/// Resets all fields.
		/// </summary>
		private void Reset()
		{
			scanner?.Dispose();
			scanner = null;

			undoIconButton.Enabled = false;

			SetResultCount(0);
			resultMemoryRecordList.Clear();

			firstScanButton.Enabled = true;
			nextScanButton.Enabled = false;

			isHexCheckBox.Enabled = true;
			//isHexCheckBox.Checked = false;

			valueTypeComboBox.Enabled = true;
			//valueTypeComboBox.SelectedItem = valueTypeComboBox.Items.Cast<EnumDescriptionDisplay<ScanValueType>>().PredicateOrFirst(e => e.Value == ScanValueType.Integer);
			OnValueTypeChanged();

			floatingOptionsGroupBox.Enabled = true;
			stringOptionsGroupBox.Enabled = true;
			scanOptionsGroupBox.Enabled = true;

			isFirstScan = true;
			undoIconButton.Enabled = false;

			SetValidCompareTypes();
		}

		/// <summary>
		/// Excutes a new scan with the provided settings and comparer.
		/// </summary>
		/// <param name="settings">The scan settings.</param>
		/// <param name="comparer">The comparer.</param>
		public void ExcuteScan(ScanSettings settings, IScanComparer comparer)
		{
			Contract.Requires(settings != null);
			Contract.Requires(comparer != null);

			Reset();

			SetGuiFromSettings(settings);

			Invoke((Action)(async () => await StartFirstScanEx(settings, comparer)));
		}

		/// <summary>
		/// Starts a new first scan with the provided settings and comparer.
		/// </summary>
		/// <param name="settings">The scan settings.</param>
		/// <param name="comparer">The comparer.</param>
		private async Task StartFirstScanEx(ScanSettings settings, IScanComparer comparer)
		{
			if (!process.IsValid)
			{
				return;
			}

			firstScanButton.Enabled = false;
			cancelScanIconButton.Visible = true;

			try
			{
				scanner = new Scanner(process, settings);

				var report = new Progress<int>(i =>
				{
					scanProgressBar.Value = i;
					SetResultCount(scanner.TotalResultCount);
				});
				cts = new CancellationTokenSource();

				await scanner.Search(comparer, report, cts.Token);

				ShowScannerResults(scanner);

				cancelScanIconButton.Visible = false;
				nextScanButton.Enabled = true;
				valueTypeComboBox.Enabled = false;

				floatingOptionsGroupBox.Enabled = false;
				stringOptionsGroupBox.Enabled = false;
				scanOptionsGroupBox.Enabled = false;

				isFirstScan = false;

				SetValidCompareTypes();
				OnCompareTypeChanged();
			}
			finally
			{
				firstScanButton.Enabled = true;

				scanProgressBar.Value = 0;
			}
		}

		/// <summary>
		/// Creates the search settings from the user input.
		/// </summary>
		/// <returns>The scan settings.</returns>
		private ScanSettings CreateSearchSettings()
		{
			Contract.Ensures(Contract.Result<ScanSettings>() != null);

			var settings = new ScanSettings
			{
				ValueType = valueTypeComboBox.SelectedValue
			};

			long.TryParse(startAddressTextBox.Text, NumberStyles.HexNumber, null, out var startAddressVar);
			long.TryParse(stopAddressTextBox.Text, NumberStyles.HexNumber, null, out var endAddressVar);
#if RECLASSNET64
			settings.StartAddress = unchecked((IntPtr)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)endAddressVar);
#else
			settings.StartAddress = unchecked((IntPtr)(int)startAddressVar);
			settings.StopAddress = unchecked((IntPtr)(int)endAddressVar);
#endif
			settings.EnableFastScan = fastScanCheckBox.Checked;
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

			settings.ScanPrivateMemory = scanPrivateCheckBox.Checked;
			settings.ScanImageMemory = scanImageCheckBox.Checked;
			settings.ScanMappedMemory = scanMappedCheckBox.Checked;
			settings.ScanWritableMemory = CheckStateToSettingState(scanWritableCheckBox.CheckState);
			settings.ScanExecutableMemory = CheckStateToSettingState(scanExecutableCheckBox.CheckState);
			settings.ScanCopyOnWriteMemory = CheckStateToSettingState(scanCopyOnWriteCheckBox.CheckState);

			return settings;
		}

		/// <summary>
		/// Sets the input fields according to the provided settings.
		/// </summary>
		/// <param name="settings">The scan settings.</param>
		private void SetGuiFromSettings(ScanSettings settings)
		{
			Contract.Requires(settings != null);

			valueTypeComboBox.SelectedValue = settings.ValueType;

			startAddressTextBox.Text = settings.StartAddress.ToString(Constants.AddressHexFormat);
			stopAddressTextBox.Text = settings.StopAddress.ToString(Constants.AddressHexFormat);

			fastScanCheckBox.Checked = settings.EnableFastScan;
			fastScanAlignmentTextBox.Text = Math.Max(1, settings.FastScanAlignment).ToString();

			CheckState SettingStateToCheckState(SettingState state)
			{
				switch (state)
				{
					case SettingState.Yes:
						return CheckState.Checked;
					case SettingState.No:
						return CheckState.Unchecked;
					default:
						return CheckState.Indeterminate;
				}
			}

			scanPrivateCheckBox.Checked = settings.ScanPrivateMemory;
			scanImageCheckBox.Checked = settings.ScanImageMemory;
			scanMappedCheckBox.Checked = settings.ScanMappedMemory;
			scanWritableCheckBox.CheckState = SettingStateToCheckState(settings.ScanWritableMemory);
			scanExecutableCheckBox.CheckState = SettingStateToCheckState(settings.ScanExecutableMemory);
			scanCopyOnWriteCheckBox.CheckState = SettingStateToCheckState(settings.ScanCopyOnWriteMemory);
		}

		/// <summary>
		/// Creates the comparer from the user input.
		/// </summary>
		/// <returns>The scan comparer.</returns>
		private IScanComparer CreateComparer(ScanSettings settings)
		{
			Contract.Requires(settings != null);
			Contract.Ensures(Contract.Result<IScanComparer>() != null);

			var compareType = compareTypeComboBox.SelectedValue;
			var checkBothInputFields = compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual;

			if (settings.ValueType == ScanValueType.Byte || settings.ValueType == ScanValueType.Short || settings.ValueType == ScanValueType.Integer || settings.ValueType == ScanValueType.Long)
			{
				var numberStyle = isHexCheckBox.Checked ? NumberStyles.HexNumber : NumberStyles.Integer;
				if (!long.TryParse(dualValueBox.Value1, numberStyle, null, out var value1)) throw new InvalidInputException(dualValueBox.Value1);
				if (!long.TryParse(dualValueBox.Value2, numberStyle, null, out var value2) && checkBothInputFields) throw new InvalidInputException(dualValueBox.Value2);

				if (compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual)
				{
					if (value1 > value2)
					{
						Utils.Swap(ref value1, ref value2);
					}
				}

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

				var nf1 = NumberFormat.GuessNumberFormat(dualValueBox.Value1);
				if (!double.TryParse(dualValueBox.Value1, NumberStyles.Float, nf1, out var value1)) throw new InvalidInputException(dualValueBox.Value1);
				var nf2 = NumberFormat.GuessNumberFormat(dualValueBox.Value2);
				if (!double.TryParse(dualValueBox.Value2, NumberStyles.Float, nf2, out var value2) && checkBothInputFields) throw new InvalidInputException(dualValueBox.Value2);

				if (compareType == ScanCompareType.Between || compareType == ScanCompareType.BetweenOrEqual)
				{
					if (value1 > value2)
					{
						Utils.Swap(ref value1, ref value2);
					}
				}

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
				if (string.IsNullOrEmpty(dualValueBox.Value1))
				{
					throw new InvalidInputException(dualValueBox.Value1);
				}

				var encoding = encodingUtf8RadioButton.Checked ? Encoding.UTF8 : encodingUtf16RadioButton.Checked ? Encoding.Unicode : Encoding.UTF32;

				return new StringMemoryComparer(dualValueBox.Value1, encoding, caseSensitiveCheckBox.Checked);
			}

			throw new InvalidOperationException();
		}

		/// <summary>
		/// Attaches the debugger to find what interacts with the selected record.
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when one or more arguments are outside the required range.</exception>
		/// <param name="record">The record.</param>
		/// <param name="writeOnly">True to search only for write access.</param>
		private static void FindWhatInteractsWithSelectedRecord(MemoryRecord record, bool writeOnly)
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

	internal class InvalidInputException : Exception
	{
		public InvalidInputException(string input)
			: base($"'{input}' is not a valid input.")
		{

		}
	}
}
