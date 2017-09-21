namespace ReClassNET.Forms
{
	partial class FoundCodeForm
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.foundCodeDataGridView = new System.Windows.Forms.DataGridView();
			this.counterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.instructionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.infoTextBox = new System.Windows.Forms.TextBox();
			this.stopButton = new System.Windows.Forms.Button();
			this.closeButton = new System.Windows.Forms.Button();
			this.createFunctionButton = new System.Windows.Forms.Button();
			this.bannerBox = new ReClassNET.UI.BannerBox();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.foundCodeDataGridView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer.Location = new System.Drawing.Point(0, 49);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.foundCodeDataGridView);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.infoTextBox);
			this.splitContainer.Size = new System.Drawing.Size(476, 426);
			this.splitContainer.SplitterDistance = 200;
			this.splitContainer.TabIndex = 0;
			// 
			// foundCodeDataGridView
			// 
			this.foundCodeDataGridView.AllowUserToAddRows = false;
			this.foundCodeDataGridView.AllowUserToDeleteRows = false;
			this.foundCodeDataGridView.AllowUserToResizeRows = false;
			this.foundCodeDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
			this.foundCodeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.foundCodeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.counterDataGridViewTextBoxColumn,
            this.instructionDataGridViewTextBoxColumn});
			this.foundCodeDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.foundCodeDataGridView.Location = new System.Drawing.Point(0, 0);
			this.foundCodeDataGridView.MultiSelect = false;
			this.foundCodeDataGridView.Name = "foundCodeDataGridView";
			this.foundCodeDataGridView.ReadOnly = true;
			this.foundCodeDataGridView.RowHeadersVisible = false;
			this.foundCodeDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.foundCodeDataGridView.Size = new System.Drawing.Size(476, 200);
			this.foundCodeDataGridView.TabIndex = 0;
			this.foundCodeDataGridView.SelectionChanged += new System.EventHandler(this.foundCodeDataGridView_SelectionChanged);
			// 
			// counterDataGridViewTextBoxColumn
			// 
			this.counterDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.counterDataGridViewTextBoxColumn.DataPropertyName = "counter";
			this.counterDataGridViewTextBoxColumn.HeaderText = "Counter";
			this.counterDataGridViewTextBoxColumn.Name = "counterDataGridViewTextBoxColumn";
			this.counterDataGridViewTextBoxColumn.ReadOnly = true;
			this.counterDataGridViewTextBoxColumn.Width = 69;
			// 
			// instructionDataGridViewTextBoxColumn
			// 
			this.instructionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.instructionDataGridViewTextBoxColumn.DataPropertyName = "instruction";
			this.instructionDataGridViewTextBoxColumn.HeaderText = "Instruction";
			this.instructionDataGridViewTextBoxColumn.Name = "instructionDataGridViewTextBoxColumn";
			this.instructionDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// infoTextBox
			// 
			this.infoTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoTextBox.Location = new System.Drawing.Point(0, 0);
			this.infoTextBox.Multiline = true;
			this.infoTextBox.Name = "infoTextBox";
			this.infoTextBox.ReadOnly = true;
			this.infoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.infoTextBox.Size = new System.Drawing.Size(476, 222);
			this.infoTextBox.TabIndex = 0;
			// 
			// stopButton
			// 
			this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.stopButton.Location = new System.Drawing.Point(489, 101);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(86, 35);
			this.stopButton.TabIndex = 1;
			this.stopButton.Text = "Stop";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.Location = new System.Drawing.Point(489, 101);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(86, 35);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Visible = false;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// createFunctionButton
			// 
			this.createFunctionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.createFunctionButton.Location = new System.Drawing.Point(489, 60);
			this.createFunctionButton.Name = "createFunctionButton";
			this.createFunctionButton.Size = new System.Drawing.Size(86, 35);
			this.createFunctionButton.TabIndex = 3;
			this.createFunctionButton.Text = "Create Function Node";
			this.createFunctionButton.UseVisualStyleBackColor = true;
			this.createFunctionButton.Click += new System.EventHandler(this.createFunctionButton_Click);
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_3D_Glasses;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(587, 48);
			this.bannerBox.TabIndex = 8;
			this.bannerBox.Text = "<>";
			this.bannerBox.Title = "Instruction Finder";
			// 
			// FoundCodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(587, 474);
			this.Controls.Add(this.bannerBox);
			this.Controls.Add(this.createFunctionButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.stopButton);
			this.Controls.Add(this.splitContainer);
			this.MinimumSize = new System.Drawing.Size(603, 464);
			this.Name = "FoundCodeForm";
			this.Text = "<>";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FoundCodeForm_FormClosed);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.foundCodeDataGridView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.DataGridView foundCodeDataGridView;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn counterDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn instructionDataGridViewTextBoxColumn;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.TextBox infoTextBox;
		private System.Windows.Forms.Button createFunctionButton;
		private UI.BannerBox bannerBox;
	}
}