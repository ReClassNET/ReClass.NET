namespace ReClassNET.Forms
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.infoLabel = new System.Windows.Forms.Label();
			this.platformLabel = new System.Windows.Forms.Label();
			this.buildTimeLabel = new System.Windows.Forms.Label();
			this.authorLabel = new System.Windows.Forms.Label();
			this.homepageLabel = new System.Windows.Forms.Label();
			this.platformValueLabel = new System.Windows.Forms.Label();
			this.buildTimeValueLabel = new System.Windows.Forms.Label();
			this.authorValueLabel = new System.Windows.Forms.Label();
			this.homepageValueLabel = new System.Windows.Forms.LinkLabel();
			this.licenseGroupBox = new System.Windows.Forms.GroupBox();
			this.licenseTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.licenseGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = null;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(409, 48);
			this.bannerBox.TabIndex = 0;
			this.bannerBox.Title = "";
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point(12, 140);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(333, 26);
			this.infoLabel.TabIndex = 1;
			this.infoLabel.Text = "This is a port of ReClass to the .NET platform with additional features.\r\nReClass" +
    " was originally written by DrUnKeN ChEeTaH.";
			// 
			// platformLabel
			// 
			this.platformLabel.AutoSize = true;
			this.platformLabel.Location = new System.Drawing.Point(12, 60);
			this.platformLabel.Name = "platformLabel";
			this.platformLabel.Size = new System.Drawing.Size(51, 13);
			this.platformLabel.TabIndex = 2;
			this.platformLabel.Text = "Platform: ";
			// 
			// buildTimeLabel
			// 
			this.buildTimeLabel.AutoSize = true;
			this.buildTimeLabel.Location = new System.Drawing.Point(12, 79);
			this.buildTimeLabel.Name = "buildTimeLabel";
			this.buildTimeLabel.Size = new System.Drawing.Size(58, 13);
			this.buildTimeLabel.TabIndex = 3;
			this.buildTimeLabel.Text = "Build time: ";
			// 
			// authorLabel
			// 
			this.authorLabel.AutoSize = true;
			this.authorLabel.Location = new System.Drawing.Point(12, 98);
			this.authorLabel.Name = "authorLabel";
			this.authorLabel.Size = new System.Drawing.Size(41, 13);
			this.authorLabel.TabIndex = 4;
			this.authorLabel.Text = "Author:";
			// 
			// homepageLabel
			// 
			this.homepageLabel.AutoSize = true;
			this.homepageLabel.Location = new System.Drawing.Point(12, 117);
			this.homepageLabel.Name = "homepageLabel";
			this.homepageLabel.Size = new System.Drawing.Size(66, 13);
			this.homepageLabel.TabIndex = 5;
			this.homepageLabel.Text = "Home Page:";
			// 
			// platformValueLabel
			// 
			this.platformValueLabel.AutoSize = true;
			this.platformValueLabel.Location = new System.Drawing.Point(84, 60);
			this.platformValueLabel.Name = "platformValueLabel";
			this.platformValueLabel.Size = new System.Drawing.Size(19, 13);
			this.platformValueLabel.TabIndex = 6;
			this.platformValueLabel.Text = "<>";
			// 
			// buildTimeValueLabel
			// 
			this.buildTimeValueLabel.AutoSize = true;
			this.buildTimeValueLabel.Location = new System.Drawing.Point(84, 79);
			this.buildTimeValueLabel.Name = "buildTimeValueLabel";
			this.buildTimeValueLabel.Size = new System.Drawing.Size(19, 13);
			this.buildTimeValueLabel.TabIndex = 7;
			this.buildTimeValueLabel.Text = "<>";
			// 
			// authorValueLabel
			// 
			this.authorValueLabel.AutoSize = true;
			this.authorValueLabel.Location = new System.Drawing.Point(84, 98);
			this.authorValueLabel.Name = "authorValueLabel";
			this.authorValueLabel.Size = new System.Drawing.Size(19, 13);
			this.authorValueLabel.TabIndex = 8;
			this.authorValueLabel.Text = "<>";
			// 
			// homepageValueLabel
			// 
			this.homepageValueLabel.AutoSize = true;
			this.homepageValueLabel.Location = new System.Drawing.Point(84, 117);
			this.homepageValueLabel.Name = "homepageValueLabel";
			this.homepageValueLabel.Size = new System.Drawing.Size(19, 13);
			this.homepageValueLabel.TabIndex = 9;
			this.homepageValueLabel.TabStop = true;
			this.homepageValueLabel.Text = "<>";
			this.homepageValueLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homepageValueLabel_LinkClicked);
			// 
			// licenseGroupBox
			// 
			this.licenseGroupBox.Controls.Add(this.licenseTextBox);
			this.licenseGroupBox.Location = new System.Drawing.Point(15, 178);
			this.licenseGroupBox.Name = "licenseGroupBox";
			this.licenseGroupBox.Size = new System.Drawing.Size(382, 174);
			this.licenseGroupBox.TabIndex = 10;
			this.licenseGroupBox.TabStop = false;
			this.licenseGroupBox.Text = "MIT License";
			// 
			// licenseTextBox
			// 
			this.licenseTextBox.Location = new System.Drawing.Point(6, 19);
			this.licenseTextBox.Multiline = true;
			this.licenseTextBox.Name = "licenseTextBox";
			this.licenseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.licenseTextBox.Size = new System.Drawing.Size(370, 149);
			this.licenseTextBox.TabIndex = 0;
			this.licenseTextBox.Text = resources.GetString("licenseTextBox.Text");
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(409, 364);
			this.Controls.Add(this.licenseGroupBox);
			this.Controls.Add(this.homepageValueLabel);
			this.Controls.Add(this.authorValueLabel);
			this.Controls.Add(this.buildTimeValueLabel);
			this.Controls.Add(this.platformValueLabel);
			this.Controls.Add(this.homepageLabel);
			this.Controls.Add(this.authorLabel);
			this.Controls.Add(this.buildTimeLabel);
			this.Controls.Add(this.platformLabel);
			this.Controls.Add(this.infoLabel);
			this.Controls.Add(this.bannerBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Info";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.licenseGroupBox.ResumeLayout(false);
			this.licenseGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.BannerBox bannerBox;
		private System.Windows.Forms.Label infoLabel;
		private System.Windows.Forms.Label platformLabel;
		private System.Windows.Forms.Label buildTimeLabel;
		private System.Windows.Forms.Label authorLabel;
		private System.Windows.Forms.Label homepageLabel;
		private System.Windows.Forms.Label platformValueLabel;
		private System.Windows.Forms.Label buildTimeValueLabel;
		private System.Windows.Forms.Label authorValueLabel;
		private System.Windows.Forms.LinkLabel homepageValueLabel;
		private System.Windows.Forms.GroupBox licenseGroupBox;
		private System.Windows.Forms.TextBox licenseTextBox;
	}
}