using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET
{
	partial class ProcessMemoryViewer : Form
	{
		private readonly NativeHelper nativeHelper;

		public ProcessMemoryViewer(NativeHelper nativeHelper, ProcessInfo process)
		{
			Contract.Requires(nativeHelper != null);

			this.nativeHelper = nativeHelper;

			InitializeComponent();

			sectionsDataGridView.AutoGenerateColumns = false;

			if (nativeHelper.IsProcessValid(process.Handle))
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("address", typeof(long));
				dt.Columns.Add("size", typeof(long));
				dt.Columns.Add("name", typeof(string));
				dt.Columns.Add("protection", typeof(string));
				dt.Columns.Add("state", typeof(string));
				dt.Columns.Add("type", typeof(string));
				dt.Columns.Add("module", typeof(string));

				nativeHelper.EnumerateRemoteSectionsAndModules(process.Handle, delegate (IntPtr baseAddress, IntPtr regionSize, string name, Natives.StateEnum state, Natives.AllocationProtectEnum protection, Natives.TypeEnum type, string modulePath)
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
	}
}
