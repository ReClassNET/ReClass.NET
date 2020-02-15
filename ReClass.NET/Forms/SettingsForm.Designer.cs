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
			this.fileAssociationGroupBox = new System.Windows.Forms.GroupBox();
			this.removeAssociationButton = new System.Windows.Forms.Button();
			this.createAssociationButton = new System.Windows.Forms.Button();
			this.associationInfoLabel = new System.Windows.Forms.Label();
			this.commentsGroupBox = new System.Windows.Forms.GroupBox();
			this.showPluginInfoCheckBox = new System.Windows.Forms.CheckBox();
			this.showStringCheckBox = new System.Windows.Forms.CheckBox();
			this.showSymbolsCheckBox = new System.Windows.Forms.CheckBox();
			this.showRttiCheckBox = new System.Windows.Forms.CheckBox();
			this.showPointerCheckBox = new System.Windows.Forms.CheckBox();
			this.showIntegerCheckBox = new System.Windows.Forms.CheckBox();
			this.showFloatCheckBox = new System.Windows.Forms.CheckBox();
			this.displayGroupBox = new System.Windows.Forms.GroupBox();
			this.randomizeWindowTitleCheckBox = new System.Windows.Forms.CheckBox();
			this.runAsAdminCheckBox = new System.Windows.Forms.CheckBox();
			this.highlightChangedValuesCheckBox = new System.Windows.Forms.CheckBox();
			this.showTextCheckBox = new System.Windows.Forms.CheckBox();
			this.showNodeOffsetCheckBox = new System.Windows.Forms.CheckBox();
			this.showNodeAddressCheckBox = new System.Windows.Forms.CheckBox();
			this.stayOnTopCheckBox = new System.Windows.Forms.CheckBox();
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
			this.boolSettingsLabel = new System.Windows.Forms.Label();
			this.boolTypeTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.functionPtrSettingsLabel = new System.Windows.Forms.Label();
			this.functionPtrTypeTextBox = new System.Windows.Forms.TextBox();
			this.utf16TextSettingsLabel = new System.Windows.Forms.Label();
			this.utf16TextTypeTextBox = new System.Windows.Forms.TextBox();
			this.utf8TextSettingsLabel = new System.Windows.Forms.Label();
			this.utf8TextTypeTextBox = new System.Windows.Forms.TextBox();
			this.matrix3x3SettingsLabel = new System.Windows.Forms.Label();
			this.matrix3x3TypeTextBox = new System.Windows.Forms.TextBox();
			this.matrix3x4SettingsLabel = new System.Windows.Forms.Label();
			this.matrix3x4TypeTextBox = new System.Windows.Forms.TextBox();
			this.matrix4x4SettingsLabel = new System.Windows.Forms.Label();
			this.matrix4x4TypeTextBox = new System.Windows.Forms.TextBox();
			this.vector2SettingsLabel = new System.Windows.Forms.Label();
			this.vector2TypeTextBox = new System.Windows.Forms.TextBox();
			this.vector3SettingsLabel = new System.Windows.Forms.Label();
			this.vector3TypeTextBox = new System.Windows.Forms.TextBox();
			this.vector4SettingsLabel = new System.Windows.Forms.Label();
			this.vector4TypeTextBox = new System.Windows.Forms.TextBox();
			this.doubleSettingsLabel = new System.Windows.Forms.Label();
			this.doubleTypeTextBox = new System.Windows.Forms.TextBox();
			this.floatSettingsLabel = new System.Windows.Forms.Label();
			this.floatTypeTextBox = new System.Windows.Forms.TextBox();
			this.uint64SettingsLabel = new System.Windows.Forms.Label();
			this.uint64TypeTextBox = new System.Windows.Forms.TextBox();
			this.uint32SettingsLabel = new System.Windows.Forms.Label();
			this.uint32TypeTextBox = new System.Windows.Forms.TextBox();
			this.uint16SettingsLabel = new System.Windows.Forms.Label();
			this.uint16TypeTextBox = new System.Windows.Forms.TextBox();
			this.uint8SettingsLabel = new System.Windows.Forms.Label();
			this.uint8TypeTextBox = new System.Windows.Forms.TextBox();
			this.int64SettingsLabel = new System.Windows.Forms.Label();
			this.int64TypeTextBox = new System.Windows.Forms.TextBox();
			this.int32SettingsLabel = new System.Windows.Forms.Label();
			this.int32TypeTextBox = new System.Windows.Forms.TextBox();
			this.int16SettingsLabel = new System.Windows.Forms.Label();
			this.int16TypeTextBox = new System.Windows.Forms.TextBox();
			this.int8SettingsLabel = new System.Windows.Forms.Label();
			this.int8TypeTextBox = new System.Windows.Forms.TextBox();
			this.bannerBox = new ReClassNET.UI.BannerBox();
			this.settingsTabControl.SuspendLayout();
			this.generalSettingsTabPage.SuspendLayout();
			this.fileAssociationGroupBox.SuspendLayout();
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
			this.generalSettingsTabPage.Controls.Add(this.fileAssociationGroupBox);
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
			// fileAssociationGroupBox
			// 
			this.fileAssociationGroupBox.Controls.Add(this.removeAssociationButton);
			this.fileAssociationGroupBox.Controls.Add(this.createAssociationButton);
			this.fileAssociationGroupBox.Controls.Add(this.associationInfoLabel);
			this.fileAssociationGroupBox.Location = new System.Drawing.Point(6, 231);
			this.fileAssociationGroupBox.Name = "fileAssociationGroupBox";
			this.fileAssociationGroupBox.Size = new System.Drawing.Size(542, 85);
			this.fileAssociationGroupBox.TabIndex = 4;
			this.fileAssociationGroupBox.TabStop = false;
			this.fileAssociationGroupBox.Text = "RCNET File Association";
			// 
			// removeAssociationButton
			// 
			this.removeAssociationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.removeAssociationButton.Location = new System.Drawing.Point(146, 52);
			this.removeAssociationButton.Name = "removeAssociationButton";
			this.removeAssociationButton.Size = new System.Drawing.Size(135, 23);
			this.removeAssociationButton.TabIndex = 2;
			this.removeAssociationButton.Text = "&Remove Association";
			this.removeAssociationButton.UseVisualStyleBackColor = true;
			this.removeAssociationButton.Click += new System.EventHandler(this.removeAssociationButton_Click);
			// 
			// createAssociationButton
			// 
			this.createAssociationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.createAssociationButton.Location = new System.Drawing.Point(9, 52);
			this.createAssociationButton.Name = "createAssociationButton";
			this.createAssociationButton.Size = new System.Drawing.Size(131, 23);
			this.createAssociationButton.TabIndex = 1;
			this.createAssociationButton.Text = "Create &Association";
			this.createAssociationButton.UseVisualStyleBackColor = true;
			this.createAssociationButton.Click += new System.EventHandler(this.createAssociationButton_Click);
			// 
			// associationInfoLabel
			// 
			this.associationInfoLabel.Location = new System.Drawing.Point(6, 21);
			this.associationInfoLabel.Name = "associationInfoLabel";
			this.associationInfoLabel.Size = new System.Drawing.Size(525, 28);
			this.associationInfoLabel.TabIndex = 0;
			this.associationInfoLabel.Text = "RCNET files can be associated with ReClass.NET. When you double-click a RCNET fil" +
    "e, they will automatically be opened by ReClass.NET.";
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
			this.displayGroupBox.Controls.Add(this.randomizeWindowTitleCheckBox);
			this.displayGroupBox.Controls.Add(this.runAsAdminCheckBox);
			this.displayGroupBox.Controls.Add(this.highlightChangedValuesCheckBox);
			this.displayGroupBox.Controls.Add(this.showTextCheckBox);
			this.displayGroupBox.Controls.Add(this.showNodeOffsetCheckBox);
			this.displayGroupBox.Controls.Add(this.showNodeAddressCheckBox);
			this.displayGroupBox.Location = new System.Drawing.Point(283, 39);
			this.displayGroupBox.Name = "displayGroupBox";
			this.displayGroupBox.Size = new System.Drawing.Size(265, 160);
			this.displayGroupBox.TabIndex = 2;
			this.displayGroupBox.TabStop = false;
			this.displayGroupBox.Text = "Display";
			// 
			// randomizeWindowTitleCheckBox
			// 
			this.randomizeWindowTitleCheckBox.AutoSize = true;
			this.randomizeWindowTitleCheckBox.Location = new System.Drawing.Point(6, 134);
			this.randomizeWindowTitleCheckBox.Name = "randomizeWindowTitleCheckBox";
			this.randomizeWindowTitleCheckBox.Size = new System.Drawing.Size(137, 17);
			this.randomizeWindowTitleCheckBox.TabIndex = 5;
			this.randomizeWindowTitleCheckBox.Text = "Randomize window title";
			this.randomizeWindowTitleCheckBox.UseVisualStyleBackColor = true;
			// 
			// runAsAdminCheckBox
			// 
			this.runAsAdminCheckBox.AutoSize = true;
			this.runAsAdminCheckBox.Location = new System.Drawing.Point(6, 111);
			this.runAsAdminCheckBox.Name = "runAsAdminCheckBox";
			this.runAsAdminCheckBox.Size = new System.Drawing.Size(200, 17);
			this.runAsAdminCheckBox.TabIndex = 4;
			this.runAsAdminCheckBox.Text = "Run as administrator (requires restart)";
			this.runAsAdminCheckBox.UseVisualStyleBackColor = true;
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
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.boolSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.boolTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.label1);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.functionPtrSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.functionPtrTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf16TextTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf8TextTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x3SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x3TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x4SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix3x4TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix4x4SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.matrix4x4TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector2SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector2TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector3SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector3TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector4SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.vector4TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.doubleSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.doubleTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.floatSettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.floatTypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint64SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint64TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint32SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint32TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint16SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint16TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint8SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.uint8TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int64SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int64TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int32SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int32TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int16SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int16TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int8SettingsLabel);
			this.typeDefinitionsSettingsTabPage.Controls.Add(this.int8TypeTextBox);
			this.typeDefinitionsSettingsTabPage.Location = new System.Drawing.Point(4, 22);
			this.typeDefinitionsSettingsTabPage.Name = "typeDefinitionsSettingsTabPage";
			this.typeDefinitionsSettingsTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.typeDefinitionsSettingsTabPage.Size = new System.Drawing.Size(554, 329);
			this.typeDefinitionsSettingsTabPage.TabIndex = 2;
			this.typeDefinitionsSettingsTabPage.Text = "Type Definitions";
			this.typeDefinitionsSettingsTabPage.UseVisualStyleBackColor = true;
			// 
			// boolSettingsLabel
			// 
			this.boolSettingsLabel.AutoSize = true;
			this.boolSettingsLabel.Location = new System.Drawing.Point(254, 35);
			this.boolSettingsLabel.Name = "boolSettingsLabel";
			this.boolSettingsLabel.Size = new System.Drawing.Size(31, 13);
			this.boolSettingsLabel.TabIndex = 46;
			this.boolSettingsLabel.Text = "Bool:";
			// 
			// boolTypeTextBox
			// 
			this.boolTypeTextBox.Location = new System.Drawing.Point(346, 32);
			this.boolTypeTextBox.Name = "boolTypeTextBox";
			this.boolTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.boolTypeTextBox.TabIndex = 45;
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
			// functionPtrSettingsLabel
			// 
			this.functionPtrSettingsLabel.AutoSize = true;
			this.functionPtrSettingsLabel.Location = new System.Drawing.Point(254, 233);
			this.functionPtrSettingsLabel.Name = "functionPtrSettingsLabel";
			this.functionPtrSettingsLabel.Size = new System.Drawing.Size(87, 13);
			this.functionPtrSettingsLabel.TabIndex = 43;
			this.functionPtrSettingsLabel.Text = "Function Pointer:";
			// 
			// functionPtrTypeTextBox
			// 
			this.functionPtrTypeTextBox.Location = new System.Drawing.Point(346, 230);
			this.functionPtrTypeTextBox.Name = "functionPtrTypeTextBox";
			this.functionPtrTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.functionPtrTypeTextBox.TabIndex = 42;
			// 
			// utf16TextSettingsLabel
			// 
			this.utf16TextSettingsLabel.AutoSize = true;
			this.utf16TextSettingsLabel.Location = new System.Drawing.Point(254, 211);
			this.utf16TextSettingsLabel.Name = "utf16TextSettingsLabel";
			this.utf16TextSettingsLabel.Size = new System.Drawing.Size(43, 13);
			this.utf16TextSettingsLabel.TabIndex = 39;
			this.utf16TextSettingsLabel.Text = "UTF16:";
			// 
			// utf16TextTypeTextBox
			// 
			this.utf16TextTypeTextBox.Location = new System.Drawing.Point(346, 208);
			this.utf16TextTypeTextBox.Name = "utf16TextTypeTextBox";
			this.utf16TextTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf16TextTypeTextBox.TabIndex = 38;
			// 
			// utf8TextSettingsLabel
			// 
			this.utf8TextSettingsLabel.AutoSize = true;
			this.utf8TextSettingsLabel.Location = new System.Drawing.Point(254, 189);
			this.utf8TextSettingsLabel.Name = "utf8TextSettingsLabel";
			this.utf8TextSettingsLabel.Size = new System.Drawing.Size(37, 13);
			this.utf8TextSettingsLabel.TabIndex = 35;
			this.utf8TextSettingsLabel.Text = "UTF8:";
			// 
			// utf8TextTypeTextBox
			// 
			this.utf8TextTypeTextBox.Location = new System.Drawing.Point(346, 186);
			this.utf8TextTypeTextBox.Name = "utf8TextTypeTextBox";
			this.utf8TextTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.utf8TextTypeTextBox.TabIndex = 34;
			// 
			// matrix3x3SettingsLabel
			// 
			this.matrix3x3SettingsLabel.AutoSize = true;
			this.matrix3x3SettingsLabel.Location = new System.Drawing.Point(254, 123);
			this.matrix3x3SettingsLabel.Name = "matrix3x3SettingsLabel";
			this.matrix3x3SettingsLabel.Size = new System.Drawing.Size(64, 13);
			this.matrix3x3SettingsLabel.TabIndex = 33;
			this.matrix3x3SettingsLabel.Text = "Matrix (3x3):";
			// 
			// matrix3x3TypeTextBox
			// 
			this.matrix3x3TypeTextBox.Location = new System.Drawing.Point(346, 120);
			this.matrix3x3TypeTextBox.Name = "matrix3x3TypeTextBox";
			this.matrix3x3TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix3x3TypeTextBox.TabIndex = 32;
			// 
			// matrix3x4SettingsLabel
			// 
			this.matrix3x4SettingsLabel.AutoSize = true;
			this.matrix3x4SettingsLabel.Location = new System.Drawing.Point(254, 145);
			this.matrix3x4SettingsLabel.Name = "matrix3x4SettingsLabel";
			this.matrix3x4SettingsLabel.Size = new System.Drawing.Size(64, 13);
			this.matrix3x4SettingsLabel.TabIndex = 31;
			this.matrix3x4SettingsLabel.Text = "Matrix (3x4):";
			// 
			// matrix3x4TypeTextBox
			// 
			this.matrix3x4TypeTextBox.Location = new System.Drawing.Point(346, 142);
			this.matrix3x4TypeTextBox.Name = "matrix3x4TypeTextBox";
			this.matrix3x4TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix3x4TypeTextBox.TabIndex = 30;
			// 
			// matrix4x4SettingsLabel
			// 
			this.matrix4x4SettingsLabel.AutoSize = true;
			this.matrix4x4SettingsLabel.Location = new System.Drawing.Point(254, 167);
			this.matrix4x4SettingsLabel.Name = "matrix4x4SettingsLabel";
			this.matrix4x4SettingsLabel.Size = new System.Drawing.Size(64, 13);
			this.matrix4x4SettingsLabel.TabIndex = 29;
			this.matrix4x4SettingsLabel.Text = "Matrix (4x4):";
			// 
			// matrix4x4TypeTextBox
			// 
			this.matrix4x4TypeTextBox.Location = new System.Drawing.Point(346, 164);
			this.matrix4x4TypeTextBox.Name = "matrix4x4TypeTextBox";
			this.matrix4x4TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.matrix4x4TypeTextBox.TabIndex = 28;
			// 
			// vector2SettingsLabel
			// 
			this.vector2SettingsLabel.AutoSize = true;
			this.vector2SettingsLabel.Location = new System.Drawing.Point(254, 57);
			this.vector2SettingsLabel.Name = "vector2SettingsLabel";
			this.vector2SettingsLabel.Size = new System.Drawing.Size(47, 13);
			this.vector2SettingsLabel.TabIndex = 27;
			this.vector2SettingsLabel.Text = "Vector2:";
			// 
			// vector2TypeTextBox
			// 
			this.vector2TypeTextBox.Location = new System.Drawing.Point(346, 54);
			this.vector2TypeTextBox.Name = "vector2TypeTextBox";
			this.vector2TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.vector2TypeTextBox.TabIndex = 26;
			// 
			// vector3SettingsLabel
			// 
			this.vector3SettingsLabel.AutoSize = true;
			this.vector3SettingsLabel.Location = new System.Drawing.Point(254, 79);
			this.vector3SettingsLabel.Name = "vector3SettingsLabel";
			this.vector3SettingsLabel.Size = new System.Drawing.Size(47, 13);
			this.vector3SettingsLabel.TabIndex = 25;
			this.vector3SettingsLabel.Text = "Vector3:";
			// 
			// vector3TypeTextBox
			// 
			this.vector3TypeTextBox.Location = new System.Drawing.Point(346, 76);
			this.vector3TypeTextBox.Name = "vector3TypeTextBox";
			this.vector3TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.vector3TypeTextBox.TabIndex = 24;
			// 
			// vector4SettingsLabel
			// 
			this.vector4SettingsLabel.AutoSize = true;
			this.vector4SettingsLabel.Location = new System.Drawing.Point(254, 101);
			this.vector4SettingsLabel.Name = "vector4SettingsLabel";
			this.vector4SettingsLabel.Size = new System.Drawing.Size(47, 13);
			this.vector4SettingsLabel.TabIndex = 23;
			this.vector4SettingsLabel.Text = "Vector4:";
			// 
			// vector4TypeTextBox
			// 
			this.vector4TypeTextBox.Location = new System.Drawing.Point(346, 98);
			this.vector4TypeTextBox.Name = "vector4TypeTextBox";
			this.vector4TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.vector4TypeTextBox.TabIndex = 22;
			// 
			// doubleSettingsLabel
			// 
			this.doubleSettingsLabel.AutoSize = true;
			this.doubleSettingsLabel.Location = new System.Drawing.Point(6, 233);
			this.doubleSettingsLabel.Name = "doubleSettingsLabel";
			this.doubleSettingsLabel.Size = new System.Drawing.Size(44, 13);
			this.doubleSettingsLabel.TabIndex = 21;
			this.doubleSettingsLabel.Text = "Double:";
			// 
			// doubleTypeTextBox
			// 
			this.doubleTypeTextBox.Location = new System.Drawing.Point(98, 230);
			this.doubleTypeTextBox.Name = "doubleTypeTextBox";
			this.doubleTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.doubleTypeTextBox.TabIndex = 20;
			// 
			// floatSettingsLabel
			// 
			this.floatSettingsLabel.AutoSize = true;
			this.floatSettingsLabel.Location = new System.Drawing.Point(6, 211);
			this.floatSettingsLabel.Name = "floatSettingsLabel";
			this.floatSettingsLabel.Size = new System.Drawing.Size(33, 13);
			this.floatSettingsLabel.TabIndex = 19;
			this.floatSettingsLabel.Text = "Float:";
			// 
			// floatTypeTextBox
			// 
			this.floatTypeTextBox.Location = new System.Drawing.Point(98, 208);
			this.floatTypeTextBox.Name = "floatTypeTextBox";
			this.floatTypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.floatTypeTextBox.TabIndex = 18;
			// 
			// uint64SettingsLabel
			// 
			this.uint64SettingsLabel.AutoSize = true;
			this.uint64SettingsLabel.Location = new System.Drawing.Point(6, 189);
			this.uint64SettingsLabel.Name = "uint64SettingsLabel";
			this.uint64SettingsLabel.Size = new System.Drawing.Size(42, 13);
			this.uint64SettingsLabel.TabIndex = 17;
			this.uint64SettingsLabel.Text = "UInt64:";
			// 
			// uint64TypeTextBox
			// 
			this.uint64TypeTextBox.Location = new System.Drawing.Point(98, 186);
			this.uint64TypeTextBox.Name = "uint64TypeTextBox";
			this.uint64TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.uint64TypeTextBox.TabIndex = 16;
			// 
			// uint32SettingsLabel
			// 
			this.uint32SettingsLabel.AutoSize = true;
			this.uint32SettingsLabel.Location = new System.Drawing.Point(6, 167);
			this.uint32SettingsLabel.Name = "uint32SettingsLabel";
			this.uint32SettingsLabel.Size = new System.Drawing.Size(42, 13);
			this.uint32SettingsLabel.TabIndex = 15;
			this.uint32SettingsLabel.Text = "UInt32:";
			// 
			// uint32TypeTextBox
			// 
			this.uint32TypeTextBox.Location = new System.Drawing.Point(98, 164);
			this.uint32TypeTextBox.Name = "uint32TypeTextBox";
			this.uint32TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.uint32TypeTextBox.TabIndex = 14;
			// 
			// uint16SettingsLabel
			// 
			this.uint16SettingsLabel.AutoSize = true;
			this.uint16SettingsLabel.Location = new System.Drawing.Point(6, 145);
			this.uint16SettingsLabel.Name = "uint16SettingsLabel";
			this.uint16SettingsLabel.Size = new System.Drawing.Size(42, 13);
			this.uint16SettingsLabel.TabIndex = 13;
			this.uint16SettingsLabel.Text = "UInt16:";
			// 
			// uint16TypeTextBox
			// 
			this.uint16TypeTextBox.Location = new System.Drawing.Point(98, 142);
			this.uint16TypeTextBox.Name = "uint16TypeTextBox";
			this.uint16TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.uint16TypeTextBox.TabIndex = 12;
			// 
			// uint8SettingsLabel
			// 
			this.uint8SettingsLabel.AutoSize = true;
			this.uint8SettingsLabel.Location = new System.Drawing.Point(6, 123);
			this.uint8SettingsLabel.Name = "uint8SettingsLabel";
			this.uint8SettingsLabel.Size = new System.Drawing.Size(36, 13);
			this.uint8SettingsLabel.TabIndex = 11;
			this.uint8SettingsLabel.Text = "UInt8:";
			// 
			// uint8TypeTextBox
			// 
			this.uint8TypeTextBox.Location = new System.Drawing.Point(98, 120);
			this.uint8TypeTextBox.Name = "uint8TypeTextBox";
			this.uint8TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.uint8TypeTextBox.TabIndex = 10;
			// 
			// int64SettingsLabel
			// 
			this.int64SettingsLabel.AutoSize = true;
			this.int64SettingsLabel.Location = new System.Drawing.Point(6, 101);
			this.int64SettingsLabel.Name = "int64SettingsLabel";
			this.int64SettingsLabel.Size = new System.Drawing.Size(34, 13);
			this.int64SettingsLabel.TabIndex = 9;
			this.int64SettingsLabel.Text = "Int64:";
			// 
			// int64TypeTextBox
			// 
			this.int64TypeTextBox.Location = new System.Drawing.Point(98, 98);
			this.int64TypeTextBox.Name = "int64TypeTextBox";
			this.int64TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.int64TypeTextBox.TabIndex = 8;
			// 
			// int32SettingsLabel
			// 
			this.int32SettingsLabel.AutoSize = true;
			this.int32SettingsLabel.Location = new System.Drawing.Point(6, 79);
			this.int32SettingsLabel.Name = "int32SettingsLabel";
			this.int32SettingsLabel.Size = new System.Drawing.Size(34, 13);
			this.int32SettingsLabel.TabIndex = 7;
			this.int32SettingsLabel.Text = "Int32:";
			// 
			// int32TypeTextBox
			// 
			this.int32TypeTextBox.Location = new System.Drawing.Point(98, 76);
			this.int32TypeTextBox.Name = "int32TypeTextBox";
			this.int32TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.int32TypeTextBox.TabIndex = 6;
			// 
			// int16SettingsLabel
			// 
			this.int16SettingsLabel.AutoSize = true;
			this.int16SettingsLabel.Location = new System.Drawing.Point(6, 57);
			this.int16SettingsLabel.Name = "int16SettingsLabel";
			this.int16SettingsLabel.Size = new System.Drawing.Size(34, 13);
			this.int16SettingsLabel.TabIndex = 5;
			this.int16SettingsLabel.Text = "Int16:";
			// 
			// int16TypeTextBox
			// 
			this.int16TypeTextBox.Location = new System.Drawing.Point(98, 54);
			this.int16TypeTextBox.Name = "int16TypeTextBox";
			this.int16TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.int16TypeTextBox.TabIndex = 4;
			// 
			// int8SettingsLabel
			// 
			this.int8SettingsLabel.AutoSize = true;
			this.int8SettingsLabel.Location = new System.Drawing.Point(6, 35);
			this.int8SettingsLabel.Name = "int8SettingsLabel";
			this.int8SettingsLabel.Size = new System.Drawing.Size(28, 13);
			this.int8SettingsLabel.TabIndex = 3;
			this.int8SettingsLabel.Text = "Int8:";
			// 
			// int8TypeTextBox
			// 
			this.int8TypeTextBox.Location = new System.Drawing.Point(98, 32);
			this.int8TypeTextBox.Name = "int8TypeTextBox";
			this.int8TypeTextBox.Size = new System.Drawing.Size(120, 20);
			this.int8TypeTextBox.TabIndex = 2;
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
			this.fileAssociationGroupBox.ResumeLayout(false);
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
		private System.Windows.Forms.CheckBox stayOnTopCheckBox;
		private System.Windows.Forms.GroupBox displayGroupBox;
		private System.Windows.Forms.CheckBox showNodeAddressCheckBox;
		private System.Windows.Forms.CheckBox showTextCheckBox;
		private System.Windows.Forms.CheckBox showNodeOffsetCheckBox;
		private System.Windows.Forms.CheckBox highlightChangedValuesCheckBox;
		private System.Windows.Forms.GroupBox commentsGroupBox;
		private System.Windows.Forms.CheckBox showRttiCheckBox;
		private System.Windows.Forms.CheckBox showPointerCheckBox;
		private System.Windows.Forms.CheckBox showIntegerCheckBox;
		private System.Windows.Forms.CheckBox showFloatCheckBox;
		private System.Windows.Forms.CheckBox showPluginInfoCheckBox;
		private System.Windows.Forms.CheckBox showStringCheckBox;
		private System.Windows.Forms.CheckBox showSymbolsCheckBox;
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
		private System.Windows.Forms.Label floatSettingsLabel;
		private System.Windows.Forms.TextBox floatTypeTextBox;
		private System.Windows.Forms.Label uint64SettingsLabel;
		private System.Windows.Forms.TextBox uint64TypeTextBox;
		private System.Windows.Forms.Label uint32SettingsLabel;
		private System.Windows.Forms.TextBox uint32TypeTextBox;
		private System.Windows.Forms.Label uint16SettingsLabel;
		private System.Windows.Forms.TextBox uint16TypeTextBox;
		private System.Windows.Forms.Label uint8SettingsLabel;
		private System.Windows.Forms.TextBox uint8TypeTextBox;
		private System.Windows.Forms.Label int64SettingsLabel;
		private System.Windows.Forms.TextBox int64TypeTextBox;
		private System.Windows.Forms.Label int32SettingsLabel;
		private System.Windows.Forms.TextBox int32TypeTextBox;
		private System.Windows.Forms.Label int16SettingsLabel;
		private System.Windows.Forms.TextBox int16TypeTextBox;
		private System.Windows.Forms.Label int8SettingsLabel;
		private System.Windows.Forms.TextBox int8TypeTextBox;
		private System.Windows.Forms.Label functionPtrSettingsLabel;
		private System.Windows.Forms.TextBox functionPtrTypeTextBox;
		private System.Windows.Forms.Label utf16TextSettingsLabel;
		private System.Windows.Forms.TextBox utf16TextTypeTextBox;
		private System.Windows.Forms.Label utf8TextSettingsLabel;
		private System.Windows.Forms.TextBox utf8TextTypeTextBox;
		private System.Windows.Forms.Label matrix3x3SettingsLabel;
		private System.Windows.Forms.TextBox matrix3x3TypeTextBox;
		private System.Windows.Forms.Label matrix3x4SettingsLabel;
		private System.Windows.Forms.TextBox matrix3x4TypeTextBox;
		private System.Windows.Forms.Label matrix4x4SettingsLabel;
		private System.Windows.Forms.TextBox matrix4x4TypeTextBox;
		private System.Windows.Forms.Label vector2SettingsLabel;
		private System.Windows.Forms.TextBox vector2TypeTextBox;
		private System.Windows.Forms.Label vector3SettingsLabel;
		private System.Windows.Forms.TextBox vector3TypeTextBox;
		private System.Windows.Forms.Label vector4SettingsLabel;
		private System.Windows.Forms.TextBox vector4TypeTextBox;
		private System.Windows.Forms.Label doubleSettingsLabel;
		private System.Windows.Forms.TextBox doubleTypeTextBox;
		private System.Windows.Forms.GroupBox nodeColorGroupBox;
		private System.Windows.Forms.Label label1;
		private UI.BannerBox bannerBox;
		private System.Windows.Forms.Label boolSettingsLabel;
		private System.Windows.Forms.TextBox boolTypeTextBox;
		private System.Windows.Forms.GroupBox fileAssociationGroupBox;
		private System.Windows.Forms.Button removeAssociationButton;
		private System.Windows.Forms.Button createAssociationButton;
		private System.Windows.Forms.Label associationInfoLabel;
		private System.Windows.Forms.CheckBox randomizeWindowTitleCheckBox;
		private System.Windows.Forms.CheckBox runAsAdminCheckBox;
	}
}