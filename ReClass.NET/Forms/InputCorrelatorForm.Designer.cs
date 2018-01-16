namespace ReClassNET.Forms
{
	partial class InputCorrelatorForm
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
			this.components = new System.ComponentModel.Container();
			this.refineTimer = new System.Windows.Forms.Timer(this.components);
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.settingsGroupBox = new System.Windows.Forms.GroupBox();
			this.removeButton = new System.Windows.Forms.Button();
			this.addButton = new System.Windows.Forms.Button();
			this.valueTypeComboBox = new ReClassNET.Forms.ScannerForm.ScanValueTypeComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.hotkeyListBox = new System.Windows.Forms.ListBox();
			this.hotkeyBox = new ReClassNET.UI.HotkeyBox();
			this.startStopButton = new System.Windows.Forms.Button();
			this.infoLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.settingsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// refineTimer
			// 
			this.refineTimer.Interval = 50;
			this.refineTimer.Tick += new System.EventHandler(this.refineTimer_Tick);
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_Canvas_Size;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(301, 48);
			this.bannerBox.TabIndex = 8;
			this.bannerBox.Text = "Scan for values correlated to input.";
			this.bannerBox.Title = "Input Correlator";
			// 
			// settingsGroupBox
			// 
			this.settingsGroupBox.Controls.Add(this.removeButton);
			this.settingsGroupBox.Controls.Add(this.addButton);
			this.settingsGroupBox.Controls.Add(this.valueTypeComboBox);
			this.settingsGroupBox.Controls.Add(this.label1);
			this.settingsGroupBox.Controls.Add(this.hotkeyListBox);
			this.settingsGroupBox.Controls.Add(this.hotkeyBox);
			this.settingsGroupBox.Location = new System.Drawing.Point(7, 54);
			this.settingsGroupBox.Name = "settingsGroupBox";
			this.settingsGroupBox.Size = new System.Drawing.Size(288, 179);
			this.settingsGroupBox.TabIndex = 9;
			this.settingsGroupBox.TabStop = false;
			this.settingsGroupBox.Text = "Settings";
			// 
			// removeButton
			// 
			this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.removeButton.Location = new System.Drawing.Point(203, 45);
			this.removeButton.Name = "removeButton";
			this.removeButton.Size = new System.Drawing.Size(79, 23);
			this.removeButton.TabIndex = 13;
			this.removeButton.Text = "Remove Key";
			this.removeButton.UseVisualStyleBackColor = true;
			this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
			// 
			// addButton
			// 
			this.addButton.Location = new System.Drawing.Point(6, 45);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(58, 23);
			this.addButton.TabIndex = 12;
			this.addButton.Text = "Add Key";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
			// 
			// valueTypeComboBox
			// 
			this.valueTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.valueTypeComboBox.Location = new System.Drawing.Point(66, 149);
			this.valueTypeComboBox.Name = "valueTypeComboBox";
			this.valueTypeComboBox.Size = new System.Drawing.Size(216, 21);
			this.valueTypeComboBox.TabIndex = 11;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 12;
			this.label1.Text = "Value Type:";
			// 
			// hotkeyListBox
			// 
			this.hotkeyListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hotkeyListBox.FormattingEnabled = true;
			this.hotkeyListBox.Location = new System.Drawing.Point(6, 74);
			this.hotkeyListBox.Name = "hotkeyListBox";
			this.hotkeyListBox.Size = new System.Drawing.Size(276, 69);
			this.hotkeyListBox.TabIndex = 11;
			// 
			// hotkeyBox
			// 
			this.hotkeyBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hotkeyBox.Input = null;
			this.hotkeyBox.Location = new System.Drawing.Point(6, 19);
			this.hotkeyBox.Name = "hotkeyBox";
			this.hotkeyBox.Size = new System.Drawing.Size(276, 20);
			this.hotkeyBox.TabIndex = 10;
			// 
			// startStopButton
			// 
			this.startStopButton.Location = new System.Drawing.Point(7, 239);
			this.startStopButton.Name = "startStopButton";
			this.startStopButton.Size = new System.Drawing.Size(288, 23);
			this.startStopButton.TabIndex = 13;
			this.startStopButton.Text = "Start Scan";
			this.startStopButton.UseVisualStyleBackColor = true;
			this.startStopButton.Click += new System.EventHandler(this.startStopButton_Click);
			// 
			// infoLabel
			// 
			this.infoLabel.AutoSize = true;
			this.infoLabel.Location = new System.Drawing.Point(4, 265);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(19, 13);
			this.infoLabel.TabIndex = 11;
			this.infoLabel.Text = "<>";
			// 
			// InputCorrelatorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(301, 287);
			this.Controls.Add(this.infoLabel);
			this.Controls.Add(this.startStopButton);
			this.Controls.Add(this.settingsGroupBox);
			this.Controls.Add(this.bannerBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputCorrelatorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Input Correlator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InputCorrelatorForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.settingsGroupBox.ResumeLayout(false);
			this.settingsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer refineTimer;
		private UI.BannerBox bannerBox;
		private System.Windows.Forms.GroupBox settingsGroupBox;
		private UI.HotkeyBox hotkeyBox;
		private System.Windows.Forms.ListBox hotkeyListBox;
		private System.Windows.Forms.Button removeButton;
		private System.Windows.Forms.Button addButton;
		private ScannerForm.ScanValueTypeComboBox valueTypeComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button startStopButton;
		private System.Windows.Forms.Label infoLabel;
	}
}