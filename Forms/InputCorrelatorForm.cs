using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Input;
using ReClassNET.MemoryScanner;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class InputCorrelatorForm : IconForm
	{
		private readonly ScannerForm scannerForm;
		private readonly KeyboardInput input;
		private InputCorrelatedScanner scanner;

		private bool isScanning = false;

		public InputCorrelatorForm(ScannerForm sf)
		{
			Contract.Requires(sf != null);

			scannerForm = sf;

			InitializeComponent();

			valueTypeComboBox.DataSource = EnumDescriptionDisplay<ScanValueType>.CreateExact(
				ScanValueType.Byte,
				ScanValueType.Short,
				ScanValueType.Integer,
				ScanValueType.Long,
				ScanValueType.Float,
				ScanValueType.Double
			);
			valueTypeComboBox.SelectedItem = valueTypeComboBox.Items.Cast<EnumDescriptionDisplay<ScanValueType>>().FirstOrDefault(e => e.Value == ScanValueType.Integer);

			input = new KeyboardInput();

			hotkeyBox.Input = input;

			rescanTimer.Interval = 400;

			infoLabel.Text = string.Empty;
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			var hotkey = hotkeyBox.Hotkey.Clone();
			if (hotkey.IsEmpty)
			{
				return;
			}

			hotkeyListBox.Items.Add(hotkey);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			var index = hotkeyListBox.SelectedIndex;
			if (index < 0)
			{
				return;
			}

			hotkeyListBox.Items.RemoveAt(index);
		}

		private async void timer_Tick(object sender, EventArgs e)
		{
			if (isScanning)
			{
				return;
			}

			isScanning = true;

			try
			{
				await scanner.CorrelateInput(CancellationToken.None, null);

				infoLabel.Text = $"Scan Count: {scanner.ScanCount} Possible Values: {scanner.TotalResultCount}";
			}
			catch (Exception ex)
			{
				Program.ShowException(ex);
			}

			isScanning = false;
		}

		private async void InputCorrelatorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			hotkeyBox.Input = null;

			rescanTimer.Enabled = false;

			if (isScanning)
			{
				e.Cancel = true;

				Hide();

				await Task.Delay(TimeSpan.FromSeconds(1));

				Close();

				return;
			}

			scanner?.Dispose();

			input?.Dispose();
		}

		private async void startStopButton_Click(object sender, EventArgs e)
		{
			if (scanner == null)
			{
				if (hotkeyListBox.Items.Count == 0)
				{
					MessageBox.Show("Please add at least one hotkey.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

					return;
				}

				scanner = new InputCorrelatedScanner(
					Program.RemoteProcess,
					input,
					hotkeyListBox.Items.Cast<KeyboardHotkey>(),
					((EnumDescriptionDisplay<ScanValueType>)valueTypeComboBox.SelectedItem).Value
				);

				settingsGroupBox.Enabled = false;

				try
				{
					await scanner.Initialize();

					startStopButton.Text = "Stop Scan";

					rescanTimer.Enabled = true;

					return;
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}
			}
			else
			{
				rescanTimer.Enabled = false;

				startStopButton.Text = "Start Scan";

				while (isScanning)
				{
					await Task.Delay(TimeSpan.FromSeconds(1));
				}

				scannerForm.ShowScannerResults(scanner);

				scanner.Dispose();
				scanner = null;
			}

			settingsGroupBox.Enabled = true;
		}
	}
}
