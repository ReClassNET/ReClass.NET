using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Project;

namespace ReClassNET.Forms
{
	public partial class EnumEditorForm : IconForm
	{
		private readonly EnumMetaData enumMetaData;

		public EnumEditorForm(EnumMetaData enumMetaData)
		{
			Contract.Requires(enumMetaData != null);

			InitializeComponent();

			this.enumMetaData = enumMetaData;

			enumNameTextBox.Text = enumMetaData.Name;
			enumFlagCheckBox.Checked = enumMetaData.UseFlagsMode;

			foreach (var kv in enumMetaData.Values)
			{
				enumDataGridView.Rows.Add(kv.Key, kv.Value);
			}
		}

		private void enumDataGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
		{
			long value = e.Row.Index;
			if (enumFlagCheckBox.Checked)
			{
				value = (long)Math.Pow(2, e.Row.Index);
			}

			e.Row.Cells[0].Value = value;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			enumMetaData.Name = enumNameTextBox.Text;

			var values = new Dictionary<long, string>();

			foreach (var row in enumDataGridView.Rows.Cast<DataGridViewRow>().Where(r => r.IsNewRow == false))
			{
				if (!long.TryParse(Convert.ToString(row.Cells[0].Value), out var valueKey))
				{
					continue;
				}

				var valueName = Convert.ToString(row.Cells[1].Value);

				values.Add(valueKey, valueName);
			}

			enumMetaData.SetData(enumFlagCheckBox.Checked, 4, values);
		}

		private void enumDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			void SetErrorText(string text)
			{
				enumDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = text;
			}

			SetErrorText(null);

			var formattedValue = Convert.ToString(e.FormattedValue);

			if (e.ColumnIndex == 0 && !long.TryParse(formattedValue, out _))
			{
				SetErrorText($"'{formattedValue}' is not a valid value.");
			}
			else if (e.ColumnIndex == 1 && string.IsNullOrWhiteSpace(formattedValue))
			{
				SetErrorText("Empty names are not allowed.");
			}
		}
	}
}
