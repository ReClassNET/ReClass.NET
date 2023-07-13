using ReClassNET.Controls;

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
            this.nodeHexValueColorBox = new ReClassNET.Controls.ColorBox();
            this.nodePluginColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeHexValueLabel = new System.Windows.Forms.Label();
            this.nodeVTableLabel = new System.Windows.Forms.Label();
            this.nodeOffsetColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeVTableColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeOffsetLabel = new System.Windows.Forms.Label();
            this.nodeTextLabel = new System.Windows.Forms.Label();
            this.nodeAddressColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeTextColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeAddressLabel = new System.Windows.Forms.Label();
            this.nodeCommentLabel = new System.Windows.Forms.Label();
            this.nodeHiddenColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeCommentColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeHiddenLabel = new System.Windows.Forms.Label();
            this.nodeIndexLabel = new System.Windows.Forms.Label();
            this.nodeSelectedColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeIndexColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeSelectedLabel = new System.Windows.Forms.Label();
            this.nodeTypeColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeValueColorBox = new ReClassNET.Controls.ColorBox();
            this.nodeTypeLabel = new System.Windows.Forms.Label();
            this.nodeNameLabel = new System.Windows.Forms.Label();
            this.nodeNameColorBox = new ReClassNET.Controls.ColorBox();
            this.backgroundLabel = new System.Windows.Forms.Label();
            this.backgroundColorBox = new ReClassNET.Controls.ColorBox();
            this.typeDefinitionsSettingsTabPage = new System.Windows.Forms.TabPage();
            this.utf32TextSettingsLabel = new System.Windows.Forms.Label();
            this.utf32TextTypeTextBox = new System.Windows.Forms.TextBox();
            this.nuintSettingsLabel = new System.Windows.Forms.Label();
            this.nuintTypeTextBox = new System.Windows.Forms.TextBox();
            this.nintSettingsLabel = new System.Windows.Forms.Label();
            this.nintTypeTextBox = new System.Windows.Forms.TextBox();
            this.boolSettingsLabel = new System.Windows.Forms.Label();
            this.boolTypeTextBox = new System.Windows.Forms.TextBox();
            this.generatorInfoLabel = new System.Windows.Forms.Label();
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
            this.bannerBox = new ReClassNET.Controls.BannerBox();
            this.projectSettingTabPage = new System.Windows.Forms.TabPage();
            this.compressZipCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsTabControl.SuspendLayout();
            this.generalSettingsTabPage.SuspendLayout();
            this.fileAssociationGroupBox.SuspendLayout();
            this.commentsGroupBox.SuspendLayout();
            this.displayGroupBox.SuspendLayout();
            this.colorsSettingTabPage.SuspendLayout();
            this.nodeColorGroupBox.SuspendLayout();
            this.typeDefinitionsSettingsTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bannerBox)).BeginInit();
            this.projectSettingTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.generalSettingsTabPage);
            this.settingsTabControl.Controls.Add(this.colorsSettingTabPage);
            this.settingsTabControl.Controls.Add(this.typeDefinitionsSettingsTabPage);
            this.settingsTabControl.Controls.Add(this.projectSettingTabPage);
            this.settingsTabControl.Location = new System.Drawing.Point(16, 74);
            this.settingsTabControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(749, 437);
            this.settingsTabControl.TabIndex = 1;
            // 
            // generalSettingsTabPage
            // 
            this.generalSettingsTabPage.Controls.Add(this.fileAssociationGroupBox);
            this.generalSettingsTabPage.Controls.Add(this.commentsGroupBox);
            this.generalSettingsTabPage.Controls.Add(this.displayGroupBox);
            this.generalSettingsTabPage.Controls.Add(this.stayOnTopCheckBox);
            this.generalSettingsTabPage.Location = new System.Drawing.Point(4, 25);
            this.generalSettingsTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.generalSettingsTabPage.Name = "generalSettingsTabPage";
            this.generalSettingsTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.generalSettingsTabPage.Size = new System.Drawing.Size(741, 408);
            this.generalSettingsTabPage.TabIndex = 0;
            this.generalSettingsTabPage.Text = "General";
            this.generalSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // fileAssociationGroupBox
            // 
            this.fileAssociationGroupBox.Controls.Add(this.removeAssociationButton);
            this.fileAssociationGroupBox.Controls.Add(this.createAssociationButton);
            this.fileAssociationGroupBox.Controls.Add(this.associationInfoLabel);
            this.fileAssociationGroupBox.Location = new System.Drawing.Point(8, 284);
            this.fileAssociationGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileAssociationGroupBox.Name = "fileAssociationGroupBox";
            this.fileAssociationGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fileAssociationGroupBox.Size = new System.Drawing.Size(723, 105);
            this.fileAssociationGroupBox.TabIndex = 4;
            this.fileAssociationGroupBox.TabStop = false;
            this.fileAssociationGroupBox.Text = "RCNET File Association";
            // 
            // removeAssociationButton
            // 
            this.removeAssociationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.removeAssociationButton.Location = new System.Drawing.Point(195, 64);
            this.removeAssociationButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.removeAssociationButton.Name = "removeAssociationButton";
            this.removeAssociationButton.Size = new System.Drawing.Size(180, 28);
            this.removeAssociationButton.TabIndex = 2;
            this.removeAssociationButton.Text = "&Remove Association";
            this.removeAssociationButton.UseVisualStyleBackColor = true;
            this.removeAssociationButton.Click += new System.EventHandler(this.removeAssociationButton_Click);
            // 
            // createAssociationButton
            // 
            this.createAssociationButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.createAssociationButton.Location = new System.Drawing.Point(12, 64);
            this.createAssociationButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.createAssociationButton.Name = "createAssociationButton";
            this.createAssociationButton.Size = new System.Drawing.Size(175, 28);
            this.createAssociationButton.TabIndex = 1;
            this.createAssociationButton.Text = "Create &Association";
            this.createAssociationButton.UseVisualStyleBackColor = true;
            this.createAssociationButton.Click += new System.EventHandler(this.createAssociationButton_Click);
            // 
            // associationInfoLabel
            // 
            this.associationInfoLabel.Location = new System.Drawing.Point(8, 26);
            this.associationInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.associationInfoLabel.Name = "associationInfoLabel";
            this.associationInfoLabel.Size = new System.Drawing.Size(700, 34);
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
            this.commentsGroupBox.Location = new System.Drawing.Point(8, 48);
            this.commentsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.commentsGroupBox.Name = "commentsGroupBox";
            this.commentsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.commentsGroupBox.Size = new System.Drawing.Size(353, 229);
            this.commentsGroupBox.TabIndex = 3;
            this.commentsGroupBox.TabStop = false;
            this.commentsGroupBox.Text = "Node Comments";
            // 
            // showPluginInfoCheckBox
            // 
            this.showPluginInfoCheckBox.AutoSize = true;
            this.showPluginInfoCheckBox.Location = new System.Drawing.Point(8, 193);
            this.showPluginInfoCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showPluginInfoCheckBox.Name = "showPluginInfoCheckBox";
            this.showPluginInfoCheckBox.Size = new System.Drawing.Size(133, 20);
            this.showPluginInfoCheckBox.TabIndex = 6;
            this.showPluginInfoCheckBox.Text = "Show Plugin Infos";
            this.showPluginInfoCheckBox.UseVisualStyleBackColor = true;
            // 
            // showStringCheckBox
            // 
            this.showStringCheckBox.AutoSize = true;
            this.showStringCheckBox.Location = new System.Drawing.Point(8, 165);
            this.showStringCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showStringCheckBox.Name = "showStringCheckBox";
            this.showStringCheckBox.Size = new System.Drawing.Size(106, 20);
            this.showStringCheckBox.TabIndex = 5;
            this.showStringCheckBox.Text = "Show Strings";
            this.showStringCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSymbolsCheckBox
            // 
            this.showSymbolsCheckBox.AutoSize = true;
            this.showSymbolsCheckBox.Location = new System.Drawing.Point(8, 137);
            this.showSymbolsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showSymbolsCheckBox.Name = "showSymbolsCheckBox";
            this.showSymbolsCheckBox.Size = new System.Drawing.Size(162, 20);
            this.showSymbolsCheckBox.TabIndex = 4;
            this.showSymbolsCheckBox.Text = "Show Debug Symbols";
            this.showSymbolsCheckBox.UseVisualStyleBackColor = true;
            // 
            // showRttiCheckBox
            // 
            this.showRttiCheckBox.AutoSize = true;
            this.showRttiCheckBox.Location = new System.Drawing.Point(8, 108);
            this.showRttiCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showRttiCheckBox.Name = "showRttiCheckBox";
            this.showRttiCheckBox.Size = new System.Drawing.Size(96, 20);
            this.showRttiCheckBox.TabIndex = 3;
            this.showRttiCheckBox.Text = "Show RTTI";
            this.showRttiCheckBox.UseVisualStyleBackColor = true;
            // 
            // showPointerCheckBox
            // 
            this.showPointerCheckBox.AutoSize = true;
            this.showPointerCheckBox.Location = new System.Drawing.Point(8, 80);
            this.showPointerCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showPointerCheckBox.Name = "showPointerCheckBox";
            this.showPointerCheckBox.Size = new System.Drawing.Size(114, 20);
            this.showPointerCheckBox.TabIndex = 2;
            this.showPointerCheckBox.Text = "Show Pointers";
            this.showPointerCheckBox.UseVisualStyleBackColor = true;
            // 
            // showIntegerCheckBox
            // 
            this.showIntegerCheckBox.AutoSize = true;
            this.showIntegerCheckBox.Location = new System.Drawing.Point(8, 52);
            this.showIntegerCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showIntegerCheckBox.Name = "showIntegerCheckBox";
            this.showIntegerCheckBox.Size = new System.Drawing.Size(151, 20);
            this.showIntegerCheckBox.TabIndex = 1;
            this.showIntegerCheckBox.Text = "Show Integer Values";
            this.showIntegerCheckBox.UseVisualStyleBackColor = true;
            // 
            // showFloatCheckBox
            // 
            this.showFloatCheckBox.AutoSize = true;
            this.showFloatCheckBox.Location = new System.Drawing.Point(8, 23);
            this.showFloatCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showFloatCheckBox.Name = "showFloatCheckBox";
            this.showFloatCheckBox.Size = new System.Drawing.Size(140, 20);
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
            this.displayGroupBox.Location = new System.Drawing.Point(377, 48);
            this.displayGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.displayGroupBox.Name = "displayGroupBox";
            this.displayGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.displayGroupBox.Size = new System.Drawing.Size(353, 197);
            this.displayGroupBox.TabIndex = 2;
            this.displayGroupBox.TabStop = false;
            this.displayGroupBox.Text = "Display";
            // 
            // randomizeWindowTitleCheckBox
            // 
            this.randomizeWindowTitleCheckBox.AutoSize = true;
            this.randomizeWindowTitleCheckBox.Location = new System.Drawing.Point(8, 165);
            this.randomizeWindowTitleCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.randomizeWindowTitleCheckBox.Name = "randomizeWindowTitleCheckBox";
            this.randomizeWindowTitleCheckBox.Size = new System.Drawing.Size(168, 20);
            this.randomizeWindowTitleCheckBox.TabIndex = 5;
            this.randomizeWindowTitleCheckBox.Text = "Randomize window title";
            this.randomizeWindowTitleCheckBox.UseVisualStyleBackColor = true;
            // 
            // runAsAdminCheckBox
            // 
            this.runAsAdminCheckBox.AutoSize = true;
            this.runAsAdminCheckBox.Location = new System.Drawing.Point(8, 137);
            this.runAsAdminCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.runAsAdminCheckBox.Name = "runAsAdminCheckBox";
            this.runAsAdminCheckBox.Size = new System.Drawing.Size(251, 20);
            this.runAsAdminCheckBox.TabIndex = 4;
            this.runAsAdminCheckBox.Text = "Run as administrator (requires restart)";
            this.runAsAdminCheckBox.UseVisualStyleBackColor = true;
            // 
            // highlightChangedValuesCheckBox
            // 
            this.highlightChangedValuesCheckBox.AutoSize = true;
            this.highlightChangedValuesCheckBox.Location = new System.Drawing.Point(8, 108);
            this.highlightChangedValuesCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.highlightChangedValuesCheckBox.Name = "highlightChangedValuesCheckBox";
            this.highlightChangedValuesCheckBox.Size = new System.Drawing.Size(184, 20);
            this.highlightChangedValuesCheckBox.TabIndex = 3;
            this.highlightChangedValuesCheckBox.Text = "Highlight Changed Values";
            this.highlightChangedValuesCheckBox.UseVisualStyleBackColor = true;
            // 
            // showTextCheckBox
            // 
            this.showTextCheckBox.AutoSize = true;
            this.showTextCheckBox.Location = new System.Drawing.Point(8, 80);
            this.showTextCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showTextCheckBox.Name = "showTextCheckBox";
            this.showTextCheckBox.Size = new System.Drawing.Size(204, 20);
            this.showTextCheckBox.TabIndex = 2;
            this.showTextCheckBox.Text = "Show Textual Representation";
            this.showTextCheckBox.UseVisualStyleBackColor = true;
            // 
            // showNodeOffsetCheckBox
            // 
            this.showNodeOffsetCheckBox.AutoSize = true;
            this.showNodeOffsetCheckBox.Location = new System.Drawing.Point(8, 52);
            this.showNodeOffsetCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showNodeOffsetCheckBox.Name = "showNodeOffsetCheckBox";
            this.showNodeOffsetCheckBox.Size = new System.Drawing.Size(136, 20);
            this.showNodeOffsetCheckBox.TabIndex = 1;
            this.showNodeOffsetCheckBox.Text = "Show Node Offset";
            this.showNodeOffsetCheckBox.UseVisualStyleBackColor = true;
            // 
            // showNodeAddressCheckBox
            // 
            this.showNodeAddressCheckBox.AutoSize = true;
            this.showNodeAddressCheckBox.Location = new System.Drawing.Point(8, 23);
            this.showNodeAddressCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.showNodeAddressCheckBox.Name = "showNodeAddressCheckBox";
            this.showNodeAddressCheckBox.Size = new System.Drawing.Size(153, 20);
            this.showNodeAddressCheckBox.TabIndex = 0;
            this.showNodeAddressCheckBox.Text = "Show Node Address";
            this.showNodeAddressCheckBox.UseVisualStyleBackColor = true;
            // 
            // stayOnTopCheckBox
            // 
            this.stayOnTopCheckBox.AutoSize = true;
            this.stayOnTopCheckBox.Location = new System.Drawing.Point(8, 7);
            this.stayOnTopCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.stayOnTopCheckBox.Name = "stayOnTopCheckBox";
            this.stayOnTopCheckBox.Size = new System.Drawing.Size(232, 20);
            this.stayOnTopCheckBox.TabIndex = 1;
            this.stayOnTopCheckBox.Text = "Force ReClass.NET to stay on top";
            this.stayOnTopCheckBox.UseVisualStyleBackColor = true;
            // 
            // colorsSettingTabPage
            // 
            this.colorsSettingTabPage.Controls.Add(this.nodeColorGroupBox);
            this.colorsSettingTabPage.Controls.Add(this.backgroundLabel);
            this.colorsSettingTabPage.Controls.Add(this.backgroundColorBox);
            this.colorsSettingTabPage.Location = new System.Drawing.Point(4, 25);
            this.colorsSettingTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.colorsSettingTabPage.Name = "colorsSettingTabPage";
            this.colorsSettingTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.colorsSettingTabPage.Size = new System.Drawing.Size(741, 408);
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
            this.nodeColorGroupBox.Location = new System.Drawing.Point(12, 53);
            this.nodeColorGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nodeColorGroupBox.Name = "nodeColorGroupBox";
            this.nodeColorGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nodeColorGroupBox.Size = new System.Drawing.Size(719, 277);
            this.nodeColorGroupBox.TabIndex = 28;
            this.nodeColorGroupBox.TabStop = false;
            this.nodeColorGroupBox.Text = "Node Colors";
            // 
            // nodeValueLabel
            // 
            this.nodeValueLabel.AutoSize = true;
            this.nodeValueLabel.Location = new System.Drawing.Point(12, 244);
            this.nodeValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeValueLabel.Name = "nodeValueLabel";
            this.nodeValueLabel.Size = new System.Drawing.Size(80, 16);
            this.nodeValueLabel.TabIndex = 17;
            this.nodeValueLabel.Text = "Value Color:";
            // 
            // nodePluginLabel
            // 
            this.nodePluginLabel.AutoSize = true;
            this.nodePluginLabel.Location = new System.Drawing.Point(381, 212);
            this.nodePluginLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodePluginLabel.Name = "nodePluginLabel";
            this.nodePluginLabel.Size = new System.Drawing.Size(106, 16);
            this.nodePluginLabel.TabIndex = 27;
            this.nodePluginLabel.Text = "Plugin Info Color:";
            // 
            // nodeHexValueColorBox
            // 
            this.nodeHexValueColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeHexValueColorBox.Location = new System.Drawing.Point(177, 144);
            this.nodeHexValueColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeHexValueColorBox.Name = "nodeHexValueColorBox";
            this.nodeHexValueColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeHexValueColorBox.TabIndex = 2;
            // 
            // nodePluginColorBox
            // 
            this.nodePluginColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodePluginColorBox.Location = new System.Drawing.Point(547, 208);
            this.nodePluginColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodePluginColorBox.Name = "nodePluginColorBox";
            this.nodePluginColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodePluginColorBox.TabIndex = 26;
            // 
            // nodeHexValueLabel
            // 
            this.nodeHexValueLabel.AutoSize = true;
            this.nodeHexValueLabel.Location = new System.Drawing.Point(12, 148);
            this.nodeHexValueLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeHexValueLabel.Name = "nodeHexValueLabel";
            this.nodeHexValueLabel.Size = new System.Drawing.Size(107, 16);
            this.nodeHexValueLabel.TabIndex = 3;
            this.nodeHexValueLabel.Text = "Hex Value Color:";
            // 
            // nodeVTableLabel
            // 
            this.nodeVTableLabel.AutoSize = true;
            this.nodeVTableLabel.Location = new System.Drawing.Point(381, 116);
            this.nodeVTableLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeVTableLabel.Name = "nodeVTableLabel";
            this.nodeVTableLabel.Size = new System.Drawing.Size(90, 16);
            this.nodeVTableLabel.TabIndex = 25;
            this.nodeVTableLabel.Text = "VTable Color:";
            // 
            // nodeOffsetColorBox
            // 
            this.nodeOffsetColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeOffsetColorBox.Location = new System.Drawing.Point(177, 112);
            this.nodeOffsetColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeOffsetColorBox.Name = "nodeOffsetColorBox";
            this.nodeOffsetColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeOffsetColorBox.TabIndex = 4;
            // 
            // nodeVTableColorBox
            // 
            this.nodeVTableColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeVTableColorBox.Location = new System.Drawing.Point(547, 112);
            this.nodeVTableColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeVTableColorBox.Name = "nodeVTableColorBox";
            this.nodeVTableColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeVTableColorBox.TabIndex = 24;
            // 
            // nodeOffsetLabel
            // 
            this.nodeOffsetLabel.AutoSize = true;
            this.nodeOffsetLabel.Location = new System.Drawing.Point(12, 116);
            this.nodeOffsetLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeOffsetLabel.Name = "nodeOffsetLabel";
            this.nodeOffsetLabel.Size = new System.Drawing.Size(79, 16);
            this.nodeOffsetLabel.TabIndex = 5;
            this.nodeOffsetLabel.Text = "Offset Color:";
            // 
            // nodeTextLabel
            // 
            this.nodeTextLabel.AutoSize = true;
            this.nodeTextLabel.Location = new System.Drawing.Point(381, 180);
            this.nodeTextLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeTextLabel.Name = "nodeTextLabel";
            this.nodeTextLabel.Size = new System.Drawing.Size(71, 16);
            this.nodeTextLabel.TabIndex = 23;
            this.nodeTextLabel.Text = "Text Color:";
            // 
            // nodeAddressColorBox
            // 
            this.nodeAddressColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeAddressColorBox.Location = new System.Drawing.Point(177, 80);
            this.nodeAddressColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeAddressColorBox.Name = "nodeAddressColorBox";
            this.nodeAddressColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeAddressColorBox.TabIndex = 6;
            // 
            // nodeTextColorBox
            // 
            this.nodeTextColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeTextColorBox.Location = new System.Drawing.Point(547, 176);
            this.nodeTextColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeTextColorBox.Name = "nodeTextColorBox";
            this.nodeTextColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeTextColorBox.TabIndex = 22;
            // 
            // nodeAddressLabel
            // 
            this.nodeAddressLabel.AutoSize = true;
            this.nodeAddressLabel.Location = new System.Drawing.Point(12, 84);
            this.nodeAddressLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeAddressLabel.Name = "nodeAddressLabel";
            this.nodeAddressLabel.Size = new System.Drawing.Size(96, 16);
            this.nodeAddressLabel.TabIndex = 7;
            this.nodeAddressLabel.Text = "Address Color:";
            // 
            // nodeCommentLabel
            // 
            this.nodeCommentLabel.AutoSize = true;
            this.nodeCommentLabel.Location = new System.Drawing.Point(381, 148);
            this.nodeCommentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeCommentLabel.Name = "nodeCommentLabel";
            this.nodeCommentLabel.Size = new System.Drawing.Size(102, 16);
            this.nodeCommentLabel.TabIndex = 21;
            this.nodeCommentLabel.Text = "Comment Color:";
            // 
            // nodeHiddenColorBox
            // 
            this.nodeHiddenColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeHiddenColorBox.Location = new System.Drawing.Point(547, 22);
            this.nodeHiddenColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeHiddenColorBox.Name = "nodeHiddenColorBox";
            this.nodeHiddenColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeHiddenColorBox.TabIndex = 8;
            // 
            // nodeCommentColorBox
            // 
            this.nodeCommentColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeCommentColorBox.Location = new System.Drawing.Point(547, 144);
            this.nodeCommentColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeCommentColorBox.Name = "nodeCommentColorBox";
            this.nodeCommentColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeCommentColorBox.TabIndex = 20;
            // 
            // nodeHiddenLabel
            // 
            this.nodeHiddenLabel.AutoSize = true;
            this.nodeHiddenLabel.Location = new System.Drawing.Point(381, 26);
            this.nodeHiddenLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeHiddenLabel.Name = "nodeHiddenLabel";
            this.nodeHiddenLabel.Size = new System.Drawing.Size(89, 16);
            this.nodeHiddenLabel.TabIndex = 9;
            this.nodeHiddenLabel.Text = "Hidden Color:";
            // 
            // nodeIndexLabel
            // 
            this.nodeIndexLabel.AutoSize = true;
            this.nodeIndexLabel.Location = new System.Drawing.Point(381, 84);
            this.nodeIndexLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeIndexLabel.Name = "nodeIndexLabel";
            this.nodeIndexLabel.Size = new System.Drawing.Size(77, 16);
            this.nodeIndexLabel.TabIndex = 19;
            this.nodeIndexLabel.Text = "Index Color:";
            // 
            // nodeSelectedColorBox
            // 
            this.nodeSelectedColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeSelectedColorBox.Location = new System.Drawing.Point(177, 22);
            this.nodeSelectedColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeSelectedColorBox.Name = "nodeSelectedColorBox";
            this.nodeSelectedColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeSelectedColorBox.TabIndex = 10;
            // 
            // nodeIndexColorBox
            // 
            this.nodeIndexColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeIndexColorBox.Location = new System.Drawing.Point(547, 80);
            this.nodeIndexColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeIndexColorBox.Name = "nodeIndexColorBox";
            this.nodeIndexColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeIndexColorBox.TabIndex = 18;
            // 
            // nodeSelectedLabel
            // 
            this.nodeSelectedLabel.AutoSize = true;
            this.nodeSelectedLabel.Location = new System.Drawing.Point(12, 26);
            this.nodeSelectedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeSelectedLabel.Name = "nodeSelectedLabel";
            this.nodeSelectedLabel.Size = new System.Drawing.Size(99, 16);
            this.nodeSelectedLabel.TabIndex = 11;
            this.nodeSelectedLabel.Text = "Selected Color:";
            // 
            // nodeTypeColorBox
            // 
            this.nodeTypeColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeTypeColorBox.Location = new System.Drawing.Point(177, 176);
            this.nodeTypeColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeTypeColorBox.Name = "nodeTypeColorBox";
            this.nodeTypeColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeTypeColorBox.TabIndex = 12;
            // 
            // nodeValueColorBox
            // 
            this.nodeValueColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeValueColorBox.Location = new System.Drawing.Point(177, 240);
            this.nodeValueColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeValueColorBox.Name = "nodeValueColorBox";
            this.nodeValueColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeValueColorBox.TabIndex = 16;
            // 
            // nodeTypeLabel
            // 
            this.nodeTypeLabel.AutoSize = true;
            this.nodeTypeLabel.Location = new System.Drawing.Point(12, 180);
            this.nodeTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeTypeLabel.Name = "nodeTypeLabel";
            this.nodeTypeLabel.Size = new System.Drawing.Size(77, 16);
            this.nodeTypeLabel.TabIndex = 13;
            this.nodeTypeLabel.Text = "Type Color:";
            // 
            // nodeNameLabel
            // 
            this.nodeNameLabel.AutoSize = true;
            this.nodeNameLabel.Location = new System.Drawing.Point(12, 212);
            this.nodeNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nodeNameLabel.Name = "nodeNameLabel";
            this.nodeNameLabel.Size = new System.Drawing.Size(82, 16);
            this.nodeNameLabel.TabIndex = 15;
            this.nodeNameLabel.Text = "Name Color:";
            // 
            // nodeNameColorBox
            // 
            this.nodeNameColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.nodeNameColorBox.Location = new System.Drawing.Point(177, 208);
            this.nodeNameColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.nodeNameColorBox.Name = "nodeNameColorBox";
            this.nodeNameColorBox.Size = new System.Drawing.Size(123, 20);
            this.nodeNameColorBox.TabIndex = 14;
            // 
            // backgroundLabel
            // 
            this.backgroundLabel.AutoSize = true;
            this.backgroundLabel.Location = new System.Drawing.Point(8, 17);
            this.backgroundLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.backgroundLabel.Name = "backgroundLabel";
            this.backgroundLabel.Size = new System.Drawing.Size(202, 16);
            this.backgroundLabel.TabIndex = 1;
            this.backgroundLabel.Text = "Memory View Background Color:";
            // 
            // backgroundColorBox
            // 
            this.backgroundColorBox.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.backgroundColorBox.Location = new System.Drawing.Point(233, 14);
            this.backgroundColorBox.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.backgroundColorBox.Name = "backgroundColorBox";
            this.backgroundColorBox.Size = new System.Drawing.Size(123, 20);
            this.backgroundColorBox.TabIndex = 0;
            // 
            // typeDefinitionsSettingsTabPage
            // 
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf32TextSettingsLabel);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.utf32TextTypeTextBox);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.nuintSettingsLabel);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.nuintTypeTextBox);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.nintSettingsLabel);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.nintTypeTextBox);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.boolSettingsLabel);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.boolTypeTextBox);
            this.typeDefinitionsSettingsTabPage.Controls.Add(this.generatorInfoLabel);
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
            this.typeDefinitionsSettingsTabPage.Location = new System.Drawing.Point(4, 25);
            this.typeDefinitionsSettingsTabPage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.typeDefinitionsSettingsTabPage.Name = "typeDefinitionsSettingsTabPage";
            this.typeDefinitionsSettingsTabPage.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.typeDefinitionsSettingsTabPage.Size = new System.Drawing.Size(741, 408);
            this.typeDefinitionsSettingsTabPage.TabIndex = 2;
            this.typeDefinitionsSettingsTabPage.Text = "Type Definitions";
            this.typeDefinitionsSettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // utf32TextSettingsLabel
            // 
            this.utf32TextSettingsLabel.AutoSize = true;
            this.utf32TextSettingsLabel.Location = new System.Drawing.Point(339, 287);
            this.utf32TextSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.utf32TextSettingsLabel.Name = "utf32TextSettingsLabel";
            this.utf32TextSettingsLabel.Size = new System.Drawing.Size(51, 16);
            this.utf32TextSettingsLabel.TabIndex = 52;
            this.utf32TextSettingsLabel.Text = "UTF32:";
            // 
            // utf32TextTypeTextBox
            // 
            this.utf32TextTypeTextBox.Location = new System.Drawing.Point(461, 283);
            this.utf32TextTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.utf32TextTypeTextBox.Name = "utf32TextTypeTextBox";
            this.utf32TextTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.utf32TextTypeTextBox.TabIndex = 51;
            // 
            // nuintSettingsLabel
            // 
            this.nuintSettingsLabel.AutoSize = true;
            this.nuintSettingsLabel.Location = new System.Drawing.Point(8, 287);
            this.nuintSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nuintSettingsLabel.Name = "nuintSettingsLabel";
            this.nuintSettingsLabel.Size = new System.Drawing.Size(43, 16);
            this.nuintSettingsLabel.TabIndex = 50;
            this.nuintSettingsLabel.Text = "NUInt:";
            // 
            // nuintTypeTextBox
            // 
            this.nuintTypeTextBox.Location = new System.Drawing.Point(131, 283);
            this.nuintTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nuintTypeTextBox.Name = "nuintTypeTextBox";
            this.nuintTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.nuintTypeTextBox.TabIndex = 49;
            // 
            // nintSettingsLabel
            // 
            this.nintSettingsLabel.AutoSize = true;
            this.nintSettingsLabel.Location = new System.Drawing.Point(8, 151);
            this.nintSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.nintSettingsLabel.Name = "nintSettingsLabel";
            this.nintSettingsLabel.Size = new System.Drawing.Size(33, 16);
            this.nintSettingsLabel.TabIndex = 48;
            this.nintSettingsLabel.Text = "NInt:";
            // 
            // nintTypeTextBox
            // 
            this.nintTypeTextBox.Location = new System.Drawing.Point(131, 148);
            this.nintTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.nintTypeTextBox.Name = "nintTypeTextBox";
            this.nintTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.nintTypeTextBox.TabIndex = 47;
            // 
            // boolSettingsLabel
            // 
            this.boolSettingsLabel.AutoSize = true;
            this.boolSettingsLabel.Location = new System.Drawing.Point(339, 43);
            this.boolSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.boolSettingsLabel.Name = "boolSettingsLabel";
            this.boolSettingsLabel.Size = new System.Drawing.Size(38, 16);
            this.boolSettingsLabel.TabIndex = 46;
            this.boolSettingsLabel.Text = "Bool:";
            // 
            // boolTypeTextBox
            // 
            this.boolTypeTextBox.Location = new System.Drawing.Point(461, 39);
            this.boolTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.boolTypeTextBox.Name = "boolTypeTextBox";
            this.boolTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.boolTypeTextBox.TabIndex = 45;
            // 
            // generatorInfoLabel
            // 
            this.generatorInfoLabel.AutoSize = true;
            this.generatorInfoLabel.Location = new System.Drawing.Point(8, 7);
            this.generatorInfoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.generatorInfoLabel.Name = "generatorInfoLabel";
            this.generatorInfoLabel.Size = new System.Drawing.Size(293, 16);
            this.generatorInfoLabel.TabIndex = 44;
            this.generatorInfoLabel.Text = "These types are used to generate the C++ code:";
            // 
            // functionPtrSettingsLabel
            // 
            this.functionPtrSettingsLabel.AutoSize = true;
            this.functionPtrSettingsLabel.Location = new System.Drawing.Point(339, 314);
            this.functionPtrSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.functionPtrSettingsLabel.Name = "functionPtrSettingsLabel";
            this.functionPtrSettingsLabel.Size = new System.Drawing.Size(105, 16);
            this.functionPtrSettingsLabel.TabIndex = 43;
            this.functionPtrSettingsLabel.Text = "Function Pointer:";
            // 
            // functionPtrTypeTextBox
            // 
            this.functionPtrTypeTextBox.Location = new System.Drawing.Point(461, 310);
            this.functionPtrTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.functionPtrTypeTextBox.Name = "functionPtrTypeTextBox";
            this.functionPtrTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.functionPtrTypeTextBox.TabIndex = 42;
            // 
            // utf16TextSettingsLabel
            // 
            this.utf16TextSettingsLabel.AutoSize = true;
            this.utf16TextSettingsLabel.Location = new System.Drawing.Point(339, 260);
            this.utf16TextSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.utf16TextSettingsLabel.Name = "utf16TextSettingsLabel";
            this.utf16TextSettingsLabel.Size = new System.Drawing.Size(51, 16);
            this.utf16TextSettingsLabel.TabIndex = 39;
            this.utf16TextSettingsLabel.Text = "UTF16:";
            // 
            // utf16TextTypeTextBox
            // 
            this.utf16TextTypeTextBox.Location = new System.Drawing.Point(461, 256);
            this.utf16TextTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.utf16TextTypeTextBox.Name = "utf16TextTypeTextBox";
            this.utf16TextTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.utf16TextTypeTextBox.TabIndex = 38;
            // 
            // utf8TextSettingsLabel
            // 
            this.utf8TextSettingsLabel.AutoSize = true;
            this.utf8TextSettingsLabel.Location = new System.Drawing.Point(339, 233);
            this.utf8TextSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.utf8TextSettingsLabel.Name = "utf8TextSettingsLabel";
            this.utf8TextSettingsLabel.Size = new System.Drawing.Size(44, 16);
            this.utf8TextSettingsLabel.TabIndex = 35;
            this.utf8TextSettingsLabel.Text = "UTF8:";
            // 
            // utf8TextTypeTextBox
            // 
            this.utf8TextTypeTextBox.Location = new System.Drawing.Point(461, 229);
            this.utf8TextTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.utf8TextTypeTextBox.Name = "utf8TextTypeTextBox";
            this.utf8TextTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.utf8TextTypeTextBox.TabIndex = 34;
            // 
            // matrix3x3SettingsLabel
            // 
            this.matrix3x3SettingsLabel.AutoSize = true;
            this.matrix3x3SettingsLabel.Location = new System.Drawing.Point(339, 151);
            this.matrix3x3SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.matrix3x3SettingsLabel.Name = "matrix3x3SettingsLabel";
            this.matrix3x3SettingsLabel.Size = new System.Drawing.Size(76, 16);
            this.matrix3x3SettingsLabel.TabIndex = 33;
            this.matrix3x3SettingsLabel.Text = "Matrix (3x3):";
            // 
            // matrix3x3TypeTextBox
            // 
            this.matrix3x3TypeTextBox.Location = new System.Drawing.Point(461, 148);
            this.matrix3x3TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.matrix3x3TypeTextBox.Name = "matrix3x3TypeTextBox";
            this.matrix3x3TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.matrix3x3TypeTextBox.TabIndex = 32;
            // 
            // matrix3x4SettingsLabel
            // 
            this.matrix3x4SettingsLabel.AutoSize = true;
            this.matrix3x4SettingsLabel.Location = new System.Drawing.Point(339, 178);
            this.matrix3x4SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.matrix3x4SettingsLabel.Name = "matrix3x4SettingsLabel";
            this.matrix3x4SettingsLabel.Size = new System.Drawing.Size(76, 16);
            this.matrix3x4SettingsLabel.TabIndex = 31;
            this.matrix3x4SettingsLabel.Text = "Matrix (3x4):";
            // 
            // matrix3x4TypeTextBox
            // 
            this.matrix3x4TypeTextBox.Location = new System.Drawing.Point(461, 175);
            this.matrix3x4TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.matrix3x4TypeTextBox.Name = "matrix3x4TypeTextBox";
            this.matrix3x4TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.matrix3x4TypeTextBox.TabIndex = 30;
            // 
            // matrix4x4SettingsLabel
            // 
            this.matrix4x4SettingsLabel.AutoSize = true;
            this.matrix4x4SettingsLabel.Location = new System.Drawing.Point(339, 206);
            this.matrix4x4SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.matrix4x4SettingsLabel.Name = "matrix4x4SettingsLabel";
            this.matrix4x4SettingsLabel.Size = new System.Drawing.Size(76, 16);
            this.matrix4x4SettingsLabel.TabIndex = 29;
            this.matrix4x4SettingsLabel.Text = "Matrix (4x4):";
            // 
            // matrix4x4TypeTextBox
            // 
            this.matrix4x4TypeTextBox.Location = new System.Drawing.Point(461, 202);
            this.matrix4x4TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.matrix4x4TypeTextBox.Name = "matrix4x4TypeTextBox";
            this.matrix4x4TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.matrix4x4TypeTextBox.TabIndex = 28;
            // 
            // vector2SettingsLabel
            // 
            this.vector2SettingsLabel.AutoSize = true;
            this.vector2SettingsLabel.Location = new System.Drawing.Point(339, 70);
            this.vector2SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vector2SettingsLabel.Name = "vector2SettingsLabel";
            this.vector2SettingsLabel.Size = new System.Drawing.Size(56, 16);
            this.vector2SettingsLabel.TabIndex = 27;
            this.vector2SettingsLabel.Text = "Vector2:";
            // 
            // vector2TypeTextBox
            // 
            this.vector2TypeTextBox.Location = new System.Drawing.Point(461, 66);
            this.vector2TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vector2TypeTextBox.Name = "vector2TypeTextBox";
            this.vector2TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.vector2TypeTextBox.TabIndex = 26;
            // 
            // vector3SettingsLabel
            // 
            this.vector3SettingsLabel.AutoSize = true;
            this.vector3SettingsLabel.Location = new System.Drawing.Point(339, 97);
            this.vector3SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vector3SettingsLabel.Name = "vector3SettingsLabel";
            this.vector3SettingsLabel.Size = new System.Drawing.Size(56, 16);
            this.vector3SettingsLabel.TabIndex = 25;
            this.vector3SettingsLabel.Text = "Vector3:";
            // 
            // vector3TypeTextBox
            // 
            this.vector3TypeTextBox.Location = new System.Drawing.Point(461, 94);
            this.vector3TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vector3TypeTextBox.Name = "vector3TypeTextBox";
            this.vector3TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.vector3TypeTextBox.TabIndex = 24;
            // 
            // vector4SettingsLabel
            // 
            this.vector4SettingsLabel.AutoSize = true;
            this.vector4SettingsLabel.Location = new System.Drawing.Point(339, 124);
            this.vector4SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.vector4SettingsLabel.Name = "vector4SettingsLabel";
            this.vector4SettingsLabel.Size = new System.Drawing.Size(56, 16);
            this.vector4SettingsLabel.TabIndex = 23;
            this.vector4SettingsLabel.Text = "Vector4:";
            // 
            // vector4TypeTextBox
            // 
            this.vector4TypeTextBox.Location = new System.Drawing.Point(461, 121);
            this.vector4TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vector4TypeTextBox.Name = "vector4TypeTextBox";
            this.vector4TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.vector4TypeTextBox.TabIndex = 22;
            // 
            // doubleSettingsLabel
            // 
            this.doubleSettingsLabel.AutoSize = true;
            this.doubleSettingsLabel.Location = new System.Drawing.Point(8, 341);
            this.doubleSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.doubleSettingsLabel.Name = "doubleSettingsLabel";
            this.doubleSettingsLabel.Size = new System.Drawing.Size(54, 16);
            this.doubleSettingsLabel.TabIndex = 21;
            this.doubleSettingsLabel.Text = "Double:";
            // 
            // doubleTypeTextBox
            // 
            this.doubleTypeTextBox.Location = new System.Drawing.Point(131, 337);
            this.doubleTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.doubleTypeTextBox.Name = "doubleTypeTextBox";
            this.doubleTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.doubleTypeTextBox.TabIndex = 20;
            // 
            // floatSettingsLabel
            // 
            this.floatSettingsLabel.AutoSize = true;
            this.floatSettingsLabel.Location = new System.Drawing.Point(8, 314);
            this.floatSettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.floatSettingsLabel.Name = "floatSettingsLabel";
            this.floatSettingsLabel.Size = new System.Drawing.Size(40, 16);
            this.floatSettingsLabel.TabIndex = 19;
            this.floatSettingsLabel.Text = "Float:";
            // 
            // floatTypeTextBox
            // 
            this.floatTypeTextBox.Location = new System.Drawing.Point(131, 310);
            this.floatTypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.floatTypeTextBox.Name = "floatTypeTextBox";
            this.floatTypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.floatTypeTextBox.TabIndex = 18;
            // 
            // uint64SettingsLabel
            // 
            this.uint64SettingsLabel.AutoSize = true;
            this.uint64SettingsLabel.Location = new System.Drawing.Point(8, 260);
            this.uint64SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uint64SettingsLabel.Name = "uint64SettingsLabel";
            this.uint64SettingsLabel.Size = new System.Drawing.Size(47, 16);
            this.uint64SettingsLabel.TabIndex = 17;
            this.uint64SettingsLabel.Text = "UInt64:";
            // 
            // uint64TypeTextBox
            // 
            this.uint64TypeTextBox.Location = new System.Drawing.Point(131, 256);
            this.uint64TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uint64TypeTextBox.Name = "uint64TypeTextBox";
            this.uint64TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.uint64TypeTextBox.TabIndex = 16;
            // 
            // uint32SettingsLabel
            // 
            this.uint32SettingsLabel.AutoSize = true;
            this.uint32SettingsLabel.Location = new System.Drawing.Point(8, 233);
            this.uint32SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uint32SettingsLabel.Name = "uint32SettingsLabel";
            this.uint32SettingsLabel.Size = new System.Drawing.Size(47, 16);
            this.uint32SettingsLabel.TabIndex = 15;
            this.uint32SettingsLabel.Text = "UInt32:";
            // 
            // uint32TypeTextBox
            // 
            this.uint32TypeTextBox.Location = new System.Drawing.Point(131, 229);
            this.uint32TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uint32TypeTextBox.Name = "uint32TypeTextBox";
            this.uint32TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.uint32TypeTextBox.TabIndex = 14;
            // 
            // uint16SettingsLabel
            // 
            this.uint16SettingsLabel.AutoSize = true;
            this.uint16SettingsLabel.Location = new System.Drawing.Point(8, 206);
            this.uint16SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uint16SettingsLabel.Name = "uint16SettingsLabel";
            this.uint16SettingsLabel.Size = new System.Drawing.Size(47, 16);
            this.uint16SettingsLabel.TabIndex = 13;
            this.uint16SettingsLabel.Text = "UInt16:";
            // 
            // uint16TypeTextBox
            // 
            this.uint16TypeTextBox.Location = new System.Drawing.Point(131, 202);
            this.uint16TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uint16TypeTextBox.Name = "uint16TypeTextBox";
            this.uint16TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.uint16TypeTextBox.TabIndex = 12;
            // 
            // uint8SettingsLabel
            // 
            this.uint8SettingsLabel.AutoSize = true;
            this.uint8SettingsLabel.Location = new System.Drawing.Point(8, 178);
            this.uint8SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.uint8SettingsLabel.Name = "uint8SettingsLabel";
            this.uint8SettingsLabel.Size = new System.Drawing.Size(40, 16);
            this.uint8SettingsLabel.TabIndex = 11;
            this.uint8SettingsLabel.Text = "UInt8:";
            // 
            // uint8TypeTextBox
            // 
            this.uint8TypeTextBox.Location = new System.Drawing.Point(131, 175);
            this.uint8TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uint8TypeTextBox.Name = "uint8TypeTextBox";
            this.uint8TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.uint8TypeTextBox.TabIndex = 10;
            // 
            // int64SettingsLabel
            // 
            this.int64SettingsLabel.AutoSize = true;
            this.int64SettingsLabel.Location = new System.Drawing.Point(8, 124);
            this.int64SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.int64SettingsLabel.Name = "int64SettingsLabel";
            this.int64SettingsLabel.Size = new System.Drawing.Size(37, 16);
            this.int64SettingsLabel.TabIndex = 9;
            this.int64SettingsLabel.Text = "Int64:";
            // 
            // int64TypeTextBox
            // 
            this.int64TypeTextBox.Location = new System.Drawing.Point(131, 121);
            this.int64TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.int64TypeTextBox.Name = "int64TypeTextBox";
            this.int64TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.int64TypeTextBox.TabIndex = 8;
            // 
            // int32SettingsLabel
            // 
            this.int32SettingsLabel.AutoSize = true;
            this.int32SettingsLabel.Location = new System.Drawing.Point(8, 97);
            this.int32SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.int32SettingsLabel.Name = "int32SettingsLabel";
            this.int32SettingsLabel.Size = new System.Drawing.Size(37, 16);
            this.int32SettingsLabel.TabIndex = 7;
            this.int32SettingsLabel.Text = "Int32:";
            // 
            // int32TypeTextBox
            // 
            this.int32TypeTextBox.Location = new System.Drawing.Point(131, 94);
            this.int32TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.int32TypeTextBox.Name = "int32TypeTextBox";
            this.int32TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.int32TypeTextBox.TabIndex = 6;
            // 
            // int16SettingsLabel
            // 
            this.int16SettingsLabel.AutoSize = true;
            this.int16SettingsLabel.Location = new System.Drawing.Point(8, 70);
            this.int16SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.int16SettingsLabel.Name = "int16SettingsLabel";
            this.int16SettingsLabel.Size = new System.Drawing.Size(37, 16);
            this.int16SettingsLabel.TabIndex = 5;
            this.int16SettingsLabel.Text = "Int16:";
            // 
            // int16TypeTextBox
            // 
            this.int16TypeTextBox.Location = new System.Drawing.Point(131, 66);
            this.int16TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.int16TypeTextBox.Name = "int16TypeTextBox";
            this.int16TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.int16TypeTextBox.TabIndex = 4;
            // 
            // int8SettingsLabel
            // 
            this.int8SettingsLabel.AutoSize = true;
            this.int8SettingsLabel.Location = new System.Drawing.Point(8, 43);
            this.int8SettingsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.int8SettingsLabel.Name = "int8SettingsLabel";
            this.int8SettingsLabel.Size = new System.Drawing.Size(30, 16);
            this.int8SettingsLabel.TabIndex = 3;
            this.int8SettingsLabel.Text = "Int8:";
            // 
            // int8TypeTextBox
            // 
            this.int8TypeTextBox.Location = new System.Drawing.Point(131, 39);
            this.int8TypeTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.int8TypeTextBox.Name = "int8TypeTextBox";
            this.int8TypeTextBox.Size = new System.Drawing.Size(159, 22);
            this.int8TypeTextBox.TabIndex = 2;
            // 
            // bannerBox
            // 
            this.bannerBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.bannerBox.Icon = global::ReClassNET.Properties.Resources.B32x32_Cogs;
            this.bannerBox.Location = new System.Drawing.Point(0, 0);
            this.bannerBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.bannerBox.Name = "bannerBox";
            this.bannerBox.Size = new System.Drawing.Size(781, 48);
            this.bannerBox.TabIndex = 2;
            this.bannerBox.Text = "Configure the global settings.";
            this.bannerBox.Title = "Settings";
            // 
            // projectSettingTabPage
            // 
            this.projectSettingTabPage.Controls.Add(this.label1);
            this.projectSettingTabPage.Controls.Add(this.compressZipCheckBox);
            this.projectSettingTabPage.Location = new System.Drawing.Point(4, 25);
            this.projectSettingTabPage.Name = "projectSettingTabPage";
            this.projectSettingTabPage.Size = new System.Drawing.Size(741, 408);
            this.projectSettingTabPage.TabIndex = 3;
            this.projectSettingTabPage.Text = "Project Settings";
            this.projectSettingTabPage.UseVisualStyleBackColor = true;
            // 
            // compressZipCheckBox
            // 
            this.compressZipCheckBox.AutoSize = true;
            this.compressZipCheckBox.Location = new System.Drawing.Point(25, 38);
            this.compressZipCheckBox.Name = "compressZipCheckBox";
            this.compressZipCheckBox.Size = new System.Drawing.Size(132, 20);
            this.compressZipCheckBox.TabIndex = 0;
            this.compressZipCheckBox.Text = "Compress as ZIP";
            this.compressZipCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 16);
            this.label1.TabIndex = 45;
            this.label1.Text = "These settings only affect the project when saved:";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 526);
            this.Controls.Add(this.bannerBox);
            this.Controls.Add(this.settingsTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.projectSettingTabPage.ResumeLayout(false);
            this.projectSettingTabPage.PerformLayout();
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
		private ColorBox backgroundColorBox;
		private System.Windows.Forms.Label nodeSelectedLabel;
		private ColorBox nodeSelectedColorBox;
		private System.Windows.Forms.Label nodeHiddenLabel;
		private ColorBox nodeHiddenColorBox;
		private System.Windows.Forms.Label nodeAddressLabel;
		private ColorBox nodeAddressColorBox;
		private System.Windows.Forms.Label nodeOffsetLabel;
		private ColorBox nodeOffsetColorBox;
		private System.Windows.Forms.Label nodeHexValueLabel;
		private ColorBox nodeHexValueColorBox;
		private System.Windows.Forms.Label backgroundLabel;
		private System.Windows.Forms.Label nodeValueLabel;
		private ColorBox nodeValueColorBox;
		private System.Windows.Forms.Label nodeNameLabel;
		private ColorBox nodeNameColorBox;
		private System.Windows.Forms.Label nodeTypeLabel;
		private ColorBox nodeTypeColorBox;
		private System.Windows.Forms.Label nodeVTableLabel;
		private ColorBox nodeVTableColorBox;
		private System.Windows.Forms.Label nodeTextLabel;
		private ColorBox nodeTextColorBox;
		private System.Windows.Forms.Label nodeCommentLabel;
		private ColorBox nodeCommentColorBox;
		private System.Windows.Forms.Label nodeIndexLabel;
		private ColorBox nodeIndexColorBox;
		private System.Windows.Forms.Label nodePluginLabel;
		private ColorBox nodePluginColorBox;
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
		private System.Windows.Forms.Label generatorInfoLabel;
		private BannerBox bannerBox;
		private System.Windows.Forms.Label boolSettingsLabel;
		private System.Windows.Forms.TextBox boolTypeTextBox;
		private System.Windows.Forms.GroupBox fileAssociationGroupBox;
		private System.Windows.Forms.Button removeAssociationButton;
		private System.Windows.Forms.Button createAssociationButton;
		private System.Windows.Forms.Label associationInfoLabel;
		private System.Windows.Forms.CheckBox randomizeWindowTitleCheckBox;
		private System.Windows.Forms.CheckBox runAsAdminCheckBox;
		private System.Windows.Forms.Label nuintSettingsLabel;
		private System.Windows.Forms.TextBox nuintTypeTextBox;
		private System.Windows.Forms.Label nintSettingsLabel;
		private System.Windows.Forms.TextBox nintTypeTextBox;
		private System.Windows.Forms.Label utf32TextSettingsLabel;
		private System.Windows.Forms.TextBox utf32TextTypeTextBox;
        private System.Windows.Forms.TabPage projectSettingTabPage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox compressZipCheckBox;
    }
}