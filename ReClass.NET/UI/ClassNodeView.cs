using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Nodes;
using ReClassNET.Project;

namespace ReClassNET.UI
{
	public partial class ClassNodeView : UserControl
	{
		/// <summary>A custom tree node for class nodes with hierarchical structure.</summary>
		private class ClassTreeNode : TreeNode
		{
			private readonly ClassNodeView control;

			public ClassNode ClassNode { get; }

			/// <summary>Constructor of the class.</summary>
			/// <param name="node">The class node.</param>
			/// <param name="control">The <see cref="ClassNodeView"/> instance this node should belong to.</param>
			public ClassTreeNode(ClassNode node, ClassNodeView control)
				: this(node, control, null)
			{
				Contract.Requires(node != null);
				Contract.Requires(control != null);
			}

			private ClassTreeNode(ClassNode node, ClassNodeView control, HashSet<ClassNode> seen)
			{
				Contract.Requires(node != null);
				Contract.Requires(control != null);

				ClassNode = node;

				this.control = control;

				Text = node.Name;

				ImageIndex = 1;
				SelectedImageIndex = 1;

				RebuildClassHierarchy(seen ?? new HashSet<ClassNode> { ClassNode });
			}

			public void Update()
			{
				Text = ClassNode.Name;

				RebuildClassHierarchy(new HashSet<ClassNode> { ClassNode });
			}

			/// <summary>Rebuilds the class hierarchy.</summary>
			/// <param name="seen">The already seen classes.</param>
			private void RebuildClassHierarchy(HashSet<ClassNode> seen)
			{
				Contract.Requires(seen != null);

				if (!control.EnableClassHierarchyView)
				{
					return;
				}

				var distinctClasses = ClassNode.Nodes
					.OfType<BaseWrapperNode>()
					.Select(w => w.ResolveMostInnerNode())
					.OfType<ClassNode>()
					.Distinct()
					.ToList();

				if (distinctClasses.SequenceEqualsEx(Nodes.Cast<ClassTreeNode>().Select(t => t.ClassNode)))
				{
					return;
				}

				Nodes.Clear();

				foreach (var child in distinctClasses)
				{
					var childSeen = new HashSet<ClassNode>(seen);
					if (childSeen.Add(child))
					{
						Nodes.Add(new ClassTreeNode(child, control, childSeen));
					}
				}

				if (control.AutoExpandClassNodes)
				{
					Expand();
				}
			}
		}

		private class NodeSorter : IComparer
		{
			public int Compare(object x, object y)
			{
				var compare = Application.CurrentCulture.CompareInfo;

				if (x is ClassTreeNode n1 && y is ClassTreeNode n2)
				{
					return compare.Compare(n1.Text, n2.Text);
				}

				return 0;
			}
		}

		private readonly TreeNode root;

		private ReClassNetProject project;

		private ClassNode selectedClass;

		public delegate void SelectionChangedEvent(object sender, ClassNode node);
		public event SelectionChangedEvent SelectionChanged;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ReClassNetProject Project
		{
			get => project;
			set
			{
				Contract.Requires(value != null);

				if (project != value)
				{
					classesTreeView.BeginUpdate();

					root.Nodes.Clear();

					if (project != null)
					{
						project.ClassAdded -= AddClass;
						project.ClassRemoved -= RemoveClass;
					}

					project = value;

					project.ClassAdded += AddClass;
					project.ClassRemoved += RemoveClass;

					project.Classes.ForEach(AddClassInternal);

					classesTreeView.Sort();

					classesTreeView.EndUpdate();
				}
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ClassNode SelectedClass
		{
			get => selectedClass;
			set
			{
				if (selectedClass != value)
				{
					selectedClass = value;
					if (selectedClass != null)
					{
						classesTreeView.SelectedNode = root.Nodes.Cast<ClassTreeNode>().FirstOrDefault(n => n.ClassNode == selectedClass);
					}

					SelectionChanged?.Invoke(this, selectedClass);
				}
			}
		}

		[DefaultValue(false)]
		public bool AutoExpandClassNodes { get; set; }

		[DefaultValue(false)]
		public bool EnableClassHierarchyView { get; set; }

		public ContextMenuStrip ProjectTreeNodeContextMenuStrip { get; set; }

		public ContextMenuStrip ClassTreeNodeContextMenuStrip { get; set; }

		public ClassNodeView()
		{
			Contract.Ensures(root != null);

			InitializeComponent();

			DoubleBuffered = true;

			classesTreeView.TreeViewNodeSorter = new NodeSorter();
			classesTreeView.ImageList = new ImageList();
			classesTreeView.ImageList.Images.Add(Properties.Resources.B16x16_Text_List_Bullets);
			classesTreeView.ImageList.Images.Add(Properties.Resources.B16x16_Class_Type);

			root = new TreeNode
			{
				Text = "Classes",
				ImageIndex = 0,
				SelectedImageIndex = 0
			};

			classesTreeView.Nodes.Add(root);
		}

		#region Event Handler

		private void classesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Level == 0)
			{
				return;
			}

			if (!(e.Node is ClassTreeNode node))
			{
				return;
			}

			if (selectedClass != node.ClassNode)
			{
				SelectedClass = node.ClassNode;
			}
		}

		private void classesTreeView_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)
			{
				return;
			}

