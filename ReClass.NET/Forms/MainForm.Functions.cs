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
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;

namespace ReClassNET.Forms
{
	public partial class MainForm : IconForm
	{
		public void AttachToProcess(string processName)
		{
			var info = Program.CoreFunctions.EnumerateProcesses().FirstOrDefault(p => string.Equals(p.Name, processName, StringComparison.OrdinalIgnoreCase));
			if (info == null)
			{
				MessageBox.Show($"Process '{processName}' could not be found.", Constants.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

				Program.Settings.LastProcess = string.Empty;
			}
			else
			{
				AttachToProcess(info);
			}
		}

		public void AttachToProcess(ProcessInfo info)
		{
			Contract.Requires(info != null);

			Program.RemoteProcess.Close();

			Program.RemoteProcess.Open(info);
			Program.RemoteProcess.UpdateProcessInformations();

			Program.Settings.LastProcess = Program.RemoteProcess.UnderlayingProcess.Name;
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

			//TODO
		}

		/// <summary>Deregisters the node type.</summary>
		/// <param name="type">The node type.</param>
		internal void DeregisterNodeType(Type type)
		{
			Contract.Requires(type != null);

			//TODO
		}

		/// <summary>Shows the code form with the given <paramref name="generator"/>.</summary>
		/// <param name="generator">The generator.</param>
		private void ShowCodeForm(ICodeGenerator generator)
		{
			Contract.Requires(generator != null);

			LinkedWindowFeatures.ShowCodeGeneratorForm(currentProject.Classes, generator);
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

		/// <summary>Loads the file as a new project.</summary>
		/// <param name="path">Full pathname of the file.</param>
		public void LoadProjectFromPath(string path)
		{
			Contract.Requires(path != null);

			var project = new ReClassNetProject();

			LoadProjectFromPath(path, ref project);

			// If the file is a ReClass.NET file remember the path.
			if (Path.GetExtension(path) == ReClassNetFile.FileExtension)
			{
				project.Path = path;
			}

			SetProject(project);
		}

		/// <summary>Loads the file into the given project.</summary>
		/// <param name="path">Full pathname of the file.</param>
		/// <param name="project">[in,out] The project.</param>
		private static void LoadProjectFromPath(string path, ref ReClassNetProject project)
		{
			Contract.Requires(path != null);
			Contract.Requires(project != null);
			Contract.Ensures(Contract.ValueAtReturn(out project) != null);

			IReClassImport import;
			switch (Path.GetExtension(path)?.ToLower())
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
					Program.Logger.Log(LogLevel.Error, $"The file '{path}' has an unknown type.");
					return;
			}
			import.Load(path, Program.Logger);
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

			loadSymbolsTask = Program.RemoteProcess
				.LoadAllSymbolsAsync(progress, loadSymbolsTaskToken.Token)
				.ContinueWith(_ => infoToolStripStatusLabel.Visible = false, TaskScheduler.FromCurrentSynchronizationContext());
		}
	}
}
