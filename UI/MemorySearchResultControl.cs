using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.MemorySearcher;
using ReClassNET.Util;

namespace ReClassNET.UI
{
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

		private readonly BindingList<ResultData> bindings;

		public MemorySearchResultControl()
		{
			InitializeComponent();

			bindings = new BindingList<ResultData>
			{
				AllowNew = true,
				AllowEdit = true,
				RaiseListChangedEvents = true
			};

			if (!Program.DesignMode)
			{
				resultDataGridView.DefaultCellStyle.Font = Program.MonoSpaceFont.Font;
			}

			resultDataGridView.AutoGenerateColumns = false;
			resultDataGridView.DataSource = bindings;
		}

		public void SetSearchResults(IEnumerable<SearchResult> results)
		{
			Contract.Requires(results != null);

			bindings.Clear();

			if (results == null)
			{
				return;
			}

			bindings.RaiseListChangedEvents = false;

			foreach (var result in results.Select(r => new ResultData(r)))
			{
				bindings.Add(result);
			}

			bindings.RaiseListChangedEvents = true;
			bindings.ResetBindings();
		}

		public void UpdateValues(RemoteProcess process)
		{
			Contract.Requires(process != null);

			foreach (var row in resultDataGridView.GetVisibleRows())
			{
				if (row.DataBoundItem is ResultData result)
				{
					result.UpdateValue(process);
				}
			}
		}
	}
}
