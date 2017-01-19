using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Core;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class ProcessBrowserForm : IconForm
	{
		private const string NoPreviousProcess = "No previous process";

		private static readonly string[] CommonProcesses = new string[]
		{
			"[system process]", "system", "svchost.exe", "services.exe", "wininit.exe",
			"smss.exe", "csrss.exe", "lsass.exe", "winlogon.exe", "wininit.exe", "dwm.exe"
		};

		private readonly CoreFunctionsManager coreFunctions;

		/// <summary>Gets the selected process.</summary>
		public ProcessInfo SelectedProcess => (processDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView)
			?.Row
			?.Field<ProcessInfo>("info");

		/// <summary>Gets if symbols should get loaded.</summary>
		public bool LoadSymbols => loadSymbolsCheckBox.Checked;

		public ProcessBrowserForm(CoreFunctionsManager coreFunctions, string previousProcess)
		{
			Contract.Requires(coreFunctions != null);

			this.coreFunctions = coreFunctions;

			InitializeComponent();

			processDataGridView.AutoGenerateColumns = false;

			// TODO: Workaround, Mono can't display a DataGridViewImageColumn.
			if (NativeMethods.IsUnix())
			{
				iconColumn.Visible = false;
			}

			previousProcessLinkLabel.Text = string.IsNullOrEmpty(previousProcess) ? NoPreviousProcess : previousProcess;

			RefreshProcessList();

			foreach (var row in processDataGridView.Rows.Cast<DataGridViewRow>())
			{
				if ((row.Cells[1].Value as string) == previousProcess)
				{
					processDataGridView.CurrentCell = row.Cells[1];
					break;
				}
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

		#region Event Handler

		private void filterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RefreshProcessList();
		}

		private void filterTextBox_TextChanged(object sender, EventArgs e)
		{
			ApplyFilter();
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			RefreshProcessList();
		}

		private void previousProcessLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			filterTextBox.Text = previousProcessLinkLabel.Text == NoPreviousProcess ? string.Empty : previousProcessLinkLabel.Text;
		}

		private void processDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			AcceptButton.PerformClick();
		}

		#endregion

		/// <summary>Queries all processes and displays them.</summary>
		private void RefreshProcessList()
		{
			var dt = new DataTable();
			dt.Columns.Add("icon", typeof(Icon));
			dt.Columns.Add("name", typeof(string));
			dt.Columns.Add("id", typeof(IntPtr));
			dt.Columns.Add("path", typeof(string));
			dt.Columns.Add("info", typeof(ProcessInfo));

			coreFunctions.EnumerateProcesses(p =>
			{
				if (!filterCheckBox.Checked || !CommonProcesses.Contains(p.Name.ToLower()))
				{
					var row = dt.NewRow();
					row["icon"] = NativeMethods.GetIconForFile(p.Path);
					row["name"] = p.Name;
					row["id"] = p.Id;
					row["path"] = p.Path;
					row["info"] = p;
					dt.Rows.Add(row);
				}
			});

			dt.DefaultView.Sort = "name ASC";

			processDataGridView.DataSource = dt;

			ApplyFilter();
		}

		private void ApplyFilter()
		{
			var filter = filterTextBox.Text;
			if (!string.IsNullOrEmpty(filter))
			{
				filter = $"name like '%{filter}%' or path like '%{filter}%'";
			}
			(processDataGridView.DataSource as DataTable).DefaultView.RowFilter = filter;
		}
	}
}
