using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Forms;
using ReClassNET.Nodes;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET
{
	public partial class ProcessMemoryViewer : IconForm
	{
		private readonly ClassNodeView classesView;

		public ProcessMemoryViewer(RemoteProcess process, ClassNodeView classesView)
		{
			Contract.Requires(process != null);
			Contract.Requires(classesView != null);

			InitializeComponent();

			this.classesView = classesView;

			sectionsDataGridView.AutoGenerateColumns = false;

			if (process.IsValid)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("address", typeof(long));
				dt.Columns.Add("size", typeof(long));
				dt.Columns.Add("name", typeof(string));
				dt.Columns.Add("protection", typeof(string));
				dt.Columns.Add("state", typeof(string));
				dt.Columns.Add("type", typeof(string));
				dt.Columns.Add("module", typeof(string));

				process.NativeHelper.EnumerateRemoteSectionsAndModules(process.Process.Handle, delegate (IntPtr baseAddress, IntPtr regionSize, string name, NativeMethods.StateEnum state, NativeMethods.AllocationProtectEnum protection, NativeMethods.TypeEnum type, string modulePath)
				{
					var row = dt.NewRow();
					row["address"] = baseAddress.ToInt64();
					row["size"] = regionSize.ToInt64();
					row["name"] = name;
					row["protection"] = protection.ToString();
					row["state"] = state.ToString();
					row["type"] = type.ToString();
					row["module"] = Path.GetFileName(modulePath);
					dt.Rows.Add(row);
				},
				null);

				sectionsDataGridView.DataSource = dt;
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

		private void sectionsDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int rowSelected = e.RowIndex;
				if (e.RowIndex != -1)
				{
					sectionsDataGridView.Rows[rowSelected].Selected = true;
				}
			}
		}

		private IntPtr GetSelectedRegionAddress()
		{
			var row = sectionsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView;
			if (row != null)
			{
				return (IntPtr)(long)row["address"];
			}
			return IntPtr.Zero;
		}

		private void setCurrentClassAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var address = GetSelectedRegionAddress();
			if (address != IntPtr.Zero)
			{
				var node = classesView.SelectedClass;
				if (node != null)
				{
					node.Address = address;
				}
			}
		}

		private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var address = GetSelectedRegionAddress();
			if (address != IntPtr.Zero)
			{
				var node = new ClassNode
				{
					Address = address
				};
				node.AddBytes(64);

				classesView.SelectedClass = node;
			}
		}

		private void sectionsDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			setCurrentClassAddressToolStripMenuItem_Click(sender, e);
		}
	}
}
