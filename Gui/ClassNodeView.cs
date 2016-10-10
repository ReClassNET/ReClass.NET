using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Nodes;

namespace ReClassNET
{
	partial class ClassNodeView : UserControl
	{
		private readonly List<ClassNode> classes = new List<ClassNode>();

		public IEnumerable<ClassNode> Classes => classes;

		private readonly TreeNode root;

		public delegate void ClassSelectedEvent(object sender, ClassNode node);
		public event ClassSelectedEvent ClassSelected;

		public ClassNode SelectedClass
		{
			get { return classesTreeView.SelectedNode.Tag as ClassNode; }
			set { if (value != null) { classesTreeView.SelectedNode = root.Nodes.Cast<TreeNode>().Where(n => n.Tag == value).FirstOrDefault(); } }
		}

		public ClassNodeView()
		{
			InitializeComponent();

			DoubleBuffered = true;

			classesTreeView.ImageList = new ImageList();
			classesTreeView.ImageList.Images.Add(Properties.Resources.text_list_bullets);
			classesTreeView.ImageList.Images.Add(Properties.Resources.class_icon);

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
			if (!classes.Contains(node))
			{
				classes.Add(node);

				node.PropertyChanged += NodePropertyChanged;

				var treeNode = new TreeNode
				{
					Text = node.Name,
					Tag = node,
					ImageIndex = 1,
					SelectedImageIndex = 1
				};

				root.Nodes.Add(treeNode);

				root.Expand();
			}
		}

		private IEnumerable<ClassNode> GetClassReferences(ClassNode node)
		{
			return classes.Where(c => c != node).Where(c => c.Descendants().Where(n => (n as BaseReferenceNode)?.InnerNode == node).Any());
		}

		public void Remove(ClassNode node)
		{
			var references = GetClassReferences(node).ToList();
			if (references.Any())
			{
				var message = "This class still has references:\n\n";
				message += string.Join("\n", references.Select(n => n.Name));

				MessageBox.Show(message, "Error");

				return;
			}

			if (classes.Remove(node))
			{
				root.Nodes.Remove(FindClassNode(node));

				ClassSelected?.Invoke(this, root.Nodes.Cast<TreeNode>().FirstOrDefault()?.Tag as ClassNode);
			}
		}

		public void RemoveUnusedClasses()
		{
			var toRemove = classes.Except(classes.Where(x => GetClassReferences(x).Any())).Where(c => c.Nodes.All(n => n is BaseHexNode)).ToList();
			foreach (var node in toRemove)
			{
				Remove(node);
			}
		}

		private TreeNode FindClassNode(ClassNode node)
		{
			return root.Nodes.Cast<TreeNode>().Where(t => t.Tag == node).FirstOrDefault();
		}

		private void NodePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			var node = sender as ClassNode;
			if (node == null)
			{
				return;
			}

			var treeNode = FindClassNode(node);

			switch (e.PropertyName)
			{
				case nameof(BaseNode.Name):
					// Name has changed, update the TreeView
					treeNode.Text = node.Name;
					break;
				case nameof(ClassNode.Nodes):
					// Child nodes have changed, update all offsets
					classes.ForEach(c => c.UpdateChildrenOffsets());
					break;
			}
		}

		private void classesTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (!string.IsNullOrEmpty(e.Label))
			{
				var node = e.Node.Tag as ClassNode;
				if (node != null)
				{
					node.Name = e.Label;
				}
			}
		}

		private void classesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Level == 0)
			{
				return;
			}

			var node = e.Node.Tag as ClassNode;
			if (node == null)
			{
				return;
			}

			ClassSelected?.Invoke(this, node);
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
			RemoveUnusedClasses();
		}

		private void deleteClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var treeNode = classesTreeView.SelectedNode;
			if (treeNode != null)
			{
				var classNode = treeNode.Tag as ClassNode;
				if (classNode != null)
				{
					Remove(classNode);
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
	}
}
