using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.CodeGenerator;
using ReClassNET.DataExchange;
using ReClassNET.Logger;
using ReClassNET.Nodes;
using ReClassNET.Plugins;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class MainForm : IconForm
	{
		private readonly NativeHelper nativeHelper;
		private readonly Settings settings;

		private readonly RemoteProcess remoteProcess;
		private readonly Memory memory;

		private readonly PluginManager pluginManager;

		private readonly ILogger logger;

		private string projectPath;

		private Task updateProcessInformationsTask;
		private Task loadSymbolsTask;
		private CancellationTokenSource loadSymbolsTaskToken;

		public MainForm(NativeHelper nativeHelper, Settings settings)
		{
			Contract.Requires(nativeHelper != null);
			Contract.Requires(settings != null);

			this.nativeHelper = nativeHelper;
			this.settings = settings;

			InitializeComponent();

			mainMenuStrip.Renderer = new CustomToolStripProfessionalRenderer(true);
			toolStrip.Renderer = new CustomToolStripProfessionalRenderer(false);

			logger = new GuiLogger();

			remoteProcess = new RemoteProcess(nativeHelper);
			remoteProcess.ProcessChanged += delegate (RemoteProcess sender)
			{
				if (sender.Process == null)
				{
					processInfoToolStripStatusLabel.Text = "No process selected";
				}
				else
				{
					var text = $"{sender.Process.Name} (PID: {sender.Process.Id})";

					Text = $"{Constants.ApplicationName} {text}";
					processInfoToolStripStatusLabel.Text = text;
				}
			};

			memory = new Memory
			{
				Process = remoteProcess
			};

			memoryViewControl.Settings = settings;
			memoryViewControl.Memory = memory;

			pluginManager = new PluginManager(new DefaultPluginHost(this, remoteProcess, logger), nativeHelper);

			ClassManager.ClassAdded += c => classesView.Add(c);
			ClassManager.ClassRemoved += c => classesView.Remove(c);

			newClassToolStripButton_Click(null, null);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			GlobalWindowManager.AddWindow(this);

			pluginManager.LoadAllPlugins(Path.Combine(Application.StartupPath, Constants.PluginsFolder));
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			pluginManager.UnloadAllPlugins();

			GlobalWindowManager.RemoveWindow(this);

			base.OnClosed(e);
		}

		private void selectProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var pb = new ProcessBrowserForm(nativeHelper, settings.LastProcess))
			{
				if (pb.ShowDialog() == DialogResult.OK)
				{
					if (remoteProcess.Process != null)
					{
						remoteProcess.Process.Close();
					}

					remoteProcess.Process = pb.SelectedProcess;
					remoteProcess.UpdateProcessInformations();
					if (pb.LoadSymbols)
					{
						loadSymbolsTaskToken = new CancellationTokenSource();
						loadSymbolsTask = remoteProcess.LoadAllSymbolsAsync(m =>
							{
								Invoke((MethodInvoker)delegate ()
								{
									infoToolStripStatusLabel.Visible = true;
									infoToolStripStatusLabel.Text = $"Loading symbols for module: {m.Name}";
								});
							}, loadSymbolsTaskToken.Token)
							.ContinueWith(
								t => { infoToolStripStatusLabel.Visible = false; },
								TaskScheduler.FromCurrentSynchronizationContext()
							);
					}

					settings.LastProcess = remoteProcess.Process.Name;
				}
			}
		}

		private void classesView_ClassSelected(object sender, ClassNode node)
		{
			memoryViewControl.ClassNode = node;

			memoryViewControl.Invalidate();
		}

		private void cleanUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClassManager.RemoveUnusedClasses();
		}

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			memoryViewControl.AddBytes(item.Value);
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

		private void memoryTypeToolStripButton_Click(object sender, EventArgs e)
		{
			var item = sender as TypeToolStripButton;
			if (item == null)
			{
				return;
			}

			memoryViewControl.ReplaceSelectedNodesWithType(item.Value);
		}

		private void newClassToolStripButton_Click(object sender, EventArgs e)
		{
			var node = ClassManager.CreateClass();
			node.AddBytes(64);

			classesView.SelectedClass = node;
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
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
					IReClassImport import = null;
					switch (Path.GetExtension(ofd.SafeFileName))
					{
						case ReClassNetFile.FileExtension:
							import = new ReClassNetFile();
							break;
						case ReClassQtFile.FileExtension:
							import = new ReClassQtFile();
							break;
						case ReClassFile.FileExtension:
							import = new ReClassFile();
							break;
						case ReClass2007File.FileExtension:
							import = new ReClass2007File();
							break;
						default:
							logger.Log(LogLevel.Error, $"The file '{ofd.SafeFileName}' has an unknown type.");
							break;
					}
					if (import != null)
					{
						var schema = import.Load(ofd.FileName, logger);
						if (schema != null)
						{
							// If we have our filetype save the path to skip the Save As dialog.
							if (import is ReClassNetFile)
							{
								projectPath = ofd.FileName;
							}

							var classes = schema.BuildNodes(logger);

							ClassManager.Clear();
							classes.ForEach(c => ClassManager.AddClass(c));

							memoryViewControl.ClassNode = classes.FirstOrDefault();
						}
					}
				}
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ClassManager.Classes.Any())
			{
				return;
			}

			if (string.IsNullOrEmpty(projectPath))
			{
				saveAsToolStripMenuItem_Click(sender, e);

				return;
			}

			var file = new ReClassNetFile();
			file.Save(projectPath, SchemaBuilder.FromNodes(ClassManager.Classes, logger), logger);
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!ClassManager.Classes.Any())
			{
				return;
			}

			using (var sfd = new SaveFileDialog())
			{
				sfd.DefaultExt = ReClassNetFile.FileExtension;
				sfd.Filter = $"{ReClassNetFile.FormatName} (*{ReClassNetFile.FileExtension})|*{ReClassNetFile.FileExtension}";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					projectPath = sfd.FileName;

					saveToolStripMenuItem_Click(sender, e);
				}
			}
		}

		private void memoryViewerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ProcessMemoryViewer(remoteProcess, classesView).Show();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var sd = new SettingsForm(settings))
			{
				sd.ShowDialog();
			}
		}

		private void clearProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClassManager.Clear();

			memoryViewControl.ClassNode = null;
		}

		private void quitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void processUpdateTimer_Tick(object sender, EventArgs e)
		{
			if (updateProcessInformationsTask == null || updateProcessInformationsTask.IsCompleted)
			{
				updateProcessInformationsTask = remoteProcess.UpdateProcessInformationsAsync();
			}
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
						ex.ShowDialog();
					}
				}
			}
		}

		private void pluginsToolStripButton_Click(object sender, EventArgs e)
		{
			using (var pf = new PluginForm(pluginManager, nativeHelper))
			{
				pf.ShowDialog();
			}
		}

		private void ControlRemoteProcessToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!remoteProcess.IsValid)
			{
				return;
			}

			var action = NativeHelper.ControlRemoteProcessAction.Terminate;
			if (sender == resumeProcessToolStripMenuItem)
			{
				action = NativeHelper.ControlRemoteProcessAction.Resume;
			}
			else if (sender == suspendProcessToolStripMenuItem)
			{
				action = NativeHelper.ControlRemoteProcessAction.Suspend;
			}

			nativeHelper.ControlRemoteProcess(remoteProcess.Process.Handle, action);
		}

		private void generateCppCodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeForm(new CppCodeGenerator());
		}

		private void generateCSharpCodeToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			ShowCodeForm(new CSharpCodeGenerator());
		}

		private void ShowCodeForm(ICodeGenerator generator)
		{
			using (var cf = new CodeForm(generator, ClassManager.Classes, logger))
			{
				cf.ShowDialog();
			}
		}

		private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// Stop the update timer
			processUpdateTimer.Stop();

			// and cancel all running tasks.
			if (loadSymbolsTask != null || updateProcessInformationsTask != null)
			{
				Hide();
				e.Cancel = true;

				if (loadSymbolsTask != null)
				{
					loadSymbolsTaskToken.Cancel();
					await loadSymbolsTask;

					loadSymbolsTask = null;
				}
				
				if (updateProcessInformationsTask != null)
				{
					await updateProcessInformationsTask;

					updateProcessInformationsTask = null;
				}

				Close();
			}
		}

		internal void AddNodeType(Type type, string text, Image icon)
		{
			Contract.Requires(type != null);
			Contract.Requires(text != null);

			var item = new TypeToolStripButton
			{
				Image = icon,
				ToolTipText = text,
				Value = type
			};
			item.Click += memoryTypeToolStripButton_Click;

			toolStrip.Items.Add(item);

			memoryViewControl.AddNodeType(type, text, icon);
		}

		internal void RemoveNodeType(Type type)
		{
			Contract.Requires(type != null);

			var item = toolStrip.Items.OfType<TypeToolStripButton>().Where(i => i.Value == type).FirstOrDefault();
			if (item != null)
			{
				item.Click -= memoryTypeToolStripButton_Click;
				toolStrip.Items.Remove(item);
			}

			memoryViewControl.RemoveNodeType(type);
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var af = new AboutForm())
			{
				af.ShowDialog();
			}
		}

		private void addXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Add Bytes", memoryViewControl.AddBytes);
		}

		private void insertXBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AskAddOrInsertBytes("Insert Bytes", memoryViewControl.InsertBytes);
		}

		private void AskAddOrInsertBytes(string title, Action<int> func)
		{
			if (memoryViewControl.ClassNode == null)
			{
				return;
			}

			using (var ib = new InputBytesForm(memoryViewControl.ClassNode.MemorySize))
			{
				ib.Text = title;

				if (ib.ShowDialog() == DialogResult.OK)
				{
					func(ib.Bytes);
				}
			}
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
	}
}
