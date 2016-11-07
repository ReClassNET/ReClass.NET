namespace ReClassNET.Forms
{
	partial class CodeForm
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
			this.codeWebBrowser = new System.Windows.Forms.WebBrowser();
			this.bannerBox = new ReClassNET.UI.BannerBox();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// codeWebBrowser
			// 
			this.codeWebBrowser.AllowNavigation = false;
			this.codeWebBrowser.AllowWebBrowserDrop = false;
			this.codeWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeWebBrowser.IsWebBrowserContextMenuEnabled = false;
			this.codeWebBrowser.Location = new System.Drawing.Point(0, 48);
			this.codeWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.codeWebBrowser.Name = "codeWebBrowser";
			this.codeWebBrowser.ScriptErrorsSuppressed = true;
			this.codeWebBrowser.Size = new System.Drawing.Size(629, 390);
			this.codeWebBrowser.TabIndex = 1;
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_Page_Code;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(629, 48);
			this.bannerBox.TabIndex = 2;
			this.bannerBox.Text = "The classes transformed into source code.";
			this.bannerBox.Title = "Code Generator";
			// 
			// CodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(629, 438);
			this.Controls.Add(this.codeWebBrowser);
			this.Controls.Add(this.bannerBox);
			this.MinimumSize = new System.Drawing.Size(350, 185);
			this.Name = "CodeForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Code Generator";
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.WebBrowser codeWebBrowser;
		private UI.BannerBox bannerBox;
	}
}