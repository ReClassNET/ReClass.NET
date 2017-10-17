using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class InputBytesForm : IconForm
	{
		private readonly int currentSize;

		public int Bytes => (int)bytesNumericUpDown.Value;

		public InputBytesForm(int currentSize)
		{
			this.currentSize = currentSize;

			InitializeComponent();

			bytesNumericUpDown.Maximum = int.MaxValue;

			FormatLabelText(currentSizeLabel, currentSize);
			FormatLabelText(newSizeLabel, currentSize);
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

		private void hexRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			bytesNumericUpDown.Hexadecimal = hexRadioButton.Checked;
		}

		private void bytesNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			FormatLabelText(newSizeLabel, currentSize + Bytes);
		}

		#endregion

		private void FormatLabelText(Label label, int size)
		{
			Contract.Requires(label != null);

			label.Text = $"0x{size:X} / {size}";
		}
	}
}
