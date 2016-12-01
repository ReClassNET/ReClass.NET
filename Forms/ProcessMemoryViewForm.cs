using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Forms;
using ReClassNET.Memory;
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

			this.classesView = classesView;

			InitializeComponent();

			sectionsDataGridView.AutoGenerateColumns = false;

			if (process.IsValid)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("address", typeof(string));
				dt.Columns.Add("address_val", typeof(IntPtr));
				dt.Columns.Add("size", typeof(ulong));
				dt.Columns.Add("name", typeof(string));
				dt.Columns.Add("protection", typeof(string));
				dt.Columns.Add("type", typeof(string));
				dt.Columns.Add("module", typeof(string));

				process.NativeHelper.EnumerateRemoteSectionsAndModules(process.Process.Handle, delegate (IntPtr baseAddress, IntPtr regionSize, string name, NativeMethods.StateEnum state, NativeMethods.AllocationProtectEnum protection, NativeMethods.TypeEnum type, string modulePath)
				{
					var row = dt.NewRow();
					row["address"] = baseAddress.ToString("X");
					row["address_val"] = baseAddress;
					row["size"] = (ulong)regionSize.ToInt64();
					row["name"] = name;
					row["protection"] = protection.ToString();
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

		#region Event Handler

		private void sectionsDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				int row = e.RowIndex;
				if (e.RowIndex != -1)
				{
					sectionsDataGridView.Rows[row].Selected = true;
				}
			}
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
				var node = ClassNode.Create();
				node.Address = address;
				node.AddBytes(64);

				classesView.SelectedClass = node;
			}
		}

		private void sectionsDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			setCurrentClassAddressToolStripMenuItem_Click(sender, e);
		}

		#endregion

		private IntPtr GetSelectedRegionAddress()
		{
			var row = sectionsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView;
			if (row != null)
			{
				return (IntPtr)row["address_val"];
			}
			return IntPtr.Zero;
		}
	}
}
