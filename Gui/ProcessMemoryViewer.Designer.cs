namespace ReClassNET
{
	partial class ProcessMemoryViewer
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.sectionsDataGridView = new System.Windows.Forms.DataGridView();
			this.addressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.sizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.protectionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.stateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.moduleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.setCurrentClassAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.createClassAtAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.regionsGroupBox = new System.Windows.Forms.GroupBox();
			((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).BeginInit();
			this.contextMenuStrip.SuspendLayout();
			this.regionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// sectionsDataGridView
			// 
			this.sectionsDataGridView.AllowUserToAddRows = false;
			this.sectionsDataGridView.AllowUserToDeleteRows = false;
			this.sectionsDataGridView.AllowUserToResizeRows = false;
			this.sectionsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sectionsDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.sectionsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.sectionsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.addressColumn,
            this.sizeColumn,
            this.nameColumn,
            this.protectionColumn,
            this.stateColumn,
            this.typeColumn,
            this.moduleColumn});
			this.sectionsDataGridView.Location = new System.Drawing.Point(6, 19);
			this.sectionsDataGridView.MultiSelect = false;
			this.sectionsDataGridView.Name = "sectionsDataGridView";
			this.sectionsDataGridView.ReadOnly = true;
			this.sectionsDataGridView.RowHeadersVisible = false;
			this.sectionsDataGridView.RowTemplate.ContextMenuStrip = this.contextMenuStrip;
			this.sectionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.sectionsDataGridView.Size = new System.Drawing.Size(978, 462);
			this.sectionsDataGridView.TabIndex = 0;
			this.sectionsDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sectionsDataGridView_CellMouseDoubleClick);
			this.sectionsDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sectionsDataGridView_CellMouseDown);
			// 
			// addressColumn
			// 
			this.addressColumn.DataPropertyName = "address";
			dataGridViewCellStyle1.Format = "X";
			this.addressColumn.DefaultCellStyle = dataGridViewCellStyle1;
			this.addressColumn.HeaderText = "Address";
			this.addressColumn.Name = "addressColumn";
			this.addressColumn.ReadOnly = true;
			// 
			// sizeColumn
			// 
			this.sizeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.sizeColumn.DataPropertyName = "size";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			dataGridViewCellStyle2.Format = "X";
			this.sizeColumn.DefaultCellStyle = dataGridViewCellStyle2;
			this.sizeColumn.HeaderText = "Size";
			this.sizeColumn.Name = "sizeColumn";
			this.sizeColumn.ReadOnly = true;
			this.sizeColumn.Width = 52;
			// 
			// nameColumn
			// 
			this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.nameColumn.DataPropertyName = "name";
			this.nameColumn.HeaderText = "Name";
			this.nameColumn.Name = "nameColumn";
			this.nameColumn.ReadOnly = true;
			this.nameColumn.Width = 60;
			// 
			// protectionColumn
			// 
			this.protectionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.protectionColumn.DataPropertyName = "protection";
			this.protectionColumn.HeaderText = "Protection";
			this.protectionColumn.Name = "protectionColumn";
			this.protectionColumn.ReadOnly = true;
			this.protectionColumn.Width = 80;
			// 
			// stateColumn
			// 
			this.stateColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.stateColumn.DataPropertyName = "state";
			this.stateColumn.HeaderText = "State";
			this.stateColumn.Name = "stateColumn";
			this.stateColumn.ReadOnly = true;
			this.stateColumn.Width = 57;
			// 
			// typeColumn
			// 
			this.typeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.typeColumn.DataPropertyName = "type";
			this.typeColumn.HeaderText = "Type";
			this.typeColumn.Name = "typeColumn";
			this.typeColumn.ReadOnly = true;
			this.typeColumn.Width = 56;
			// 
			// moduleColumn
			// 
			this.moduleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.moduleColumn.DataPropertyName = "module";
			this.moduleColumn.HeaderText = "Module";
			this.moduleColumn.Name = "moduleColumn";
			this.moduleColumn.ReadOnly = true;
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setCurrentClassAddressToolStripMenuItem,
            this.toolStripSeparator1,
            this.createClassAtAddressToolStripMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip";
			this.contextMenuStrip.Size = new System.Drawing.Size(203, 54);
			// 
			// setCurrentClassAddressToolStripMenuItem
			// 
			this.setCurrentClassAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.exchange_icon;
			this.setCurrentClassAddressToolStripMenuItem.Name = "setCurrentClassAddressToolStripMenuItem";
			this.setCurrentClassAddressToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.setCurrentClassAddressToolStripMenuItem.Text = "Set current class address";
			this.setCurrentClassAddressToolStripMenuItem.Click += new System.EventHandler(this.setCurrentClassAddressToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(199, 6);
			// 
			// createClassAtAddressToolStripMenuItem
			// 
			this.createClassAtAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.button_class_add;
			this.createClassAtAddressToolStripMenuItem.Name = "createClassAtAddressToolStripMenuItem";
			this.createClassAtAddressToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
			this.createClassAtAddressToolStripMenuItem.Text = "Create class at address";
			this.createClassAtAddressToolStripMenuItem.Click += new System.EventHandler(this.createClassAtAddressToolStripMenuItem_Click);
			// 
			// regionsGroupBox
			// 
			this.regionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.regionsGroupBox.Controls.Add(this.sectionsDataGridView);
			this.regionsGroupBox.Location = new System.Drawing.Point(12, 12);
			this.regionsGroupBox.Name = "regionsGroupBox";
			this.regionsGroupBox.Size = new System.Drawing.Size(990, 487);
			this.regionsGroupBox.TabIndex = 1;
			this.regionsGroupBox.TabStop = false;
			this.regionsGroupBox.Text = "Memory Regions";
			// 
			// ProcessMemoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 511);
			this.Controls.Add(this.regionsGroupBox);
			this.Name = "ProcessMemoryViewer";
			this.Text = "ProcessMemoryViewer";
			((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).EndInit();
			this.contextMenuStrip.ResumeLayout(false);
			this.regionsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView sectionsDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn addressColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sizeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn protectionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn stateColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn moduleColumn;
		private System.Windows.Forms.GroupBox regionsGroupBox;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem setCurrentClassAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem createClassAtAddressToolStripMenuItem;
	}
}