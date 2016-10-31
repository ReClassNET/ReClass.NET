using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.UI
{
	public partial class ClassNodeView : UserControl
	{
		private class ClassTreeNode : TreeNode
		{
			public ClassNode ClassNode { get; }

			public ClassTreeNode(ClassNode node)
			{
				Contract.Requires(node != null);

				ClassNode = node;

				node.PropertyChanged += NodePropertyChanged;
				node.NodesChanged += RebuildClassHierarchy;

				Text = node.Name;

				ImageIndex = 1;
				SelectedImageIndex = 1;
			}

			private void NodePropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				var node = sender as ClassNode;
				if (node == null)
				{
					return;
				}

				switch (e.PropertyName)
				{
					case nameof(BaseNode.Name):
						// Name has changed, update the TreeView
						Text = node.Name;
						break;
				}
			}

			private void RebuildClassHierarchy(BaseNode sender)
			{
				Nodes.Clear();

				foreach (var child in ClassNode.Nodes
					.OfType<BaseReferenceNode>()
					.Select(r => r.InnerNode)
					.OfType<ClassNode>())
				{
					Nodes.Add(new ClassTreeNode(child));
				}
			}
		}

		private readonly TreeNode root;

		private ClassNode selectedClass;

		public delegate void SelectionChangedEvent(object sender, ClassNode node);
		public event SelectionChangedEvent SelectionChanged;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ClassNode SelectedClass
		{
			get { return selectedClass; }
			set
			{
				if (selectedClass != value)
				{
					selectedClass = value;
					if (selectedClass != null)
					{
						classesTreeView.SelectedNode = root.Nodes.Cast<TreeNode>().Where(n => n.Tag == selectedClass).FirstOrDefault();
					}

					SelectionChanged?.Invoke(this, selectedClass);
				}
			}
		}

		public ClassNodeView()
		{
			InitializeComponent();

			DoubleBuffered = true;

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

		public void Add(ClassNode node)
		{
			Contract.Requires(node != null);

			root.Nodes.Add(new ClassTreeNode(node));

			root.ExpandAll();

			classesTreeView.Sort();
		}

		public void Remove(ClassNode node)
		{
			var tn = FindClassTreeNode(node);
			if (tn != null)
			{
				root.Nodes.Remove(tn);

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
		}

		private TreeNode FindClassTreeNode(ClassNode node)
		{
			return root.Nodes.OfType<ClassTreeNode>().Where(t => t.ClassNode == node).FirstOrDefault();
		}

		private void classesTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Label))
			{
				var node = e.Node as ClassTreeNode;
				if (node != null)
				{
					node.ClassNode.Name = e.Label;
				}
			}
		}

		private void classesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Level == 0)
			{
				return;
			}

			var node = e.Node as ClassTreeNode;
			if (node == null)
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
			if (e.Button == MouseButtons.Right)
			{
				var node = classesTreeView.GetNodeAt(e.X, e.Y);

				if (node != null && node != root)
				{
					classesTreeView.SelectedNode = node;

					contextMenuStrip.Show(classesTreeView, e.Location);
				}
			}
		}

		private void removeUnusedClassesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ClassManager.RemoveUnusedClasses();
		}

		private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var treeNode = classesTreeView.SelectedNode as ClassTreeNode;
			if (treeNode != null)
			{
				try
				{
					ClassManager.Remove(treeNode.ClassNode);
				}
				catch (ClassReferencedException ex)
				{
					ex.ShowDialog();
				}
			}
		}

		private void renameClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var treeNode = classesTreeView.SelectedNode;
			if (treeNode != null)
			{
				treeNode.BeginEdit();
			}
		}

		private void enableHierarchyViewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			enableHierarchyViewToolStripMenuItem.Checked = !enableHierarchyViewToolStripMenuItem.Checked;

			classesTreeView.ShowPlusMinus = enableHierarchyViewToolStripMenuItem.Checked;
		}
	}
}
