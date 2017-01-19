using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Native;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class ProcessInfoForm : IconForm
	{
		private readonly RemoteProcess process;
		private readonly ClassNodeView classesView;

		/// <summary>The context menu of the sections grid view.</summary>
		public ContextMenuStrip GridContextMenu => contextMenuStrip;

		public ProcessInfoForm(RemoteProcess process, ClassNodeView classesView)
		{
			Contract.Requires(process != null);
			Contract.Requires(classesView != null);

			this.process = process;
			this.classesView = classesView;

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

			if (process.IsValid)
			{
				var sections = new DataTable();
				sections.Columns.Add("address", typeof(string));
				sections.Columns.Add("size", typeof(string));
				sections.Columns.Add("name", typeof(string));
				sections.Columns.Add("protection", typeof(string));
				sections.Columns.Add("type", typeof(string));
				sections.Columns.Add("module", typeof(string));
				sections.Columns.Add("section", typeof(Section));

				var modules = new DataTable();
				modules.Columns.Add("icon", typeof(Icon));
				modules.Columns.Add("name", typeof(string));
				modules.Columns.Add("address", typeof(string));
				modules.Columns.Add("size", typeof(string));
				modules.Columns.Add("path", typeof(string));
				modules.Columns.Add("module", typeof(Module));

				process.EnumerateRemoteSectionsAndModules(
					delegate (Section section)
					{
						var row = sections.NewRow();
						row["address"] = section.Start.ToString(Constants.StringHexFormat);
						row["size"] = section.Size.ToString(Constants.StringHexFormat);
						row["name"] = section.Name;
						row["protection"] = section.Protection.ToString();
						row["type"] = section.Type.ToString();
						row["module"] = section.ModuleName;
						row["section"] = section;
						sections.Rows.Add(row);
					},
					delegate (Module module)
					{
						var row = modules.NewRow();
						row["icon"] = NativeMethods.GetIconForFile(module.Path);
						row["name"] = module.Name;
						row["address"] = module.Start.ToString(Constants.StringHexFormat);
						row["size"] = module.Size.ToString(Constants.StringHexFormat);
						row["path"] = module.Path;
						row["module"] = module;
						modules.Rows.Add(row);
					}
				);

				sectionsDataGridView.DataSource = sections;
				modulesDataGridView.DataSource = modules;
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

		private void SelectRow_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			var dgv = sender as DataGridView;
			if (dgv == null)
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
			var classNode = classesView.SelectedClass;
			if (classNode == null)
			{
				return;
			}

			IntPtr address;
			if (GetToolStripSourceControl(sender) == modulesDataGridView)
			{
				address = GetSelectedModule()?.Start ?? IntPtr.Zero;
			}
			else
			{
				address = GetSelectedSection()?.Start ?? IntPtr.Zero;
			}

			classNode.Address = address;
		}

		private void createClassAtAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var node = ClassNode.Create();
			node.AddBytes(64);

			if (GetToolStripSourceControl(sender) == modulesDataGridView)
			{
				node.Address = GetSelectedModule()?.Start ?? IntPtr.Zero;
			}
			else
			{
				node.Address = GetSelectedSection()?.Start ?? IntPtr.Zero;
			}

			classesView.SelectedClass = node;
		}

		private void dumpToolStripMenuItem_Click(object sender, EventArgs e)
		{
			bool isModule;
			string fileName;
			var initialDirectory = string.Empty;
			IntPtr address;
			int size;

			if (GetToolStripSourceControl(sender) == modulesDataGridView)
			{
				var module = GetSelectedModule();
				if (module == null)
				{
					return;
				}

				isModule = true;
				fileName = $"{Path.GetFileNameWithoutExtension(module.Name)}_Dumped{Path.GetExtension(module.Name)}";
				initialDirectory = Path.GetDirectoryName(module.Path);
				address = module.Start;
				size = module.Size.ToInt32();
			}
			else
			{
				var section = GetSelectedSection();
				if (section == null)
				{
					return;
				}

				isModule = false;
				fileName = $"Section_{section.Start.ToString("X")}_{section.End.ToString("X")}.dat";
				address = section.Start;
				size = section.Size.ToInt32();
			}

			using (var sfd = new SaveFileDialog())
			{
				sfd.FileName = fileName;
				sfd.Filter = "All|*.*";
				sfd.InitialDirectory = initialDirectory;

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					var dumper = new Dumper(process);

					try
					{
						using (var stream = sfd.OpenFile())
						{
							if (isModule)
							{
								dumper.DumpModule(address, size, stream);
							}
							else
							{
								dumper.DumpSection(address, size, stream);
							}

							MessageBox.Show("Module successfully dumped.", "ReClass.NET", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						Program.ShowException(ex);
					}
				}
			}
		}

		private void sectionsDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			setCurrentClassAddressToolStripMenuItem_Click(sender, e);

			Close();
		}

		#endregion

		private Control GetToolStripSourceControl(object sender)
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
