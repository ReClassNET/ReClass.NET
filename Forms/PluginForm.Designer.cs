namespace ReClassNET.Forms
{
	partial class PluginForm
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
			this.tabControl = new System.Windows.Forms.TabControl();
			this.pluginsTabPage = new System.Windows.Forms.TabPage();
			this.descriptionGroupBox = new System.Windows.Forms.GroupBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.pluginsDataGridView = new System.Windows.Forms.DataGridView();
			this.iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.versionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.authorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nativesTabPage = new System.Windows.Forms.TabPage();
			this.label10 = new System.Windows.Forms.Label();
			this.controlRemoteProcessComboBox = new System.Windows.Forms.ComboBox();
			this.label9 = new System.Windows.Forms.Label();
			this.disassembleRemoteCodeComboBox = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.writeRemoteMemoryComboBox = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.readRemoteMemoryComboBox = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.closeRemoteProcessComboBox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.openRemoteProcessComboBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.isProcessValidComboBox = new System.Windows.Forms.ComboBox();
			this.enumerateProcessesComboBox = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.enumerateRemoteSectionsAndModulesComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.getMoreLinkLabel = new System.Windows.Forms.LinkLabel();
			this.closeButton = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.pluginsTabPage.SuspendLayout();
			this.descriptionGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pluginsDataGridView)).BeginInit();
			this.nativesTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.pluginsTabPage);
			this.tabControl.Controls.Add(this.nativesTabPage);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(716, 328);
			this.tabControl.TabIndex = 0;
			// 
			// pluginsTabPage
			// 
			this.pluginsTabPage.Controls.Add(this.descriptionGroupBox);
			this.pluginsTabPage.Controls.Add(this.pluginsDataGridView);
			this.pluginsTabPage.Location = new System.Drawing.Point(4, 22);
			this.pluginsTabPage.Name = "pluginsTabPage";
			this.pluginsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.pluginsTabPage.Size = new System.Drawing.Size(708, 302);
			this.pluginsTabPage.TabIndex = 0;
			this.pluginsTabPage.Text = "Plugins";
			this.pluginsTabPage.UseVisualStyleBackColor = true;
			// 
			// descriptionGroupBox
			// 
			this.descriptionGroupBox.Controls.Add(this.descriptionLabel);
			this.descriptionGroupBox.Location = new System.Drawing.Point(6, 206);
			this.descriptionGroupBox.Name = "descriptionGroupBox";
			this.descriptionGroupBox.Size = new System.Drawing.Size(696, 90);
			this.descriptionGroupBox.TabIndex = 1;
			this.descriptionGroupBox.TabStop = false;
			this.descriptionGroupBox.Text = "<>";
			// 
			// descriptionLabel
			// 
			this.descriptionLabel.Location = new System.Drawing.Point(6, 16);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new System.Drawing.Size(684, 65);
			this.descriptionLabel.TabIndex = 0;
			this.descriptionLabel.Text = "<>";
			// 
			// pluginsDataGridView
			// 
			this.pluginsDataGridView.AllowUserToAddRows = false;
			this.pluginsDataGridView.AllowUserToDeleteRows = false;
			this.pluginsDataGridView.AllowUserToResizeRows = false;
			this.pluginsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.pluginsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.pluginsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconColumn,
            this.nameColumn,
            this.versionColumn,
            this.authorColumn});
			this.pluginsDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
			this.pluginsDataGridView.Location = new System.Drawing.Point(3, 3);
			this.pluginsDataGridView.MultiSelect = false;
			this.pluginsDataGridView.Name = "pluginsDataGridView";
			this.pluginsDataGridView.ReadOnly = true;
			this.pluginsDataGridView.RowHeadersVisible = false;
			this.pluginsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.pluginsDataGridView.Size = new System.Drawing.Size(702, 197);
			this.pluginsDataGridView.TabIndex = 0;
			this.pluginsDataGridView.SelectionChanged += new System.EventHandler(this.pluginsDataGridView_SelectionChanged);
			// 
			// iconColumn
			// 
			this.iconColumn.DataPropertyName = "Icon";
			this.iconColumn.HeaderText = "";
			this.iconColumn.MinimumWidth = 18;
			this.iconColumn.Name = "iconColumn";
			this.iconColumn.ReadOnly = true;
			this.iconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.iconColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.iconColumn.Width = 18;
			// 
			// nameColumn
			// 
			this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.nameColumn.DataPropertyName = "Name";
			this.nameColumn.HeaderText = "Name";
			this.nameColumn.Name = "nameColumn";
			this.nameColumn.ReadOnly = true;
			// 
			// versionColumn
			// 
			this.versionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.versionColumn.DataPropertyName = "Version";
			this.versionColumn.HeaderText = "Version";
			this.versionColumn.Name = "versionColumn";
			this.versionColumn.ReadOnly = true;
			this.versionColumn.Width = 67;
			// 
			// authorColumn
			// 
			this.authorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.authorColumn.DataPropertyName = "Author";
			this.authorColumn.HeaderText = "Author";
			this.authorColumn.Name = "authorColumn";
			this.authorColumn.ReadOnly = true;
			this.authorColumn.Width = 63;
			// 
			// nativesTabPage
			// 
			this.nativesTabPage.Controls.Add(this.label10);
			this.nativesTabPage.Controls.Add(this.controlRemoteProcessComboBox);
			this.nativesTabPage.Controls.Add(this.label9);
			this.nativesTabPage.Controls.Add(this.disassembleRemoteCodeComboBox);
			this.nativesTabPage.Controls.Add(this.label8);
			this.nativesTabPage.Controls.Add(this.writeRemoteMemoryComboBox);
			this.nativesTabPage.Controls.Add(this.label7);
			this.nativesTabPage.Controls.Add(this.readRemoteMemoryComboBox);
			this.nativesTabPage.Controls.Add(this.label6);
			this.nativesTabPage.Controls.Add(this.closeRemoteProcessComboBox);
			this.nativesTabPage.Controls.Add(this.label5);
			this.nativesTabPage.Controls.Add(this.openRemoteProcessComboBox);
			this.nativesTabPage.Controls.Add(this.label4);
			this.nativesTabPage.Controls.Add(this.isProcessValidComboBox);
			this.nativesTabPage.Controls.Add(this.enumerateProcessesComboBox);
			this.nativesTabPage.Controls.Add(this.label3);
			this.nativesTabPage.Controls.Add(this.label2);
			this.nativesTabPage.Controls.Add(this.enumerateRemoteSectionsAndModulesComboBox);
			this.nativesTabPage.Controls.Add(this.label1);
			this.nativesTabPage.Location = new System.Drawing.Point(4, 22);
			this.nativesTabPage.Name = "nativesTabPage";
			this.nativesTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.nativesTabPage.Size = new System.Drawing.Size(708, 302);
			this.nativesTabPage.TabIndex = 1;
			this.nativesTabPage.Text = "Native Helper";
			this.nativesTabPage.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 268);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(115, 13);
			this.label10.TabIndex = 18;
			this.label10.Text = "ControlRemoteProcess";
			// 
			// controlRemoteProcessComboBox
			// 
			this.controlRemoteProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.controlRemoteProcessComboBox.FormattingEnabled = true;
			this.controlRemoteProcessComboBox.Location = new System.Drawing.Point(218, 265);
			this.controlRemoteProcessComboBox.Name = "controlRemoteProcessComboBox";
			this.controlRemoteProcessComboBox.Size = new System.Drawing.Size(161, 21);
			this.controlRemoteProcessComboBox.TabIndex = 17;
			this.controlRemoteProcessComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 241);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 13);
			this.label9.TabIndex = 16;
			this.label9.Text = "DisassembleRemoteCode";
			// 
			// disassembleRemoteCodeComboBox
			// 
			this.disassembleRemoteCodeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.disassembleRemoteCodeComboBox.FormattingEnabled = true;
			this.disassembleRemoteCodeComboBox.Location = new System.Drawing.Point(218, 238);
			this.disassembleRemoteCodeComboBox.Name = "disassembleRemoteCodeComboBox";
			this.disassembleRemoteCodeComboBox.Size = new System.Drawing.Size(161, 21);
			this.disassembleRemoteCodeComboBox.TabIndex = 15;
			this.disassembleRemoteCodeComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 214);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(106, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "WriteRemoteMemory";
			// 
			// writeRemoteMemoryComboBox
			// 
			this.writeRemoteMemoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.writeRemoteMemoryComboBox.FormattingEnabled = true;
			this.writeRemoteMemoryComboBox.Location = new System.Drawing.Point(218, 211);
			this.writeRemoteMemoryComboBox.Name = "writeRemoteMemoryComboBox";
			this.writeRemoteMemoryComboBox.Size = new System.Drawing.Size(161, 21);
			this.writeRemoteMemoryComboBox.TabIndex = 13;
			this.writeRemoteMemoryComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 187);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(107, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "ReadRemoteMemory";
			// 
			// readRemoteMemoryComboBox
			// 
			this.readRemoteMemoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.readRemoteMemoryComboBox.FormattingEnabled = true;
			this.readRemoteMemoryComboBox.Location = new System.Drawing.Point(218, 184);
			this.readRemoteMemoryComboBox.Name = "readRemoteMemoryComboBox";
			this.readRemoteMemoryComboBox.Size = new System.Drawing.Size(161, 21);
			this.readRemoteMemoryComboBox.TabIndex = 11;
			this.readRemoteMemoryComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 160);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(108, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "CloseRemoteProcess";
			// 
			// closeRemoteProcessComboBox
			// 
			this.closeRemoteProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.closeRemoteProcessComboBox.FormattingEnabled = true;
			this.closeRemoteProcessComboBox.Location = new System.Drawing.Point(218, 157);
			this.closeRemoteProcessComboBox.Name = "closeRemoteProcessComboBox";
			this.closeRemoteProcessComboBox.Size = new System.Drawing.Size(161, 21);
			this.closeRemoteProcessComboBox.TabIndex = 9;
			this.closeRemoteProcessComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 133);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(108, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "OpenRemoteProcess";
			// 
			// openRemoteProcessComboBox
			// 
			this.openRemoteProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.openRemoteProcessComboBox.FormattingEnabled = true;
			this.openRemoteProcessComboBox.Location = new System.Drawing.Point(218, 130);
			this.openRemoteProcessComboBox.Name = "openRemoteProcessComboBox";
			this.openRemoteProcessComboBox.Size = new System.Drawing.Size(161, 21);
			this.openRemoteProcessComboBox.TabIndex = 7;
			this.openRemoteProcessComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 106);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "IsProcessValid";
			// 
			// isProcessValidComboBox
			// 
			this.isProcessValidComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.isProcessValidComboBox.FormattingEnabled = true;
			this.isProcessValidComboBox.Location = new System.Drawing.Point(218, 103);
			this.isProcessValidComboBox.Name = "isProcessValidComboBox";
			this.isProcessValidComboBox.Size = new System.Drawing.Size(161, 21);
			this.isProcessValidComboBox.TabIndex = 5;
			this.isProcessValidComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// enumerateProcessesComboBox
			// 
			this.enumerateProcessesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.enumerateProcessesComboBox.FormattingEnabled = true;
			this.enumerateProcessesComboBox.Location = new System.Drawing.Point(218, 49);
			this.enumerateProcessesComboBox.Name = "enumerateProcessesComboBox";
			this.enumerateProcessesComboBox.Size = new System.Drawing.Size(161, 21);
			this.enumerateProcessesComboBox.TabIndex = 4;
			this.enumerateProcessesComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 79);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(195, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "EnumerateRemoteSectionsAndModules";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 52);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "EnumerateProcesses";
			// 
			// enumerateRemoteSectionsAndModulesComboBox
			// 
			this.enumerateRemoteSectionsAndModulesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.enumerateRemoteSectionsAndModulesComboBox.FormattingEnabled = true;
			this.enumerateRemoteSectionsAndModulesComboBox.Location = new System.Drawing.Point(218, 76);
			this.enumerateRemoteSectionsAndModulesComboBox.Name = "enumerateRemoteSectionsAndModulesComboBox";
			this.enumerateRemoteSectionsAndModulesComboBox.Size = new System.Drawing.Size(161, 21);
			this.enumerateRemoteSectionsAndModulesComboBox.TabIndex = 1;
			this.enumerateRemoteSectionsAndModulesComboBox.SelectionChangeCommitted += new System.EventHandler(this.NativeMethodComboBox_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(409, 26);
			this.label1.TabIndex = 0;
			this.label1.Text = "Plugins can provide different methods how ReClass.NET accesses a remote process.\r" +
    "\nYou can select these methods here:";
			// 
			// getMoreLinkLabel
			// 
			this.getMoreLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.getMoreLinkLabel.AutoSize = true;
			this.getMoreLinkLabel.Location = new System.Drawing.Point(9, 348);
			this.getMoreLinkLabel.Name = "getMoreLinkLabel";
			this.getMoreLinkLabel.Size = new System.Drawing.Size(95, 13);
			this.getMoreLinkLabel.TabIndex = 1;
			this.getMoreLinkLabel.TabStop = true;
			this.getMoreLinkLabel.Text = "Get more plugins...";
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeButton.Location = new System.Drawing.Point(653, 343);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			// 
			// PluginForm
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(740, 375);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.getMoreLinkLabel);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "PluginForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Plugins";
			this.tabControl.ResumeLayout(false);
			this.pluginsTabPage.ResumeLayout(false);
			this.descriptionGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pluginsDataGridView)).EndInit();
			this.nativesTabPage.ResumeLayout(false);
			this.nativesTabPage.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage pluginsTabPage;
		private System.Windows.Forms.GroupBox descriptionGroupBox;
		private System.Windows.Forms.DataGridView pluginsDataGridView;
		private System.Windows.Forms.TabPage nativesTabPage;
		private System.Windows.Forms.LinkLabel getMoreLinkLabel;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox disassembleRemoteCodeComboBox;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox writeRemoteMemoryComboBox;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox readRemoteMemoryComboBox;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox closeRemoteProcessComboBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox openRemoteProcessComboBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox isProcessValidComboBox;
		private System.Windows.Forms.ComboBox enumerateProcessesComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox enumerateRemoteSectionsAndModulesComboBox;
		private System.Windows.Forms.DataGridViewImageColumn iconColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn versionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn authorColumn;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.ComboBox controlRemoteProcessComboBox;
	}
}