namespace ReClassNET.UI
{
	partial class ColorBox
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.valueTextBox = new System.Windows.Forms.TextBox();
			this.colorPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// valueTextBox
			// 
			this.valueTextBox.Location = new System.Drawing.Point(37, 0);
			this.valueTextBox.Name = "valueTextBox";
			this.valueTextBox.Size = new System.Drawing.Size(86, 20);
			this.valueTextBox.TabIndex = 0;
			this.valueTextBox.TextChanged += new System.EventHandler(this.OnTextChanged);
			// 
			// colorPanel
			// 
			this.colorPanel.Location = new System.Drawing.Point(0, 0);
			this.colorPanel.Name = "colorPanel";
			this.colorPanel.Size = new System.Drawing.Size(30, 20);
			this.colorPanel.TabIndex = 1;
			this.colorPanel.Click += new System.EventHandler(this.OnPanelClick);
			this.colorPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPanelPaint);
			// 
			// ColorBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.colorPanel);
			this.Controls.Add(this.valueTextBox);
			this.Name = "ColorBox";
			this.Size = new System.Drawing.Size(123, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox valueTextBox;
		private System.Windows.Forms.Panel colorPanel;
	}
}
