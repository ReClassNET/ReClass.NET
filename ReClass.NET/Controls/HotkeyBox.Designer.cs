namespace ReClassNET.UI
{
	partial class HotkeyBox
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
			this.components = new System.ComponentModel.Container();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.textBox = new System.Windows.Forms.TextBox();
			this.clearButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 50;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Enabled = false;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Name = "textBox";
			this.textBox.Size = new System.Drawing.Size(140, 20);
			this.textBox.TabIndex = 0;
			this.textBox.Enter += new System.EventHandler(this.textBox_Enter);
			this.textBox.Leave += new System.EventHandler(this.textBox_Leave);
			// 
			// clearButton
			// 
			this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.clearButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Delete;
			this.clearButton.Location = new System.Drawing.Point(142, 0);
			this.clearButton.Name = "clearButton";
			this.clearButton.Size = new System.Drawing.Size(20, 20);
			this.clearButton.TabIndex = 1;
			this.clearButton.UseVisualStyleBackColor = true;
			this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
			// 
			// HotkeyBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.clearButton);
			this.Controls.Add(this.textBox);
			this.Name = "HotkeyBox";
			this.Size = new System.Drawing.Size(162, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button clearButton;
	}
}