			var node = classesTreeView.GetNodeAt(e.X, e.Y);
			if (node == null)
			{
				return;
			}

			if (node is ClassTreeNode)
			{
				classesTreeView.SelectedNode = node;

				var cms = ClassTreeNodeContextMenuStrip;
				cms?.Show(classesTreeView, e.Location);
			}
			else
			{
				var cms = ProjectTreeNodeContextMenuStrip;
				cms?.Show(classesTreeView, e.Location);
			}
		}

		private void removeUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//project.RemoveUnusedClasses();
		}

		private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*if (classesTreeView.SelectedNode is ClassTreeNode treeNode)
			{
				try
				{
					project.Remove(treeNode.ClassNode);
				}
				catch (ClassReferencedException ex)
				{
					Program.Logger.Log(ex);
				}
			}*/
		}

		private void renameClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*var treeNode = classesTreeView.SelectedNode;
			treeNode?.BeginEdit();*/
		}

		private void classesTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Label))
			{
				if (e.Node is ClassTreeNode node)
				{
					node.ClassNode.Name = e.Label;

					// Cancel the edit if the class refused the name.
					// This prevents the tree node from using the wrong name.
					if (node.ClassNode.Name != e.Label)
					{
						e.CancelEdit = true;
					}
				}
			}
		}

		private void enableHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*enableHierarchyViewToolStripMenuItem.Checked = !enableHierarchyViewToolStripMenuItem.Checked;

			expandAllClassesToolStripMenuItem.Enabled =
				collapseAllClassesToolStripMenuItem.Enabled = enableHierarchyViewToolStripMenuItem.Checked;

			EnableClassHierarchyView = enableHierarchyViewToolStripMenuItem.Checked;

			var classes = root.Nodes.Cast<ClassTreeNode>().Select(t => t.ClassNode).ToList();

			classesTreeView.BeginUpdate();

			root.Nodes.Clear();

			classes.ForEach(AddClassInternal);

			classesTreeView.Sort();

			classesTreeView.EndUpdate();*/
		}

		private void autoExpandHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*autoExpandHierarchyViewToolStripMenuItem.Checked = !autoExpandHierarchyViewToolStripMenuItem.Checked;

			AutoExpandClassNodes = autoExpandHierarchyViewToolStripMenuItem.Checked;

			if (AutoExpandClassNodes)
			{
				root.ExpandAll();
			}*/
		}

		private void expandAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//root.ExpandAll();
		}

		private void collapseAllClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//root.Nodes.Cast<TreeNode>().ForEach(n => n.Collapse());
		}

		private void addNewClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//LinkedWindowFeatures.CreateDefaultClass();
		}

		private void showCodeOfClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			/*if (classesTreeView.SelectedNode is ClassTreeNode node)
			{
				LinkedWindowFeatures.ShowCodeGeneratorForm(node.ClassNode.Yield(), Project.TypeMapping);
			}*/
		}

		#endregion

		public void Clear()
		{
			root.Nodes.Clear();
		}

		/// <summary>Adds the class to the view.</summary>
		/// <param name="node">The class to add.</param>
		public void AddClass(ClassNode node)
		{
			Contract.Requires(node != null);

			AddClasses(new[] { node });
		}

		public void AddClasses(IEnumerable<ClassNode> nodes)
		{
			Contract.Requires(nodes != null);

			classesTreeView.BeginUpdate();

			foreach (var node in nodes)
			{
				AddClassInternal(node);
			}

			classesTreeView.Sort();

			classesTreeView.EndUpdate();
		}

		/// <summary>Removes the class from the view.</summary>
		/// <param name="node">The class to remove.</param>
		public void RemoveClass(ClassNode node)
		{
			foreach (var tn in FindClassTreeNodes(node))
			{
				tn.Remove();
			}

			if (selectedClass == node)
			{
				if (root.Nodes.Count > 0)
				{
					classesTreeView.SelectedNode = root.Nodes[0];
				}
				else
				{
					SelectedClass = null;
				}
			}
		}

		/// <summary>
		/// Adds a new <see cref="ClassTreeNode"/> to the tree.
		/// </summary>
		/// <param name="node">The class to add.</param>
		private void AddClassInternal(ClassNode node)
		{
			Contract.Requires(node != null);

			root.Nodes.Add(new ClassTreeNode(node, this));

			root.Expand();
		}

		/// <summary>Searches for the <see cref="ClassTreeNode"/> which represents the class.</summary>
		/// <param name="node">The class to search.</param>
		/// <returns>The found class tree node.</returns>
		private ClassTreeNode FindMainClassTreeNode(ClassNode node)
		{
			return root.Nodes
				.Cast<ClassTreeNode>()
				.FirstOrDefault(t => t.ClassNode == node);
		}

		/// <summary>Searches for the ClassTreeNode which represents the class.</summary>
		/// <param name="node">The class to search.</param>
		/// <returns>The found class tree node.</returns>
		private IEnumerable<ClassTreeNode> FindClassTreeNodes(ClassNode node)
		{
			return root.Nodes
				.Cast<ClassTreeNode>()
				.Traverse(t => t.Nodes.Cast<ClassTreeNode>())
				.Where(n => n.ClassNode == node);
		}

		public void UpdateClassNode(ClassNode node)
		{
			foreach (var tn in FindClassTreeNodes(node))
			{
				tn.Update();
			}
		}
	}
}
