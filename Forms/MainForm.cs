using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.CodeGenerator;
using ReClassNET.Core;
using ReClassNET.DataExchange;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class MainForm : IconForm
	{
		private readonly CoreFunctionsManager coreFunctions;

		private readonly RemoteProcess remoteProcess;

		private readonly PluginManager pluginManager;

		private ReClassNetProject currentProject;
		public ReClassNetProject CurrentProject => currentProject;

		private Task updateProcessInformationsTask;
		private Task loadSymbolsTask;
		private CancellationTokenSource loadSymbolsTaskToken;

		public ClassNodeView ClassView => classesView;

		public MenuStrip MainMenu => mainMenuStrip;

		public MainForm(CoreFunctionsManager coreFunctions)
		{
			Contract.Requires(coreFunctions != null);
			Contract.Ensures(remoteProcess != null);
			Contract.Ensures(pluginManager != null);
			Contract.Ensures(currentProject != null);

			this.coreFunctions = coreFunctions;

			InitializeComponent();

			mainMenuStrip.Renderer = new CustomToolStripProfessionalRenderer(true);
			toolStrip.Renderer = new CustomToolStripProfessionalRenderer(false);

			remoteProcess = new RemoteProcess(coreFunctions);
			remoteProcess.ProcessAttached += (sender) =>
			{
				var text = $"{sender.UnderlayingProcess.Name} (PID: {sender.UnderlayingProcess.Id.ToString()})";

				Text = $"{Constants.ApplicationName} - {text}";
				processInfoToolStripStatusLabel.Text = text;
			};
			remoteProcess.ProcessClosed += (sender) =>
			{
				Text = Constants.ApplicationName;
				processInfoToolStripStatusLabel.Text = "No process selected";
			};

			memoryViewControl.Memory = new MemoryBuffer
			{
				Process = remoteProcess
			};

			pluginManager = new PluginManager(new DefaultPluginHost(this, remoteProcess, Program.Logger), coreFunctions);

			SetProject(new ReClassNetProject());

			newClassToolStripButton_Click(null, null);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);

			pluginManager.LoadAllPlugins(Path.Combine(Application.StartupPath, Constants.PluginsFolder), Program.Logger);
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			pluginManager.UnloadAllPlugins();

			GlobalWindowManager.RemoveWindow(this);

			base.OnFormClosed(e);
		}

		#region Event Handler

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

			remoteProcess.Dispose();
		}

		#region Menustrip

		private void attachToProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var pb = new ProcessBrowserForm(coreFunctions, Program.Settings.LastProcess))
			{
				if (pb.ShowDialog() == DialogResult.OK)
				{
					if (pb.SelectedProcess != null)
					{
						remoteProcess.Close();

						remoteProcess.Open(pb.SelectedProcess);
						remoteProcess.UpdateProcessInformations();
						if (pb.LoadSymbols)
						{
							LoadAllSymbolsForCurrentProcess();
						}

						Program.Settings.LastProcess = remoteProcess.UnderlayingProcess.Name;
					}
				}
			}
		}

		private void detachToolStripMenuItem_Click(object sender, EventArgs e)
		{
			remoteProcess.Close();
		}

		private void newClassToolStripButton_Click(object sender, EventArgs e)
		{
			CreateNewDefaultClass();
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				var path = ShowOpenProjectFileDialog();
				if (path != null)
				{
					var project = new ReClassNetProject();

					LoadFileFromPath(path, ref project);

					// If the file is a ReClass.NET file remember the path.
					if (Path.GetExtension(path) == ReClassNetFile.FileExtension)
					{
						project.Path = path;
					}

					SetProject(project);
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
					LoadFileFromPath(path, ref currentProject);
				}
			}
			catch (Exception ex)
			{
				Program.Logger.Log(ex);
			}
		}

		private void clearProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetProject(new ReClassNetProject());

			memoryViewControl.ClassNode = null;
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
			using (var sd = new SettingsForm(Program.Settings))
			{
				sd.ShowDialog();
			}
		}

		private void pluginsToolStripButton_Click(object sender, EventArgs e)
		{
			using (var pf = new PluginForm(pluginManager, coreFunctions))
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
			new ProcessInfoForm(remoteProcess, classesView).Show();
		}

		private void ControlRemoteProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!remoteProcess.IsValid)
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

			remoteProcess.ControlRemoteProcess(action);
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
						remoteProcess.Symbols.LoadSymbolsFromPDB(ofd.FileName);
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

		private void cleanUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			currentProject.RemoveUnusedClasses();
		}

		private void generateCppCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeForm(new CppCodeGenerator());
		}

		private void generateCSharpCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeForm(new CSharpCodeGenerator());
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

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			memoryViewControl.AddBytes(item.Value);
		}

		private void addXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Add Bytes", memoryViewControl.AddBytes);
		}

		private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			memoryViewControl.InsertBytes(item.Value);
		}

		private void insertXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Insert Bytes", memoryViewControl.InsertBytes);
		}

		private void memoryTypeToolStripButton_Click(object sender, EventArgs e)
		{
			var item = sender as TypeToolStripButton;
			if (item == null)
			{
				return;
			}

			memoryViewControl.ReplaceSelectedNodesWithType(item.Value);
		}

		#endregion

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var files = e.Data.GetData(DataFormats.FileDrop) as string[];
				if (files != null && files.Any())
				{
					switch (Path.GetExtension(files.First()))
					{
						case ReClassNetFile.FileExtension:
						case ReClassQtFile.FileExtension:
						case ReClassFile.FileExtension:
						case ReClass2007File.FileExtension:
							e.Effect = DragDropEffects.Copy;
							break;
					}
				}
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			var files = e.Data.GetData(DataFormats.FileDrop) as string[];
			if (files != null && files.Any())
			{
				try
				{
					var path = files.First();

					var project = new ReClassNetProject();

					LoadFileFromPath(path, ref project);

					// If the file is a ReClass.NET file remember the path.
					if (Path.GetExtension(path) == ReClassNetFile.FileExtension)
					{
						project.Path = path;
					}

					SetProject(project);
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

			updateProcessInformationsTask = remoteProcess.UpdateProcessInformationsAsync();
		}

		private void classesView_ClassSelected(object sender, ClassNode node)
		{
			memoryViewControl.ClassNode = node;

			memoryViewControl.Invalidate();
		}

		private void memoryViewControl_SelectionChanged(object sender, EventArgs e)
		{
			var memoryView = sender as MemoryViewControl;
			if (memoryView == null)
			{
				return;
			}

			var count = memoryView.SelectedNodes.Count();
			var node = memoryView.SelectedNodes.FirstOrDefault();

			addBytesToolStripDropDownButton.Enabled = node?.ParentNode != null || node is ClassNode;
			insertBytesToolStripDropDownButton.Enabled = count == 1 && node?.ParentNode != null;

			var enabled = count > 0 && !(node is ClassNode);
			toolStrip.Items.OfType<TypeToolStripButton>().ForEach(b => b.Enabled = enabled);
		}

		#endregion

		/// <summary>Creates a new default class.</summary>
		public void CreateNewDefaultClass()
		{
			var node = ClassNode.Create();
			node.AddBytes(64);

			var mainModule = remoteProcess.GetModuleByName(remoteProcess.UnderlayingProcess?.Name);
			if (mainModule != null)
			{
				node.Address = mainModule.Start;
			}

			classesView.SelectedClass = node;
		}

		/// <summary>Sets the current project.</summary>
		/// <param name="newProject">The new project.</param>
		public void SetProject(ReClassNetProject newProject)
		{
			Contract.Requires(newProject != null);

			if (currentProject == newProject)
			{
				return;
			}

			if (currentProject != null)
			{
				ClassNode.ClassCreated -= currentProject.AddClass;
			}

			currentProject = newProject;

			ClassUtil.Classes = currentProject.Classes;

			ClassNode.ClassCreated += currentProject.AddClass;

			classesView.Project = currentProject;
			memoryViewControl.Project = currentProject;

			memoryViewControl.ClassNode = currentProject.Classes.FirstOrDefault();
		}

		/// <summary>Registers the node type which will create the ToolStrip and MenuStrip entries.</summary>
		/// <param name="type">The node type.</param>
		/// <param name="name">The name of the node type.</param>
		/// <param name="icon">The icon of the node type.</param>
		internal void RegisterNodeType(Type type, string name, Image icon)
		{
			Contract.Requires(type != null);
			Contract.Requires(name != null);
			Contract.Requires(icon != null);

			var item = new TypeToolStripButton
			{
				Image = icon,
				ToolTipText = name,
				Value = type
			};
			item.Click += memoryTypeToolStripButton_Click;

			toolStrip.Items.Add(item);

			memoryViewControl.RegisterNodeType(type, name, icon);
		}

		/// <summary>Deregisters the node type.</summary>
		/// <param name="type">The node type.</param>
		internal void DeregisterNodeType(Type type)
		{
			Contract.Requires(type != null);

			var item = toolStrip.Items.OfType<TypeToolStripButton>().FirstOrDefault(i => i.Value == type);
			if (item != null)
			{
				item.Click -= memoryTypeToolStripButton_Click;
				toolStrip.Items.Remove(item);
			}

			memoryViewControl.DeregisterNodeType(type);
		}

		/// <summary>Shows the code form with the given <paramref name="generator"/>.</summary>
		/// <param name="generator">The generator.</param>
		private void ShowCodeForm(ICodeGenerator generator)
		{
			Contract.Requires(generator != null);

			new CodeForm(generator, currentProject.Classes, Program.Logger).Show();
		}

		/// <summary>Opens the <see cref="InputBytesForm"/> and calls <paramref name="callback"/> with the result.</summary>
		/// <param name="title">The title of the input form.</param>
		/// <param name="callback">The function to call afterwards.</param>
		private void AskAddOrInsertBytes(string title, Action<int> callback)
		{
			Contract.Requires(title != null);
			Contract.Requires(callback != null);

			if (memoryViewControl.ClassNode == null)
			{
				return;
			}

			using (var ib = new InputBytesForm(memoryViewControl.ClassNode.MemorySize))
			{
				ib.Text = title;

				if (ib.ShowDialog() == DialogResult.OK)
				{
					callback(ib.Bytes);
				}
			}
		}

		/// <summary>Shows an <see cref="OpenFileDialog"/> with all valid file extensions.</summary>
		/// <returns>The path to the selected file or null if no file was selected.</returns>
		public static string ShowOpenProjectFileDialog()
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.CheckFileExists = true;
				ofd.Filter = $"All ReClass Types |*{ReClassNetFile.FileExtension};*{ReClassFile.FileExtension};*{ReClassQtFile.FileExtension};*{ReClass2007File.FileExtension}"
					+ $"|{ReClassNetFile.FormatName} (*{ReClassNetFile.FileExtension})|*{ReClassNetFile.FileExtension}"
					+ $"|{ReClassFile.FormatName} (*{ReClassFile.FileExtension})|*{ReClassFile.FileExtension}"
					+ $"|{ReClassQtFile.FormatName} (*{ReClassQtFile.FileExtension})|*{ReClassQtFile.FileExtension}"
					+ $"|{ReClass2007File.FormatName} (*{ReClass2007File.FileExtension})|*{ReClass2007File.FileExtension}";

				if (ofd.ShowDialog() == DialogResult.OK)
				{
					return ofd.FileName;
				}
			}

			return null;
		}

		/// <summary>Loads the file into the given project.</summary>
		/// <param name="filePath">Full pathname of the file.</param>
		/// <param name="project">[in,out] The project.</param>
		private void LoadFileFromPath(string filePath, ref ReClassNetProject project)
		{
			Contract.Requires(filePath != null);
			Contract.Requires(project != null);
			Contract.Ensures(Contract.ValueAtReturn(out project) != null);

			IReClassImport import = null;
			switch (Path.GetExtension(filePath))
			{
				case ReClassNetFile.FileExtension:
					import = new ReClassNetFile(project);
					break;
				case ReClassQtFile.FileExtension:
					import = new ReClassQtFile(project);
					break;
				case ReClassFile.FileExtension:
					import = new ReClassFile(project);
					break;
				case ReClass2007File.FileExtension:
					import = new ReClass2007File(project);
					break;
				default:
					Program.Logger.Log(LogLevel.Error, $"The file '{filePath}' has an unknown type.");
					break;
			}
			import?.Load(filePath, Program.Logger);
		}

		/// <summary>Loads all symbols for the current process and displays the progress status.</summary>
		private void LoadAllSymbolsForCurrentProcess()
		{
			if (loadSymbolsTask != null && !loadSymbolsTask.IsCompleted)
			{
				return;
			}

			infoToolStripStatusLabel.Visible = true;

			int index = 0;

			var progress = new Progress<Tuple<Module, IEnumerable<Module>>>(
				report =>
				{
					infoToolStripStatusLabel.Text = $"[{++index}/{report.Item2.Count()}] Loading symbols for module: {report.Item1.Name}";
				}
			);

			loadSymbolsTaskToken = new CancellationTokenSource();

			loadSymbolsTask = remoteProcess
				.LoadAllSymbolsAsync(progress, loadSymbolsTaskToken.Token)
				.ContinueWith(_ => infoToolStripStatusLabel.Visible = false, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
