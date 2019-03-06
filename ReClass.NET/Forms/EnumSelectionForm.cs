using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Project;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class EnumSelectionForm : IconForm
	{
		private readonly IReadOnlyList<EnumMetaData> allEnums;

		public EnumMetaData SelectedItem => itemListBox.SelectedItem as EnumMetaData;

		public EnumSelectionForm(IEnumerable<EnumMetaData> classes)
		{
			Contract.Requires(classes != null);

			allEnums = classes.ToList();

			InitializeComponent();

			ShowFilteredClasses();
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

		private void filterNameTextBox_TextChanged(object sender, EventArgs e)
		{
			ShowFilteredClasses();
		}

		private void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = SelectedItem != null;
		}

		private void ShowFilteredClasses()
		{
			IEnumerable<EnumMetaData> classes = allEnums;

			if (!string.IsNullOrEmpty(filterNameTextBox.Text))
			{
				classes = classes.Where(c => c.Name.IndexOf(filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			itemListBox.DataSource = classes.ToList();
		}
	}
}
