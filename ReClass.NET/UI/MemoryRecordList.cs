using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.Util;

namespace ReClassNET.UI
{
	public delegate void MemorySearchResultControlResultDoubleClickEventHandler(object sender, MemoryRecord record);

	public partial class MemoryRecordList : UserControl
	{
		public bool ShowDescriptionColumn
		{
			get => descriptionColumn.Visible;
			set => descriptionColumn.Visible = value;
		}

		public bool ShowAddressColumn
		{
			get => addressColumn.Visible;
			set => addressColumn.Visible = value;
		}

		public bool ShowValueTypeColumn
		{
			get => valueTypeColumn.Visible;
			set => valueTypeColumn.Visible = value;
		}

		public bool ShowValueColumn
		{
			get => valueColumn.Visible;
			set => valueColumn.Visible = value;
		}

		public bool ShowPreviousValueColumn
		{
			get => previousValueColumn.Visible;
			set => previousValueColumn.Visible = value;
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList<MemoryRecord> Records => bindings;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MemoryRecord SelectedRecord => GetSelectedRecords().FirstOrDefault();

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IList<MemoryRecord> SelectedRecords => GetSelectedRecords().ToList();

		public override ContextMenuStrip ContextMenuStrip
		{
			get;
			set;
		}

		public event MemorySearchResultControlResultDoubleClickEventHandler RecordDoubleClick;

		private readonly BindingList<MemoryRecord> bindings;

		public MemoryRecordList()
		{
			InitializeComponent();

			if (Program.DesignMode)
			{
				return;
			}

			bindings = new BindingList<MemoryRecord>
			{
				AllowNew = true,
				AllowEdit = true,
				RaiseListChangedEvents = true
			};

			resultDataGridView.AutoGenerateColumns = false;
			resultDataGridView.DefaultCellStyle.Font = new Font(
				Program.MonoSpaceFont.Font.FontFamily,
				DpiUtil.ScaleIntX(11),
				GraphicsUnit.Pixel
			);
			resultDataGridView.DataSource = bindings;
		}

		#region Event Handler

		private void resultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			OnRecordDoubleClick((MemoryRecord)resultDataGridView.Rows[e.RowIndex].DataBoundItem);
		}

		private void resultDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 1) // Address
			{
				var record = (MemoryRecord)resultDataGridView.Rows[e.RowIndex].DataBoundItem;
				if (record.IsRelativeAddress)
				{
					e.CellStyle.ForeColor = Color.ForestGreen;
					e.FormattingApplied = true;
				}
			}
			else if (e.ColumnIndex == 3) // Value
			{
				var record = (MemoryRecord)resultDataGridView.Rows[e.RowIndex].DataBoundItem;
				e.CellStyle.ForeColor = record.HasChangedValue ? Color.Red : Color.Black;
				e.FormattingApplied = true;
			}
		}

		private void resultDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (e.RowIndex != -1)
				{
					var row = resultDataGridView.Rows[e.RowIndex];
					if (!row.Selected && !(ModifierKeys == Keys.Shift || ModifierKeys == Keys.Control))
					{
						resultDataGridView.ClearSelection();
					}
					row.Selected = true;
				}
			}
		}

		private void resultDataGridView_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
		{
			e.ContextMenuStrip = ContextMenuStrip;
		}

		#endregion

		private IEnumerable<MemoryRecord> GetSelectedRecords() => resultDataGridView.SelectedRows.Cast<DataGridViewRow>().Select(r => (MemoryRecord)r.DataBoundItem);

		/// <summary>
		/// Sets the records to display.
		/// </summary>
		/// <param name="records">The records.</param>
		public void SetRecords(IEnumerable<MemoryRecord> records)
		{
			Contract.Requires(records != null);

			bindings.Clear();

			bindings.RaiseListChangedEvents = false;

			foreach (var record in records)
			{
				bindings.Add(record);
			}

			bindings.RaiseListChangedEvents = true;
			bindings.ResetBindings();
		}

		/// <summary>
		/// Removes all records.
		/// </summary>
		public void Clear()
		{
			bindings.Clear();
		}

		/// <summary>
		/// Refreshes the data of all displayed records.
		/// </summary>
		/// <param name="process">The process.</param>
		public void RefreshValues(RemoteProcess process)
		{
			Contract.Requires(process != null);

			foreach (var record in resultDataGridView.GetVisibleRows().Select(r => (MemoryRecord)r.DataBoundItem))
			{
				record.RefreshValue(process);
			}
		}

		private void OnRecordDoubleClick(MemoryRecord record)
		{
			var evt = RecordDoubleClick;
			evt?.Invoke(this, record);
		}
	}
}
