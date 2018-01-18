namespace ReClassNET.Forms
{
	partial class NamedAddressesForm
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
			this.addressTextBox = new ReClassNET.UI.PlaceholderTextBox();
			this.nameTextBox = new ReClassNET.UI.PlaceholderTextBox();
			this.namedAddressesListBox = new System.Windows.Forms.ListBox();
			this.removeAddressIconButton = new ReClassNET.UI.IconButton();
			this.addAddressIconButton = new ReClassNET.UI.IconButton();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B16x16_Custom_Type;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(429, 48);
			this.bannerBox.TabIndex = 10;
			this.bannerBox.Text = "Give special memory addresses meaningfull names.";
			this.bannerBox.Title = "Named Addresses";
			// 
			// addressTextBox
			// 
			this.addressTextBox.Location = new System.Drawing.Point(13, 55);
			this.addressTextBox.Name = "addressTextBox";
			this.addressTextBox.PlaceholderText = "Address";
			this.addressTextBox.Size = new System.Drawing.Size(154, 20);
			this.addressTextBox.TabIndex = 1;
			this.addressTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
			// 
			// nameTextBox
			// 
			this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.nameTextBox.Location = new System.Drawing.Point(173, 55);
			this.nameTextBox.Name = "nameTextBox";
			this.nameTextBox.PlaceholderText = "Name";
			this.nameTextBox.Size = new System.Drawing.Size(190, 20);
			this.nameTextBox.TabIndex = 2;
			this.nameTextBox.TextChanged += new System.EventHandler(this.InputTextBox_TextChanged);
			// 
			// namedAddressesListBox
			// 
			this.namedAddressesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.namedAddressesListBox.FormattingEnabled = true;
			this.namedAddressesListBox.Location = new System.Drawing.Point(13, 81);
			this.namedAddressesListBox.Name = "namedAddressesListBox";
			this.namedAddressesListBox.Size = new System.Drawing.Size(404, 186);
			this.namedAddressesListBox.TabIndex = 0;
			this.namedAddressesListBox.SelectedIndexChanged += new System.EventHandler(this.namedAddressesListBox_SelectedIndexChanged);
			// 
			// removeAddressIconButton
			// 
			this.removeAddressIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeAddressIconButton.Enabled = false;
			this.removeAddressIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Remove;
			this.removeAddressIconButton.Location = new System.Drawing.Point(394, 54);
			this.removeAddressIconButton.Name = "removeAddressIconButton";
			this.removeAddressIconButton.Pressed = false;
			this.removeAddressIconButton.Selected = false;
			this.removeAddressIconButton.Size = new System.Drawing.Size(23, 22);
			this.removeAddressIconButton.TabIndex = 4;
			this.removeAddressIconButton.Click += new System.EventHandler(this.removeAddressIconButton_Click);
			// 
			// addAddressIconButton
			// 
			this.addAddressIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.addAddressIconButton.Enabled = false;
			this.addAddressIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add;
			this.addAddressIconButton.Location = new System.Drawing.Point(369, 54);
			this.addAddressIconButton.Name = "addAddressIconButton";
			this.addAddressIconButton.Pressed = false;
			this.addAddressIconButton.Selected = false;
			this.addAddressIconButton.Size = new System.Drawing.Size(23, 22);
			this.addAddressIconButton.TabIndex = 3;
			this.addAddressIconButton.Click += new System.EventHandler(this.addAddressIconButton_Click);
			// 
			// NamedAddressesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(429, 279);
			this.Controls.Add(this.addAddressIconButton);
			this.Controls.Add(this.removeAddressIconButton);
			this.Controls.Add(this.namedAddressesListBox);
			this.Controls.Add(this.nameTextBox);
			this.Controls.Add(this.addressTextBox);
			this.Controls.Add(this.bannerBox);
			this.MinimumSize = new System.Drawing.Size(445, 317);
			this.Name = "NamedAddressesForm";
			this.Text = "ReClass.NET - Named Addresses";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.BannerBox bannerBox;
		private UI.PlaceholderTextBox addressTextBox;
		private UI.PlaceholderTextBox nameTextBox;
		private System.Windows.Forms.ListBox namedAddressesListBox;
		private UI.IconButton removeAddressIconButton;
		private UI.IconButton addAddressIconButton;
	}
}