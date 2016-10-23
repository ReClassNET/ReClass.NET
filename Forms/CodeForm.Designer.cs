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
			this.SuspendLayout();
			// 
			// codeWebBrowser
			// 
			this.codeWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.codeWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.codeWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.codeWebBrowser.Name = "codeWebBrowser";
			this.codeWebBrowser.Size = new System.Drawing.Size(502, 430);
			this.codeWebBrowser.TabIndex = 1;
			// 
			// CodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(502, 430);
			this.Controls.Add(this.codeWebBrowser);
			this.Name = "CodeForm";
			this.Text = "ReClass.NET - Code";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.WebBrowser codeWebBrowser;
	}
}