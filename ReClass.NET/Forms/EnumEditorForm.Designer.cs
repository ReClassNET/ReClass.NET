namespace ReClassNET.Forms
{
	partial class EnumEditorForm
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
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.enumNameLabel = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.enumFlagCheckBox = new System.Windows.Forms.CheckBox();
			this.enumNameTextBox = new System.Windows.Forms.TextBox();
			this.enumDataGridView = new System.Windows.Forms.DataGridView();
			this.enumValueKeyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.enumValueNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.enumUnderlyingTypeSizeLabel = new System.Windows.Forms.Label();
			this.enumUnderlyingTypeSizeComboBox = new ReClassNET.Forms.UnderlyingSizeComboBox();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.enumDataGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B16x16_Class_Type;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(380, 48);
			this.bannerBox.TabIndex = 15;
			this.bannerBox.Text = "Edit an enum of the project.";
			this.bannerBox.Title = "Enum Editor";
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.saveButton.Location = new System.Drawing.Point(214, 250);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(75, 23);
			this.saveButton.TabIndex = 22;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// enumNameLabel
			// 
			this.enumNameLabel.AutoSize = true;
			this.enumNameLabel.Location = new System.Drawing.Point(9, 57);
			this.enumNameLabel.Name = "enumNameLabel";
			this.enumNameLabel.Size = new System.Drawing.Size(38, 13);
			this.enumNameLabel.TabIndex = 21;
			this.enumNameLabel.Text = "Name:";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(295, 250);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 19;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// enumFlagCheckBox
			// 
			this.enumFlagCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.enumFlagCheckBox.AutoSize = true;
			this.enumFlagCheckBox.Location = new System.Drawing.Point(267, 82);
			this.enumFlagCheckBox.Name = "enumFlagCheckBox";
			this.enumFlagCheckBox.Size = new System.Drawing.Size(103, 17);
			this.enumFlagCheckBox.TabIndex = 18;
			this.enumFlagCheckBox.Text = "Use Flags Mode";
			this.enumFlagCheckBox.UseVisualStyleBackColor = true;
			// 
			// enumNameTextBox
			// 
			this.enumNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.enumNameTextBox.Location = new System.Drawing.Point(53, 54);
			this.enumNameTextBox.Name = "enumNameTextBox";
			this.enumNameTextBox.Size = new System.Drawing.Size(315, 20);
			this.enumNameTextBox.TabIndex = 17;
			// 
			// enumDataGridView
			// 
			this.enumDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.enumDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.enumDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enumValueKeyColumn,
            this.enumValueNameColumn});
			this.enumDataGridView.Location = new System.Drawing.Point(12, 108);
			this.enumDataGridView.Name = "enumDataGridView";
			this.enumDataGridView.RowHeadersVisible = false;
			this.enumDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.enumDataGridView.Size = new System.Drawing.Size(358, 136);
			this.enumDataGridView.TabIndex = 16;
			this.enumDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.enumDataGridView_CellValidating);
			this.enumDataGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.enumDataGridView_DefaultValuesNeeded);
			// 
			// enumValueKeyColumn
			// 
			this.enumValueKeyColumn.HeaderText = "Value";
			this.enumValueKeyColumn.Name = "enumValueKeyColumn";
			// 
			// enumValueNameColumn
			// 
			this.enumValueNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.enumValueNameColumn.HeaderText = "Name";
			this.enumValueNameColumn.Name = "enumValueNameColumn";
			// 
			// enumUnderlyingTypeSizeLabel
			// 
			this.enumUnderlyingTypeSizeLabel.AutoSize = true;
			this.enumUnderlyingTypeSizeLabel.Location = new System.Drawing.Point(9, 83);
			this.enumUnderlyingTypeSizeLabel.Name = "enumUnderlyingTypeSizeLabel";
			this.enumUnderlyingTypeSizeLabel.Size = new System.Drawing.Size(30, 13);
			this.enumUnderlyingTypeSizeLabel.TabIndex = 24;
			this.enumUnderlyingTypeSizeLabel.Text = "Size:";
			// 
			// enumUnderlyingTypeSizeComboBox
			// 
			this.enumUnderlyingTypeSizeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.enumUnderlyingTypeSizeComboBox.Location = new System.Drawing.Point(53, 80);
			this.enumUnderlyingTypeSizeComboBox.Name = "enumUnderlyingTypeSizeComboBox";
			this.enumUnderlyingTypeSizeComboBox.Size = new System.Drawing.Size(208, 21);
			this.enumUnderlyingTypeSizeComboBox.TabIndex = 25;
			// 
			// EnumEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(380, 282);
			this.Controls.Add(this.enumUnderlyingTypeSizeComboBox);
			this.Controls.Add(this.enumUnderlyingTypeSizeLabel);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.enumNameLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.enumFlagCheckBox);
			this.Controls.Add(this.enumNameTextBox);
			this.Controls.Add(this.enumDataGridView);
			this.Controls.Add(this.bannerBox);
			this.MinimumSize = new System.Drawing.Size(396, 321);
			this.Name = "EnumEditorForm";
			this.Text = "ReClass.NET - Enum Editor";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.enumDataGridView)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.BannerBox bannerBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Label enumNameLabel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox enumFlagCheckBox;
		private System.Windows.Forms.TextBox enumNameTextBox;
		private System.Windows.Forms.DataGridView enumDataGridView;
		private System.Windows.Forms.Label enumUnderlyingTypeSizeLabel;
		private System.Windows.Forms.DataGridViewTextBoxColumn enumValueKeyColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn enumValueNameColumn;
		private UnderlyingSizeComboBox enumUnderlyingTypeSizeComboBox;
	}
}