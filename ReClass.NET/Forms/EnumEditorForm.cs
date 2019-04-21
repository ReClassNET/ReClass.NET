using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Project;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class EnumEditorForm : IconForm
	{
		private readonly EnumDescription @enum;

		public EnumEditorForm(EnumDescription @enum)
		{
			Contract.Requires(@enum != null);

			InitializeComponent();

			this.@enum = @enum;

			enumNameTextBox.Text = @enum.Name;
			enumUnderlyingTypeSizeComboBox.SelectedValue = @enum.Size;
			enumFlagCheckBox.Checked = @enum.UseFlagsMode;

			foreach (var kv in @enum.Values)
			{
				enumDataGridView.Rows.Add(kv.Value, kv.Key);
			}
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
			@enum.Name = enumNameTextBox.Text;

			var values = new Dictionary<string, long>();

			foreach (var row in enumDataGridView.Rows.Cast<DataGridViewRow>().Where(r => r.IsNewRow == false))
			{
				if (!long.TryParse(Convert.ToString(row.Cells[0].Value), out var itemValue))
				{
					continue;
				}

				var itemName = Convert.ToString(row.Cells[1].Value);

				values.Add(itemName, itemValue);
			}

			@enum.SetData(enumFlagCheckBox.Checked, enumUnderlyingTypeSizeComboBox.SelectedValue, values);
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

	internal class UnderlyingSizeComboBox : EnumComboBox<EnumDescription.UnderlyingTypeSize> { }
}
