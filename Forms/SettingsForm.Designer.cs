namespace ReClassNET.Forms
{
	partial class SettingsForm
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
			this.settingsTabControl = new System.Windows.Forms.TabControl();
			this.generalSettingsTabPage = new System.Windows.Forms.TabPage();
			this.commentsGroupBox = new System.Windows.Forms.GroupBox();
			this.showPluginInfoCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showStringCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showSymbolsCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showRttiCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showPointerCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showIntegerCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showFloatCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.displayGroupBox = new System.Windows.Forms.GroupBox();
			this.highlightChangedValuesCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showTextCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showNodeOffsetCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.showNodeAddressCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.stayOnTopCheckBox = new ReClassNET.UI.SettingsCheckBox();
			this.colorsSettingTabPage = new System.Windows.Forms.TabPage();
			this.nodeColorGroupBox = new System.Windows.Forms.GroupBox();
			this.nodeValueLabel = new System.Windows.Forms.Label();
			this.nodePluginLabel = new System.Windows.Forms.Label();
			this.nodeHexValueColorBox = new ReClassNET.UI.ColorBox();
			this.nodePluginColorBox = new ReClassNET.UI.ColorBox();
			this.nodeHexValueLabel = new System.Windows.Forms.Label();
			this.nodeVTableLabel = new System.Windows.Forms.Label();
			this.nodeOffsetColorBox = new ReClassNET.UI.ColorBox();
			this.nodeVTableColorBox = new ReClassNET.UI.ColorBox();
			this.nodeOffsetLabel = new System.Windows.Forms.Label();
			this.nodeTextLabel = new System.Windows.Forms.Label();
			this.nodeAddressColorBox = new ReClassNET.UI.ColorBox();
			this.nodeTextColorBox = new ReClassNET.UI.ColorBox();
			this.nodeAddressLabel = new System.Windows.Forms.Label();
			this.nodeCommentLabel = new System.Windows.Forms.Label();
			this.nodeHiddenColorBox = new ReClassNET.UI.ColorBox();
			this.nodeCommentColorBox = new ReClassNET.UI.ColorBox();
			this.nodeHiddenLabel = new System.Windows.Forms.Label();
			this.nodeIndexLabel = new System.Windows.Forms.Label();
			this.nodeSelectedColorBox = new ReClassNET.UI.ColorBox();
			this.nodeIndexColorBox = new ReClassNET.UI.ColorBox();
			this.nodeSelectedLabel = new System.Windows.Forms.Label();
			this.nodeTypeColorBox = new ReClassNET.UI.ColorBox();
			this.nodeValueColorBox = new ReClassNET.UI.ColorBox();
			this.nodeTypeLabel = new System.Windows.Forms.Label();
			this.nodeNameLabel = new System.Windows.Forms.Label();
			this.nodeNameColorBox = new ReClassNET.UI.ColorBox();
			this.backgroundLabel = new System.Windows.Forms.Label();
			this.backgroundColorBox = new ReClassNET.UI.ColorBox();
			this.typeDefinitionsSettingsTabPage = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.functionPtrLabel = new System.Windows.Forms.Label();
			this.functionPtrTextBox = new ReClassNET.UI.SettingsTextBox();
			this.utf16TextPtrLabel = new System.Windows.Forms.Label();
			this.utf16TextPtrTextBox = new ReClassNET.UI.SettingsTextBox();
			this.utf16TextLabel = new System.Windows.Forms.Label();
			this.utf16TextTextBox = new ReClassNET.UI.SettingsTextBox();
			this.utf8TextPtrLabel = new System.Windows.Forms.Label();
			this.utf8TextPtrTextBox = new ReClassNET.UI.SettingsTextBox();
			this.utf8TextLabel = new System.Windows.Forms.Label();
			this.utf8TextTextBox = new ReClassNET.UI.SettingsTextBox();
			this.matrix3x3Label = new System.Windows.Forms.Label();
			this.matrix3x3TextBox = new ReClassNET.UI.SettingsTextBox();
			this.matrix3x4Label = new System.Windows.Forms.Label();
			this.matrix3x4TextBox = new ReClassNET.UI.SettingsTextBox();
			this.matrix4x4Label = new System.Windows.Forms.Label();
			this.matrix4x4TextBox = new ReClassNET.UI.SettingsTextBox();
			this.vector2Label = new System.Windows.Forms.Label();
			this.vector2TextBox = new ReClassNET.UI.SettingsTextBox();
			this.vector3Label = new System.Windows.Forms.Label();
			this.vector3TextBox = new ReClassNET.UI.SettingsTextBox();
			this.vector4Label = new System.Windows.Forms.Label();
			this.vector4TextBox = new ReClassNET.UI.SettingsTextBox();
			this.doubleLabel = new System.Windows.Forms.Label();
			this.doubleTextBox = new ReClassNET.UI.SettingsTextBox();
			this.floatLabel = new System.Windows.Forms.Label();
			this.floatTextBox = new ReClassNET.UI.SettingsTextBox();
			this.uint64Label = new System.Windows.Forms.Label();
			this.uint64TextBox = new ReClassNET.UI.SettingsTextBox();
			this.uint32Label = new System.Windows.Forms.Label();
			this.uint32TextBox = new ReClassNET.UI.SettingsTextBox();
			this.uint16Label = new System.Windows.Forms.Label();
			this.uint16TextBox = new ReClassNET.UI.SettingsTextBox();
			this.uint8Label = new System.Windows.Forms.Label();
			this.uint8TextBox = new ReClassNET.UI.SettingsTextBox();
			this.int64Label = new System.Windows.Forms.Label();
			this.int64TextBox = new ReClassNET.UI.SettingsTextBox();
			this.int32Label = new System.Windows.Forms.Label();
			this.int32TextBox = new ReClassNET.UI.SettingsTextBox();
			this.int16Label = new System.Windows.Forms.Label();
			this.int16TextBox = new ReClassNET.UI.SettingsTextBox();
			this.int8Label = new System.Windows.Forms.Label();
			this.int8TextBox = new ReClassNET.UI.SettingsTextBox();
			this.paddingLabel = new System.Windows.Forms.Label();
			this.paddingTextBox = new ReClassNET.UI.SettingsTextBox();
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.settingsTabControl.SuspendLayout();
			this.generalSettingsTabPage.SuspendLayout();
			this.commentsGroupBox.SuspendLayout();
			this.displayGroupBox.SuspendLayout();
			this.colorsSettingTabPage.SuspendLayout();
			this.nodeColorGroupBox.SuspendLayout();
			this.typeDefinitionsSettingsTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
			this.SuspendLayout();
			// 
			// settingsTabControl
			// 
			this.settingsTabControl.Controls.Add(this.generalSettingsTabPage);
			this.settingsTabControl.Controls.Add(this.colorsSettingTabPage);
			this.settingsTabControl.Controls.Add(this.typeDefinitionsSettingsTabPage);
			this.settingsTabControl.Location = new System.Drawing.Point(12, 60);
			this.settingsTabControl.Name = "settingsTabControl";
			this.settingsTabControl.SelectedIndex = 0;
			this.settingsTabControl.Size = new System.Drawing.Size(562, 355);
			this.settingsTabControl.TabIndex = 1;
			// 
			// generalSettingsTabPage
			// 
			this.generalSettingsTabPage.Controls.Add(this.commentsGroupBox);
			this.generalSettingsTabPage.Controls.Add(this.displayGroupBox);
			this.generalSettingsTabPage.Controls.Add(this.stayOnTopCheckBox);
			this.generalSettingsTabPage.Location = new System.Drawing.Point(4, 22);
			this.generalSettingsTabPage.Name = "generalSettingsTabPage";
			this.generalSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.generalSettingsTabPage.Size = new System.Drawing.Size(554, 329);
			this.generalSettingsTabPage.TabIndex = 0;
			this.generalSettingsTabPage.Text = "General";
			this.generalSettingsTabPage.UseVisualStyleBackColor = true;
			// 
			// commentsGroupBox
			// 
			this.commentsGroupBox.Controls.Add(this.showPluginInfoCheckBox);
			this.commentsGroupBox.Controls.Add(this.showStringCheckBox);
			this.commentsGroupBox.Controls.Add(this.showSymbolsCheckBox);
			this.commentsGroupBox.Controls.Add(this.showRttiCheckBox);
			this.commentsGroupBox.Controls.Add(this.showPointerCheckBox);
			this.commentsGroupBox.Controls.Add(this.showIntegerCheckBox);
			this.commentsGroupBox.Controls.Add(this.showFloatCheckBox);
			this.commentsGroupBox.Location = new System.Drawing.Point(6, 39);
			this.commentsGroupBox.Name = "commentsGroupBox";
			this.commentsGroupBox.Size = new System.Drawing.Size(265, 186);
			this.commentsGroupBox.TabIndex = 3;
			this.commentsGroupBox.TabStop = false;
			this.commentsGroupBox.Text = "Node Comments";
			// 
			// showPluginInfoCheckBox
			// 
			this.showPluginInfoCheckBox.AutoSize = true;
			this.showPluginInfoCheckBox.Location = new System.Drawing.Point(6, 157);
			this.showPluginInfoCheckBox.Name = "showPluginInfoCheckBox";
			this.showPluginInfoCheckBox.Size = new System.Drawing.Size(111, 17);
			this.showPluginInfoCheckBox.TabIndex = 6;
			this.showPluginInfoCheckBox.Text = "Show Plugin Infos";
			this.showPluginInfoCheckBox.UseVisualStyleBackColor = true;
			// 
			// showStringCheckBox
			// 
			this.showStringCheckBox.AutoSize = true;
			this.showStringCheckBox.Location = new System.Drawing.Point(6, 134);
			this.showStringCheckBox.Name = "showStringCheckBox";
			this.showStringCheckBox.Size = new System.Drawing.Size(88, 17);
			this.showStringCheckBox.TabIndex = 5;
			this.showStringCheckBox.Text = "Show Strings";
			this.showStringCheckBox.UseVisualStyleBackColor = true;
			// 
			// showSymbolsCheckBox
			// 
			this.showSymbolsCheckBox.AutoSize = true;
			this.showSymbolsCheckBox.Location = new System.Drawing.Point(6, 111);
			this.showSymbolsCheckBox.Name = "showSymbolsCheckBox";
			this.showSymbolsCheckBox.Size = new System.Drawing.Size(130, 17);
			this.showSymbolsCheckBox.TabIndex = 4;
			this.showSymbolsCheckBox.Text = "Show Debug Symbols";
			this.showSymbolsCheckBox.UseVisualStyleBackColor = true;
			// 
			// showRttiCheckBox
			// 
			this.showRttiCheckBox.AutoSize = true;
			this.showRttiCheckBox.Location = new System.Drawing.Point(6, 88);
			this.showRttiCheckBox.Name = "showRttiCheckBox";
			this.showRttiCheckBox.Size = new System.Drawing.Size(81, 17);
			this.showRttiCheckBox.TabIndex = 3;
			this.showRttiCheckBox.Text = "Show RTTI";
			this.showRttiCheckBox.UseVisualStyleBackColor = true;
			// 
			// showPointerCheckBox
			// 
			this.showPointerCheckBox.AutoSize = true;
			this.showPointerCheckBox.Location = new System.Drawing.Point(6, 65);
			this.showPointerCheckBox.Name = "showPointerCheckBox";
			this.showPointerCheckBox.Size = new System.Drawing.Size(94, 17);
			this.showPointerCheckBox.TabIndex = 2;
			this.showPointerCheckBox.Text = "Show Pointers";
			this.showPointerCheckBox.UseVisualStyleBackColor = true;
			// 
			// showIntegerCheckBox
			// 
			this.showIntegerCheckBox.AutoSize = true;
			this.showIntegerCheckBox.Location = new System.Drawing.Point(6, 42);
			this.showIntegerCheckBox.Name = "showIntegerCheckBox";
			this.showIntegerCheckBox.Size = new System.Drawing.Size(124, 17);
			this.showIntegerCheckBox.TabIndex = 1;
			this.showIntegerCheckBox.Text = "Show Integer Values";
			this.showIntegerCheckBox.UseVisualStyleBackColor = true;
			// 
			// showFloatCheckBox
			// 
			this.showFloatCheckBox.AutoSize = true;
			this.showFloatCheckBox.Location = new System.Drawing.Point(6, 19);
			this.showFloatCheckBox.Name = "showFloatCheckBox";
			this.showFloatCheckBox.Size = new System.Drawing.Size(114, 17);
			this.showFloatCheckBox.TabIndex = 0;
			this.showFloatCheckBox.Text = "Show Float Values";
			this.showFloatCheckBox.UseVisualStyleBackColor = true;
			// 
			// displayGroupBox
			// 
			this.displayGroupBox.Controls.Add(this.highlightChangedValuesCheckBox);
			this.displayGroupBox.Controls.Add(this.showTextCheckBox);
			this.displayGroupBox.Controls.Add(this.showNodeOffsetCheckBox);
			this.displayGroupBox.Controls.Add(this.showNodeAddressCheckBox);
			this.displayGroupBox.Location = new System.Drawing.Point(283, 39);
			this.displayGroupBox.Name = "displayGroupBox";
			this.displayGroupBox.Size = new System.Drawing.Size(265, 113);
			this.displayGroupBox.TabIndex = 2;
			this.displayGroupBox.TabStop = false;
			this.displayGroupBox.Text = "Display";
			// 
			// highlightChangedValuesCheckBox
			// 
			this.highlightChangedValuesCheckBox.AutoSize = true;
			this.highlightChangedValuesCheckBox.Location = new System.Drawing.Point(6, 88);
			this.highlightChangedValuesCheckBox.Name = "highlightChangedValuesCheckBox";
			this.highlightChangedValuesCheckBox.Size = new System.Drawing.Size(148, 17);
			this.highlightChangedValuesCheckBox.TabIndex = 3;
			this.highlightChangedValuesCheckBox.Text = "Highlight Changed Values";
			this.highlightChangedValuesCheckBox.UseVisualStyleBackColor = true;
			// 
			// showTextCheckBox
			// 
			this.showTextCheckBox.AutoSize = true;
			this.showTextCheckBox.Location = new System.Drawing.Point(6, 65);
			this.showTextCheckBox.Name = "showTextCheckBox";
			this.showTextCheckBox.Size = new System.Drawing.Size(166, 17);
			this.showTextCheckBox.TabIndex = 2;
			this.showTextCheckBox.Text = "Show Textual Representation";
			this.showTextCheckBox.UseVisualStyleBackColor = true;
			// 
			// showNodeOffsetCheckBox
			// 
			this.showNodeOffsetCheckBox.AutoSize = true;
			this.showNodeOffsetCheckBox.Location = new System.Drawing.Point(6, 42);
			this.showNodeOffsetCheckBox.Name = "showNodeOffsetCheckBox";
			this.showNodeOffsetCheckBox.Size = new System.Drawing.Size(113, 17);
			this.showNodeOffsetCheckBox.TabIndex = 1;
			this.showNodeOffsetCheckBox.Text = "Show Node Offset";
			this.showNodeOffsetCheckBox.UseVisualStyleBackColor = true;
			// 
			// showNodeAddressCheckBox
			// 
			this.showNodeAddressCheckBox.AutoSize = true;
			this.showNodeAddressCheckBox.Location = new System.Drawing.Point(6, 19);
			this.showNodeAddressCheckBox.Name = "showNodeAddressCheckBox";
			this.showNodeAddressCheckBox.Size = new System.Drawing.Size(123, 17);
			this.showNodeAddressCheckBox.TabIndex = 0;
			this.showNodeAddressCheckBox.Text = "Show Node Address";
			this.showNodeAddressCheckBox.UseVisualStyleBackColor = true;
			// 
			// stayOnTopCheckBox
			// 
			this.stayOnTopCheckBox.AutoSize = true;
			this.stayOnTopCheckBox.Location = new System.Drawing.Point(6, 6);
			this.stayOnTopCheckBox.Name = "stayOnTopCheckBox";
			this.stayOnTopCheckBox.Size = new System.Drawing.Size(187, 17);
			this.stayOnTopCheckBox.TabIndex = 1;
			this.stayOnTopCheckBox.Text = "Force ReClass.NET to stay on top";
			this.stayOnTopCheckBox.UseVisualStyleBackColor = true;
			// 
			// colorsSettingTabPage
			// 
			this.colorsSettingTabPage.Controls.Add(this.nodeColorGroupBox);
			this.colorsSettingTabPage.Controls.Add(this.backgroundLabel);
			this.colorsSettingTabPage.Controls.Add(this.backgroundColorBox);
			this.colorsSettingTabPage.Location = new System.Drawing.Point(4, 22);
			this.colorsSettingTabPage.Name = "colorsSettingTabPage";
			this.colorsSettingTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.colorsSettingTabPage.Size = new System.Drawing.Size(554, 329);
			this.colorsSettingTabPage.TabIndex = 1;
			this.colorsSettingTabPage.Text = "Colors";
			this.colorsSettingTabPage.UseVisualStyleBackColor = true;
			// 
			// nodeColorGroupBox
			// 
			this.nodeColorGroupBox.Controls.Add(this.nodeValueLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodePluginLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeHexValueColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodePluginColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeHexValueLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeVTableLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeOffsetColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeVTableColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeOffsetLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeTextLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeAddressColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeTextColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeAddressLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeCommentLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeHiddenColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeCommentColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeHiddenLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeIndexLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeSelectedColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeIndexColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeSelectedLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeTypeColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeValueColorBox);
			this.nodeColorGroupBox.Controls.Add(this.nodeTypeLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeNameLabel);
			this.nodeColorGroupBox.Controls.Add(this.nodeNameColorBox);
			this.nodeColorGroupBox.Location = new System.Drawing.Point(9, 43);
			this.nodeColorGroupBox.Name = "nodeColorGroupBox";
			this.nodeColorGroupBox.Size = new System.Drawing.Size(539, 225);
			this.nodeColorGroupBox.TabIndex = 28;
			this.nodeColorGroupBox.TabStop = false;
			this.nodeColorGroupBox.Text = "Node Colors";
			// 
			// nodeValueLabel
			// 
			this.nodeValueLabel.AutoSize = true;
			this.nodeValueLabel.Location = new System.Drawing.Point(9, 198);
			this.nodeValueLabel.Name = "nodeValueLabel";
			this.nodeValueLabel.Size = new System.Drawing.Size(64, 13);
			this.nodeValueLabel.TabIndex = 17;
			this.nodeValueLabel.Text = "Value Color:";
			// 
			// nodePluginLabel
			// 
			this.nodePluginLabel.AutoSize = true;
			this.nodePluginLabel.Location = new System.Drawing.Point(286, 172);
			this.nodePluginLabel.Name = "nodePluginLabel";
			this.nodePluginLabel.Size = new System.Drawing.Size(87, 13);
			this.nodePluginLabel.TabIndex = 27;
			this.nodePluginLabel.Text = "Plugin Info Color:";
			// 
			// nodeHexValueColorBox
			// 
			this.nodeHexValueColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeHexValueColorBox.Location = new System.Drawing.Point(133, 117);
			this.nodeHexValueColorBox.Name = "nodeHexValueColorBox";
			this.nodeHexValueColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeHexValueColorBox.TabIndex = 2;
			// 
			// nodePluginColorBox
			// 
			this.nodePluginColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodePluginColorBox.Location = new System.Drawing.Point(410, 169);
			this.nodePluginColorBox.Name = "nodePluginColorBox";
			this.nodePluginColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodePluginColorBox.TabIndex = 26;
			// 
			// nodeHexValueLabel
			// 
			this.nodeHexValueLabel.AutoSize = true;
			this.nodeHexValueLabel.Location = new System.Drawing.Point(9, 120);
			this.nodeHexValueLabel.Name = "nodeHexValueLabel";
			this.nodeHexValueLabel.Size = new System.Drawing.Size(86, 13);
			this.nodeHexValueLabel.TabIndex = 3;
			this.nodeHexValueLabel.Text = "Hex Value Color:";
			// 
			// nodeVTableLabel
			// 
			this.nodeVTableLabel.AutoSize = true;
			this.nodeVTableLabel.Location = new System.Drawing.Point(286, 94);
			this.nodeVTableLabel.Name = "nodeVTableLabel";
			this.nodeVTableLabel.Size = new System.Drawing.Size(71, 13);
			this.nodeVTableLabel.TabIndex = 25;
			this.nodeVTableLabel.Text = "VTable Color:";
			// 
			// nodeOffsetColorBox
			// 
			this.nodeOffsetColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeOffsetColorBox.Location = new System.Drawing.Point(133, 91);
			this.nodeOffsetColorBox.Name = "nodeOffsetColorBox";
			this.nodeOffsetColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeOffsetColorBox.TabIndex = 4;
			// 
			// nodeVTableColorBox
			// 
			this.nodeVTableColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeVTableColorBox.Location = new System.Drawing.Point(410, 91);
			this.nodeVTableColorBox.Name = "nodeVTableColorBox";
			this.nodeVTableColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeVTableColorBox.TabIndex = 24;
			// 
			// nodeOffsetLabel
			// 
			this.nodeOffsetLabel.AutoSize = true;
			this.nodeOffsetLabel.Location = new System.Drawing.Point(9, 94);
			this.nodeOffsetLabel.Name = "nodeOffsetLabel";
			this.nodeOffsetLabel.Size = new System.Drawing.Size(65, 13);
			this.nodeOffsetLabel.TabIndex = 5;
			this.nodeOffsetLabel.Text = "Offset Color:";
			// 
			// nodeTextLabel
			// 
			this.nodeTextLabel.AutoSize = true;
			this.nodeTextLabel.Location = new System.Drawing.Point(286, 146);
			this.nodeTextLabel.Name = "nodeTextLabel";
			this.nodeTextLabel.Size = new System.Drawing.Size(58, 13);
			this.nodeTextLabel.TabIndex = 23;
			this.nodeTextLabel.Text = "Text Color:";
			// 
			// nodeAddressColorBox
			// 
			this.nodeAddressColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeAddressColorBox.Location = new System.Drawing.Point(133, 65);
			this.nodeAddressColorBox.Name = "nodeAddressColorBox";
			this.nodeAddressColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeAddressColorBox.TabIndex = 6;
			// 
			// nodeTextColorBox
			// 
			this.nodeTextColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeTextColorBox.Location = new System.Drawing.Point(410, 143);
			this.nodeTextColorBox.Name = "nodeTextColorBox";
			this.nodeTextColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeTextColorBox.TabIndex = 22;
			// 
			// nodeAddressLabel
			// 
			this.nodeAddressLabel.AutoSize = true;
			this.nodeAddressLabel.Location = new System.Drawing.Point(9, 68);
			this.nodeAddressLabel.Name = "nodeAddressLabel";
			this.nodeAddressLabel.Size = new System.Drawing.Size(75, 13);
			this.nodeAddressLabel.TabIndex = 7;
			this.nodeAddressLabel.Text = "Address Color:";
			// 
			// nodeCommentLabel
			// 
			this.nodeCommentLabel.AutoSize = true;
			this.nodeCommentLabel.Location = new System.Drawing.Point(286, 120);
			this.nodeCommentLabel.Name = "nodeCommentLabel";
			this.nodeCommentLabel.Size = new System.Drawing.Size(81, 13);
			this.nodeCommentLabel.TabIndex = 21;
			this.nodeCommentLabel.Text = "Comment Color:";
			// 
			// nodeHiddenColorBox
			// 
			this.nodeHiddenColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeHiddenColorBox.Location = new System.Drawing.Point(410, 18);
			this.nodeHiddenColorBox.Name = "nodeHiddenColorBox";
			this.nodeHiddenColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeHiddenColorBox.TabIndex = 8;
			// 
			// nodeCommentColorBox
			// 
			this.nodeCommentColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeCommentColorBox.Location = new System.Drawing.Point(410, 117);
			this.nodeCommentColorBox.Name = "nodeCommentColorBox";
			this.nodeCommentColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeCommentColorBox.TabIndex = 20;
			// 
			// nodeHiddenLabel
			// 
			this.nodeHiddenLabel.AutoSize = true;
			this.nodeHiddenLabel.Location = new System.Drawing.Point(286, 21);
			this.nodeHiddenLabel.Name = "nodeHiddenLabel";
			this.nodeHiddenLabel.Size = new System.Drawing.Size(71, 13);
			this.nodeHiddenLabel.TabIndex = 9;
			this.nodeHiddenLabel.Text = "Hidden Color:";
			// 
			// nodeIndexLabel
			// 
			this.nodeIndexLabel.AutoSize = true;
			this.nodeIndexLabel.Location = new System.Drawing.Point(286, 68);
			this.nodeIndexLabel.Name = "nodeIndexLabel";
			this.nodeIndexLabel.Size = new System.Drawing.Size(63, 13);
			this.nodeIndexLabel.TabIndex = 19;
			this.nodeIndexLabel.Text = "Index Color:";
			// 
			// nodeSelectedColorBox
			// 
			this.nodeSelectedColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeSelectedColorBox.Location = new System.Drawing.Point(133, 18);
			this.nodeSelectedColorBox.Name = "nodeSelectedColorBox";
			this.nodeSelectedColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeSelectedColorBox.TabIndex = 10;
			// 
			// nodeIndexColorBox
			// 
			this.nodeIndexColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeIndexColorBox.Location = new System.Drawing.Point(410, 65);
			this.nodeIndexColorBox.Name = "nodeIndexColorBox";
			this.nodeIndexColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeIndexColorBox.TabIndex = 18;
			// 
			// nodeSelectedLabel
			// 
			this.nodeSelectedLabel.AutoSize = true;
			this.nodeSelectedLabel.Location = new System.Drawing.Point(9, 21);
			this.nodeSelectedLabel.Name = "nodeSelectedLabel";
			this.nodeSelectedLabel.Size = new System.Drawing.Size(79, 13);
			this.nodeSelectedLabel.TabIndex = 11;
			this.nodeSelectedLabel.Text = "Selected Color:";
			// 
			// nodeTypeColorBox
			// 
			this.nodeTypeColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeTypeColorBox.Location = new System.Drawing.Point(133, 143);
			this.nodeTypeColorBox.Name = "nodeTypeColorBox";
			this.nodeTypeColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeTypeColorBox.TabIndex = 12;
			// 
			// nodeValueColorBox
			// 
			this.nodeValueColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeValueColorBox.Location = new System.Drawing.Point(133, 195);
			this.nodeValueColorBox.Name = "nodeValueColorBox";
			this.nodeValueColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeValueColorBox.TabIndex = 16;
			// 
			// nodeTypeLabel
			// 
			this.nodeTypeLabel.AutoSize = true;
			this.nodeTypeLabel.Location = new System.Drawing.Point(9, 146);
			this.nodeTypeLabel.Name = "nodeTypeLabel";
			this.nodeTypeLabel.Size = new System.Drawing.Size(61, 13);
			this.nodeTypeLabel.TabIndex = 13;
			this.nodeTypeLabel.Text = "Type Color:";
			// 
			// nodeNameLabel
			// 
			this.nodeNameLabel.AutoSize = true;
			this.nodeNameLabel.Location = new System.Drawing.Point(9, 172);
			this.nodeNameLabel.Name = "nodeNameLabel";
			this.nodeNameLabel.Size = new System.Drawing.Size(65, 13);
			this.nodeNameLabel.TabIndex = 15;
			this.nodeNameLabel.Text = "Name Color:";
			// 
			// nodeNameColorBox
			// 
			this.nodeNameColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.nodeNameColorBox.Location = new System.Drawing.Point(133, 169);
			this.nodeNameColorBox.Name = "nodeNameColorBox";
			this.nodeNameColorBox.Size = new System.Drawing.Size(123, 20);
			this.nodeNameColorBox.TabIndex = 14;
			// 
			// backgroundLabel
			// 
			this.backgroundLabel.AutoSize = true;
			this.backgroundLabel.Location = new System.Drawing.Point(6, 14);
			this.backgroundLabel.Name = "backgroundLabel";
			this.backgroundLabel.Size = new System.Drawing.Size(161, 13);
			this.backgroundLabel.TabIndex = 1;
			this.backgroundLabel.Text = "Memory View Background Color:";
			// 
			// backgroundColorBox
			// 
			this.backgroundColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.backgroundColorBox.Location = new System.Drawing.Point(175, 11);
			this.backgroundColorBox.Name = "backgroundColorBox";
			this.backgroundColorBox.Size = new System.Drawing.Size(123, 20);
			this.backgroundColorBox.TabIndex = 0;
			// 
			// typeDefinitionsSettingsTabPage
			// 
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.label1);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.functionPtrLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.functionPtrTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextPtrLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextPtrTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextPtrLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextPtrTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x3Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x3TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x4Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x4TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix4x4Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix4x4TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector2Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector2TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector3Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector3TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector4Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector4TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.doubleLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.doubleTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.floatLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.floatTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint64Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint64TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint32Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint32TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint16Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint16TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint8Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint8TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int64Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int64TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int32Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int32TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int16Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int16TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int8Label);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int8TextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.paddingLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.paddingTextBox);
			this.typeDefinitionsSettingsTabPage.Location = new System.Drawing.Point(4, 22);
			this.typeDefinitionsSettingsTabPage.Name = "typeDefinitionsSettingsTabPage";
			this.typeDefinitionsSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.typeDefinitionsSettingsTabPage.Size = new System.Drawing.Size(554, 329);
			this.typeDefinitionsSettingsTabPage.TabIndex = 2;
			this.typeDefinitionsSettingsTabPage.Text = "Type Definitions";
			this.typeDefinitionsSettingsTabPage.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(214, 13);
			this.label1.TabIndex = 44;
			this.label1.Text = "These types are used to generate the code:";
			// 
			// functionPtrLabel
			// 
			this.functionPtrLabel.AutoSize = true;
			this.functionPtrLabel.Location = new System.Drawing.Point(254, 295);
			this.functionPtrLabel.Name = "functionPtrLabel";
			this.functionPtrLabel.Size = new System.Drawing.Size(87, 13);
			this.functionPtrLabel.TabIndex = 43;
			this.functionPtrLabel.Text = "Function Pointer:";
			// 
			// functionPtrTextBox
			// 
			this.functionPtrTextBox.Location = new System.Drawing.Point(346, 292);
			this.functionPtrTextBox.Name = "functionPtrTextBox";
			this.functionPtrTextBox.Size = new System.Drawing.Size(120, 20);
			this.functionPtrTextBox.TabIndex = 42;
			// 
			// utf16TextPtrLabel
			// 
			this.utf16TextPtrLabel.AutoSize = true;
			this.utf16TextPtrLabel.Location = new System.Drawing.Point(254, 269);
			this.utf16TextPtrLabel.Name = "utf16TextPtrLabel";
			this.utf16TextPtrLabel.Size = new System.Drawing.Size(79, 13);
			this.utf16TextPtrLabel.TabIndex = 41;
			this.utf16TextPtrLabel.Text = "UTF16 Pointer:";
			// 
			// utf16TextPtrTextBox
			// 
			this.utf16TextPtrTextBox.Location = new System.Drawing.Point(346, 266);
			this.utf16TextPtrTextBox.Name = "utf16TextPtrTextBox";
			this.utf16TextPtrTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf16TextPtrTextBox.TabIndex = 40;
			// 
			// utf16TextLabel
			// 
			this.utf16TextLabel.AutoSize = true;
			this.utf16TextLabel.Location = new System.Drawing.Point(254, 243);
			this.utf16TextLabel.Name = "utf16TextLabel";
			this.utf16TextLabel.Size = new System.Drawing.Size(43, 13);
			this.utf16TextLabel.TabIndex = 39;
			this.utf16TextLabel.Text = "UTF16:";
			// 
			// utf16TextTextBox
			// 
			this.utf16TextTextBox.Location = new System.Drawing.Point(346, 240);
			this.utf16TextTextBox.Name = "utf16TextTextBox";
			this.utf16TextTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf16TextTextBox.TabIndex = 38;
			// 
			// utf8TextPtrLabel
			// 
			this.utf8TextPtrLabel.AutoSize = true;
			this.utf8TextPtrLabel.Location = new System.Drawing.Point(254, 217);
			this.utf8TextPtrLabel.Name = "utf8TextPtrLabel";
			this.utf8TextPtrLabel.Size = new System.Drawing.Size(73, 13);
			this.utf8TextPtrLabel.TabIndex = 37;
			this.utf8TextPtrLabel.Text = "UTF8 Pointer:";
			// 
			// utf8TextPtrTextBox
			// 
			this.utf8TextPtrTextBox.Location = new System.Drawing.Point(346, 214);
			this.utf8TextPtrTextBox.Name = "utf8TextPtrTextBox";
			this.utf8TextPtrTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf8TextPtrTextBox.TabIndex = 36;
			// 
			// utf8TextLabel
			// 
			this.utf8TextLabel.AutoSize = true;
			this.utf8TextLabel.Location = new System.Drawing.Point(254, 191);
			this.utf8TextLabel.Name = "utf8TextLabel";
			this.utf8TextLabel.Size = new System.Drawing.Size(37, 13);
			this.utf8TextLabel.TabIndex = 35;
			this.utf8TextLabel.Text = "UTF8:";
			// 
			// utf8TextTextBox
			// 
			this.utf8TextTextBox.Location = new System.Drawing.Point(346, 188);
			this.utf8TextTextBox.Name = "utf8TextTextBox";
			this.utf8TextTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf8TextTextBox.TabIndex = 34;
			// 
			// matrix3x3Label
			// 
			this.matrix3x3Label.AutoSize = true;
			this.matrix3x3Label.Location = new System.Drawing.Point(254, 113);
			this.matrix3x3Label.Name = "matrix3x3Label";
			this.matrix3x3Label.Size = new System.Drawing.Size(64, 13);
			this.matrix3x3Label.TabIndex = 33;
			this.matrix3x3Label.Text = "Matrix (3x3):";
			// 
			// matrix3x3TextBox
			// 
			this.matrix3x3TextBox.Location = new System.Drawing.Point(346, 110);
			this.matrix3x3TextBox.Name = "matrix3x3TextBox";
			this.matrix3x3TextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix3x3TextBox.TabIndex = 32;
			// 
			// matrix3x4Label
			// 
			this.matrix3x4Label.AutoSize = true;
			this.matrix3x4Label.Location = new System.Drawing.Point(254, 139);
			this.matrix3x4Label.Name = "matrix3x4Label";
			this.matrix3x4Label.Size = new System.Drawing.Size(64, 13);
			this.matrix3x4Label.TabIndex = 31;
			this.matrix3x4Label.Text = "Matrix (3x4):";
			// 
			// matrix3x4TextBox
			// 
			this.matrix3x4TextBox.Location = new System.Drawing.Point(346, 136);
			this.matrix3x4TextBox.Name = "matrix3x4TextBox";
			this.matrix3x4TextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix3x4TextBox.TabIndex = 30;
			// 
			// matrix4x4Label
			// 
			this.matrix4x4Label.AutoSize = true;
			this.matrix4x4Label.Location = new System.Drawing.Point(254, 165);
			this.matrix4x4Label.Name = "matrix4x4Label";
			this.matrix4x4Label.Size = new System.Drawing.Size(64, 13);
			this.matrix4x4Label.TabIndex = 29;
			this.matrix4x4Label.Text = "Matrix (4x4):";
			// 
			// matrix4x4TextBox
			// 
			this.matrix4x4TextBox.Location = new System.Drawing.Point(346, 162);
			this.matrix4x4TextBox.Name = "matrix4x4TextBox";
			this.matrix4x4TextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix4x4TextBox.TabIndex = 28;
			// 
			// vector2Label
			// 
			this.vector2Label.AutoSize = true;
			this.vector2Label.Location = new System.Drawing.Point(254, 35);
			this.vector2Label.Name = "vector2Label";
			this.vector2Label.Size = new System.Drawing.Size(47, 13);
			this.vector2Label.TabIndex = 27;
			this.vector2Label.Text = "Vector2:";
			// 
			// vector2TextBox
			// 
			this.vector2TextBox.Location = new System.Drawing.Point(346, 32);
			this.vector2TextBox.Name = "vector2TextBox";
			this.vector2TextBox.Size = new System.Drawing.Size(120, 20);
			this.vector2TextBox.TabIndex = 26;
			// 
			// vector3Label
			// 
			this.vector3Label.AutoSize = true;
			this.vector3Label.Location = new System.Drawing.Point(254, 61);
			this.vector3Label.Name = "vector3Label";
			this.vector3Label.Size = new System.Drawing.Size(47, 13);
			this.vector3Label.TabIndex = 25;
			this.vector3Label.Text = "Vector3:";
			// 
			// vector3TextBox
			// 
			this.vector3TextBox.Location = new System.Drawing.Point(346, 58);
			this.vector3TextBox.Name = "vector3TextBox";
			this.vector3TextBox.Size = new System.Drawing.Size(120, 20);
			this.vector3TextBox.TabIndex = 24;
			// 
			// vector4Label
			// 
			this.vector4Label.AutoSize = true;
			this.vector4Label.Location = new System.Drawing.Point(254, 87);
			this.vector4Label.Name = "vector4Label";
			this.vector4Label.Size = new System.Drawing.Size(47, 13);
			this.vector4Label.TabIndex = 23;
			this.vector4Label.Text = "Vector4:";
			// 
			// vector4TextBox
			// 
			this.vector4TextBox.Location = new System.Drawing.Point(346, 84);
			this.vector4TextBox.Name = "vector4TextBox";
			this.vector4TextBox.Size = new System.Drawing.Size(120, 20);
			this.vector4TextBox.TabIndex = 22;
			// 
			// doubleLabel
			// 
			this.doubleLabel.AutoSize = true;
			this.doubleLabel.Location = new System.Drawing.Point(6, 295);
			this.doubleLabel.Name = "doubleLabel";
			this.doubleLabel.Size = new System.Drawing.Size(44, 13);
			this.doubleLabel.TabIndex = 21;
			this.doubleLabel.Text = "Double:";
			// 
			// doubleTextBox
			// 
			this.doubleTextBox.Location = new System.Drawing.Point(98, 292);
			this.doubleTextBox.Name = "doubleTextBox";
			this.doubleTextBox.Size = new System.Drawing.Size(120, 20);
			this.doubleTextBox.TabIndex = 20;
			// 
			// floatLabel
			// 
			this.floatLabel.AutoSize = true;
			this.floatLabel.Location = new System.Drawing.Point(6, 269);
			this.floatLabel.Name = "floatLabel";
			this.floatLabel.Size = new System.Drawing.Size(33, 13);
			this.floatLabel.TabIndex = 19;
			this.floatLabel.Text = "Float:";
			// 
			// floatTextBox
			// 
			this.floatTextBox.Location = new System.Drawing.Point(98, 266);
			this.floatTextBox.Name = "floatTextBox";
			this.floatTextBox.Size = new System.Drawing.Size(120, 20);
			this.floatTextBox.TabIndex = 18;
			// 
			// uint64Label
			// 
			this.uint64Label.AutoSize = true;
			this.uint64Label.Location = new System.Drawing.Point(6, 243);
			this.uint64Label.Name = "uint64Label";
			this.uint64Label.Size = new System.Drawing.Size(42, 13);
			this.uint64Label.TabIndex = 17;
			this.uint64Label.Text = "UInt64:";
			// 
			// uint64TextBox
			// 
			this.uint64TextBox.Location = new System.Drawing.Point(98, 240);
			this.uint64TextBox.Name = "uint64TextBox";
			this.uint64TextBox.Size = new System.Drawing.Size(120, 20);
			this.uint64TextBox.TabIndex = 16;
			// 
			// uint32Label
			// 
			this.uint32Label.AutoSize = true;
			this.uint32Label.Location = new System.Drawing.Point(6, 217);
			this.uint32Label.Name = "uint32Label";
			this.uint32Label.Size = new System.Drawing.Size(42, 13);
			this.uint32Label.TabIndex = 15;
			this.uint32Label.Text = "UInt32:";
			// 
			// uint32TextBox
			// 
			this.uint32TextBox.Location = new System.Drawing.Point(98, 214);
			this.uint32TextBox.Name = "uint32TextBox";
			this.uint32TextBox.Size = new System.Drawing.Size(120, 20);
			this.uint32TextBox.TabIndex = 14;
			// 
			// uint16Label
			// 
			this.uint16Label.AutoSize = true;
			this.uint16Label.Location = new System.Drawing.Point(6, 191);
			this.uint16Label.Name = "uint16Label";
			this.uint16Label.Size = new System.Drawing.Size(42, 13);
			this.uint16Label.TabIndex = 13;
			this.uint16Label.Text = "UInt16:";
			// 
			// uint16TextBox
			// 
			this.uint16TextBox.Location = new System.Drawing.Point(98, 188);
			this.uint16TextBox.Name = "uint16TextBox";
			this.uint16TextBox.Size = new System.Drawing.Size(120, 20);
			this.uint16TextBox.TabIndex = 12;
			// 
			// uint8Label
			// 
			this.uint8Label.AutoSize = true;
			this.uint8Label.Location = new System.Drawing.Point(6, 165);
			this.uint8Label.Name = "uint8Label";
			this.uint8Label.Size = new System.Drawing.Size(36, 13);
			this.uint8Label.TabIndex = 11;
			this.uint8Label.Text = "UInt8:";
			// 
			// uint8TextBox
			// 
			this.uint8TextBox.Location = new System.Drawing.Point(98, 162);
			this.uint8TextBox.Name = "uint8TextBox";
			this.uint8TextBox.Size = new System.Drawing.Size(120, 20);
			this.uint8TextBox.TabIndex = 10;
			// 
			// int64Label
			// 
			this.int64Label.AutoSize = true;
			this.int64Label.Location = new System.Drawing.Point(6, 139);
			this.int64Label.Name = "int64Label";
			this.int64Label.Size = new System.Drawing.Size(34, 13);
			this.int64Label.TabIndex = 9;
			this.int64Label.Text = "Int64:";
			// 
			// int64TextBox
			// 
			this.int64TextBox.Location = new System.Drawing.Point(98, 136);
			this.int64TextBox.Name = "int64TextBox";
			this.int64TextBox.Size = new System.Drawing.Size(120, 20);
			this.int64TextBox.TabIndex = 8;
			// 
			// int32Label
			// 
			this.int32Label.AutoSize = true;
			this.int32Label.Location = new System.Drawing.Point(6, 113);
			this.int32Label.Name = "int32Label";
			this.int32Label.Size = new System.Drawing.Size(34, 13);
			this.int32Label.TabIndex = 7;
			this.int32Label.Text = "Int32:";
			// 
			// int32TextBox
			// 
			this.int32TextBox.Location = new System.Drawing.Point(98, 110);
			this.int32TextBox.Name = "int32TextBox";
			this.int32TextBox.Size = new System.Drawing.Size(120, 20);
			this.int32TextBox.TabIndex = 6;
			// 
			// int16Label
			// 
			this.int16Label.AutoSize = true;
			this.int16Label.Location = new System.Drawing.Point(6, 87);
			this.int16Label.Name = "int16Label";
			this.int16Label.Size = new System.Drawing.Size(34, 13);
			this.int16Label.TabIndex = 5;
			this.int16Label.Text = "Int16:";
			// 
			// int16TextBox
			// 
			this.int16TextBox.Location = new System.Drawing.Point(98, 84);
			this.int16TextBox.Name = "int16TextBox";
			this.int16TextBox.Size = new System.Drawing.Size(120, 20);
			this.int16TextBox.TabIndex = 4;
			// 
			// int8Label
			// 
			this.int8Label.AutoSize = true;
			this.int8Label.Location = new System.Drawing.Point(6, 61);
			this.int8Label.Name = "int8Label";
			this.int8Label.Size = new System.Drawing.Size(28, 13);
			this.int8Label.TabIndex = 3;
			this.int8Label.Text = "Int8:";
			// 
			// int8TextBox
			// 
			this.int8TextBox.Location = new System.Drawing.Point(98, 58);
			this.int8TextBox.Name = "int8TextBox";
			this.int8TextBox.Size = new System.Drawing.Size(120, 20);
			this.int8TextBox.TabIndex = 2;
			// 
			// paddingLabel
			// 
			this.paddingLabel.AutoSize = true;
			this.paddingLabel.Location = new System.Drawing.Point(6, 35);
			this.paddingLabel.Name = "paddingLabel";
			this.paddingLabel.Size = new System.Drawing.Size(49, 13);
			this.paddingLabel.TabIndex = 1;
			this.paddingLabel.Text = "Padding:";
			// 
			// paddingTextBox
			// 
			this.paddingTextBox.Location = new System.Drawing.Point(98, 32);
			this.paddingTextBox.Name = "paddingTextBox";
			this.paddingTextBox.Size = new System.Drawing.Size(120, 20);
			this.paddingTextBox.TabIndex = 0;
			// 
			// bannerBox
			// 
			this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_Cogs;
			this.bannerBox.Location = new System.Drawing.Point(0, 0);
			this.bannerBox.Name = "bannerBox";
			this.bannerBox.Size = new System.Drawing.Size(586, 48);
			this.bannerBox.TabIndex = 2;
			this.bannerBox.Text = "Configure the global settings.";
			this.bannerBox.Title = "Settings";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(586, 427);
			this.Controls.Add(this.bannerBox);
			this.Controls.Add(this.settingsTabControl);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SettingsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReClass.NET - Settings";
			this.settingsTabControl.ResumeLayout(false);
			this.generalSettingsTabPage.ResumeLayout(false);
			this.generalSettingsTabPage.PerformLayout();
			this.commentsGroupBox.ResumeLayout(false);
			this.commentsGroupBox.PerformLayout();
			this.displayGroupBox.ResumeLayout(false);
			this.displayGroupBox.PerformLayout();
			this.colorsSettingTabPage.ResumeLayout(false);
			this.colorsSettingTabPage.PerformLayout();
			this.nodeColorGroupBox.ResumeLayout(false);
			this.nodeColorGroupBox.PerformLayout();
			this.typeDefinitionsSettingsTabPage.ResumeLayout(false);
			this.typeDefinitionsSettingsTabPage.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bannerBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl settingsTabControl;
		private System.Windows.Forms.TabPage generalSettingsTabPage;
		private System.Windows.Forms.TabPage colorsSettingTabPage;
		private System.Windows.Forms.TabPage typeDefinitionsSettingsTabPage;
		private ReClassNET.UI.SettingsCheckBox stayOnTopCheckBox;
		private System.Windows.Forms.GroupBox displayGroupBox;
		private ReClassNET.UI.SettingsCheckBox showNodeAddressCheckBox;
		private ReClassNET.UI.SettingsCheckBox showTextCheckBox;
		private ReClassNET.UI.SettingsCheckBox showNodeOffsetCheckBox;
		private ReClassNET.UI.SettingsCheckBox highlightChangedValuesCheckBox;
		private System.Windows.Forms.GroupBox commentsGroupBox;
		private ReClassNET.UI.SettingsCheckBox showRttiCheckBox;
		private ReClassNET.UI.SettingsCheckBox showPointerCheckBox;
		private ReClassNET.UI.SettingsCheckBox showIntegerCheckBox;
		private ReClassNET.UI.SettingsCheckBox showFloatCheckBox;
		private ReClassNET.UI.SettingsCheckBox showPluginInfoCheckBox;
		private ReClassNET.UI.SettingsCheckBox showStringCheckBox;
		private ReClassNET.UI.SettingsCheckBox showSymbolsCheckBox;
		private UI.ColorBox backgroundColorBox;
		private System.Windows.Forms.Label nodeSelectedLabel;
		private UI.ColorBox nodeSelectedColorBox;
		private System.Windows.Forms.Label nodeHiddenLabel;
		private UI.ColorBox nodeHiddenColorBox;
		private System.Windows.Forms.Label nodeAddressLabel;
		private UI.ColorBox nodeAddressColorBox;
		private System.Windows.Forms.Label nodeOffsetLabel;
		private UI.ColorBox nodeOffsetColorBox;
		private System.Windows.Forms.Label nodeHexValueLabel;
		private UI.ColorBox nodeHexValueColorBox;
		private System.Windows.Forms.Label backgroundLabel;
		private System.Windows.Forms.Label nodeValueLabel;
		private UI.ColorBox nodeValueColorBox;
		private System.Windows.Forms.Label nodeNameLabel;
		private UI.ColorBox nodeNameColorBox;
		private System.Windows.Forms.Label nodeTypeLabel;
		private UI.ColorBox nodeTypeColorBox;
		private System.Windows.Forms.Label nodeVTableLabel;
		private UI.ColorBox nodeVTableColorBox;
		private System.Windows.Forms.Label nodeTextLabel;
		private UI.ColorBox nodeTextColorBox;
		private System.Windows.Forms.Label nodeCommentLabel;
		private UI.ColorBox nodeCommentColorBox;
		private System.Windows.Forms.Label nodeIndexLabel;
		private UI.ColorBox nodeIndexColorBox;
		private System.Windows.Forms.Label nodePluginLabel;
		private UI.ColorBox nodePluginColorBox;
		private System.Windows.Forms.Label floatLabel;
		private UI.SettingsTextBox floatTextBox;
		private System.Windows.Forms.Label uint64Label;
		private UI.SettingsTextBox uint64TextBox;
		private System.Windows.Forms.Label uint32Label;
		private UI.SettingsTextBox uint32TextBox;
		private System.Windows.Forms.Label uint16Label;
		private UI.SettingsTextBox uint16TextBox;
		private System.Windows.Forms.Label uint8Label;
		private UI.SettingsTextBox uint8TextBox;
		private System.Windows.Forms.Label int64Label;
		private UI.SettingsTextBox int64TextBox;
		private System.Windows.Forms.Label int32Label;
		private UI.SettingsTextBox int32TextBox;
		private System.Windows.Forms.Label int16Label;
		private UI.SettingsTextBox int16TextBox;
		private System.Windows.Forms.Label int8Label;
		private UI.SettingsTextBox int8TextBox;
		private System.Windows.Forms.Label paddingLabel;
		private UI.SettingsTextBox paddingTextBox;
		private System.Windows.Forms.Label functionPtrLabel;
		private UI.SettingsTextBox functionPtrTextBox;
		private System.Windows.Forms.Label utf16TextPtrLabel;
		private UI.SettingsTextBox utf16TextPtrTextBox;
		private System.Windows.Forms.Label utf16TextLabel;
		private UI.SettingsTextBox utf16TextTextBox;
		private System.Windows.Forms.Label utf8TextPtrLabel;
		private UI.SettingsTextBox utf8TextPtrTextBox;
		private System.Windows.Forms.Label utf8TextLabel;
		private UI.SettingsTextBox utf8TextTextBox;
		private System.Windows.Forms.Label matrix3x3Label;
		private UI.SettingsTextBox matrix3x3TextBox;
		private System.Windows.Forms.Label matrix3x4Label;
		private UI.SettingsTextBox matrix3x4TextBox;
		private System.Windows.Forms.Label matrix4x4Label;
		private UI.SettingsTextBox matrix4x4TextBox;
		private System.Windows.Forms.Label vector2Label;
		private UI.SettingsTextBox vector2TextBox;
		private System.Windows.Forms.Label vector3Label;
		private UI.SettingsTextBox vector3TextBox;
		private System.Windows.Forms.Label vector4Label;
		private UI.SettingsTextBox vector4TextBox;
		private System.Windows.Forms.Label doubleLabel;
		private UI.SettingsTextBox doubleTextBox;
		private System.Windows.Forms.GroupBox nodeColorGroupBox;
		private System.Windows.Forms.Label label1;
		private UI.BannerBox bannerBox;
	}
}