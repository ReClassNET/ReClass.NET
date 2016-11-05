using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

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

		private void hexRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			bytesNumericUpDown.Hexadecimal = hexRadioButton.Checked;
		}

		private void bytesNumericUpDown_ValueChanged(object sender, EventArgs e)
		{
			FormatLabelText(newSizeLabel, currentSize + Bytes);
		}

		private void FormatLabelText(Label label, int size)
		{
			Contract.Requires(label != null);

			label.Text = $"0x{size:X} / {size}";
		}
	}
}
