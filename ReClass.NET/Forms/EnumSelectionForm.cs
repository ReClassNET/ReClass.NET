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
		private readonly ReClassNetProject project;

		public EnumDescription SelectedItem => itemListBox.SelectedItem as EnumDescription;

		public EnumSelectionForm(ReClassNetProject project)
		{
			Contract.Requires(project != null);

			this.project = project;

			InitializeComponent();

			ShowFilteredEnums();
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
			ShowFilteredEnums();
		}

		private void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = editEnumIconButton.Enabled = removeEnumIconButton.Enabled = SelectedItem != null;
		}

		private void editEnumIconButton_Click(object sender, EventArgs e)
		{
			var @enum = SelectedItem;
			if (@enum == null)
			{
				return;
			}

			using (var eef = new EnumEditorForm(@enum))
			{
				eef.ShowDialog();
			}
		}

		private void addEnumIconButton_Click(object sender, EventArgs e)
		{
			var @enum = new EnumDescription
			{
				Name = "Enum"
			};

			using (var eef = new EnumEditorForm(@enum))
			{
				if (eef.ShowDialog() == DialogResult.OK)
				{
					project.AddEnum(@enum);

					ShowFilteredEnums();
				}
			}
		}

		private void removeEnumIconButton_Click(object sender, EventArgs e)
		{
			var @enum = SelectedItem;
			if (@enum == null)
			{
				return;
			}

			project.RemoveEnum(@enum);

			ShowFilteredEnums();
		}

		private void ShowFilteredEnums()
		{
			IEnumerable<EnumDescription> enums = project.Enums;

			if (!string.IsNullOrEmpty(filterNameTextBox.Text))
			{
				enums = enums.Where(c => c.Name.IndexOf(filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			itemListBox.DataSource = enums.ToList();
		}
	}
}
