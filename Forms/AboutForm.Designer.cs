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
			this.bannerBox = new ReClassNET.UI.BannerBox();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
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
			this.bannerBox.Text = "bannerBox1";
			this.bannerBox.Title = "";
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(409, 298);
			this.Controls.Add(this.bannerBox);
			this.Name = "AboutForm";
			this.Text = "AboutForm";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private UI.BannerBox bannerBox;
	}
}