using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.AddressParser;
using ReClassNET.CodeGenerator;
using ReClassNET.Core;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.Project;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class MainForm : IconForm
	{
		private readonly PluginManager pluginManager;

		private ReClassNetProject currentProject;
		public ReClassNetProject CurrentProject => currentProject;

		private ClassNode currentClassNode;

		private readonly MemoryBuffer memoryViewBuffer = new MemoryBuffer();

		private Task updateProcessInformationsTask;
		private Task loadSymbolsTask;
		private CancellationTokenSource loadSymbolsTaskToken;

		public ProjectView ProjectView => projectView;

		public MenuStrip MainMenu => mainMenuStrip;

		public ClassNode CurrentClassNode
		{
			get => currentClassNode;
			set
			{
				currentClassNode = value;

				projectView.SelectedClass = value;

				memoryViewControl.Reset();
				memoryViewControl.Invalidate();
			}
		}

		private void UpdateWindowTitle(string extra = null)
		{
			var title = $"{(Program.Settings.RandomizeWindowTitle ? Utils.RandomString(Program.GlobalRandom.Next(15, 20)) : Constants.ApplicationName)} ({Constants.Platform})";
			if (!string.IsNullOrEmpty(extra))
			{
				title += $" - {extra}";
			}
			Text = title;
		}

		public MainForm()
		{
			Contract.Ensures(pluginManager != null);
			Contract.Ensures(currentProject != null);

			InitializeComponent();
			UpdateWindowTitle();

			mainMenuStrip.Renderer = new CustomToolStripProfessionalRenderer(true, true);
			toolStrip.Renderer = new CustomToolStripProfessionalRenderer(true, false);

			Program.RemoteProcess.ProcessAttached += sender =>
			{
				var text = $"{sender.UnderlayingProcess.Name} (ID: {sender.UnderlayingProcess.Id.ToString()})";
				processInfoToolStripStatusLabel.Text = text;
				UpdateWindowTitle(text);

			};
			Program.RemoteProcess.ProcessClosed += sender =>
			{
				UpdateWindowTitle();
				processInfoToolStripStatusLabel.Text = "No process selected";
			};

			pluginManager = new PluginManager(new DefaultPluginHost(this, Program.RemoteProcess, Program.Logger));
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);

			pluginManager.LoadAllPlugins(Path.Combine(Application.StartupPath, Constants.PluginsFolder), Program.Logger);

			toolStrip.Items.AddRange(NodeTypesBuilder.CreateToolStripButtons(ReplaceSelectedNodesWithType).ToArray());
			changeTypeToolStripMenuItem.DropDownItems.AddRange(NodeTypesBuilder.CreateToolStripMenuItems(ReplaceSelectedNodesWithType, false).ToArray());

			var createDefaultProject = true;

			if (Program.CommandLineArgs.FileName != null)
			{
				try
				{
					LoadProjectFromPath(Program.CommandLineArgs.FileName);

					createDefaultProject = false;
				}
				catch (Exception ex)
				{
					Program.Logger.Log(ex);
				}
			}
			
			if (createDefaultProject)
			{
				SetProject(new ReClassNetProject());

				LinkedWindowFeatures.CreateDefaultClass();
			}

			if (Program.CommandLineArgs[Constants.CommandLineOptions.AttachTo] != null)
			{
				AttachToProcess(Program.CommandLineArgs[Constants.CommandLineOptions.AttachTo]);
			}
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			pluginManager.UnloadAllPlugins();

			GlobalWindowManager.RemoveWindow(this);

			base.OnFormClosed(e);
		}

		private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Stop the update timer
			processUpdateTimer.Stop();

			// and cancel all running tasks.
			if (loadSymbolsTask != null || updateProcessInformationsTask != null)
			{
				e.Cancel = true;

				Hide();

				if (loadSymbolsTask != null)
				{
					loadSymbolsTaskToken.Cancel();

					try
					{
						await loadSymbolsTask;
					}
					catch
					{

					}

					loadSymbolsTask = null;
				}

				if (updateProcessInformationsTask != null)
				{
					try
					{
						await updateProcessInformationsTask;
					}
					catch
					{

					}

					updateProcessInformationsTask = null;
				}

				Close();
			}
		}

		#region Menustrip

		private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
		{
			var lastProcess = Program.Settings.LastProcess;
			if (string.IsNullOrEmpty(lastProcess))
			{
				reattachToProcessToolStripMenuItem.Visible = false;
			}
			else
			{
				reattachToProcessToolStripMenuItem.Visible = true;
				reattachToProcessToolStripMenuItem.Text = $"Re-Attach to '{lastProcess}'";
			}
		}

		private void reattachToProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var lastProcess = Program.Settings.LastProcess;
			if (string.IsNullOrEmpty(lastProcess))
			{
				return;
			}

			AttachToProcess(lastProcess);
		}

		private void detachToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.RemoteProcess.Close();
		}

		private void newClassToolStripButton_Click(object sender, EventArgs e)
		{
			LinkedWindowFeatures.CreateDefaultClass();
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				var path = ShowOpenProjectFileDialog();
				if (path != null)
				{
					LoadProjectFromPath(path);
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Log(ex);
			}
		}

		private void mergeWithProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				var path = ShowOpenProjectFileDialog();
				if (path != null)
				{
					LoadProjectFromPath(path, ref currentProject);
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Log(ex);
			}
		}

		private void goToClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var csf = new ClassSelectionForm(currentProject.Classes.OrderBy(c => c.Name)))
			{
				if (csf.ShowDialog() == DialogResult.OK)
				{
					var selectedClassNode = csf.SelectedClass;
					if (selectedClassNode != null)
					{
						projectView.SelectedClass = selectedClassNode;
					}
				}
			}
		}

		private void clearProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetProject(new ReClassNetProject());
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!currentProject.Classes.Any())
			{
				return;
			}

			if (string.IsNullOrEmpty(currentProject.Path))
			{
				saveAsToolStripMenuItem_Click(sender, e);

				return;
			}

			var file = new ReClassNetFile(currentProject);
			file.Save(currentProject.Path, Program.Logger);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!currentProject.Classes.Any())
			{
				return;
			}

			using (var sfd = new SaveFileDialog())
			{
				sfd.DefaultExt = ReClassNetFile.FileExtension;
				sfd.Filter = $"{ReClassNetFile.FormatName} (*{ReClassNetFile.FileExtension})|*{ReClassNetFile.FileExtension}";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					currentProject.Path = sfd.FileName;

					saveToolStripMenuItem_Click(sender, e);
				}
			}
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sd = new SettingsForm(Program.Settings, CurrentProject.TypeMapping))
			{
				sd.ShowDialog();
			}
		}

		private void pluginsToolStripButton_Click(object sender, EventArgs e)
		{
			using (var pf = new PluginForm(pluginManager))
			{
				pf.ShowDialog();
			}
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void memoryViewerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ProcessInfoForm(Program.RemoteProcess).Show();
		}

		private void memorySearcherToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ScannerForm(Program.RemoteProcess).Show();
		}

		private void namedAddressesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new NamedAddressesForm(Program.RemoteProcess).Show();
		}

		private void loadSymbolToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.Filter = "Program Debug Database (*.pdb)|*.pdb|All Files (*.*)|*.*";

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					try
					{
						Program.RemoteProcess.Symbols.LoadSymbolsFromPDB(ofd.FileName);
					}
					catch (Exception ex)
					{
						Program.Logger.Log(ex);
					}
				}
			}
		}

		private void loadSymbolsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LoadAllSymbolsForCurrentProcess();
		}

		private void ControlRemoteProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.RemoteProcess.IsValid)
			{
				return;
			}

			var action = ControlRemoteProcessAction.Terminate;
			if (sender == resumeProcessToolStripMenuItem)
			{
				action = ControlRemoteProcessAction.Resume;
			}
			else if (sender == suspendProcessToolStripMenuItem)
			{
				action = ControlRemoteProcessAction.Suspend;
			}

			Program.RemoteProcess.ControlRemoteProcess(action);
		}

		private void cleanUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			currentProject.RemoveUnusedClasses();
		}

		private void generateCppCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeGeneratorForm(new CppCodeGenerator(currentProject.TypeMapping));
		}

		private void generateCSharpCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeGeneratorForm(new CSharpCodeGenerator());
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var af = new AboutForm())
			{
				af.ShowDialog();
			}
		}

		#endregion

		#region Toolstrip

		private void attachToProcessToolStripSplitButton_ButtonClick(object sender, EventArgs e)
		{
			using (var pb = new ProcessBrowserForm(Program.Settings.LastProcess))
			{
				if (pb.ShowDialog() == DialogResult.OK)
				{
					if (pb.SelectedProcess != null)
					{
						AttachToProcess(pb.SelectedProcess);

						if (pb.LoadSymbols)
						{
							LoadAllSymbolsForCurrentProcess();
						}
					}
				}
			}
		}

		private void attachToProcessToolStripSplitButton_DropDownClosed(object sender, EventArgs e)
		{
			attachToProcessToolStripSplitButton.DropDownItems.Clear();
		}

		private void attachToProcessToolStripSplitButton_DropDownOpening(object sender, EventArgs e)
		{
			attachToProcessToolStripSplitButton.DropDownItems.AddRange(
				Program.CoreFunctions.EnumerateProcesses()
					.OrderBy(p => p.Name).ThenBy(p => p.Id, IntPtrComparer.Instance)
					.Select(p => new ToolStripMenuItem($"[{p.Id}] {p.Name}", p.Icon, (sender2, e2) => AttachToProcess(p)))
					.Cast<ToolStripItem>()
					.ToArray()
			);
		}

		private void selectedNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();

			var count = selectedNodes.Count;
			var node = selectedNodes.Select(s => s.Node).FirstOrDefault();
			var parentNode = node?.GetParentContainer();

			var nodeIsClass = node is ClassNode;
			var nodeIsSearchableValueNode = false;
			switch (node)
			{
				case BaseHexNode _:
				case FloatNode _:
				case DoubleNode _:
				case Int8Node _:
				case UInt8Node _:
				case Int16Node _:
				case UInt16Node _:
				case Int32Node _:
				case UInt32Node _:
				case Int64Node _:
				case UInt64Node _:
				case Utf8TextNode _:
				case Utf16TextNode _:
				case Utf32TextNode _:
					nodeIsSearchableValueNode = true;
					break;
			}

			addBytesToolStripMenuItem.Enabled = parentNode != null || nodeIsClass;
			insertBytesToolStripMenuItem.Enabled = count == 1 && parentNode != null;

			changeTypeToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;

			createClassFromNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;
			dissectNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;
			searchForEqualValuesToolStripMenuItem.Enabled = count == 1 && nodeIsSearchableValueNode;

			pasteNodesToolStripMenuItem.Enabled = count == 1 && ReClassClipboard.ContainsNodes;
			removeToolStripMenuItem.Enabled = !nodeIsClass;

			copyAddressToolStripMenuItem.Enabled = !nodeIsClass;

			showCodeOfClassToolStripMenuItem.Enabled = nodeIsClass;
			shrinkClassToolStripMenuItem.Enabled = nodeIsClass;

			hideNodesToolStripMenuItem.Enabled = selectedNodes.All(h => !(h.Node is ClassNode));

			unhideChildNodesToolStripMenuItem.Enabled = count == 1 && node is BaseContainerNode bcn && bcn.Nodes.Any(n => n.IsHidden);
			unhideNodesAboveToolStripMenuItem.Enabled = count == 1 && parentNode != null && parentNode.TryGetPredecessor(node, out var predecessor) && predecessor.IsHidden;
			unhideNodesBelowToolStripMenuItem.Enabled = count == 1 && parentNode != null && parentNode.TryGetSuccessor(node, out var successor) && successor.IsHidden;
		}

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!(sender is IntegerToolStripMenuItem item))
			{
				return;
			}

			AddBytesToClass(item.Value);
		}

		private void addXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Add Bytes", AddBytesToClass);
		}

		private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!(sender is IntegerToolStripMenuItem item))
			{
				return;
			}

			InsertBytesInClass(item.Value);
		}

		private void insertXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Insert Bytes", InsertBytesInClass);
		}

		private void createClassFromNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();

			if (selectedNodes.Count > 0 && !(selectedNodes[0].Node is ClassNode))
			{
				if (selectedNodes[0].Node.GetParentContainer() is ClassNode parentNode)
				{
					var newClassNode = ClassNode.Create();
					selectedNodes.Select(h => h.Node).ForEach(newClassNode.AddNode);

					var classInstanceNode = new ClassInstanceNode();
					classInstanceNode.ChangeInnerNode(newClassNode);

					parentNode.InsertNode(selectedNodes[0].Node, classInstanceNode);

					selectedNodes.Select(h => h.Node).ForEach(c => parentNode.RemoveNode(c));

					ClearSelection();
				}
			}
		}

		private void dissectNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var hexNodes = memoryViewControl.GetSelectedNodes().Where(h => h.Node is BaseHexNode).ToList();
			if (!hexNodes.Any())
			{
				return;
			}

			foreach (var g in hexNodes.GroupBy(n => n.Node.GetParentContainer()))
			{
				NodeDissector.DissectNodes(g.Select(h => (BaseHexNode)h.Node), Program.RemoteProcess, g.First().Memory);
			}

			ClearSelection();
		}

		private void searchForEqualValuesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selectedNode = memoryViewControl.GetSelectedNodes().FirstOrDefault();
			if (selectedNode == null)
			{
				return;
			}

			IScanComparer comparer;
			switch (selectedNode.Node)
			{
				case BaseHexNode node:
					comparer = new ArrayOfBytesMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory));
					break;
				case FloatNode node:
					comparer = new FloatMemoryComparer(ScanCompareType.Equal, ScanRoundMode.Normal, 2, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case DoubleNode node:
					comparer = new DoubleMemoryComparer(ScanCompareType.Equal, ScanRoundMode.Normal, 2, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int8Node node:
					comparer = new ByteMemoryComparer(ScanCompareType.Equal, (byte)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt8Node node:
					comparer = new ByteMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int16Node node:
					comparer = new ShortMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt16Node node:
					comparer = new ShortMemoryComparer(ScanCompareType.Equal, (short)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int32Node node:
					comparer = new IntegerMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt32Node node:
					comparer = new IntegerMemoryComparer(ScanCompareType.Equal, (int)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int64Node node:
					comparer = new LongMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt64Node node:
					comparer = new LongMemoryComparer(ScanCompareType.Equal, (long)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Utf8TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.UTF8, true);
					break;
				case Utf16TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.Unicode, true);
					break;
				case Utf32TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.UTF32, true);
					break;
				default:
					return;
			}

			LinkedWindowFeatures.StartMemoryScan(comparer);
		}

		private void findOutWhatAccessesThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedNode(false);
		}

		private void findOutWhatWritesToThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedNode(true);
		}

		private void copyNodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopySelectedNodesToClipboard();
		}

		private void pasteNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PasteNodeFromClipboardToSelection();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedNodes();
		}

		private void hideNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HideSelectedNodes();
		}

		private void unhideChildNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideChildNodes();
		}

		private void unhideNodesAboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideNodesAbove();
		}

		private void unhideNodesBelowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideNodesBelow();
		}

		private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count > 0)
			{
				Clipboard.SetText(selectedNodes.First().Address.ToString("X"));
			}
		}

		private void showCodeOfClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (memoryViewControl.GetSelectedNodes().FirstOrDefault()?.Node is ClassNode node)
			{
				ShowPartialCodeGeneratorForm(new[] { node });
			}
		}

		private void shrinkClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var node = memoryViewControl.GetSelectedNodes().Select(s => s.Node).FirstOrDefault();
			if (!(node is ClassNode classNode))
			{
				return;
			}

			foreach (var nodeToDelete in classNode.Nodes.Reverse().TakeWhile(n => n is BaseHexNode))
			{
				classNode.RemoveNode(nodeToDelete);
			}
		}

		#endregion

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Any())
				{
					switch (Path.GetExtension(files.First()))
					{
						case ReClassNetFile.FileExtension:
						case ReClassQtFile.FileExtension:
						case ReClassFile.FileExtension:
							e.Effect = DragDropEffects.Copy;
							break;
					}
				}
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			if (e.Data.GetData(DataFormats.FileDrop) is string[] files && files.Any())
			{
				try
				{
					var path = files.First();

					LoadProjectFromPath(path);
				}
				catch (Exception ex)
				{
					Program.Logger.Log(ex);
				}
			}
		}

		private void processUpdateTimer_Tick(object sender, EventArgs e)
		{
			if (updateProcessInformationsTask != null && !updateProcessInformationsTask.IsCompleted)
			{
				return;
			}

			updateProcessInformationsTask = Program.RemoteProcess.UpdateProcessInformationsAsync();
		}

		private void classesView_ClassSelected(object sender, ClassNode node)
		{
			CurrentClassNode = node;
		}

		private void memoryViewControl_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.C)
				{
					CopySelectedNodesToClipboard();
				}
				else if (e.KeyCode == Keys.V)
				{
					PasteNodeFromClipboardToSelection();
				}
			}
			else if (e.KeyCode == Keys.Delete)
			{
				RemoveSelectedNodes();
			}
		}

		private void memoryViewControl_SelectionChanged(object sender, EventArgs e)
		{
			if (!(sender is MemoryViewControl memoryView))
			{
				return;
			}

			var selectedNodes = memoryView.GetSelectedNodes();

			var node = selectedNodes.FirstOrDefault()?.Node;
			var parentContainer = node?.GetParentContainer();

			addBytesToolStripDropDownButton.Enabled = parentContainer != null || node is ClassNode;
			insertBytesToolStripDropDownButton.Enabled = selectedNodes.Count == 1 && parentContainer != null;

			var enabled = selectedNodes.Count > 0 && !(node is ClassNode);
			toolStrip.Items.OfType<TypeToolStripButton>().ForEach(b => b.Enabled = enabled);
		}

		private void memoryViewControl_ChangeClassTypeClick(object sender, NodeClickEventArgs e)
		{
			var classes = CurrentProject.Classes.OrderBy(c => c.Name);

			if (e.Node is FunctionNode functionNode)
			{
				var noneClass = new ClassNode(false)
				{
					Name = "None"
				};

				using (var csf = new ClassSelectionForm(classes.Prepend(noneClass)))
				{
					if (csf.ShowDialog() == DialogResult.OK)
					{
						var selectedClassNode = csf.SelectedClass;
						if (selectedClassNode != null)
						{
							if (selectedClassNode == noneClass)
							{
								selectedClassNode = null;
							}

							functionNode.BelongsToClass = selectedClassNode;
						}
					}
				}
			}
			else if (e.Node is BaseWrapperNode refNode)
			{
				using (var csf = new ClassSelectionForm(classes))
				{
					if (csf.ShowDialog() == DialogResult.OK)
					{
						var selectedClassNode = csf.SelectedClass;
						if (refNode.CanChangeInnerNodeTo(selectedClassNode))
						{
							if (!refNode.GetRootWrapperNode().ShouldPerformCycleCheckForInnerNode() || IsCycleFree(e.Node.GetParentClass(), selectedClassNode))
							{
								refNode.ChangeInnerNode(selectedClassNode);
							}
						}
					}
				}
			}
		}

		private void memoryViewControl_ChangeWrappedTypeClick(object sender, NodeClickEventArgs e)
		{
			if (e.Node is BaseWrapperNode wrapperNode)
			{
				var items = NodeTypesBuilder.CreateToolStripMenuItems(t =>
				{
					var node = BaseNode.CreateInstanceFromType(t);
					if (wrapperNode.CanChangeInnerNodeTo(node))
					{
						wrapperNode.ChangeInnerNode(node);
					}
				}, wrapperNode.CanChangeInnerNodeTo(null));

				var menu = new ContextMenuStrip();
				menu.Items.AddRange(items.ToArray());
				menu.Show(this, e.Location);
			}
		}

		private void memoryViewControl_ChangeEnumTypeClick(object sender, NodeClickEventArgs e)
		{
			if (e.Node is EnumNode enumNode)
			{
				using (var csf = new EnumSelectionForm(CurrentProject))
				{
					var size = enumNode.Enum.Size;

					if (csf.ShowDialog() == DialogResult.OK)
					{
						var @enum = csf.SelectedItem;
						if (@enum != null)
						{
							enumNode.ChangeEnum(@enum);
						}
					}

					if (size != enumNode.Enum.Size)
					{
						// Update the parent container because the enum size has changed.
						enumNode.GetParentContainer()?.ChildHasChanged(enumNode);
					}
				}

				foreach (var @enum in CurrentProject.Enums)
				{
					projectView.UpdateEnumNode(@enum);
				}
			}
		}

		private void showCodeOfClassToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			var classNode = projectView.SelectedClass;
			if (classNode == null)
			{
				return;
			}

			ShowPartialCodeGeneratorForm(new[] { classNode });
		}

		private void enableHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var isChecked = !enableHierarchyViewToolStripMenuItem.Checked;

			enableHierarchyViewToolStripMenuItem.Checked = isChecked;

			expandAllClassesToolStripMenuItem.Enabled = collapseAllClassesToolStripMenuItem.Enabled = isChecked;

			projectView.EnableClassHierarchyView = isChecked;
		}

		private void autoExpandHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var isChecked = !autoExpandHierarchyViewToolStripMenuItem.Checked;

			autoExpandHierarchyViewToolStripMenuItem.Checked = isChecked;

			projectView.AutoExpandClassNodes = isChecked;
		}

		private void expandAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			projectView.ExpandAllClassNodes();
		}

		private void collapseAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			projectView.CollapseAllClassNodes();
		}

		private void removeUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CurrentProject.RemoveUnusedClasses();
		}

		private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var classNode = projectView.SelectedClass;
			if (classNode == null)
			{
				return;
			}

			try
			{
				CurrentProject.Remove(classNode);
			}
			catch (ClassReferencedException ex)
			{
				Program.Logger.Log(ex);
			}
		}

		private void editEnumsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var elf = new EnumListForm(currentProject))
			{
				elf.ShowDialog();
			}
		}

		private void editEnumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var @enum = projectView.SelectedEnum;
			if (@enum != null)
			{
				using (var eef = new EnumEditorForm(@enum))
				{
					eef.ShowDialog();
				}
			}
		}

		private void showEnumsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var elf = new EnumListForm(currentProject))
			{
				elf.ShowDialog();
			}
		}

		private void memoryViewControl_DrawContextRequested(object sender, DrawContextRequestEventArgs args)
		{
			var process = Program.RemoteProcess;

			var classNode = CurrentClassNode;
			if (classNode != null)
			{
				memoryViewBuffer.Size = classNode.MemorySize;

				IntPtr address;
				try
				{
					address = process.ParseAddress(classNode.AddressFormula);
				}
				catch (ParseException)
				{
					address = IntPtr.Zero;
				}
				memoryViewBuffer.UpdateFrom(process, address);

				args.Settings = Program.Settings;
				args.Process = process;
				args.Memory = memoryViewBuffer;
				args.Node = classNode;
				args.BaseAddress = address;
			}
		}
	}
}
