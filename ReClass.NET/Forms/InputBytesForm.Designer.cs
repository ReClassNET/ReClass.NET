namespace ReClassNET.Forms
{
	partial class InputBytesForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.hexRadioButton = new System.Windows.Forms.RadioButton();
			this.decimalRadioButton = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.currentSizeLabel = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.newSizeLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.bytesNumericUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.bytesNumericUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(5, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Number of Bytes to add:";
			// 
			// hexRadioButton
			// 
			this.hexRadioButton.AutoSize = true;
			this.hexRadioButton.Location = new System.Drawing.Point(77, 51);
			this.hexRadioButton.Name = "hexRadioButton";
			this.hexRadioButton.Size = new System.Drawing.Size(44, 17);
			this.hexRadioButton.TabIndex = 2;
			this.hexRadioButton.Text = "Hex";
			this.hexRadioButton.UseVisualStyleBackColor = true;
			this.hexRadioButton.CheckedChanged += new System.EventHandler(this.hexRadioButton_CheckedChanged);
			// 
			// decimalRadioButton
			// 
			this.decimalRadioButton.AutoSize = true;
			this.decimalRadioButton.Checked = true;
			this.decimalRadioButton.Location = new System.Drawing.Point(8, 51);
			this.decimalRadioButton.Name = "decimalRadioButton";
			this.decimalRadioButton.Size = new System.Drawing.Size(63, 17);
			this.decimalRadioButton.TabIndex = 3;
			this.decimalRadioButton.TabStop = true;
			this.decimalRadioButton.Text = "Decimal";
			this.decimalRadioButton.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(5, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Current class size:";
			// 
			// currentSizeLabel
			// 
			this.currentSizeLabel.AutoSize = true;
			this.currentSizeLabel.Location = new System.Drawing.Point(111, 79);
			this.currentSizeLabel.Name = "currentSizeLabel";
			this.currentSizeLabel.Size = new System.Drawing.Size(19, 13);
			this.currentSizeLabel.TabIndex = 5;
			this.currentSizeLabel.Text = "<>";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(5, 98);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "New class size:";
			// 
			// newSizeLabel
			// 
			this.newSizeLabel.AutoSize = true;
			this.newSizeLabel.Location = new System.Drawing.Point(111, 98);
			this.newSizeLabel.Name = "newSizeLabel";
			this.newSizeLabel.Size = new System.Drawing.Size(19, 13);
			this.newSizeLabel.TabIndex = 7;
			this.newSizeLabel.Text = "<>";
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(146, 121);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 8;
			this.okButton.Text = "OK";
			this.okButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// bytesNumericUpDown
			// 
			this.bytesNumericUpDown.Location = new System.Drawing.Point(8, 25);
			this.bytesNumericUpDown.Name = "bytesNumericUpDown";
			this.bytesNumericUpDown.Size = new System.Drawing.Size(212, 20);
			this.bytesNumericUpDown.TabIndex = 9;
			this.bytesNumericUpDown.ValueChanged += new System.EventHandler(this.bytesNumericUpDown_ValueChanged);
			// 
			// InputBytesForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(228, 151);
			this.Controls.Add(this.bytesNumericUpDown);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.newSizeLabel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.currentSizeLabel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.decimalRadioButton);
			this.Controls.Add(this.hexRadioButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBytesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "<>";
			((System.ComponentModel.ISupportInitialize)(this.bytesNumericUpDown)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton hexRadioButton;
		private System.Windows.Forms.RadioButton decimalRadioButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label currentSizeLabel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label newSizeLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.NumericUpDown bytesNumericUpDown;
	}
}