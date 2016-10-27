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
			this.bannerImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.bannerImage)).BeginInit();
			this.SuspendLayout();
			// 
			// bannerImage
			// 
			this.bannerImage.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerImage.Location = new System.Drawing.Point(0, 0);
			this.bannerImage.Name = "bannerImage";
			this.bannerImage.Size = new System.Drawing.Size(409, 48);
			this.bannerImage.TabIndex = 1;
			this.bannerImage.TabStop = false;
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(409, 298);
			this.Controls.Add(this.bannerImage);
			this.Name = "AboutForm";
			this.Text = "AboutForm";
			((System.ComponentModel.ISupportInitialize)(this.bannerImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox bannerImage;
	}
}