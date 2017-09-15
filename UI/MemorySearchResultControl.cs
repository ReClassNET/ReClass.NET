using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.MemorySearcher;
using ReClassNET.Util;

namespace ReClassNET.UI
{
	public delegate void MemorySearchResultControlResultDoubleClickEventHandler(object sender, MemoryRecord record);

	public partial class MemorySearchResultControl : UserControl
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

		public bool ShowValuesHexadecimal { get; set; }

		public event MemorySearchResultControlResultDoubleClickEventHandler RecordDoubleClick;

		private readonly BindingList<MemoryRecord> bindings;

		public MemorySearchResultControl()
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
			resultDataGridView.DefaultCellStyle.Font = Program.MonoSpaceFont.Font;
			resultDataGridView.DataSource = bindings;
		}

		public void SetRecords(IEnumerable<MemoryRecord> results)
		{
			Contract.Requires(results != null);

			bindings.Clear();

			if (results == null)
			{
				return;
			}

			bindings.RaiseListChangedEvents = false;

			foreach (var result in results)
			{
				bindings.Add(result);
			}

			bindings.RaiseListChangedEvents = true;
			bindings.ResetBindings();
		}

		public void AddRecord(MemoryRecord result)
		{
			Contract.Requires(result != null);

			bindings.Add(result);
		}

		public void Clear()
		{
			SetRecords(null);
		}

		public void RefreshValues()
		{
			foreach (var record in resultDataGridView.GetVisibleRows().Select(r => (MemoryRecord)r.DataBoundItem))
			{
				record.RefreshValue();
			}
		}

		private void OnRecordDoubleClick(MemoryRecord record)
		{
			var evt = RecordDoubleClick;
			evt?.Invoke(this, record);
		}

		private void resultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			OnRecordDoubleClick((MemoryRecord)resultDataGridView.Rows[e.RowIndex].DataBoundItem);
		}
	}
}
