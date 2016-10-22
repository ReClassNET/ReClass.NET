namespace ReClassNET
{
	partial class ProcessBrowser
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
			this.processDataGridView = new System.Windows.Forms.DataGridView();
			this.iconColumn = new System.Windows.Forms.DataGridViewImageColumn();
			this.processNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pidColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.pathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.filterCheckBox = new System.Windows.Forms.CheckBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.openProcessButton = new System.Windows.Forms.Button();
			this.loadSymbolsCheckBox = new System.Windows.Forms.CheckBox();
			this.searchGroupBox = new System.Windows.Forms.GroupBox();
			this.previousProcessLinkLabel = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.filterTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.processDataGridView)).BeginInit();
			this.searchGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// processDataGridView
			// 
			this.processDataGridView.AllowUserToAddRows = false;
			this.processDataGridView.AllowUserToDeleteRows = false;
			this.processDataGridView.AllowUserToResizeColumns = false;
			this.processDataGridView.AllowUserToResizeRows = false;
			this.processDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.processDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.processDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.processDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iconColumn,
            this.processNameColumn,
            this.pidColumn,
            this.pathColumn});
			this.processDataGridView.Location = new System.Drawing.Point(12, 151);
			this.processDataGridView.MultiSelect = false;
			this.processDataGridView.Name = "processDataGridView";
			this.processDataGridView.ReadOnly = true;
			this.processDataGridView.RowHeadersVisible = false;
			this.processDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.processDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.processDataGridView.Size = new System.Drawing.Size(549, 291);
			this.processDataGridView.TabIndex = 0;
			this.processDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.processDataGridView_CellMouseDoubleClick);
			// 
			// iconColumn
			// 
			this.iconColumn.DataPropertyName = "icon";
			this.iconColumn.HeaderText = "";
			this.iconColumn.MinimumWidth = 18;
			this.iconColumn.Name = "iconColumn";
			this.iconColumn.ReadOnly = true;
			this.iconColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.iconColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.iconColumn.Width = 18;
			// 
			// processNameColumn
			// 
			this.processNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.processNameColumn.DataPropertyName = "name";
			this.processNameColumn.HeaderText = "Process";
			this.processNameColumn.Name = "processNameColumn";
			this.processNameColumn.ReadOnly = true;
			this.processNameColumn.Width = 70;
			// 
			// pidColumn
			// 
			this.pidColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.pidColumn.DataPropertyName = "id";
			this.pidColumn.HeaderText = "PID";
			this.pidColumn.Name = "pidColumn";
			this.pidColumn.ReadOnly = true;
			this.pidColumn.Width = 50;
			// 
			// pathColumn
			// 
			this.pathColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.pathColumn.DataPropertyName = "path";
			this.pathColumn.HeaderText = "Path";
			this.pathColumn.Name = "pathColumn";
			this.pathColumn.ReadOnly = true;
			// 
			// filterCheckBox
			// 
			this.filterCheckBox.AutoSize = true;
			this.filterCheckBox.Checked = true;
			this.filterCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.filterCheckBox.Location = new System.Drawing.Point(9, 72);
			this.filterCheckBox.Name = "filterCheckBox";
			this.filterCheckBox.Size = new System.Drawing.Size(158, 17);
			this.filterCheckBox.TabIndex = 1;
			this.filterCheckBox.Text = "Exclude common processes";
			this.filterCheckBox.UseVisualStyleBackColor = true;
			this.filterCheckBox.CheckedChanged += new System.EventHandler(this.filterCheckBox_CheckedChanged);
			// 
			// refreshButton
			// 
			this.refreshButton.Image = global::ReClassNET.Properties.Resources.arrow_refresh;
			this.refreshButton.Location = new System.Drawing.Point(9, 99);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(158, 23);
			this.refreshButton.TabIndex = 2;
			this.refreshButton.Text = "Refresh";
			this.refreshButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.refreshButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// openProcessButton
			// 
			this.openProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.openProcessButton.Image = global::ReClassNET.Properties.Resources.accept;
			this.openProcessButton.Location = new System.Drawing.Point(12, 471);
			this.openProcessButton.Name = "openProcessButton";
			this.openProcessButton.Size = new System.Drawing.Size(549, 23);
			this.openProcessButton.TabIndex = 3;
			this.openProcessButton.Text = "Open Process";
			this.openProcessButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.openProcessButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.openProcessButton.UseVisualStyleBackColor = true;
			this.openProcessButton.Click += new System.EventHandler(this.openProcessButton_Click);
			// 
			// loadSymbolsCheckBox
			// 
			this.loadSymbolsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.loadSymbolsCheckBox.AutoSize = true;
			this.loadSymbolsCheckBox.Location = new System.Drawing.Point(12, 448);
			this.loadSymbolsCheckBox.Name = "loadSymbolsCheckBox";
			this.loadSymbolsCheckBox.Size = new System.Drawing.Size(92, 17);
			this.loadSymbolsCheckBox.TabIndex = 4;
			this.loadSymbolsCheckBox.Text = "Load Symbols";
			this.loadSymbolsCheckBox.UseVisualStyleBackColor = true;
			// 
			// searchGroupBox
			// 
			this.searchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.searchGroupBox.Controls.Add(this.previousProcessLinkLabel);
			this.searchGroupBox.Controls.Add(this.label2);
			this.searchGroupBox.Controls.Add(this.label1);
			this.searchGroupBox.Controls.Add(this.filterCheckBox);
			this.searchGroupBox.Controls.Add(this.refreshButton);
			this.searchGroupBox.Controls.Add(this.filterTextBox);
			this.searchGroupBox.Location = new System.Drawing.Point(12, 12);
			this.searchGroupBox.Name = "searchGroupBox";
			this.searchGroupBox.Size = new System.Drawing.Size(549, 133);
			this.searchGroupBox.TabIndex = 5;
			this.searchGroupBox.TabStop = false;
			this.searchGroupBox.Text = "Search for Process";
			// 
			// previousProcessLinkLabel
			// 
			this.previousProcessLinkLabel.AutoSize = true;
			this.previousProcessLinkLabel.Location = new System.Drawing.Point(103, 47);
			this.previousProcessLinkLabel.Name = "previousProcessLinkLabel";
			this.previousProcessLinkLabel.Size = new System.Drawing.Size(19, 13);
			this.previousProcessLinkLabel.TabIndex = 3;
			this.previousProcessLinkLabel.TabStop = true;
			this.previousProcessLinkLabel.Text = "<>";
			this.previousProcessLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.previousProcessLinkLabel_LinkClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 47);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Previous Process:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(79, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Process Name:";
			// 
			// filterTextBox
			// 
			this.filterTextBox.Location = new System.Drawing.Point(103, 19);
			this.filterTextBox.Name = "filterTextBox";
			this.filterTextBox.Size = new System.Drawing.Size(270, 20);
			this.filterTextBox.TabIndex = 0;
			this.filterTextBox.TextChanged += new System.EventHandler(this.filterTextBox_TextChanged);
			// 
			// ProcessBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(573, 506);
			this.Controls.Add(this.searchGroupBox);
			this.Controls.Add(this.loadSymbolsCheckBox);
			this.Controls.Add(this.openProcessButton);
			this.Controls.Add(this.processDataGridView);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProcessBrowser";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Process";
			((System.ComponentModel.ISupportInitialize)(this.processDataGridView)).EndInit();
			this.searchGroupBox.ResumeLayout(false);
			this.searchGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView processDataGridView;
		private System.Windows.Forms.CheckBox filterCheckBox;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.Button openProcessButton;
		private System.Windows.Forms.CheckBox loadSymbolsCheckBox;
		private System.Windows.Forms.DataGridViewImageColumn iconColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn processNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn pidColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn pathColumn;
		private System.Windows.Forms.GroupBox searchGroupBox;
		private System.Windows.Forms.LinkLabel previousProcessLinkLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox filterTextBox;
	}
}