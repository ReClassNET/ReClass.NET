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
			((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// sectionsDataGridView
			// 
			this.sectionsDataGridView.AllowUserToAddRows = false;
			this.sectionsDataGridView.AllowUserToDeleteRows = false;
			this.sectionsDataGridView.AllowUserToResizeRows = false;
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
			this.sectionsDataGridView.Location = new System.Drawing.Point(12, 22);
			this.sectionsDataGridView.MultiSelect = false;
			this.sectionsDataGridView.Name = "sectionsDataGridView";
			this.sectionsDataGridView.ReadOnly = true;
			this.sectionsDataGridView.RowHeadersVisible = false;
			this.sectionsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.sectionsDataGridView.Size = new System.Drawing.Size(990, 446);
			this.sectionsDataGridView.TabIndex = 0;
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
			// ProcessMemoryViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 511);
			this.Controls.Add(this.sectionsDataGridView);
			this.Name = "ProcessMemoryViewer";
			this.Text = "ProcessMemoryViewer";
			((System.ComponentModel.ISupportInitialize)(this.sectionsDataGridView)).EndInit();
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
	}
}