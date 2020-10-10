namespace ReClassNET.UI
{
	partial class ProjectView
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
			this.projectTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// projectTreeView
			// 
			this.projectTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.projectTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectTreeView.HideSelection = false;
			this.projectTreeView.LabelEdit = true;
			this.projectTreeView.Location = new System.Drawing.Point(0, 0);
			this.projectTreeView.Name = "projectTreeView";
			this.projectTreeView.ShowRootLines = false;
			this.projectTreeView.Size = new System.Drawing.Size(150, 150);
			this.projectTreeView.TabIndex = 0;
			this.projectTreeView.BeforeLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.projectTreeView_BeforeLabelEdit);
			this.projectTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.projectTreeView_AfterLabelEdit);
			this.projectTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.projectTreeView_AfterSelect);
			this.projectTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.projectTreeView_MouseUp);
			// 
			// ClassNodeView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.projectTreeView);
			this.Name = "ClassNodeView";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView projectTreeView;
	}
}
