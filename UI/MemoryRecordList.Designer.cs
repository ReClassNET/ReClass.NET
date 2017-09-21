namespace ReClassNET.UI
{
	partial class MemoryRecordList
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
			this.resultDataGridView = new System.Windows.Forms.DataGridView();
			this.descriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.addressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueTypeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.previousValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.resultDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// resultDataGridView
			// 
			this.resultDataGridView.AllowUserToAddRows = false;
			this.resultDataGridView.AllowUserToDeleteRows = false;
			this.resultDataGridView.AllowUserToResizeRows = false;
			this.resultDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.resultDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this.resultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.resultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionColumn,
            this.addressColumn,
            this.valueTypeColumn,
            this.valueColumn,
            this.previousValueColumn});
			this.resultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultDataGridView.Location = new System.Drawing.Point(0, 0);
			this.resultDataGridView.Name = "resultDataGridView";
			this.resultDataGridView.ReadOnly = true;
			this.resultDataGridView.RowHeadersVisible = false;
			this.resultDataGridView.RowTemplate.Height = 19;
			this.resultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.resultDataGridView.Size = new System.Drawing.Size(290, 327);
			this.resultDataGridView.TabIndex = 15;
			this.resultDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultDataGridView_CellDoubleClick);
			this.resultDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.resultDataGridView_CellFormatting);
			this.resultDataGridView.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.resultDataGridView_CellMouseDown);
			this.resultDataGridView.RowContextMenuStripNeeded += new System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventHandler(this.resultDataGridView_RowContextMenuStripNeeded);
			// 
			// descriptionColumn
			// 
			this.descriptionColumn.DataPropertyName = "Description";
			this.descriptionColumn.HeaderText = "Description";
			this.descriptionColumn.Name = "descriptionColumn";
			this.descriptionColumn.ReadOnly = true;
			// 
			// addressColumn
			// 
			this.addressColumn.DataPropertyName = "AddressStr";
			this.addressColumn.HeaderText = "Address";
			this.addressColumn.MinimumWidth = 70;
			this.addressColumn.Name = "addressColumn";
			this.addressColumn.ReadOnly = true;
			// 
			// valueTypeColumn
			// 
			this.valueTypeColumn.DataPropertyName = "ValueType";
			this.valueTypeColumn.HeaderText = "Value Type";
			this.valueTypeColumn.Name = "valueTypeColumn";
			this.valueTypeColumn.ReadOnly = true;
			// 
			// valueColumn
			// 
			this.valueColumn.DataPropertyName = "ValueStr";
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.ReadOnly = true;
			// 
			// previousValueColumn
			// 
			this.previousValueColumn.DataPropertyName = "PreviousValueStr";
			this.previousValueColumn.HeaderText = "Previous";
			this.previousValueColumn.Name = "previousValueColumn";
			this.previousValueColumn.ReadOnly = true;
			// 
			// MemoryRecordList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.resultDataGridView);
			this.Name = "MemoryRecordList";
			this.Size = new System.Drawing.Size(290, 327);
			((System.ComponentModel.ISupportInitialize)(this.resultDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView resultDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn addressColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueTypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn previousValueColumn;
	}
}
