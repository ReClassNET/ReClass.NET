using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.CodeGenerator;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Extensions;
using ReClassNET.Logger;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Project;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Forms
{
	public partial class MainForm
	{
		public void ShowPartialCodeGeneratorForm(IReadOnlyList<ClassNode> partialClasses)
		{
			Contract.Requires(partialClasses != null);

			ShowCodeGeneratorForm(partialClasses, new EnumDescription[0], new CppCodeGenerator(currentProject.TypeMapping));
		}

		public void ShowCodeGeneratorForm(ICodeGenerator generator)
		{
			Contract.Requires(generator != null);

			ShowCodeGeneratorForm(currentProject.Classes, currentProject.Enums, generator);
		}

		public void ShowCodeGeneratorForm(IReadOnlyList<ClassNode> classes, IReadOnlyList<EnumDescription> enums, ICodeGenerator generator)
		{
			Contract.Requires(classes != null);
			Contract.Requires(generator != null);
			Contract.Requires(enums != null);

			new CodeForm(generator, classes, enums, Program.Logger).Show();
		}

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

			void UpdateClassNodes(BaseNode node)
			{
				projectView.UpdateClassNode((ClassNode)node);
			}

			currentProject = newProject;
			currentProject.ClassAdded += c =>
			{
				projectView.AddClass(c);
				c.NodesChanged += UpdateClassNodes;
				c.NameChanged += UpdateClassNodes;
			};
			currentProject.ClassRemoved += c =>
			{
				projectView.RemoveClass(c);
				c.NodesChanged -= UpdateClassNodes;
				c.NameChanged -= UpdateClassNodes;
			};
			currentProject.EnumAdded += e => { projectView.AddEnum(e); };

			ClassNode.ClassCreated += currentProject.AddClass;

			projectView.Clear();
			projectView.AddEnums(currentProject.Enums);
			projectView.AddClasses(currentProject.Classes);
			CurrentClassNode = currentProject.Classes.FirstOrDefault();
		}

		/// <summary>Opens the <see cref="InputBytesForm"/> and calls <paramref name="callback"/> with the result.</summary>
		/// <param name="title">The title of the input form.</param>
		/// <param name="callback">The function to call afterwards.</param>
		private void AskAddOrInsertBytes(string title, Action<int> callback)
		{
			Contract.Requires(title != null);
			Contract.Requires(callback != null);

			var classNode = CurrentClassNode;
			if (classNode == null)
			{
				return;
			}

			using (var ib = new InputBytesForm(classNode.MemorySize))
			{
				ib.Text = title;

				if (ib.ShowDialog() == DialogResult.OK)
				{
					callback(ib.Bytes);
				}
			}
		}

		/// <summary>
		/// Adds <paramref name="bytes"/> bytes at the end of the current class.
		/// </summary>
		/// <param name="bytes">Amount of bytes</param>
		public void AddBytesToClass(int bytes)
		{
			Contract.Requires(bytes >= 0);

			var node = memoryViewControl.GetSelectedNodes().Select(h => h.Node).FirstOrDefault();
			if (node == null)
			{
				return;
			}

			(node as BaseContainerNode ?? node.GetParentContainer())?.AddBytes(bytes);

			Invalidate();
		}

		/// <summary>
		/// Inserts <paramref name="bytes"/> bytes at the first selected node to the current class.
		/// </summary>
		/// <param name="bytes">Amount of bytes</param>
		public void InsertBytesInClass(int bytes)
		{
			Contract.Requires(bytes >= 0);

			var node = memoryViewControl.GetSelectedNodes().Select(h => h.Node).FirstOrDefault();
			if (node == null)
			{
				return;
			}

			(node as BaseContainerNode ?? node.GetParentContainer())?.InsertBytes(node, bytes);

			Invalidate();
		}

		/// <summary>
		/// Unselects all selected nodes.
		/// </summary>
		public void ClearSelection()
		{
			memoryViewControl.ClearSelection();
		}

		/// <summary>Shows an <see cref="OpenFileDialog"/> with all valid file extensions.</summary>
		/// <returns>The path to the selected file or null if no file was selected.</returns>
		public static string ShowOpenProjectFileDialog()
		{
			using (var ofd = new OpenFileDialog())
			{
				ofd.CheckFileExists = true;
				ofd.Filter = $"All ReClass Types |*{ReClassNetFile.FileExtension};*{ReClassFile.FileExtension};*{ReClassQtFile.FileExtension}"
					+ $"|{ReClassNetFile.FormatName} (*{ReClassNetFile.FileExtension})|*{ReClassNetFile.FileExtension}"
					+ $"|{ReClassFile.FormatName} (*{ReClassFile.FileExtension})|*{ReClassFile.FileExtension}"
					+ $"|{ReClassQtFile.FormatName} (*{ReClassQtFile.FileExtension})|*{ReClassQtFile.FileExtension}";

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

			var progress = new Progress<Tuple<Module, IReadOnlyList<Module>>>(
				report =>
				{
					infoToolStripStatusLabel.Text = $"[{++index}/{report.Item2.Count}] Loading symbols for module: {report.Item1.Name}";
				}
			);

			loadSymbolsTaskToken = new CancellationTokenSource();

			loadSymbolsTask = Program.RemoteProcess
				.LoadAllSymbolsAsync(progress, loadSymbolsTaskToken.Token)
				.ContinueWith(_ => infoToolStripStatusLabel.Visible = false, TaskScheduler.FromCurrentSynchronizationContext());
		}

		public void ReplaceSelectedNodesWithType(Type type)
		{
			Contract.Requires(type != null);
			Contract.Requires(type.IsSubclassOf(typeof(BaseNode)));

			var selectedNodes = memoryViewControl.GetSelectedNodes();

			var newSelected = new List<MemoryViewControl.SelectedNodeInfo>(selectedNodes.Count);

			var hotSpotPartitions = selectedNodes
				.WhereNot(s => s.Node is ClassNode)
				.GroupBy(s => s.Node.GetParentContainer())
				.Select(g => new
				{
					Container = g.Key,
					Partitions = g.OrderBy(s => s.Node.Offset)
						.GroupWhile((s1, s2) => s1.Node.Offset + s1.Node.MemorySize == s2.Node.Offset)
				});

			foreach (var containerPartitions in hotSpotPartitions)
			{
				containerPartitions.Container.BeginUpdate();

				foreach (var partition in containerPartitions.Partitions)
				{
					var hotSpotsToReplace = new Queue<MemoryViewControl.SelectedNodeInfo>(partition);
					while (hotSpotsToReplace.Count > 0)
					{
						var selected = hotSpotsToReplace.Dequeue();

						var node = BaseNode.CreateInstanceFromType(type);

						var createdNodes = new List<BaseNode>();
						containerPartitions.Container.ReplaceChildNode(selected.Node, node, ref createdNodes);

						node.IsSelected = true;

						var info = new MemoryViewControl.SelectedNodeInfo(node, selected.Process, selected.Memory, selected.Address, selected.Level);

						newSelected.Add(info);

						// If more than one node is selected I assume the user wants to replace the complete range with the desired node type.
						if (selectedNodes.Count > 1)
						{
							foreach (var createdNode in createdNodes)
							{
								hotSpotsToReplace.Enqueue(new MemoryViewControl.SelectedNodeInfo(createdNode, selected.Process, selected.Memory, selected.Address + createdNode.Offset - node.Offset, selected.Level));
							}
						}
					}
				}

				containerPartitions.Container.EndUpdate();
			}

			memoryViewControl.ClearSelection();

			if (newSelected.Count > 0)
			{
				memoryViewControl.SetSelectedNodes(newSelected);
			}
		}

		private void FindWhatInteractsWithSelectedNode(bool writeOnly)
		{
			var selectedNode = memoryViewControl.GetSelectedNodes().FirstOrDefault();
			if (selectedNode == null)
			{
				return;
			}

			LinkedWindowFeatures.FindWhatInteractsWithAddress(selectedNode.Address, selectedNode.Node.MemorySize, writeOnly);
		}

		private void CopySelectedNodesToClipboard()
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count > 0)
			{
				ReClassClipboard.Copy(selectedNodes.Select(h => h.Node), Program.Logger);
			}
		}

		private void PasteNodeFromClipboardToSelection()
		{
			var result = ReClassClipboard.Paste(CurrentProject, Program.Logger);
			foreach (var pastedClassNode in result.Item1)
			{
				if (!CurrentProject.ContainsClass(pastedClassNode.Uuid))
				{
					CurrentProject.AddClass(pastedClassNode);
				}
			}

			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count == 1)
			{
				var selectedNode = selectedNodes[0].Node;
				var containerNode = selectedNode.GetParentContainer();
				var classNode = selectedNode.GetParentClass();
				if (containerNode != null && classNode != null)
				{
					containerNode.BeginUpdate();

					foreach (var node in result.Item2)
					{
						if (node is BaseWrapperNode)
						{
							var rootWrapper = node.GetRootWrapperNode();
							Debug.Assert(rootWrapper == node);

							if (rootWrapper.ShouldPerformCycleCheckForInnerNode())
							{
								if (rootWrapper.ResolveMostInnerNode() is ClassNode innerNode)
								{
									if (!IsCycleFree(classNode, innerNode))
									{
										continue;
									}
								}
							}
						}

						containerNode.InsertNode(selectedNode, node);
					}

					containerNode.EndUpdate();
				}
			}
		}

		private void RemoveSelectedNodes()
		{
			memoryViewControl.GetSelectedNodes()
				.WhereNot(h => h.Node is ClassNode)
				.ForEach(h => h.Node.GetParentContainer().RemoveNode(h.Node));

			ClearSelection();
		}

		private void HideSelectedNodes()
		{
			foreach (var hotSpot in memoryViewControl.GetSelectedNodes())
			{
				hotSpot.Node.IsHidden = true;
			}

			ClearSelection();
		}

		private void UnhideChildNodes()
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count != 1)
			{
				return;
			}

			if (!(selectedNodes[0].Node is BaseContainerNode containerNode))
			{
				return;
			}

			foreach (var bn in containerNode.Nodes)
			{
				bn.IsHidden = false;
				bn.IsSelected = false;
			}

			containerNode.IsSelected = false;

			ClearSelection();
		}

		private void UnhideNodesBelow()
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count != 1)
			{
				return;
			}

			var selectedNode = selectedNodes[0].Node;

			var parentNode = selectedNode.GetParentContainer();
			if (parentNode == null)
			{
				return;
			}

			var hiddenNodeStartIndex = parentNode.FindNodeIndex(selectedNode) + 1;
			if (hiddenNodeStartIndex >= parentNode.Nodes.Count)
			{
				return;
			}

			for (var i = hiddenNodeStartIndex; i < parentNode.Nodes.Count; i++)
			{
				var indexNode = parentNode.Nodes[i];
				if (indexNode.IsHidden)
				{
					indexNode.IsHidden = false;
					indexNode.IsSelected = false;
				}
				else
				{
					break;
				}
			}

			selectedNode.IsSelected = false;

			ClearSelection();
		}

		private void UnhideNodesAbove()
		{
			var selectedNodes = memoryViewControl.GetSelectedNodes();
			if (selectedNodes.Count != 1)
			{
				return;
			}

			var selectedNode = selectedNodes[0].Node;

			var parentNode = selectedNode.GetParentContainer();
			if (parentNode == null)
			{
				return;
			}

			var hiddenNodeStartIndex = parentNode.FindNodeIndex(selectedNode) - 1;
			if (hiddenNodeStartIndex < 0)
			{
				return;
			}

			for (var i = hiddenNodeStartIndex; i > -1; i--)
			{
				var indexNode = parentNode.Nodes[i];
				if (indexNode.IsHidden)
				{
					indexNode.IsHidden = false;
					indexNode.IsSelected = false;
				}
				else
				{
					break;
				}
			}

			selectedNode.IsSelected = false;

			ClearSelection();
		}

		private bool IsCycleFree(ClassNode parent, ClassNode node)
		{
			if (ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, node, CurrentProject.Classes))
			{
				MessageBox.Show("Invalid operation because this would create a class cycle.", "Cycle Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);

				return false;
			}

			return true;
		}
	}
}
