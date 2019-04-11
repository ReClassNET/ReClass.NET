namespace ReClassNET.Forms
{
	partial class EnumListForm
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
			this.filterNameTextBox = new ReClassNET.UI.PlaceholderTextBox();
			this.itemListBox = new System.Windows.Forms.ListBox();
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.addEnumIconButton = new ReClassNET.UI.IconButton();
			this.removeEnumIconButton = new ReClassNET.UI.IconButton();
			this.editEnumIconButton = new ReClassNET.UI.IconButton();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// filterNameTextBox
			// 
			this.filterNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.filterNameTextBox.Location = new System.Drawing.Point(12, 60);
			this.filterNameTextBox.Name = "filterNameTextBox";
			this.filterNameTextBox.PlaceholderText = "Filter by Enum Name...";
			this.filterNameTextBox.Size = new System.Drawing.Size(411, 20);
			this.filterNameTextBox.TabIndex = 10;
			this.filterNameTextBox.TextChanged += new System.EventHandler(this.filterNameTextBox_TextChanged);
			// 
			// itemListBox
			// 
			this.itemListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.itemListBox.DisplayMember = "Name";
			this.itemListBox.FormattingEnabled = true;
			this.itemListBox.Location = new System.Drawing.Point(12, 86);
			this.itemListBox.Name = "itemListBox";
			this.itemListBox.Size = new System.Drawing.Size(492, 212);
			this.itemListBox.TabIndex = 11;
			this.itemListBox.SelectedIndexChanged += new System.EventHandler(this.itemListBox_SelectedIndexChanged);
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B16x16_Class_Type;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(516, 48);
			this.bannerBox.TabIndex = 14;
			this.bannerBox.Text = "Edit the enums of the project.";
			this.bannerBox.Title = "Enums";
			// 
			// addEnumIconButton
			// 
			this.addEnumIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addEnumIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add;
			this.addEnumIconButton.Location = new System.Drawing.Point(456, 59);
			this.addEnumIconButton.Name = "addEnumIconButton";
			this.addEnumIconButton.Pressed = false;
			this.addEnumIconButton.Selected = false;
			this.addEnumIconButton.Size = new System.Drawing.Size(23, 22);
			this.addEnumIconButton.TabIndex = 15;
			this.addEnumIconButton.Click += new System.EventHandler(this.addEnumIconButton_Click);
			// 
			// removeEnumIconButton
			// 
			this.removeEnumIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeEnumIconButton.Enabled = false;
			this.removeEnumIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Remove;
			this.removeEnumIconButton.Location = new System.Drawing.Point(481, 59);
			this.removeEnumIconButton.Name = "removeEnumIconButton";
			this.removeEnumIconButton.Pressed = false;
			this.removeEnumIconButton.Selected = false;
			this.removeEnumIconButton.Size = new System.Drawing.Size(23, 22);
			this.removeEnumIconButton.TabIndex = 16;
			this.removeEnumIconButton.Click += new System.EventHandler(this.removeEnumIconButton_Click);
			// 
			// editEnumIconButton
			// 
			this.editEnumIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.editEnumIconButton.Enabled = false;
			this.editEnumIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Custom_Type;
			this.editEnumIconButton.Location = new System.Drawing.Point(431, 59);
			this.editEnumIconButton.Name = "editEnumIconButton";
			this.editEnumIconButton.Pressed = false;
			this.editEnumIconButton.Selected = false;
			this.editEnumIconButton.Size = new System.Drawing.Size(23, 22);
			this.editEnumIconButton.TabIndex = 16;
			this.editEnumIconButton.Click += new System.EventHandler(this.editEnumIconButton_Click);
			// 
			// EnumListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(516, 308);
			this.Controls.Add(this.editEnumIconButton);
			this.Controls.Add(this.addEnumIconButton);
			this.Controls.Add(this.removeEnumIconButton);
			this.Controls.Add(this.filterNameTextBox);
			this.Controls.Add(this.itemListBox);
			this.Controls.Add(this.bannerBox);
			this.Name = "EnumListForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Enums";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private UI.PlaceholderTextBox filterNameTextBox;
		private System.Windows.Forms.ListBox itemListBox;
		private UI.BannerBox bannerBox;
		private UI.IconButton addEnumIconButton;
		private UI.IconButton removeEnumIconButton;
		private UI.IconButton editEnumIconButton;
	}
}