using ReClassNET.Gui;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ReClassNET
{
	partial class ProcessBrowser : IconForm
	{
		private const string NoPreviousProcess = "No previous process";

		private readonly NativeHelper nativeHelper;

		private static string[] CommonProcesses = new string[]
		{
			"[system process]", "system", "svchost.exe", "services.exe", "wininit.exe",
			"smss.exe", "csrss.exe", "lsass.exe", "winlogon.exe", "wininit.exe", "dwm.exe"
		};

		public ProcessInfo SelectedProcess
		{
			get
			{
				var row = (processDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView)?.Row;
				if (row != null)
				{
					return new ProcessInfo
					{
						Id = row.Field<int>("pid"),
						Handle = nativeHelper.OpenRemoteProcess(row.Field<int>("pid"), Natives.PROCESS_ALL_ACCESS),
						Name = row.Field<string>("name"),
						Path = row.Field<string>("path")
					};
				}
				return null;
			}
		}
		public bool LoadSymbols => loadSymbolsCheckBox.Checked;

		public ProcessBrowser(NativeHelper nativeHelper, string previousProcess)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;

			InitializeComponent();

			processDataGridView.AutoGenerateColumns = false;

			previousProcessLinkLabel.Text = string.IsNullOrEmpty(previousProcess) ? NoPreviousProcess : previousProcess;

			RefreshProcessList();
		}

		private void filterCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			RefreshProcessList();
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			RefreshProcessList();
		}

		private void openProcessButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}

		private void RefreshProcessList()
		{
			var dt = new DataTable();
			dt.Columns.Add("icon", typeof(Icon));
			dt.Columns.Add("name", typeof(string));
			dt.Columns.Add("pid", typeof(int));
			dt.Columns.Add("path", typeof(string));

			nativeHelper.EnumerateProcesses((pid, path) =>
			{
				var moduleName = Path.GetFileName(path);
				if (!filterCheckBox.Checked || !CommonProcesses.Contains(moduleName.ToLower()))
				{
					var row = dt.NewRow();
					row["icon"] = ShellIcon.GetSmallIcon(path);
					row["name"] = moduleName;
					row["pid"] = pid;
					row["path"] = path;
					dt.Rows.Add(row);
				}
			});

			processDataGridView.DataSource = dt;
		}

		private void filterTextBox_TextChanged(object sender, EventArgs e)
		{
			var filter = filterTextBox.Text;
			if (!string.IsNullOrEmpty(filter))
			{
				filter = $"name like '%{filter}%' or path like '%{filter}%'";
			}
			(processDataGridView.DataSource as DataTable).DefaultView.RowFilter = filter;
		}

		private void previousProcessLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			filterTextBox.Text = previousProcessLinkLabel.Text == NoPreviousProcess ? string.Empty : previousProcessLinkLabel.Text;
		}

		private void processDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			openProcessButton_Click(sender, e);
		}
	}

	public class ProcessInfo
	{
		public int Id;
		public IntPtr Handle;
		public string Name;
		public string Path;
	}
}
