namespace ReClassNET.Forms
{
	partial class ScannerForm
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
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.filterGroupBox = new System.Windows.Forms.GroupBox();
			this.valueTypeComboBox = new ReClassNET.Forms.ScannerForm.ScanValueTypeComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.compareTypeComboBox = new ReClassNET.Forms.ScannerForm.ScanCompareTypeComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.isHexCheckBox = new System.Windows.Forms.CheckBox();
			this.dualValueBox = new ReClassNET.UI.DualValueBox();
			this.scanOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.fastScanAlignmentTextBox = new System.Windows.Forms.TextBox();
			this.fastScanCheckBox = new System.Windows.Forms.CheckBox();
			this.scanCopyOnWriteCheckBox = new System.Windows.Forms.CheckBox();
			this.scanExecutableCheckBox = new System.Windows.Forms.CheckBox();
			this.scanWritableCheckBox = new System.Windows.Forms.CheckBox();
			this.scanMappedCheckBox = new System.Windows.Forms.CheckBox();
			this.scanImageCheckBox = new System.Windows.Forms.CheckBox();
			this.scanPrivateCheckBox = new System.Windows.Forms.CheckBox();
			this.stopAddressTextBox = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.startAddressTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
			this.floatingOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.roundTruncateRadioButton = new System.Windows.Forms.RadioButton();
			this.roundLooseRadioButton = new System.Windows.Forms.RadioButton();
			this.roundStrictRadioButton = new System.Windows.Forms.RadioButton();
			this.stringOptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.caseSensitiveCheckBox = new System.Windows.Forms.CheckBox();
			this.encodingUtf32RadioButton = new System.Windows.Forms.RadioButton();
			this.encodingUtf16RadioButton = new System.Windows.Forms.RadioButton();
			this.encodingUtf8RadioButton = new System.Windows.Forms.RadioButton();
			this.firstScanButton = new System.Windows.Forms.Button();
			this.nextScanButton = new System.Windows.Forms.Button();
			this.scanProgressBar = new System.Windows.Forms.ProgressBar();
			this.resultCountLabel = new System.Windows.Forms.Label();
			this.updateValuesTimer = new System.Windows.Forms.Timer(this.components);
			this.resultMemoryRecordList = new ReClassNET.UI.MemoryRecordList();
			this.resultListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.addSelectedResultsToAddressListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeSelectedRecordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.changeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.descriptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.valueTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.valueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.setCurrentClassAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createClassAtAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.findOutWhatAccessesThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findOutWhatWritesToThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addressListMemoryRecordList = new ReClassNET.UI.MemoryRecordList();
			this.toolStripPanel = new System.Windows.Forms.ToolStripPanel();
			this.menuToolStrip = new System.Windows.Forms.ToolStrip();
			this.openAddressFileToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveAddressFileToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveAddressFileAsToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.clearAddressListToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.infoToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.undoIconButton = new ReClassNET.UI.IconButton();
			this.showInputCorrelatorIconButton = new ReClassNET.UI.IconButton();
			this.cancelScanIconButton = new ReClassNET.UI.IconButton();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.filterGroupBox.SuspendLayout();
			this.scanOptionsGroupBox.SuspendLayout();
			this.flowLayoutPanel.SuspendLayout();
			this.floatingOptionsGroupBox.SuspendLayout();
			this.stringOptionsGroupBox.SuspendLayout();
			this.resultListContextMenuStrip.SuspendLayout();
			this.toolStripPanel.SuspendLayout();
			this.menuToolStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_Eye;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(612, 48);
			this.bannerBox.TabIndex = 7;
			this.bannerBox.Text = "Scan the process memory for specific values.";
			this.bannerBox.Title = "Scanner";
			// 
			// filterGroupBox
			// 
			this.filterGroupBox.Controls.Add(this.valueTypeComboBox);
			this.filterGroupBox.Controls.Add(this.label3);
			this.filterGroupBox.Controls.Add(this.compareTypeComboBox);
			this.filterGroupBox.Controls.Add(this.label1);
			this.filterGroupBox.Controls.Add(this.isHexCheckBox);
			this.filterGroupBox.Controls.Add(this.dualValueBox);
			this.filterGroupBox.Location = new System.Drawing.Point(3, 3);
			this.filterGroupBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
			this.filterGroupBox.Name = "filterGroupBox";
			this.filterGroupBox.Size = new System.Drawing.Size(308, 103);
			this.filterGroupBox.TabIndex = 8;
			this.filterGroupBox.TabStop = false;
			this.filterGroupBox.Text = "Filter";
			// 
			// valueTypeComboBox
			// 
			this.valueTypeComboBox.Location = new System.Drawing.Point(72, 74);
			this.valueTypeComboBox.Name = "valueTypeComboBox";
			this.valueTypeComboBox.Size = new System.Drawing.Size(224, 21);
			this.valueTypeComboBox.TabIndex = 8;
			this.valueTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.valueTypeComboBox_SelectionChangeCommitted);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 77);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Value Type:";
			// 
			// compareTypeComboBox
			// 
			this.compareTypeComboBox.Location = new System.Drawing.Point(72, 50);
			this.compareTypeComboBox.Name = "compareTypeComboBox";
			this.compareTypeComboBox.Size = new System.Drawing.Size(224, 21);
			this.compareTypeComboBox.TabIndex = 5;
			this.compareTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.scanTypeComboBox_SelectionChangeCommitted);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 53);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Scan Type:";
			// 
			// isHexCheckBox
			// 
			this.isHexCheckBox.AutoSize = true;
			this.isHexCheckBox.Location = new System.Drawing.Point(6, 28);
			this.isHexCheckBox.Name = "isHexCheckBox";
			this.isHexCheckBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.isHexCheckBox.Size = new System.Drawing.Size(56, 17);
			this.isHexCheckBox.TabIndex = 3;
			this.isHexCheckBox.Text = "Is Hex";
			this.isHexCheckBox.UseVisualStyleBackColor = true;
			// 
			// dualValueBox
			// 
			this.dualValueBox.Location = new System.Drawing.Point(72, 12);
			this.dualValueBox.Name = "dualValueBox";
			this.dualValueBox.ShowSecondInputField = false;
			this.dualValueBox.Size = new System.Drawing.Size(224, 34);
			this.dualValueBox.TabIndex = 2;
			this.dualValueBox.Value1 = "0";
			this.dualValueBox.Value2 = "0";
			// 
			// scanOptionsGroupBox
			// 
			this.scanOptionsGroupBox.Controls.Add(this.fastScanAlignmentTextBox);
			this.scanOptionsGroupBox.Controls.Add(this.fastScanCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanCopyOnWriteCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanExecutableCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanWritableCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanMappedCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanImageCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.scanPrivateCheckBox);
			this.scanOptionsGroupBox.Controls.Add(this.stopAddressTextBox);
			this.scanOptionsGroupBox.Controls.Add(this.label4);
			this.scanOptionsGroupBox.Controls.Add(this.startAddressTextBox);
			this.scanOptionsGroupBox.Controls.Add(this.label2);
			this.scanOptionsGroupBox.Location = new System.Drawing.Point(3, 239);
			this.scanOptionsGroupBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.scanOptionsGroupBox.Name = "scanOptionsGroupBox";
			this.scanOptionsGroupBox.Size = new System.Drawing.Size(308, 141);
			this.scanOptionsGroupBox.TabIndex = 9;
			this.scanOptionsGroupBox.TabStop = false;
			this.scanOptionsGroupBox.Text = "Scan Options";
			// 
			// fastScanAlignmentTextBox
			// 
			this.fastScanAlignmentTextBox.Location = new System.Drawing.Point(144, 112);
			this.fastScanAlignmentTextBox.Name = "fastScanAlignmentTextBox";
			this.fastScanAlignmentTextBox.Size = new System.Drawing.Size(26, 20);
			this.fastScanAlignmentTextBox.TabIndex = 11;
			// 
			// fastScanCheckBox
			// 
			this.fastScanCheckBox.AutoSize = true;
			this.fastScanCheckBox.Checked = true;
			this.fastScanCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.fastScanCheckBox.Location = new System.Drawing.Point(9, 114);
			this.fastScanCheckBox.Name = "fastScanCheckBox";
			this.fastScanCheckBox.Size = new System.Drawing.Size(129, 17);
			this.fastScanCheckBox.TabIndex = 10;
			this.fastScanCheckBox.Text = "Fast Scan, Alignment:";
			this.fastScanCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanCopyOnWriteCheckBox
			// 
			this.scanCopyOnWriteCheckBox.AutoSize = true;
			this.scanCopyOnWriteCheckBox.Location = new System.Drawing.Point(189, 91);
			this.scanCopyOnWriteCheckBox.Name = "scanCopyOnWriteCheckBox";
			this.scanCopyOnWriteCheckBox.Size = new System.Drawing.Size(95, 17);
			this.scanCopyOnWriteCheckBox.TabIndex = 9;
			this.scanCopyOnWriteCheckBox.Text = "Copy On Write";
			this.scanCopyOnWriteCheckBox.ThreeState = true;
			this.scanCopyOnWriteCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanExecutableCheckBox
			// 
			this.scanExecutableCheckBox.AutoSize = true;
			this.scanExecutableCheckBox.Checked = true;
			this.scanExecutableCheckBox.CheckState = System.Windows.Forms.CheckState.Indeterminate;
			this.scanExecutableCheckBox.Location = new System.Drawing.Point(91, 91);
			this.scanExecutableCheckBox.Name = "scanExecutableCheckBox";
			this.scanExecutableCheckBox.Size = new System.Drawing.Size(79, 17);
			this.scanExecutableCheckBox.TabIndex = 8;
			this.scanExecutableCheckBox.Text = "Executable";
			this.scanExecutableCheckBox.ThreeState = true;
			this.scanExecutableCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanWritableCheckBox
			// 
			this.scanWritableCheckBox.AutoSize = true;
			this.scanWritableCheckBox.Checked = true;
			this.scanWritableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.scanWritableCheckBox.Location = new System.Drawing.Point(9, 91);
			this.scanWritableCheckBox.Name = "scanWritableCheckBox";
			this.scanWritableCheckBox.Size = new System.Drawing.Size(65, 17);
			this.scanWritableCheckBox.TabIndex = 7;
			this.scanWritableCheckBox.Text = "Writable";
			this.scanWritableCheckBox.ThreeState = true;
			this.scanWritableCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanMappedCheckBox
			// 
			this.scanMappedCheckBox.AutoSize = true;
			this.scanMappedCheckBox.Location = new System.Drawing.Point(189, 68);
			this.scanMappedCheckBox.Name = "scanMappedCheckBox";
			this.scanMappedCheckBox.Size = new System.Drawing.Size(65, 17);
			this.scanMappedCheckBox.TabIndex = 6;
			this.scanMappedCheckBox.Text = "Mapped";
			this.scanMappedCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanImageCheckBox
			// 
			this.scanImageCheckBox.AutoSize = true;
			this.scanImageCheckBox.Checked = true;
			this.scanImageCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.scanImageCheckBox.Location = new System.Drawing.Point(91, 68);
			this.scanImageCheckBox.Name = "scanImageCheckBox";
			this.scanImageCheckBox.Size = new System.Drawing.Size(55, 17);
			this.scanImageCheckBox.TabIndex = 5;
			this.scanImageCheckBox.Text = "Image";
			this.scanImageCheckBox.UseVisualStyleBackColor = true;
			// 
			// scanPrivateCheckBox
			// 
			this.scanPrivateCheckBox.AutoSize = true;
			this.scanPrivateCheckBox.Checked = true;
			this.scanPrivateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.scanPrivateCheckBox.Location = new System.Drawing.Point(9, 68);
			this.scanPrivateCheckBox.Name = "scanPrivateCheckBox";
			this.scanPrivateCheckBox.Size = new System.Drawing.Size(59, 17);
			this.scanPrivateCheckBox.TabIndex = 4;
			this.scanPrivateCheckBox.Text = "Private";
			this.scanPrivateCheckBox.UseVisualStyleBackColor = true;
			// 
			// stopAddressTextBox
			// 
			this.stopAddressTextBox.Location = new System.Drawing.Point(66, 42);
			this.stopAddressTextBox.Name = "stopAddressTextBox";
			this.stopAddressTextBox.Size = new System.Drawing.Size(218, 20);
			this.stopAddressTextBox.TabIndex = 3;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 45);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(32, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Stop:";
			// 
			// startAddressTextBox
			// 
			this.startAddressTextBox.Location = new System.Drawing.Point(66, 19);
			this.startAddressTextBox.Name = "startAddressTextBox";
			this.startAddressTextBox.Size = new System.Drawing.Size(218, 20);
			this.startAddressTextBox.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Start:";
			// 
			// flowLayoutPanel
			// 
			this.flowLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.flowLayoutPanel.Controls.Add(this.filterGroupBox);
			this.flowLayoutPanel.Controls.Add(this.floatingOptionsGroupBox);
			this.flowLayoutPanel.Controls.Add(this.stringOptionsGroupBox);
			this.flowLayoutPanel.Controls.Add(this.scanOptionsGroupBox);
			this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel.Location = new System.Drawing.Point(292, 80);
			this.flowLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new System.Drawing.Size(317, 314);
			this.flowLayoutPanel.TabIndex = 9;
			this.flowLayoutPanel.WrapContents = false;
			// 
			// floatingOptionsGroupBox
			// 
			this.floatingOptionsGroupBox.Controls.Add(this.roundTruncateRadioButton);
			this.floatingOptionsGroupBox.Controls.Add(this.roundLooseRadioButton);
			this.floatingOptionsGroupBox.Controls.Add(this.roundStrictRadioButton);
			this.floatingOptionsGroupBox.Location = new System.Drawing.Point(3, 107);
			this.floatingOptionsGroupBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 1);
			this.floatingOptionsGroupBox.Name = "floatingOptionsGroupBox";
			this.floatingOptionsGroupBox.Size = new System.Drawing.Size(308, 64);
			this.floatingOptionsGroupBox.TabIndex = 9;
			this.floatingOptionsGroupBox.TabStop = false;
			this.floatingOptionsGroupBox.Visible = false;
			// 
			// roundTruncateRadioButton
			// 
			this.roundTruncateRadioButton.AutoSize = true;
			this.roundTruncateRadioButton.Location = new System.Drawing.Point(72, 42);
			this.roundTruncateRadioButton.Name = "roundTruncateRadioButton";
			this.roundTruncateRadioButton.Size = new System.Drawing.Size(68, 17);
			this.roundTruncateRadioButton.TabIndex = 2;
			this.roundTruncateRadioButton.Text = "Truncate";
			this.infoToolTip.SetToolTip(this.roundTruncateRadioButton, "123.45 == 123.99");
			this.roundTruncateRadioButton.UseVisualStyleBackColor = true;
			// 
			// roundLooseRadioButton
			// 
			this.roundLooseRadioButton.AutoSize = true;
			this.roundLooseRadioButton.Checked = true;
			this.roundLooseRadioButton.Location = new System.Drawing.Point(72, 26);
			this.roundLooseRadioButton.Name = "roundLooseRadioButton";
			this.roundLooseRadioButton.Size = new System.Drawing.Size(103, 17);
			this.roundLooseRadioButton.TabIndex = 1;
			this.roundLooseRadioButton.TabStop = true;
			this.roundLooseRadioButton.Text = "Rounded (loose)";
			this.infoToolTip.SetToolTip(this.roundLooseRadioButton, "123.44 <= x <= 123.46");
			this.roundLooseRadioButton.UseVisualStyleBackColor = true;
			// 
			// roundStrictRadioButton
			// 
			this.roundStrictRadioButton.AutoSize = true;
			this.roundStrictRadioButton.Location = new System.Drawing.Point(72, 10);
			this.roundStrictRadioButton.Name = "roundStrictRadioButton";
			this.roundStrictRadioButton.Size = new System.Drawing.Size(100, 17);
			this.roundStrictRadioButton.TabIndex = 0;
			this.roundStrictRadioButton.Text = "Rounded (strict)";
			this.infoToolTip.SetToolTip(this.roundStrictRadioButton, "123.45 == 123.454319");
			this.roundStrictRadioButton.UseVisualStyleBackColor = true;
			// 
			// stringOptionsGroupBox
			// 
			this.stringOptionsGroupBox.Controls.Add(this.caseSensitiveCheckBox);
			this.stringOptionsGroupBox.Controls.Add(this.encodingUtf32RadioButton);
			this.stringOptionsGroupBox.Controls.Add(this.encodingUtf16RadioButton);
			this.stringOptionsGroupBox.Controls.Add(this.encodingUtf8RadioButton);
			this.stringOptionsGroupBox.Location = new System.Drawing.Point(3, 172);
			this.stringOptionsGroupBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
			this.stringOptionsGroupBox.Name = "stringOptionsGroupBox";
			this.stringOptionsGroupBox.Size = new System.Drawing.Size(308, 64);
			this.stringOptionsGroupBox.TabIndex = 10;
			this.stringOptionsGroupBox.TabStop = false;
			this.stringOptionsGroupBox.Visible = false;
			// 
			// caseSensitiveCheckBox
			// 
			this.caseSensitiveCheckBox.AutoSize = true;
			this.caseSensitiveCheckBox.Checked = true;
			this.caseSensitiveCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.caseSensitiveCheckBox.Location = new System.Drawing.Point(164, 10);
			this.caseSensitiveCheckBox.Name = "caseSensitiveCheckBox";
			this.caseSensitiveCheckBox.Size = new System.Drawing.Size(94, 17);
			this.caseSensitiveCheckBox.TabIndex = 3;
			this.caseSensitiveCheckBox.Text = "Case sensitive";
			this.infoToolTip.SetToolTip(this.caseSensitiveCheckBox, "ASD == asd");
			this.caseSensitiveCheckBox.UseVisualStyleBackColor = true;
			// 
			// encodingUtf32RadioButton
			// 
			this.encodingUtf32RadioButton.AutoSize = true;
			this.encodingUtf32RadioButton.Location = new System.Drawing.Point(72, 42);
			this.encodingUtf32RadioButton.Name = "encodingUtf32RadioButton";
			this.encodingUtf32RadioButton.Size = new System.Drawing.Size(61, 17);
			this.encodingUtf32RadioButton.TabIndex = 2;
			this.encodingUtf32RadioButton.Text = "UTF-32";
			this.encodingUtf32RadioButton.UseVisualStyleBackColor = true;
			// 
			// encodingUtf16RadioButton
			// 
			this.encodingUtf16RadioButton.AutoSize = true;
			this.encodingUtf16RadioButton.Location = new System.Drawing.Point(72, 26);
			this.encodingUtf16RadioButton.Name = "encodingUtf16RadioButton";
			this.encodingUtf16RadioButton.Size = new System.Drawing.Size(61, 17);
			this.encodingUtf16RadioButton.TabIndex = 1;
			this.encodingUtf16RadioButton.Text = "UTF-16";
			this.encodingUtf16RadioButton.UseVisualStyleBackColor = true;
			// 
			// encodingUtf8RadioButton
			// 
			this.encodingUtf8RadioButton.AutoSize = true;
			this.encodingUtf8RadioButton.Checked = true;
			this.encodingUtf8RadioButton.Location = new System.Drawing.Point(72, 10);
			this.encodingUtf8RadioButton.Name = "encodingUtf8RadioButton";
			this.encodingUtf8RadioButton.Size = new System.Drawing.Size(55, 17);
			this.encodingUtf8RadioButton.TabIndex = 0;
			this.encodingUtf8RadioButton.TabStop = true;
			this.encodingUtf8RadioButton.Text = "UTF-8";
			this.encodingUtf8RadioButton.UseVisualStyleBackColor = true;
			// 
			// firstScanButton
			// 
			this.firstScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.firstScanButton.Location = new System.Drawing.Point(292, 54);
			this.firstScanButton.Name = "firstScanButton";
			this.firstScanButton.Size = new System.Drawing.Size(75, 23);
			this.firstScanButton.TabIndex = 11;
			this.firstScanButton.Text = "First Scan";
			this.firstScanButton.UseVisualStyleBackColor = true;
			this.firstScanButton.Click += new System.EventHandler(this.firstScanButton_Click);
			// 
			// nextScanButton
			// 
			this.nextScanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.nextScanButton.Enabled = false;
			this.nextScanButton.Location = new System.Drawing.Point(373, 54);
			this.nextScanButton.Name = "nextScanButton";
			this.nextScanButton.Size = new System.Drawing.Size(75, 23);
			this.nextScanButton.TabIndex = 12;
			this.nextScanButton.Text = "Next Scan";
			this.nextScanButton.UseVisualStyleBackColor = true;
			this.nextScanButton.Click += new System.EventHandler(this.nextScanButton_Click);
			// 
			// scanProgressBar
			// 
			this.scanProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.scanProgressBar.Location = new System.Drawing.Point(454, 54);
			this.scanProgressBar.Name = "scanProgressBar";
			this.scanProgressBar.Size = new System.Drawing.Size(149, 23);
			this.scanProgressBar.TabIndex = 13;
			// 
			// resultCountLabel
			// 
			this.resultCountLabel.AutoSize = true;
			this.resultCountLabel.Location = new System.Drawing.Point(8, 59);
			this.resultCountLabel.Name = "resultCountLabel";
			this.resultCountLabel.Size = new System.Drawing.Size(19, 13);
			this.resultCountLabel.TabIndex = 15;
			this.resultCountLabel.Text = "<>";
			// 
			// updateValuesTimer
			// 
			this.updateValuesTimer.Enabled = true;
			this.updateValuesTimer.Interval = 1000;
			this.updateValuesTimer.Tick += new System.EventHandler(this.updateValuesTimer_Tick);
			// 
			// resultMemoryRecordList
			// 
			this.resultMemoryRecordList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.resultMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
			this.resultMemoryRecordList.Location = new System.Drawing.Point(11, 80);
			this.resultMemoryRecordList.Name = "resultMemoryRecordList";
			this.resultMemoryRecordList.ShowAddressColumn = true;
			this.resultMemoryRecordList.ShowDescriptionColumn = false;
			this.resultMemoryRecordList.ShowPreviousValueColumn = true;
			this.resultMemoryRecordList.ShowValueColumn = true;
			this.resultMemoryRecordList.ShowValueTypeColumn = false;
			this.resultMemoryRecordList.Size = new System.Drawing.Size(268, 314);
			this.resultMemoryRecordList.TabIndex = 16;
			this.resultMemoryRecordList.RecordDoubleClick += new ReClassNET.UI.MemorySearchResultControlResultDoubleClickEventHandler(this.memorySearchResultControl_ResultDoubleClick);
			// 
			// resultListContextMenuStrip
			// 
			this.resultListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.addSelectedResultsToAddressListToolStripMenuItem,
			this.removeSelectedRecordsToolStripMenuItem,
			this.changeToolStripMenuItem,
			this.toolStripSeparator2,
			this.setCurrentClassAddressToolStripMenuItem,
			this.createClassAtAddressToolStripMenuItem,
			this.toolStripSeparator3,
			this.findOutWhatAccessesThisAddressToolStripMenuItem,
			this.findOutWhatWritesToThisAddressToolStripMenuItem,
			this.toolStripSeparator4,
			this.copyAddressToolStripMenuItem});
			this.resultListContextMenuStrip.Name = "resultListContextMenuStrip";
			this.resultListContextMenuStrip.Size = new System.Drawing.Size(270, 198);
			this.resultListContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.resultListContextMenuStrip_Opening);
			// 
			// addSelectedResultsToAddressListToolStripMenuItem
			// 
			this.addSelectedResultsToAddressListToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Tree_Expand;
			this.addSelectedResultsToAddressListToolStripMenuItem.Name = "addSelectedResultsToAddressListToolStripMenuItem";
			this.addSelectedResultsToAddressListToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.addSelectedResultsToAddressListToolStripMenuItem.Text = "Add selected results to address list";
			this.addSelectedResultsToAddressListToolStripMenuItem.Click += new System.EventHandler(this.addSelectedResultsToAddressListToolStripMenuItem_Click);
			// 
			// removeSelectedRecordsToolStripMenuItem
			// 
			this.removeSelectedRecordsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Delete;
			this.removeSelectedRecordsToolStripMenuItem.Name = "removeSelectedRecordsToolStripMenuItem";
			this.removeSelectedRecordsToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.removeSelectedRecordsToolStripMenuItem.Text = "Remove selected records";
			this.removeSelectedRecordsToolStripMenuItem.Click += new System.EventHandler(this.removeSelectedRecordsToolStripMenuItem_Click);
			// 
			// changeToolStripMenuItem
			// 
			this.changeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.descriptionToolStripMenuItem,
			this.addressToolStripMenuItem,
			this.valueTypeToolStripMenuItem,
			this.valueToolStripMenuItem});
			this.changeToolStripMenuItem.Enabled = false;
			this.changeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Textfield_Rename;
			this.changeToolStripMenuItem.Name = "changeToolStripMenuItem";
			this.changeToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.changeToolStripMenuItem.Text = "Change...";
			// 
			// descriptionToolStripMenuItem
			// 
			this.descriptionToolStripMenuItem.Name = "descriptionToolStripMenuItem";
			this.descriptionToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.descriptionToolStripMenuItem.Text = "Description";
			// 
			// addressToolStripMenuItem
			// 
			this.addressToolStripMenuItem.Name = "addressToolStripMenuItem";
			this.addressToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.addressToolStripMenuItem.Text = "Address";
			// 
			// valueTypeToolStripMenuItem
			// 
			this.valueTypeToolStripMenuItem.Name = "valueTypeToolStripMenuItem";
			this.valueTypeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.valueTypeToolStripMenuItem.Text = "Value Type";
			// 
			// valueToolStripMenuItem
			// 
			this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
			this.valueToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.valueToolStripMenuItem.Text = "Value";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(266, 6);
			// 
			// setCurrentClassAddressToolStripMenuItem
			// 
			this.setCurrentClassAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Exchange_Button;
			this.setCurrentClassAddressToolStripMenuItem.Name = "setCurrentClassAddressToolStripMenuItem";
			this.setCurrentClassAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.setCurrentClassAddressToolStripMenuItem.Text = "Set current class address";
			this.setCurrentClassAddressToolStripMenuItem.Click += new System.EventHandler(this.setCurrentClassAddressToolStripMenuItem_Click);
			// 
			// createClassAtAddressToolStripMenuItem
			// 
			this.createClassAtAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Add;
			this.createClassAtAddressToolStripMenuItem.Name = "createClassAtAddressToolStripMenuItem";
			this.createClassAtAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.createClassAtAddressToolStripMenuItem.Text = "Create class at address";
			this.createClassAtAddressToolStripMenuItem.Click += new System.EventHandler(this.createClassAtAddressToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(266, 6);
			// 
			// findOutWhatAccessesThisAddressToolStripMenuItem
			// 
			this.findOutWhatAccessesThisAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Find_Access;
			this.findOutWhatAccessesThisAddressToolStripMenuItem.Name = "findOutWhatAccessesThisAddressToolStripMenuItem";
			this.findOutWhatAccessesThisAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.findOutWhatAccessesThisAddressToolStripMenuItem.Text = "Find out what accesses this address...";
			this.findOutWhatAccessesThisAddressToolStripMenuItem.Click += new System.EventHandler(this.findOutWhatAccessesThisAddressToolStripMenuItem_Click);
			// 
			// findOutWhatWritesToThisAddressToolStripMenuItem
			// 
			this.findOutWhatWritesToThisAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Find_Write;
			this.findOutWhatWritesToThisAddressToolStripMenuItem.Name = "findOutWhatWritesToThisAddressToolStripMenuItem";
			this.findOutWhatWritesToThisAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.findOutWhatWritesToThisAddressToolStripMenuItem.Text = "Find out what writes to this address...";
			this.findOutWhatWritesToThisAddressToolStripMenuItem.Click += new System.EventHandler(this.findOutWhatWritesToThisAddressToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(266, 6);
			// 
			// copyAddressToolStripMenuItem
			// 
			this.copyAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Copy;
			this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
			this.copyAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.copyAddressToolStripMenuItem.Text = "Copy Address";
			this.copyAddressToolStripMenuItem.Click += new System.EventHandler(this.copyAddressToolStripMenuItem_Click);
			// 
			// addressListMemoryRecordList
			// 
			this.addressListMemoryRecordList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)
			| System.Windows.Forms.AnchorStyles.Right)));
			this.addressListMemoryRecordList.ContextMenuStrip = this.resultListContextMenuStrip;
			this.addressListMemoryRecordList.Location = new System.Drawing.Point(11, 425);
			this.addressListMemoryRecordList.Name = "addressListMemoryRecordList";
			this.addressListMemoryRecordList.ShowAddressColumn = true;
			this.addressListMemoryRecordList.ShowDescriptionColumn = true;
			this.addressListMemoryRecordList.ShowPreviousValueColumn = false;
			this.addressListMemoryRecordList.ShowValueColumn = true;
			this.addressListMemoryRecordList.ShowValueTypeColumn = true;
			this.addressListMemoryRecordList.Size = new System.Drawing.Size(592, 169);
			this.addressListMemoryRecordList.TabIndex = 17;
			// 
			// toolStripPanel
			// 
			this.toolStripPanel.Controls.Add(this.menuToolStrip);
			this.toolStripPanel.Location = new System.Drawing.Point(11, 397);
			this.toolStripPanel.Name = "toolStripPanel";
			this.toolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
			this.toolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.toolStripPanel.RowMargin = new System.Windows.Forms.Padding(0);
			this.toolStripPanel.Size = new System.Drawing.Size(128, 25);
			// 
			// menuToolStrip
			// 
			this.menuToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.menuToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.menuToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.openAddressFileToolStripButton,
			this.saveAddressFileToolStripButton,
			this.saveAddressFileAsToolStripButton,
			this.toolStripSeparator1,
			this.clearAddressListToolStripButton});
			this.menuToolStrip.Location = new System.Drawing.Point(0, 0);
			this.menuToolStrip.Name = "menuToolStrip";
			this.menuToolStrip.Size = new System.Drawing.Size(101, 25);
			this.menuToolStrip.TabIndex = 0;
			// 
			// openAddressFileToolStripButton
			// 
			this.openAddressFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openAddressFileToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Folder;
			this.openAddressFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openAddressFileToolStripButton.Name = "openAddressFileToolStripButton";
			this.openAddressFileToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.openAddressFileToolStripButton.ToolTipText = "Open...";
			this.openAddressFileToolStripButton.Click += new System.EventHandler(this.openAddressFileToolStripButton_Click);
			// 
			// saveAddressFileToolStripButton
			// 
			this.saveAddressFileToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveAddressFileToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Save;
			this.saveAddressFileToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveAddressFileToolStripButton.Name = "saveAddressFileToolStripButton";
			this.saveAddressFileToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.saveAddressFileToolStripButton.ToolTipText = "Save";
			this.saveAddressFileToolStripButton.Click += new System.EventHandler(this.saveAddressFileToolStripButton_Click);
			// 
			// saveAddressFileAsToolStripButton
			// 
			this.saveAddressFileAsToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveAddressFileAsToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Save_As;
			this.saveAddressFileAsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveAddressFileAsToolStripButton.Name = "saveAddressFileAsToolStripButton";
			this.saveAddressFileAsToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.saveAddressFileAsToolStripButton.ToolTipText = "Save As...";
			this.saveAddressFileAsToolStripButton.Click += new System.EventHandler(this.saveAsToolStripButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// clearAddressListToolStripButton
			// 
			this.clearAddressListToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.clearAddressListToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Delete;
			this.clearAddressListToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.clearAddressListToolStripButton.Name = "clearAddressListToolStripButton";
			this.clearAddressListToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.clearAddressListToolStripButton.ToolTipText = "Clear";
			this.clearAddressListToolStripButton.Click += new System.EventHandler(this.clearAddressListToolStripButton_Click);
			// 
			// infoToolTip
			// 
			this.infoToolTip.AutomaticDelay = 100;
			// 
			// undoIconButton
			// 
			this.undoIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.undoIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Undo;
			this.undoIconButton.Location = new System.Drawing.Point(256, 54);
			this.undoIconButton.Name = "undoIconButton";
			this.undoIconButton.Pressed = false;
			this.undoIconButton.Selected = false;
			this.undoIconButton.Size = new System.Drawing.Size(23, 22);
			this.undoIconButton.TabIndex = 18;
			this.undoIconButton.Click += new System.EventHandler(this.undoIconButton_Click);
			// 
			// showInputCorrelatorIconButton
			// 
			this.showInputCorrelatorIconButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.showInputCorrelatorIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Canvas_Size;
			this.showInputCorrelatorIconButton.Location = new System.Drawing.Point(580, 398);
			this.showInputCorrelatorIconButton.Name = "showInputCorrelatorIconButton";
			this.showInputCorrelatorIconButton.Pressed = false;
			this.showInputCorrelatorIconButton.Selected = false;
			this.showInputCorrelatorIconButton.Size = new System.Drawing.Size(23, 22);
			this.showInputCorrelatorIconButton.TabIndex = 19;
			this.showInputCorrelatorIconButton.Click += new System.EventHandler(this.showInputCorrelatorIconButton_Click);
			// 
			// cancelScanIconButton
			// 
			this.cancelScanIconButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Delete;
			this.cancelScanIconButton.Location = new System.Drawing.Point(517, 54);
			this.cancelScanIconButton.Name = "cancelScanIconButton";
			this.cancelScanIconButton.Pressed = false;
			this.cancelScanIconButton.Selected = false;
			this.cancelScanIconButton.Size = new System.Drawing.Size(23, 22);
			this.cancelScanIconButton.TabIndex = 21;
			this.cancelScanIconButton.Visible = false;
			this.cancelScanIconButton.Click += new System.EventHandler(this.cancelScanIconButton_Click);
			// 
			// ScannerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(612, 607);
			this.Controls.Add(this.cancelScanIconButton);
			this.Controls.Add(this.showInputCorrelatorIconButton);
			this.Controls.Add(this.undoIconButton);
			this.Controls.Add(this.toolStripPanel);
			this.Controls.Add(this.addressListMemoryRecordList);
			this.Controls.Add(this.resultMemoryRecordList);
			this.Controls.Add(this.resultCountLabel);
			this.Controls.Add(this.scanProgressBar);
			this.Controls.Add(this.nextScanButton);
			this.Controls.Add(this.firstScanButton);
			this.Controls.Add(this.flowLayoutPanel);
			this.Controls.Add(this.bannerBox);
			this.MinimumSize = new System.Drawing.Size(628, 622);
			this.Name = "ScannerForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Scanner";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MemorySearchForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.filterGroupBox.ResumeLayout(false);
			this.filterGroupBox.PerformLayout();
			this.scanOptionsGroupBox.ResumeLayout(false);
			this.scanOptionsGroupBox.PerformLayout();
			this.flowLayoutPanel.ResumeLayout(false);
			this.floatingOptionsGroupBox.ResumeLayout(false);
			this.floatingOptionsGroupBox.PerformLayout();
			this.stringOptionsGroupBox.ResumeLayout(false);
			this.stringOptionsGroupBox.PerformLayout();
			this.resultListContextMenuStrip.ResumeLayout(false);
			this.toolStripPanel.ResumeLayout(false);
			this.toolStripPanel.PerformLayout();
			this.menuToolStrip.ResumeLayout(false);
			this.menuToolStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private UI.BannerBox bannerBox;
		private System.Windows.Forms.GroupBox filterGroupBox;
		private UI.DualValueBox dualValueBox;
		private System.Windows.Forms.CheckBox isHexCheckBox;
		private ScanCompareTypeComboBox compareTypeComboBox;
		private System.Windows.Forms.Label label1;
		private ScanValueTypeComboBox valueTypeComboBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox scanOptionsGroupBox;
		private System.Windows.Forms.TextBox fastScanAlignmentTextBox;
		private System.Windows.Forms.CheckBox fastScanCheckBox;
		private System.Windows.Forms.CheckBox scanCopyOnWriteCheckBox;
		private System.Windows.Forms.CheckBox scanExecutableCheckBox;
		private System.Windows.Forms.CheckBox scanWritableCheckBox;
		private System.Windows.Forms.CheckBox scanMappedCheckBox;
		private System.Windows.Forms.CheckBox scanImageCheckBox;
		private System.Windows.Forms.CheckBox scanPrivateCheckBox;
		private System.Windows.Forms.TextBox stopAddressTextBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox startAddressTextBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;
		private System.Windows.Forms.GroupBox floatingOptionsGroupBox;
		private System.Windows.Forms.RadioButton roundTruncateRadioButton;
		private System.Windows.Forms.RadioButton roundLooseRadioButton;
		private System.Windows.Forms.RadioButton roundStrictRadioButton;
		private System.Windows.Forms.GroupBox stringOptionsGroupBox;
		private System.Windows.Forms.CheckBox caseSensitiveCheckBox;
		private System.Windows.Forms.RadioButton encodingUtf32RadioButton;
		private System.Windows.Forms.RadioButton encodingUtf16RadioButton;
		private System.Windows.Forms.RadioButton encodingUtf8RadioButton;
		private System.Windows.Forms.Button firstScanButton;
		private System.Windows.Forms.Button nextScanButton;
		private System.Windows.Forms.ProgressBar scanProgressBar;
		private System.Windows.Forms.Label resultCountLabel;
		private System.Windows.Forms.Timer updateValuesTimer;
		private UI.MemoryRecordList resultMemoryRecordList;
		private UI.MemoryRecordList addressListMemoryRecordList;
		private System.Windows.Forms.ToolStripPanel toolStripPanel;
		private System.Windows.Forms.ToolStrip menuToolStrip;
		private System.Windows.Forms.ToolStripButton openAddressFileToolStripButton;
		private System.Windows.Forms.ToolStripButton saveAddressFileToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton clearAddressListToolStripButton;
		private System.Windows.Forms.ToolTip infoToolTip;
		private System.Windows.Forms.ToolStripButton saveAddressFileAsToolStripButton;
		private System.Windows.Forms.ContextMenuStrip resultListContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem addSelectedResultsToAddressListToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem setCurrentClassAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createClassAtAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatAccessesThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatWritesToThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem removeSelectedRecordsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem changeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem descriptionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem valueTypeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem valueToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
		private UI.IconButton undoIconButton;
		private UI.IconButton showInputCorrelatorIconButton;
		private UI.IconButton cancelScanIconButton;
	}
}