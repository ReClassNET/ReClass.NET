namespace ReClassNET.UI
{
	partial class ClassNodeView
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.classesTreeView = new System.Windows.Forms.TreeView();
			this.classContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.renameClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.removeUnusedClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rootContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.autoExpandHierarchyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.classContextMenuStrip.SuspendLayout();
			this.rootContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// classesTreeView
			// 
			this.classesTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.classesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classesTreeView.HideSelection = false;
			this.classesTreeView.LabelEdit = true;
			this.classesTreeView.Location = new System.Drawing.Point(0, 0);
			this.classesTreeView.Name = "classesTreeView";
			this.classesTreeView.ShowRootLines = false;
			this.classesTreeView.Size = new System.Drawing.Size(150, 150);
			this.classesTreeView.TabIndex = 0;
			this.classesTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.classesTreeView_AfterLabelEdit);
			this.classesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.classesTreeView_AfterSelect);
			this.classesTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.classesTreeView_MouseUp);
			// 
			// classContextMenuStrip
			// 
			this.classContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameClassToolStripMenuItem,
            this.deleteClassToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeUnusedClassesToolStripMenuItem});
			this.classContextMenuStrip.Name = "contextMenuStrip";
			this.classContextMenuStrip.Size = new System.Drawing.Size(199, 76);
			// 
			// renameClassToolStripMenuItem
			// 
			this.renameClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Textfield_Rename;
			this.renameClassToolStripMenuItem.Name = "renameClassToolStripMenuItem";
			this.renameClassToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.renameClassToolStripMenuItem.Text = "Rename class";
			this.renameClassToolStripMenuItem.Click += new System.EventHandler(this.renameClassToolStripMenuItem_Click);
			// 
			// deleteClassToolStripMenuItem
			// 
			this.deleteClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Remove;
			this.deleteClassToolStripMenuItem.Name = "deleteClassToolStripMenuItem";
			this.deleteClassToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.deleteClassToolStripMenuItem.Text = "Delete class";
			this.deleteClassToolStripMenuItem.Click += new System.EventHandler(this.deleteClassToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
			// 
			// removeUnusedClassesToolStripMenuItem
			// 
			this.removeUnusedClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Chart_Delete;
			this.removeUnusedClassesToolStripMenuItem.Name = "removeUnusedClassesToolStripMenuItem";
			this.removeUnusedClassesToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.removeUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
			this.removeUnusedClassesToolStripMenuItem.Click += new System.EventHandler(this.removeUnusedClassesToolStripMenuItem_Click);
			// 
			// rootContextMenuStrip
			// 
			this.rootContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoExpandHierarchyViewToolStripMenuItem});
			this.rootContextMenuStrip.Name = "rootContextMenuStrip";
			this.rootContextMenuStrip.Size = new System.Drawing.Size(221, 48);
			// 
			// autoExpandHierarchyViewToolStripMenuItem
			// 
			this.autoExpandHierarchyViewToolStripMenuItem.Checked = true;
			this.autoExpandHierarchyViewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.autoExpandHierarchyViewToolStripMenuItem.Name = "autoExpandHierarchyViewToolStripMenuItem";
			this.autoExpandHierarchyViewToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.autoExpandHierarchyViewToolStripMenuItem.Text = "Auto expand hierarchy view";
			this.autoExpandHierarchyViewToolStripMenuItem.Click += new System.EventHandler(this.autoExpandHierarchyViewToolStripMenuItem_Click);
			// 
			// ClassNodeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.classesTreeView);
			this.Name = "ClassNodeView";
			this.classContextMenuStrip.ResumeLayout(false);
			this.rootContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView classesTreeView;
		private System.Windows.Forms.ContextMenuStrip classContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem renameClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem removeUnusedClassesToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip rootContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem autoExpandHierarchyViewToolStripMenuItem;
	}
}
