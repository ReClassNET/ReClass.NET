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
			this.codeRichTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// codeRichTextBox
			// 
			this.codeRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.codeRichTextBox.Location = new System.Drawing.Point(12, 12);
			this.codeRichTextBox.Name = "codeRichTextBox";
			this.codeRichTextBox.Size = new System.Drawing.Size(478, 406);
			this.codeRichTextBox.TabIndex = 0;
			this.codeRichTextBox.Text = "";
			// 
			// CodeForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(502, 430);
			this.Controls.Add(this.codeRichTextBox);
			this.Name = "CodeForm";
			this.Text = "ReClass.NET - Code";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox codeRichTextBox;
	}
}