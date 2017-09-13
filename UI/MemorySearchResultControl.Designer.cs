namespace ReClassNET.UI
{
	partial class MemorySearchResultControl
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
			this.resultAddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.resultValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.resultPreviousValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.resultDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// resultDataGridView
			// 
			this.resultDataGridView.AllowUserToAddRows = false;
			this.resultDataGridView.AllowUserToDeleteRows = false;
			this.resultDataGridView.AllowUserToResizeRows = false;
			this.resultDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
			this.resultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.resultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.descriptionColumn,
            this.resultAddressColumn,
            this.resultValueColumn,
            this.resultPreviousValueColumn});
			this.resultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resultDataGridView.Location = new System.Drawing.Point(0, 0);
			this.resultDataGridView.MultiSelect = false;
			this.resultDataGridView.Name = "resultDataGridView";
			this.resultDataGridView.ReadOnly = true;
			this.resultDataGridView.RowHeadersVisible = false;
			this.resultDataGridView.RowTemplate.Height = 19;
			this.resultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.resultDataGridView.Size = new System.Drawing.Size(290, 327);
			this.resultDataGridView.TabIndex = 15;
			// 
			// descriptionColumn
			// 
			this.descriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.descriptionColumn.DataPropertyName = "Description";
			this.descriptionColumn.HeaderText = "Description";
			this.descriptionColumn.Name = "descriptionColumn";
			this.descriptionColumn.ReadOnly = true;
			// 
			// resultAddressColumn
			// 
			this.resultAddressColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.resultAddressColumn.DataPropertyName = "Address";
			this.resultAddressColumn.HeaderText = "Address";
			this.resultAddressColumn.Name = "resultAddressColumn";
			this.resultAddressColumn.ReadOnly = true;
			// 
			// resultValueColumn
			// 
			this.resultValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.resultValueColumn.DataPropertyName = "Value";
			this.resultValueColumn.HeaderText = "Value";
			this.resultValueColumn.Name = "resultValueColumn";
			this.resultValueColumn.ReadOnly = true;
			this.resultValueColumn.Width = 59;
			// 
			// resultPreviousValueColumn
			// 
			this.resultPreviousValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.resultPreviousValueColumn.DataPropertyName = "Previous";
			this.resultPreviousValueColumn.HeaderText = "Previous";
			this.resultPreviousValueColumn.Name = "resultPreviousValueColumn";
			this.resultPreviousValueColumn.ReadOnly = true;
			this.resultPreviousValueColumn.Width = 73;
			// 
			// MemorySearchResultControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.resultDataGridView);
			this.Name = "MemorySearchResultControl";
			this.Size = new System.Drawing.Size(290, 327);
			((System.ComponentModel.ISupportInitialize)(this.resultDataGridView)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView resultDataGridView;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn resultAddressColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn resultValueColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn resultPreviousValueColumn;
	}
}
