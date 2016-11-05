namespace ReClassNET.Forms
{
	partial class LogForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.closeButton = new System.Windows.Forms.Button();
			this.copyToClipboardButton = new System.Windows.Forms.Button();
			this.entriesDataGridView = new System.Windows.Forms.DataGridView();
			this.iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.messageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.entriesDataGridView)).BeginInit();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeButton.Location = new System.Drawing.Point(466, 206);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(109, 23);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// copyToClipboardButton
			// 
			this.copyToClipboardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.copyToClipboardButton.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Copy;
			this.copyToClipboardButton.Location = new System.Drawing.Point(12, 206);
			this.copyToClipboardButton.Name = "copyToClipboardButton";
			this.copyToClipboardButton.Size = new System.Drawing.Size(120, 23);
			this.copyToClipboardButton.TabIndex = 3;
			this.copyToClipboardButton.Text = "Copy to Clipboard";
			this.copyToClipboardButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.copyToClipboardButton.UseVisualStyleBackColor = true;
			this.copyToClipboardButton.Click += new System.EventHandler(this.copyToClipboardButton_Click);
			// 
			// entriesDataGridView
			// 
			this.entriesDataGridView.AllowUserToAddRows = false;
			this.entriesDataGridView.AllowUserToDeleteRows = false;
			this.entriesDataGridView.AllowUserToResizeColumns = false;
			this.entriesDataGridView.AllowUserToResizeRows = false;
			this.entriesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.entriesDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
			this.entriesDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.entriesDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			this.entriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.entriesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconColumn,
            this.messageColumn});
			this.entriesDataGridView.Location = new System.Drawing.Point(12, 12);
			this.entriesDataGridView.MultiSelect = false;
			this.entriesDataGridView.Name = "entriesDataGridView";
			this.entriesDataGridView.ReadOnly = true;
			this.entriesDataGridView.RowHeadersVisible = false;
			this.entriesDataGridView.RowTemplate.ContextMenuStrip = this.contextMenuStrip;
			this.entriesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.entriesDataGridView.Size = new System.Drawing.Size(563, 188);
			this.entriesDataGridView.TabIndex = 1;
			this.entriesDataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.entriesDataGridView_CellContentDoubleClick);
			// 
			// iconColumn
			// 
			this.iconColumn.DataPropertyName = "Icon";
			this.iconColumn.HeaderText = "";
			this.iconColumn.MinimumWidth = 18;
			this.iconColumn.Name = "iconColumn";
			this.iconColumn.ReadOnly = true;
			this.iconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.iconColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.iconColumn.Width = 18;
			// 
			// messageColumn
			// 
			this.messageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.messageColumn.DataPropertyName = "Message";
			this.messageColumn.HeaderText = "Message";
			this.messageColumn.Name = "messageColumn";
			this.messageColumn.ReadOnly = true;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDetailsToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(150, 26);
			// 
			// showDetailsToolStripMenuItem
			// 
			this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
			this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
			this.showDetailsToolStripMenuItem.Text = "Show details...";
			this.showDetailsToolStripMenuItem.Click += new System.EventHandler(this.showDetailsToolStripMenuItem_Click);
			// 
			// LogForm
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(587, 237);
			this.Controls.Add(this.copyToClipboardButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.entriesDataGridView);
			this.Name = "LogForm";
			this.Text = "ReClass.NET - Diagnostic Messages";
			((System.ComponentModel.ISupportInitialize)(this.entriesDataGridView)).EndInit();
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.DataGridView entriesDataGridView;
		private System.Windows.Forms.DataGridViewImageColumn iconColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn messageColumn;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Button copyToClipboardButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
	}
}