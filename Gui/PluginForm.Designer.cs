namespace ReClassNET.Gui
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
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.descriptionGroupBox = new System.Windows.Forms.GroupBox();
			this.descriptionLabel = new System.Windows.Forms.Label();
			this.pluginsDataGridView = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label9 = new System.Windows.Forms.Label();
			this.comboBox8 = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.comboBox7 = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.comboBox6 = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.comboBox5 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.comboBox4 = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.getMoreLinkLabel = new System.Windows.Forms.LinkLabel();
			this.closeButton = new System.Windows.Forms.Button();
			this.iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.versionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.authorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.descriptionGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pluginsDataGridView)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Location = new System.Drawing.Point(12, 12);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(716, 312);
			this.tabControl.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.descriptionGroupBox);
			this.tabPage1.Controls.Add(this.pluginsDataGridView);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(708, 286);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Plugins";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// descriptionGroupBox
			// 
			this.descriptionGroupBox.Controls.Add(this.descriptionLabel);
			this.descriptionGroupBox.Location = new System.Drawing.Point(6, 190);
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
			this.pluginsDataGridView.Size = new System.Drawing.Size(702, 181);
			this.pluginsDataGridView.TabIndex = 0;
			this.pluginsDataGridView.SelectionChanged += new System.EventHandler(this.pluginsDataGridView_SelectionChanged);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label9);
			this.tabPage2.Controls.Add(this.comboBox8);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.comboBox7);
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.comboBox6);
			this.tabPage2.Controls.Add(this.label6);
			this.tabPage2.Controls.Add(this.comboBox5);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.comboBox4);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.comboBox3);
			this.tabPage2.Controls.Add(this.comboBox2);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.comboBox1);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(708, 286);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Native Helper";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 247);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(128, 13);
			this.label9.TabIndex = 16;
			this.label9.Text = "DisassembleRemoteCode";
			// 
			// comboBox8
			// 
			this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox8.FormattingEnabled = true;
			this.comboBox8.Location = new System.Drawing.Point(206, 244);
			this.comboBox8.Name = "comboBox8";
			this.comboBox8.Size = new System.Drawing.Size(121, 21);
			this.comboBox8.TabIndex = 15;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 220);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(106, 13);
			this.label8.TabIndex = 14;
			this.label8.Text = "WriteRemoteMemory";
			// 
			// comboBox7
			// 
			this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox7.FormattingEnabled = true;
			this.comboBox7.Location = new System.Drawing.Point(206, 217);
			this.comboBox7.Name = "comboBox7";
			this.comboBox7.Size = new System.Drawing.Size(121, 21);
			this.comboBox7.TabIndex = 13;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 193);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(107, 13);
			this.label7.TabIndex = 12;
			this.label7.Text = "ReadRemoteMemory";
			// 
			// comboBox6
			// 
			this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox6.FormattingEnabled = true;
			this.comboBox6.Location = new System.Drawing.Point(206, 190);
			this.comboBox6.Name = "comboBox6";
			this.comboBox6.Size = new System.Drawing.Size(121, 21);
			this.comboBox6.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 166);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(108, 13);
			this.label6.TabIndex = 10;
			this.label6.Text = "CloseRemoteProcess";
			// 
			// comboBox5
			// 
			this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox5.FormattingEnabled = true;
			this.comboBox5.Location = new System.Drawing.Point(206, 163);
			this.comboBox5.Name = "comboBox5";
			this.comboBox5.Size = new System.Drawing.Size(121, 21);
			this.comboBox5.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 139);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(108, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "OpenRemoteProcess";
			// 
			// comboBox4
			// 
			this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox4.FormattingEnabled = true;
			this.comboBox4.Location = new System.Drawing.Point(206, 136);
			this.comboBox4.Name = "comboBox4";
			this.comboBox4.Size = new System.Drawing.Size(121, 21);
			this.comboBox4.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 112);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "IsProcessValid";
			// 
			// comboBox3
			// 
			this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Location = new System.Drawing.Point(206, 109);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(121, 21);
			this.comboBox3.TabIndex = 5;
			// 
			// comboBox2
			// 
			this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Location = new System.Drawing.Point(206, 55);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(121, 21);
			this.comboBox2.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 85);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(195, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "EnumerateRemoteSectionsAndModules";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(107, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "EnumerateProcesses";
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(206, 82);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 1;
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
			this.getMoreLinkLabel.AutoSize = true;
			this.getMoreLinkLabel.Location = new System.Drawing.Point(9, 332);
			this.getMoreLinkLabel.Name = "getMoreLinkLabel";
			this.getMoreLinkLabel.Size = new System.Drawing.Size(95, 13);
			this.getMoreLinkLabel.TabIndex = 1;
			this.getMoreLinkLabel.TabStop = true;
			this.getMoreLinkLabel.Text = "Get more plugins...";
			// 
			// closeButton
			// 
			this.closeButton.Location = new System.Drawing.Point(653, 327);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
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
			// PluginForm
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(740, 359);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.getMoreLinkLabel);
			this.Controls.Add(this.tabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "PluginForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Plugins";
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.descriptionGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pluginsDataGridView)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.GroupBox descriptionGroupBox;
		private System.Windows.Forms.DataGridView pluginsDataGridView;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.LinkLabel getMoreLinkLabel;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Label descriptionLabel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox comboBox8;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox comboBox7;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox comboBox6;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.ComboBox comboBox5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox4;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.DataGridViewImageColumn iconColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn versionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn authorColumn;
	}
}