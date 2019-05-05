using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class ProcessInfoForm : IconForm
	{
		private readonly IProcessReader process;

		/// <summary>The context menu of the sections grid view.</summary>
		public ContextMenuStrip GridContextMenu => contextMenuStrip;

		public ProcessInfoForm(IProcessReader process)
		{
			Contract.Requires(process != null);

			this.process = process;

			InitializeComponent();

			tabControl.ImageList = new ImageList();
			tabControl.ImageList.Images.Add(Properties.Resources.B16x16_Category);
			tabControl.ImageList.Images.Add(Properties.Resources.B16x16_Page_White_Stack);
			modulesTabPage.ImageIndex = 0;
			sectionsTabPage.ImageIndex = 1;

			modulesDataGridView.AutoGenerateColumns = false;
			sectionsDataGridView.AutoGenerateColumns = false;

			// TODO: Workaround, Mono can't display a DataGridViewImageColumn.
			if (NativeMethods.IsUnix())
			{
				moduleIconDataGridViewImageColumn.Visible = false;
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

		private async void ProcessInfoForm_Load(object sender, EventArgs e)
		{
			var sectionsTable = new DataTable();
			sectionsTable.Columns.Add("address", typeof(string));
			sectionsTable.Columns.Add("size", typeof(string));
			sectionsTable.Columns.Add("name", typeof(string));
			sectionsTable.Columns.Add("protection", typeof(string));
			sectionsTable.Columns.Add("type", typeof(string));
			sectionsTable.Columns.Add("module", typeof(string));
			sectionsTable.Columns.Add("section", typeof(Section));

			var modulesTable = new DataTable();
			modulesTable.Columns.Add("icon", typeof(Icon));
			modulesTable.Columns.Add("name", typeof(string));
			modulesTable.Columns.Add("address", typeof(string));
			modulesTable.Columns.Add("size", typeof(string));
			modulesTable.Columns.Add("path", typeof(string));
			modulesTable.Columns.Add("module", typeof(Module));

			await Task.Run(() =>
			{
				if (process.EnumerateRemoteSectionsAndModules(out var sections, out var modules))
				{
					foreach (var section in sections)
					{
						var row = sectionsTable.NewRow();
						row["address"] = section.Start.ToString(Constants.AddressHexFormat);
						row["size"] = section.Size.ToString(Constants.AddressHexFormat);
						row["name"] = section.Name;
						row["protection"] = section.Protection.ToString();
						row["type"] = section.Type.ToString();
						row["module"] = section.ModuleName;
						row["section"] = section;
						sectionsTable.Rows.Add(row);
					}
					foreach (var module in modules)
					{
						var row = modulesTable.NewRow();
						row["icon"] = NativeMethods.GetIconForFile(module.Path);
						row["name"] = module.Name;
						row["address"] = module.Start.ToString(Constants.AddressHexFormat);
						row["size"] = module.Size.ToString(Constants.AddressHexFormat);
						row["path"] = module.Path;
						row["module"] = module;
						modulesTable.Rows.Add(row);
					}
				}
			});

			sectionsDataGridView.DataSource = sectionsTable;
			modulesDataGridView.DataSource = modulesTable;
		}

		private void SelectRow_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (!(sender is DataGridView dgv))
			{
				return;
			}

			if (e.Button == MouseButtons.Right)
			{
				int row = e.RowIndex;
				if (e.RowIndex != -1)
				{
					dgv.Rows[row].Selected = true;
				}
			}
		}

		private void setCurrentClassAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LinkedWindowFeatures.SetCurrentClassAddress(GetSelectedAddress(sender));
		}

		private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LinkedWindowFeatures.CreateClassAtAddress(GetSelectedAddress(sender), true);
		}

		private void dumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Func<SaveFileDialog> createDialogFn;
			Action<IRemoteMemoryReader, Stream> dumpFn;

			if (GetToolStripSourceControl(sender) == modulesDataGridView)
			{
				var module = GetSelectedModule();
				if (module == null)
				{
					return;
				}

				createDialogFn = () => new SaveFileDialog
				{
					FileName = $"{Path.GetFileNameWithoutExtension(module.Name)}_Dumped{Path.GetExtension(module.Name)}",
					InitialDirectory = Path.GetDirectoryName(module.Path)
				};

				dumpFn = (reader, stream) =>
				{
					Dumper.DumpModule(reader, module, stream);

					MessageBox.Show("Module successfully dumped.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				};
			}
			else
			{
				var section = GetSelectedSection();
				if (section == null)
				{
					return;
				}

				createDialogFn = () => new SaveFileDialog
				{
					FileName = $"Section_{section.Start.ToString("X")}_{section.End.ToString("X")}.dat"
				};

				dumpFn = (reader, stream) =>
				{
					Dumper.DumpSection(reader, section, stream);

					MessageBox.Show("Section successfully dumped.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				};
			}

			using (var sfd = createDialogFn())
			{
				sfd.Filter = "All|*.*";

				if (sfd.ShowDialog() != DialogResult.OK)
				{
					return;
				}

				try
				{
					using (var stream = sfd.OpenFile())
					{
						dumpFn(process, stream);
					}
				}
				catch (Exception ex)
				{
					Program.ShowException(ex);
				}
			}
		}

		private void sectionsDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			setCurrentClassAddressToolStripMenuItem_Click(sender, e);

			Close();
		}

		#endregion

		private IntPtr GetSelectedAddress(object sender)
		{
			if (GetToolStripSourceControl(sender) == modulesDataGridView)
			{
				return GetSelectedModule()?.Start ?? IntPtr.Zero;
			}
			else
			{
				return GetSelectedSection()?.Start ?? IntPtr.Zero;
			}
		}

		private static Control GetToolStripSourceControl(object sender)
		{
			return ((sender as ToolStripMenuItem)?.GetCurrentParent() as ContextMenuStrip)?.SourceControl;
		}

		private Module GetSelectedModule()
		{
			var row = modulesDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView;
			return row?["module"] as Module;
		}

		private Section GetSelectedSection()
		{
			var row = sectionsDataGridView.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault()?.DataBoundItem as DataRowView;
			return row?["section"] as Section;
		}
	}
}
