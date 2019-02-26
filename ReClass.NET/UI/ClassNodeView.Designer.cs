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
			this.classesTreeView = new System.Windows.Forms.TreeView();
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
			// ClassNodeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.classesTreeView);
			this.Name = "ClassNodeView";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView classesTreeView;
	}
}
