namespace ReClassNET.Forms
{
	partial class ClassSelectionForm
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
			this.classesListBox = new System.Windows.Forms.ListBox();
			this.filterNameTextBox = new ReClassNET.UI.PlaceholderTextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.selectButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B16x16_Class_Type;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(516, 48);
			this.bannerBox.TabIndex = 9;
			this.bannerBox.Text = "Select a class of the project.";
			this.bannerBox.Title = "Class Selection";
			// 
			// classesListBox
			// 
			this.classesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.classesListBox.DisplayMember = "Name";
			this.classesListBox.FormattingEnabled = true;
			this.classesListBox.Location = new System.Drawing.Point(12, 80);
			this.classesListBox.Name = "classesListBox";
			this.classesListBox.Size = new System.Drawing.Size(492, 186);
			this.classesListBox.TabIndex = 2;
			this.classesListBox.SelectedIndexChanged += new System.EventHandler(this.classesListBox_SelectedIndexChanged);
			this.classesListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.classesListBox_MouseDoubleClick);
			// 
			// filterNameTextBox
			// 
			this.filterNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filterNameTextBox.Location = new System.Drawing.Point(12, 54);
			this.filterNameTextBox.Name = "filterNameTextBox";
			this.filterNameTextBox.PlaceholderText = "Filter by Class Name...";
			this.filterNameTextBox.Size = new System.Drawing.Size(492, 20);
			this.filterNameTextBox.TabIndex = 1;
			this.filterNameTextBox.TextChanged += new System.EventHandler(this.filterNameTextBox_TextChanged);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(430, 272);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// selectButton
			// 
			this.selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.selectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.selectButton.Enabled = false;
			this.selectButton.Location = new System.Drawing.Point(328, 272);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(95, 23);
			this.selectButton.TabIndex = 3;
			this.selectButton.Text = "Select Class";
			this.selectButton.UseVisualStyleBackColor = true;
			// 
			// ClassSelectionForm
			// 
			this.AcceptButton = this.selectButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(516, 306);
			this.Controls.Add(this.selectButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.filterNameTextBox);
			this.Controls.Add(this.classesListBox);
			this.Controls.Add(this.bannerBox);
			this.Name = "ClassSelectionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Class Selection";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.BannerBox bannerBox;
		private System.Windows.Forms.ListBox classesListBox;
		private UI.PlaceholderTextBox filterNameTextBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button selectButton;
	}
}