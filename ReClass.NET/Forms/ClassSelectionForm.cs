using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Nodes;

namespace ReClassNET.Forms
{
	public partial class ClassSelectionForm : IconForm
	{
		private readonly List<ClassNode> allClasses;

		public ClassNode SelectedClass => classesListBox.SelectedItem as ClassNode;

		public ClassSelectionForm(IEnumerable<ClassNode> classes)
		{
			Contract.Requires(classes != null);

			allClasses = classes.ToList();

			InitializeComponent();

			ShowFilteredClasses();
		}

		private void filterNameTextBox_TextChanged(object sender, EventArgs e)
		{
			ShowFilteredClasses();
		}

		private void classesListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = SelectedClass != null;
		}

		private void classesListBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (SelectedClass != null)
			{
				selectButton.PerformClick();
			}
		}

		private void ShowFilteredClasses()
		{
			IEnumerable<ClassNode> classes = allClasses;

			if (!string.IsNullOrEmpty(filterNameTextBox.Text))
			{
				classes = classes.Where(c => c.Name.IndexOf(filterNameTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			classesListBox.DataSource = classes.ToList();
		}
	}
}
