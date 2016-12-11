using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Memory;
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

		private readonly NativeHelper nativeHelper;

		/// <summary>Gets the selected process.</summary>
		public ProcessInfo SelectedProcess
		{
			get
			{
				var row = (processDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView)?.Row;
				if (row != null)
				{
					return new ProcessInfo(nativeHelper, row.Field<IntPtr>("id"), row.Field<string>("name"), row.Field<string>("path"));
				}
				return null;
			}
		}

		/// <summary>Gets if symbols should get loaded.</summary>
		public bool LoadSymbols => loadSymbolsCheckBox.Checked;

		public ProcessBrowserForm(NativeHelper nativeHelper, string previousProcess)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;

			InitializeComponent();

			processDataGridView.AutoGenerateColumns = false;

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

			nativeHelper.EnumerateProcesses(delegate (ref NativeHelper.EnumerateProcessData data)
			{
				var moduleName = Path.GetFileName(data.Path);
				if (!filterCheckBox.Checked || !CommonProcesses.Contains(moduleName.ToLower()))
				{
					var row = dt.NewRow();
					row["icon"] = ShellIcon.GetSmallIcon(data.Path);
					row["name"] = moduleName;
					row["id"] = data.Id;
					row["path"] = data.Path;
					dt.Rows.Add(row);
				}

				return true;
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
