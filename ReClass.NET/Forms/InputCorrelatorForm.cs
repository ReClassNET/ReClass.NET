using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Input;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class InputCorrelatorForm : IconForm
	{
		private static readonly TimeSpan refineInterval = TimeSpan.FromMilliseconds(400);

		private readonly ScannerForm scannerForm;
		private readonly RemoteProcess process;

		private readonly KeyboardInput input;
		private InputCorrelatedScanner scanner;

		private bool isScanning = false;
		private DateTime lastRefineTime;

		public InputCorrelatorForm(ScannerForm scannerForm, RemoteProcess process)
		{
			Contract.Requires(scannerForm != null);
			Contract.Requires(process != null);

			this.scannerForm = scannerForm;
			this.process = process;

			InitializeComponent();

			valueTypeComboBox.SetAvailableValues(
				ScanValueType.Byte,
				ScanValueType.Short,
				ScanValueType.Integer,
				ScanValueType.Long,
				ScanValueType.Float,
				ScanValueType.Double
			);
			valueTypeComboBox.SelectedValue = ScanValueType.Integer;

			input = new KeyboardInput();

			hotkeyBox.Input = input;

			infoLabel.Text = string.Empty;
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

		private async void InputCorrelatorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			hotkeyBox.Input = null;

			refineTimer.Enabled = false;

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

		private void addButton_Click(object sender, EventArgs e)
		{
			var hotkey = hotkeyBox.Hotkey.Clone();
			if (hotkey.IsEmpty)
			{
				return;
			}

			hotkeyListBox.Items.Add(hotkey);

			hotkeyBox.Clear();
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
					process,
					input,
					hotkeyListBox.Items.Cast<KeyboardHotkey>(),
					valueTypeComboBox.SelectedValue
				);

				settingsGroupBox.Enabled = false;

				try
				{
					await scanner.Initialize();

					startStopButton.Text = "Stop Scan";

					refineTimer.Enabled = true;

					return;
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}
			}
			else
			{
				refineTimer.Enabled = false;

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

		private async void refineTimer_Tick(object sender, EventArgs e)
		{
			if (isScanning)
			{
				return;
			}

			scanner.CorrelateInput();

			if (lastRefineTime + refineInterval < DateTime.Now)
			{
				isScanning = true;

				try
				{
					await scanner.RefineResults(CancellationToken.None, null);

					infoLabel.Text = $"Scan Count: {scanner.ScanCount} Possible Values: {scanner.TotalResultCount}";
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}

				isScanning = false;

				lastRefineTime = DateTime.Now;
			}
		}

		#endregion
	}
}
