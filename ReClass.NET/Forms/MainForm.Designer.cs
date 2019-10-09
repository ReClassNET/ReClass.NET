namespace ReClassNET.Forms
{
	partial class MainForm
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

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.processUpdateTimer = new System.Windows.Forms.Timer(this.components);
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.projectView = new ReClassNET.UI.ProjectView();
			this.projectClassContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
			this.removeUnusedClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
			this.showCodeOfClassToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.projectClassesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.enableHierarchyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoExpandHierarchyViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
			this.expandAllClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.collapseAllClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
			this.addNewClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectEnumContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editEnumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectEnumsContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.editEnumsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.memoryViewControl = new ReClassNET.UI.MemoryViewControl();
			this.selectedNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.changeTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.integerToolStripMenuItem1 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem2 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem3 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem4 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem5 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem6 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem7 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.insertBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.integerToolStripMenuItem8 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem9 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem10 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem11 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem12 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem13 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.integerToolStripMenuItem14 = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.createClassFromNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.dissectNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.searchForEqualValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.findOutWhatAccessesThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findOutWhatWritesToThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.copyNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.hideNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideChildNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.showCodeOfClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shrinkClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.attachToProcessToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.openProjectToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.newClassToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.addBytesToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.add4BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add8BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add64BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add256BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add1024BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add2048BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add4096BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.addXBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insertBytesToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
			this.insert4BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert8BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert64BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert256BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert1024BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert2048BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert4096BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insertXBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nodeTypesToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.processInfoToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.infoToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.attachToProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.reattachToProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.detachToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mergeWithProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.processInformationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.memorySearcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.namedAddressesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.loadSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadSymbolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.resumeProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.suspendProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.terminateProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.goToClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cleanUnusedClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showEnumsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.generateCppCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateCSharpCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.projectClassContextMenuStrip.SuspendLayout();
			this.projectClassesContextMenuStrip.SuspendLayout();
			this.projectEnumContextMenuStrip.SuspendLayout();
			this.projectEnumsContextMenuStrip.SuspendLayout();
			this.selectedNodeContextMenuStrip.SuspendLayout();
			this.toolStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.mainMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// processUpdateTimer
			// 
			this.processUpdateTimer.Enabled = true;
			this.processUpdateTimer.Interval = 5000;
			this.processUpdateTimer.Tick += new System.EventHandler(this.processUpdateTimer_Tick);
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer.Location = new System.Drawing.Point(0, 49);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.projectView);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer.Panel2.Controls.Add(this.memoryViewControl);
			this.splitContainer.Size = new System.Drawing.Size(1141, 524);
			this.splitContainer.SplitterDistance = 201;
			this.splitContainer.TabIndex = 4;
			// 
			// projectView
			// 
			this.projectView.ClassContextMenuStrip = this.projectClassContextMenuStrip;
			this.projectView.ClassesContextMenuStrip = this.projectClassesContextMenuStrip;
			this.projectView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectView.EnumContextMenuStrip = this.projectEnumContextMenuStrip;
			this.projectView.EnumsContextMenuStrip = this.projectEnumsContextMenuStrip;
			this.projectView.Location = new System.Drawing.Point(0, 0);
			this.projectView.Name = "projectView";
			this.projectView.Size = new System.Drawing.Size(201, 524);
			this.projectView.TabIndex = 0;
			this.projectView.SelectionChanged += new ReClassNET.UI.ProjectView.SelectionChangedEvent(this.classesView_ClassSelected);
			// 
			// projectClassContextMenuStrip
			// 
			this.projectClassContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteClassToolStripMenuItem,
            this.toolStripSeparator19,
            this.removeUnusedClassesToolStripMenuItem,
            this.toolStripSeparator20,
            this.showCodeOfClassToolStripMenuItem2});
			this.projectClassContextMenuStrip.Name = "contextMenuStrip";
			this.projectClassContextMenuStrip.Size = new System.Drawing.Size(206, 82);
			// 
			// deleteClassToolStripMenuItem
			// 
			this.deleteClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Remove;
			this.deleteClassToolStripMenuItem.Name = "deleteClassToolStripMenuItem";
			this.deleteClassToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.deleteClassToolStripMenuItem.Text = "Delete class";
			this.deleteClassToolStripMenuItem.Click += new System.EventHandler(this.deleteClassToolStripMenuItem_Click);
			// 
			// toolStripSeparator19
			// 
			this.toolStripSeparator19.Name = "toolStripSeparator19";
			this.toolStripSeparator19.Size = new System.Drawing.Size(202, 6);
			// 
			// removeUnusedClassesToolStripMenuItem
			// 
			this.removeUnusedClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Chart_Delete;
			this.removeUnusedClassesToolStripMenuItem.Name = "removeUnusedClassesToolStripMenuItem";
			this.removeUnusedClassesToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
			this.removeUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
			this.removeUnusedClassesToolStripMenuItem.Click += new System.EventHandler(this.removeUnusedClassesToolStripMenuItem_Click);
			// 
			// toolStripSeparator20
			// 
			this.toolStripSeparator20.Name = "toolStripSeparator20";
			this.toolStripSeparator20.Size = new System.Drawing.Size(202, 6);
			// 
			// showCodeOfClassToolStripMenuItem2
			// 
			this.showCodeOfClassToolStripMenuItem2.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Code_Cpp;
			this.showCodeOfClassToolStripMenuItem2.Name = "showCodeOfClassToolStripMenuItem2";
			this.showCodeOfClassToolStripMenuItem2.Size = new System.Drawing.Size(205, 22);
			this.showCodeOfClassToolStripMenuItem2.Text = "Show C++ Code of Class";
			this.showCodeOfClassToolStripMenuItem2.Click += new System.EventHandler(this.showCodeOfClassToolStripMenuItem2_Click);
			// 
			// projectClassesContextMenuStrip
			// 
			this.projectClassesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableHierarchyViewToolStripMenuItem,
            this.autoExpandHierarchyViewToolStripMenuItem,
            this.toolStripSeparator21,
            this.expandAllClassesToolStripMenuItem,
            this.collapseAllClassesToolStripMenuItem,
            this.toolStripSeparator22,
            this.addNewClassToolStripMenuItem});
			this.projectClassesContextMenuStrip.Name = "rootContextMenuStrip";
			this.projectClassesContextMenuStrip.Size = new System.Drawing.Size(221, 126);
			// 
			// enableHierarchyViewToolStripMenuItem
			// 
			this.enableHierarchyViewToolStripMenuItem.Name = "enableHierarchyViewToolStripMenuItem";
			this.enableHierarchyViewToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.enableHierarchyViewToolStripMenuItem.Text = "Enable hierarchy view";
			this.enableHierarchyViewToolStripMenuItem.Click += new System.EventHandler(this.enableHierarchyViewToolStripMenuItem_Click);
			// 
			// autoExpandHierarchyViewToolStripMenuItem
			// 
			this.autoExpandHierarchyViewToolStripMenuItem.Name = "autoExpandHierarchyViewToolStripMenuItem";
			this.autoExpandHierarchyViewToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.autoExpandHierarchyViewToolStripMenuItem.Text = "Auto expand hierarchy view";
			this.autoExpandHierarchyViewToolStripMenuItem.Click += new System.EventHandler(this.autoExpandHierarchyViewToolStripMenuItem_Click);
			// 
			// toolStripSeparator21
			// 
			this.toolStripSeparator21.Name = "toolStripSeparator21";
			this.toolStripSeparator21.Size = new System.Drawing.Size(217, 6);
			// 
			// expandAllClassesToolStripMenuItem
			// 
			this.expandAllClassesToolStripMenuItem.Enabled = false;
			this.expandAllClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Tree_Expand;
			this.expandAllClassesToolStripMenuItem.Name = "expandAllClassesToolStripMenuItem";
			this.expandAllClassesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.expandAllClassesToolStripMenuItem.Text = "Expand all classes";
			this.expandAllClassesToolStripMenuItem.Click += new System.EventHandler(this.expandAllClassesToolStripMenuItem_Click);
			// 
			// collapseAllClassesToolStripMenuItem
			// 
			this.collapseAllClassesToolStripMenuItem.Enabled = false;
			this.collapseAllClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Tree_Collapse;
			this.collapseAllClassesToolStripMenuItem.Name = "collapseAllClassesToolStripMenuItem";
			this.collapseAllClassesToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.collapseAllClassesToolStripMenuItem.Text = "Collapse all classes";
			this.collapseAllClassesToolStripMenuItem.Click += new System.EventHandler(this.collapseAllClassesToolStripMenuItem_Click);
			// 
			// toolStripSeparator22
			// 
			this.toolStripSeparator22.Name = "toolStripSeparator22";
			this.toolStripSeparator22.Size = new System.Drawing.Size(217, 6);
			// 
			// addNewClassToolStripMenuItem
			// 
			this.addNewClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Add;
			this.addNewClassToolStripMenuItem.Name = "addNewClassToolStripMenuItem";
			this.addNewClassToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
			this.addNewClassToolStripMenuItem.Text = "Add new class";
			this.addNewClassToolStripMenuItem.Click += new System.EventHandler(this.newClassToolStripButton_Click);
			// 
			// projectEnumContextMenuStrip
			// 
			this.projectEnumContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editEnumToolStripMenuItem});
			this.projectEnumContextMenuStrip.Name = "projectEnumContextMenuStrip";
			this.projectEnumContextMenuStrip.Size = new System.Drawing.Size(138, 26);
			// 
			// editEnumToolStripMenuItem
			// 
			this.editEnumToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Enum_Type;
			this.editEnumToolStripMenuItem.Name = "editEnumToolStripMenuItem";
			this.editEnumToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.editEnumToolStripMenuItem.Text = "Edit Enum...";
			this.editEnumToolStripMenuItem.Click += new System.EventHandler(this.editEnumToolStripMenuItem_Click);
			// 
			// projectEnumsContextMenuStrip
			// 
			this.projectEnumsContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editEnumsToolStripMenuItem});
			this.projectEnumsContextMenuStrip.Name = "projectEnumsContextMenuStrip";
			this.projectEnumsContextMenuStrip.Size = new System.Drawing.Size(143, 26);
			// 
			// editEnumsToolStripMenuItem
			// 
			this.editEnumsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Category;
			this.editEnumsToolStripMenuItem.Name = "editEnumsToolStripMenuItem";
			this.editEnumsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
			this.editEnumsToolStripMenuItem.Text = "Edit enums...";
			this.editEnumsToolStripMenuItem.Click += new System.EventHandler(this.editEnumsToolStripMenuItem_Click);
			// 
			// memoryViewControl
			// 
			this.memoryViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.memoryViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoryViewControl.Location = new System.Drawing.Point(0, 0);
			this.memoryViewControl.Name = "memoryViewControl";
			this.memoryViewControl.NodeContextMenuStrip = this.selectedNodeContextMenuStrip;
			this.memoryViewControl.Size = new System.Drawing.Size(936, 524);
			this.memoryViewControl.TabIndex = 0;
			this.memoryViewControl.DrawContextRequested += new ReClassNET.UI.DrawContextRequestEventHandler(this.memoryViewControl_DrawContextRequested);
			this.memoryViewControl.SelectionChanged += new System.EventHandler(this.memoryViewControl_SelectionChanged);
			this.memoryViewControl.ChangeClassTypeClick += new ReClassNET.UI.NodeClickEventHandler(this.memoryViewControl_ChangeClassTypeClick);
			this.memoryViewControl.ChangeWrappedTypeClick += new ReClassNET.UI.NodeClickEventHandler(this.memoryViewControl_ChangeWrappedTypeClick);
			this.memoryViewControl.ChangeEnumTypeClick += new ReClassNET.UI.NodeClickEventHandler(this.memoryViewControl_ChangeEnumTypeClick);
			this.memoryViewControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.memoryViewControl_KeyDown);
			// 
			// selectedNodeContextMenuStrip
			// 
			this.selectedNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeTypeToolStripMenuItem,
            this.addBytesToolStripMenuItem,
            this.insertBytesToolStripMenuItem,
            this.toolStripSeparator8,
            this.createClassFromNodesToolStripMenuItem,
            this.toolStripSeparator13,
            this.dissectNodesToolStripMenuItem,
            this.toolStripSeparator9,
            this.searchForEqualValuesToolStripMenuItem,
            this.toolStripSeparator15,
            this.findOutWhatAccessesThisAddressToolStripMenuItem,
            this.findOutWhatWritesToThisAddressToolStripMenuItem,
            this.toolStripSeparator14,
            this.copyNodeToolStripMenuItem,
            this.pasteNodesToolStripMenuItem,
            this.toolStripSeparator10,
            this.removeToolStripMenuItem,
            this.toolStripSeparator12,
            this.hideNodesToolStripMenuItem,
            this.unhideNodesToolStripMenuItem,
            this.toolStripSeparator18,
            this.copyAddressToolStripMenuItem,
            this.toolStripSeparator11,
            this.showCodeOfClassToolStripMenuItem,
            this.shrinkClassToolStripMenuItem});
			this.selectedNodeContextMenuStrip.Name = "selectedNodeContextMenuStrip";
			this.selectedNodeContextMenuStrip.Size = new System.Drawing.Size(270, 410);
			this.selectedNodeContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.selectedNodeContextMenuStrip_Opening);
			// 
			// changeTypeToolStripMenuItem
			// 
			this.changeTypeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Exchange_Button;
			this.changeTypeToolStripMenuItem.Name = "changeTypeToolStripMenuItem";
			this.changeTypeToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.changeTypeToolStripMenuItem.Text = "Change Type";
			// 
			// addBytesToolStripMenuItem
			// 
			this.addBytesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.integerToolStripMenuItem1,
            this.integerToolStripMenuItem2,
            this.integerToolStripMenuItem3,
            this.integerToolStripMenuItem4,
            this.integerToolStripMenuItem5,
            this.integerToolStripMenuItem6,
            this.integerToolStripMenuItem7,
            this.toolStripMenuItem1});
			this.addBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_X;
			this.addBytesToolStripMenuItem.Name = "addBytesToolStripMenuItem";
			this.addBytesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.addBytesToolStripMenuItem.Text = "Add Bytes";
			// 
			// integerToolStripMenuItem1
			// 
			this.integerToolStripMenuItem1.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_4;
			this.integerToolStripMenuItem1.Name = "integerToolStripMenuItem1";
			this.integerToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem1.Text = "Add 4 Bytes";
			this.integerToolStripMenuItem1.Value = 4;
			this.integerToolStripMenuItem1.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem2
			// 
			this.integerToolStripMenuItem2.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_8;
			this.integerToolStripMenuItem2.Name = "integerToolStripMenuItem2";
			this.integerToolStripMenuItem2.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem2.Text = "Add 8 Bytes";
			this.integerToolStripMenuItem2.Value = 8;
			this.integerToolStripMenuItem2.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem3
			// 
			this.integerToolStripMenuItem3.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_64;
			this.integerToolStripMenuItem3.Name = "integerToolStripMenuItem3";
			this.integerToolStripMenuItem3.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem3.Text = "Add 64 Bytes";
			this.integerToolStripMenuItem3.Value = 64;
			this.integerToolStripMenuItem3.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem4
			// 
			this.integerToolStripMenuItem4.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_256;
			this.integerToolStripMenuItem4.Name = "integerToolStripMenuItem4";
			this.integerToolStripMenuItem4.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem4.Text = "Add 256 Bytes";
			this.integerToolStripMenuItem4.Value = 256;
			this.integerToolStripMenuItem4.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem5
			// 
			this.integerToolStripMenuItem5.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_1024;
			this.integerToolStripMenuItem5.Name = "integerToolStripMenuItem5";
			this.integerToolStripMenuItem5.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem5.Text = "Add 1024 Bytes";
			this.integerToolStripMenuItem5.Value = 1024;
			this.integerToolStripMenuItem5.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem6
			// 
			this.integerToolStripMenuItem6.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_2048;
			this.integerToolStripMenuItem6.Name = "integerToolStripMenuItem6";
			this.integerToolStripMenuItem6.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem6.Text = "Add 2048 Bytes";
			this.integerToolStripMenuItem6.Value = 2048;
			this.integerToolStripMenuItem6.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem7
			// 
			this.integerToolStripMenuItem7.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_4096;
			this.integerToolStripMenuItem7.Name = "integerToolStripMenuItem7";
			this.integerToolStripMenuItem7.Size = new System.Drawing.Size(154, 22);
			this.integerToolStripMenuItem7.Text = "Add 4096 Bytes";
			this.integerToolStripMenuItem7.Value = 4096;
			this.integerToolStripMenuItem7.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_X;
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
			this.toolStripMenuItem1.Text = "Add ... Bytes";
			this.toolStripMenuItem1.Click += new System.EventHandler(this.addXBytesToolStripMenuItem_Click);
			// 
			// insertBytesToolStripMenuItem
			// 
			this.insertBytesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.integerToolStripMenuItem8,
            this.integerToolStripMenuItem9,
            this.integerToolStripMenuItem10,
            this.integerToolStripMenuItem11,
            this.integerToolStripMenuItem12,
            this.integerToolStripMenuItem13,
            this.integerToolStripMenuItem14,
            this.toolStripMenuItem2});
			this.insertBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_X;
			this.insertBytesToolStripMenuItem.Name = "insertBytesToolStripMenuItem";
			this.insertBytesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.insertBytesToolStripMenuItem.Text = "Insert Bytes";
			// 
			// integerToolStripMenuItem8
			// 
			this.integerToolStripMenuItem8.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_4;
			this.integerToolStripMenuItem8.Name = "integerToolStripMenuItem8";
			this.integerToolStripMenuItem8.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem8.Text = "Insert 4 Bytes";
			this.integerToolStripMenuItem8.Value = 4;
			this.integerToolStripMenuItem8.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem9
			// 
			this.integerToolStripMenuItem9.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_8;
			this.integerToolStripMenuItem9.Name = "integerToolStripMenuItem9";
			this.integerToolStripMenuItem9.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem9.Text = "Insert 8 Bytes";
			this.integerToolStripMenuItem9.Value = 8;
			this.integerToolStripMenuItem9.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem10
			// 
			this.integerToolStripMenuItem10.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_64;
			this.integerToolStripMenuItem10.Name = "integerToolStripMenuItem10";
			this.integerToolStripMenuItem10.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem10.Text = "Insert 64 Bytes";
			this.integerToolStripMenuItem10.Value = 64;
			this.integerToolStripMenuItem10.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem11
			// 
			this.integerToolStripMenuItem11.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_256;
			this.integerToolStripMenuItem11.Name = "integerToolStripMenuItem11";
			this.integerToolStripMenuItem11.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem11.Text = "Insert 256 Bytes";
			this.integerToolStripMenuItem11.Value = 256;
			this.integerToolStripMenuItem11.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem12
			// 
			this.integerToolStripMenuItem12.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_1024;
			this.integerToolStripMenuItem12.Name = "integerToolStripMenuItem12";
			this.integerToolStripMenuItem12.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem12.Text = "Insert 1024 Bytes";
			this.integerToolStripMenuItem12.Value = 1024;
			this.integerToolStripMenuItem12.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem13
			// 
			this.integerToolStripMenuItem13.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_2048;
			this.integerToolStripMenuItem13.Name = "integerToolStripMenuItem13";
			this.integerToolStripMenuItem13.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem13.Text = "Insert 2048 Bytes";
			this.integerToolStripMenuItem13.Value = 2048;
			this.integerToolStripMenuItem13.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// integerToolStripMenuItem14
			// 
			this.integerToolStripMenuItem14.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_4096;
			this.integerToolStripMenuItem14.Name = "integerToolStripMenuItem14";
			this.integerToolStripMenuItem14.Size = new System.Drawing.Size(161, 22);
			this.integerToolStripMenuItem14.Text = "Insert 4096 Bytes";
			this.integerToolStripMenuItem14.Value = 4096;
			this.integerToolStripMenuItem14.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_X;
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(161, 22);
			this.toolStripMenuItem2.Text = "Insert ... Bytes";
			this.toolStripMenuItem2.Click += new System.EventHandler(this.insertXBytesToolStripMenuItem_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(266, 6);
			// 
			// createClassFromNodesToolStripMenuItem
			// 
			this.createClassFromNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Add;
			this.createClassFromNodesToolStripMenuItem.Name = "createClassFromNodesToolStripMenuItem";
			this.createClassFromNodesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.createClassFromNodesToolStripMenuItem.Text = "Create Class from Nodes";
			this.createClassFromNodesToolStripMenuItem.Click += new System.EventHandler(this.createClassFromNodesToolStripMenuItem_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(266, 6);
			// 
			// dissectNodesToolStripMenuItem
			// 
			this.dissectNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Camera;
			this.dissectNodesToolStripMenuItem.Name = "dissectNodesToolStripMenuItem";
			this.dissectNodesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.dissectNodesToolStripMenuItem.Text = "Dissect Node(s)";
			this.dissectNodesToolStripMenuItem.Click += new System.EventHandler(this.dissectNodesToolStripMenuItem_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(266, 6);
			// 
			// searchForEqualValuesToolStripMenuItem
			// 
			this.searchForEqualValuesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.searchForEqualValuesToolStripMenuItem.Name = "searchForEqualValuesToolStripMenuItem";
			this.searchForEqualValuesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.searchForEqualValuesToolStripMenuItem.Text = "Search for equal values...";
			this.searchForEqualValuesToolStripMenuItem.Click += new System.EventHandler(this.searchForEqualValuesToolStripMenuItem_Click);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(266, 6);
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
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(266, 6);
			// 
			// copyNodeToolStripMenuItem
			// 
			this.copyNodeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Copy;
			this.copyNodeToolStripMenuItem.Name = "copyNodeToolStripMenuItem";
			this.copyNodeToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.copyNodeToolStripMenuItem.Text = "Copy Node(s)";
			this.copyNodeToolStripMenuItem.Click += new System.EventHandler(this.copyNodeToolStripMenuItem_Click);
			// 
			// pasteNodesToolStripMenuItem
			// 
			this.pasteNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Paste;
			this.pasteNodesToolStripMenuItem.Name = "pasteNodesToolStripMenuItem";
			this.pasteNodesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.pasteNodesToolStripMenuItem.Text = "Paste Node(s)";
			this.pasteNodesToolStripMenuItem.Click += new System.EventHandler(this.pasteNodesToolStripMenuItem_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(266, 6);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Delete;
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.removeToolStripMenuItem.Text = "Remove Node(s)";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(266, 6);
			// 
			// hideNodesToolStripMenuItem
			// 
			this.hideNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.hideNodesToolStripMenuItem.Name = "hideNodesToolStripMenuItem";
			this.hideNodesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.hideNodesToolStripMenuItem.Text = "Hide selected Node(s)";
			this.hideNodesToolStripMenuItem.Click += new System.EventHandler(this.hideNodesToolStripMenuItem_Click);
			// 
			// unhideNodesToolStripMenuItem
			// 
			this.unhideNodesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unhideChildNodesToolStripMenuItem,
            this.unhideNodesAboveToolStripMenuItem,
            this.unhideNodesBelowToolStripMenuItem});
			this.unhideNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.unhideNodesToolStripMenuItem.Name = "unhideNodesToolStripMenuItem";
			this.unhideNodesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.unhideNodesToolStripMenuItem.Text = "Unhide...";
			// 
			// unhideChildNodesToolStripMenuItem
			// 
			this.unhideChildNodesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.unhideChildNodesToolStripMenuItem.Name = "unhideChildNodesToolStripMenuItem";
			this.unhideChildNodesToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.unhideChildNodesToolStripMenuItem.Text = "... Child Node(s)";
			this.unhideChildNodesToolStripMenuItem.Click += new System.EventHandler(this.unhideChildNodesToolStripMenuItem_Click);
			// 
			// unhideNodesAboveToolStripMenuItem
			// 
			this.unhideNodesAboveToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.unhideNodesAboveToolStripMenuItem.Name = "unhideNodesAboveToolStripMenuItem";
			this.unhideNodesAboveToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.unhideNodesAboveToolStripMenuItem.Text = "... Node(s) above";
			this.unhideNodesAboveToolStripMenuItem.Click += new System.EventHandler(this.unhideNodesAboveToolStripMenuItem_Click);
			// 
			// unhideNodesBelowToolStripMenuItem
			// 
			this.unhideNodesBelowToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.unhideNodesBelowToolStripMenuItem.Name = "unhideNodesBelowToolStripMenuItem";
			this.unhideNodesBelowToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
			this.unhideNodesBelowToolStripMenuItem.Text = "... Node(s) below";
			this.unhideNodesBelowToolStripMenuItem.Click += new System.EventHandler(this.unhideNodesBelowToolStripMenuItem_Click);
			// 
			// toolStripSeparator18
			// 
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			this.toolStripSeparator18.Size = new System.Drawing.Size(266, 6);
			// 
			// copyAddressToolStripMenuItem
			// 
			this.copyAddressToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Copy;
			this.copyAddressToolStripMenuItem.Name = "copyAddressToolStripMenuItem";
			this.copyAddressToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.copyAddressToolStripMenuItem.Text = "Copy Address";
			this.copyAddressToolStripMenuItem.Click += new System.EventHandler(this.copyAddressToolStripMenuItem_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(266, 6);
			// 
			// showCodeOfClassToolStripMenuItem
			// 
			this.showCodeOfClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Code_Cpp;
			this.showCodeOfClassToolStripMenuItem.Name = "showCodeOfClassToolStripMenuItem";
			this.showCodeOfClassToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.showCodeOfClassToolStripMenuItem.Text = "Show C++ Code of Class";
			this.showCodeOfClassToolStripMenuItem.Click += new System.EventHandler(this.showCodeOfClassToolStripMenuItem_Click);
			// 
			// shrinkClassToolStripMenuItem
			// 
			this.shrinkClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Chart_Delete;
			this.shrinkClassToolStripMenuItem.Name = "shrinkClassToolStripMenuItem";
			this.shrinkClassToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.shrinkClassToolStripMenuItem.Text = "Shrink Class";
			this.shrinkClassToolStripMenuItem.Click += new System.EventHandler(this.shrinkClassToolStripMenuItem_Click);
			// 
			// toolStrip
			// 
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attachToProcessToolStripSplitButton,
            this.toolStripSeparator6,
            this.openProjectToolStripButton,
            this.saveToolStripButton,
            this.toolStripSeparator7,
            this.newClassToolStripButton,
            this.addBytesToolStripDropDownButton,
            this.insertBytesToolStripDropDownButton,
            this.nodeTypesToolStripSeparator});
			this.toolStrip.Location = new System.Drawing.Point(0, 24);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(1141, 25);
			this.toolStrip.TabIndex = 3;
			// 
			// attachToProcessToolStripSplitButton
			// 
			this.attachToProcessToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.attachToProcessToolStripSplitButton.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier;
			this.attachToProcessToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.attachToProcessToolStripSplitButton.Name = "attachToProcessToolStripSplitButton";
			this.attachToProcessToolStripSplitButton.Size = new System.Drawing.Size(32, 22);
			this.attachToProcessToolStripSplitButton.ToolTipText = "Attach to Process...";
			this.attachToProcessToolStripSplitButton.ButtonClick += new System.EventHandler(this.attachToProcessToolStripSplitButton_ButtonClick);
			this.attachToProcessToolStripSplitButton.DropDownClosed += new System.EventHandler(this.attachToProcessToolStripSplitButton_DropDownClosed);
			this.attachToProcessToolStripSplitButton.DropDownOpening += new System.EventHandler(this.attachToProcessToolStripSplitButton_DropDownOpening);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
			// 
			// openProjectToolStripButton
			// 
			this.openProjectToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openProjectToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Folder;
			this.openProjectToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openProjectToolStripButton.Name = "openProjectToolStripButton";
			this.openProjectToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.openProjectToolStripButton.ToolTipText = "Open Project...";
			this.openProjectToolStripButton.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
			// 
			// saveToolStripButton
			// 
			this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Save;
			this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripButton.Name = "saveToolStripButton";
			this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.saveToolStripButton.ToolTipText = "Save Project";
			this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
			// 
			// newClassToolStripButton
			// 
			this.newClassToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newClassToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Add;
			this.newClassToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newClassToolStripButton.Name = "newClassToolStripButton";
			this.newClassToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.newClassToolStripButton.Text = "addClassToolStripButton";
			this.newClassToolStripButton.ToolTipText = "Add a new class to this project";
			this.newClassToolStripButton.Click += new System.EventHandler(this.newClassToolStripButton_Click);
			// 
			// addBytesToolStripDropDownButton
			// 
			this.addBytesToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.addBytesToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add4BytesToolStripMenuItem,
            this.add8BytesToolStripMenuItem,
            this.add64BytesToolStripMenuItem,
            this.add256BytesToolStripMenuItem,
            this.add1024BytesToolStripMenuItem,
            this.add2048BytesToolStripMenuItem,
            this.add4096BytesToolStripMenuItem,
            this.addXBytesToolStripMenuItem});
			this.addBytesToolStripDropDownButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_X;
			this.addBytesToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.addBytesToolStripDropDownButton.Name = "addBytesToolStripDropDownButton";
			this.addBytesToolStripDropDownButton.Size = new System.Drawing.Size(29, 22);
			// 
			// add4BytesToolStripMenuItem
			// 
			this.add4BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_4;
			this.add4BytesToolStripMenuItem.Name = "add4BytesToolStripMenuItem";
			this.add4BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add4BytesToolStripMenuItem.Tag = "";
			this.add4BytesToolStripMenuItem.Text = "Add 4 Bytes";
			this.add4BytesToolStripMenuItem.Value = 4;
			this.add4BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add8BytesToolStripMenuItem
			// 
			this.add8BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_8;
			this.add8BytesToolStripMenuItem.Name = "add8BytesToolStripMenuItem";
			this.add8BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add8BytesToolStripMenuItem.Text = "Add 8 Bytes";
			this.add8BytesToolStripMenuItem.Value = 8;
			this.add8BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add64BytesToolStripMenuItem
			// 
			this.add64BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_64;
			this.add64BytesToolStripMenuItem.Name = "add64BytesToolStripMenuItem";
			this.add64BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add64BytesToolStripMenuItem.Text = "Add 64 Bytes";
			this.add64BytesToolStripMenuItem.Value = 64;
			this.add64BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add256BytesToolStripMenuItem
			// 
			this.add256BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_256;
			this.add256BytesToolStripMenuItem.Name = "add256BytesToolStripMenuItem";
			this.add256BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add256BytesToolStripMenuItem.Text = "Add 256 Bytes";
			this.add256BytesToolStripMenuItem.Value = 256;
			this.add256BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add1024BytesToolStripMenuItem
			// 
			this.add1024BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_1024;
			this.add1024BytesToolStripMenuItem.Name = "add1024BytesToolStripMenuItem";
			this.add1024BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add1024BytesToolStripMenuItem.Text = "Add 1024 Bytes";
			this.add1024BytesToolStripMenuItem.Value = 1024;
			this.add1024BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add2048BytesToolStripMenuItem
			// 
			this.add2048BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_2048;
			this.add2048BytesToolStripMenuItem.Name = "add2048BytesToolStripMenuItem";
			this.add2048BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add2048BytesToolStripMenuItem.Text = "Add 2048 Bytes";
			this.add2048BytesToolStripMenuItem.Value = 2048;
			this.add2048BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// add4096BytesToolStripMenuItem
			// 
			this.add4096BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_4096;
			this.add4096BytesToolStripMenuItem.Name = "add4096BytesToolStripMenuItem";
			this.add4096BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.add4096BytesToolStripMenuItem.Text = "Add 4096 Bytes";
			this.add4096BytesToolStripMenuItem.Value = 4096;
			this.add4096BytesToolStripMenuItem.Click += new System.EventHandler(this.addBytesToolStripMenuItem_Click);
			// 
			// addXBytesToolStripMenuItem
			// 
			this.addXBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_X;
			this.addXBytesToolStripMenuItem.Name = "addXBytesToolStripMenuItem";
			this.addXBytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
			this.addXBytesToolStripMenuItem.Text = "Add ... Bytes";
			this.addXBytesToolStripMenuItem.Click += new System.EventHandler(this.addXBytesToolStripMenuItem_Click);
			// 
			// insertBytesToolStripDropDownButton
			// 
			this.insertBytesToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.insertBytesToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insert4BytesToolStripMenuItem,
            this.insert8BytesToolStripMenuItem,
            this.insert64BytesToolStripMenuItem,
            this.insert256BytesToolStripMenuItem,
            this.insert1024BytesToolStripMenuItem,
            this.insert2048BytesToolStripMenuItem,
            this.insert4096BytesToolStripMenuItem,
            this.insertXBytesToolStripMenuItem});
			this.insertBytesToolStripDropDownButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_X;
			this.insertBytesToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.insertBytesToolStripDropDownButton.Name = "insertBytesToolStripDropDownButton";
			this.insertBytesToolStripDropDownButton.Size = new System.Drawing.Size(29, 22);
			this.insertBytesToolStripDropDownButton.ToolTipText = "Insert bytes at selected position";
			// 
			// insert4BytesToolStripMenuItem
			// 
			this.insert4BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_4;
			this.insert4BytesToolStripMenuItem.Name = "insert4BytesToolStripMenuItem";
			this.insert4BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert4BytesToolStripMenuItem.Tag = "";
			this.insert4BytesToolStripMenuItem.Text = "Insert 4 Bytes";
			this.insert4BytesToolStripMenuItem.Value = 4;
			this.insert4BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert8BytesToolStripMenuItem
			// 
			this.insert8BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_8;
			this.insert8BytesToolStripMenuItem.Name = "insert8BytesToolStripMenuItem";
			this.insert8BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert8BytesToolStripMenuItem.Text = "Insert 8 Bytes";
			this.insert8BytesToolStripMenuItem.Value = 8;
			this.insert8BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert64BytesToolStripMenuItem
			// 
			this.insert64BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_64;
			this.insert64BytesToolStripMenuItem.Name = "insert64BytesToolStripMenuItem";
			this.insert64BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert64BytesToolStripMenuItem.Text = "Insert 64 Bytes";
			this.insert64BytesToolStripMenuItem.Value = 64;
			this.insert64BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert256BytesToolStripMenuItem
			// 
			this.insert256BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_256;
			this.insert256BytesToolStripMenuItem.Name = "insert256BytesToolStripMenuItem";
			this.insert256BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert256BytesToolStripMenuItem.Text = "Insert 256 Bytes";
			this.insert256BytesToolStripMenuItem.Value = 256;
			this.insert256BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert1024BytesToolStripMenuItem
			// 
			this.insert1024BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_1024;
			this.insert1024BytesToolStripMenuItem.Name = "insert1024BytesToolStripMenuItem";
			this.insert1024BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert1024BytesToolStripMenuItem.Text = "Insert 1024 Bytes";
			this.insert1024BytesToolStripMenuItem.Value = 1024;
			this.insert1024BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert2048BytesToolStripMenuItem
			// 
			this.insert2048BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_2048;
			this.insert2048BytesToolStripMenuItem.Name = "insert2048BytesToolStripMenuItem";
			this.insert2048BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert2048BytesToolStripMenuItem.Text = "Insert 2048 Bytes";
			this.insert2048BytesToolStripMenuItem.Value = 2048;
			this.insert2048BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insert4096BytesToolStripMenuItem
			// 
			this.insert4096BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_4096;
			this.insert4096BytesToolStripMenuItem.Name = "insert4096BytesToolStripMenuItem";
			this.insert4096BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insert4096BytesToolStripMenuItem.Text = "Insert 4096 Bytes";
			this.insert4096BytesToolStripMenuItem.Value = 4096;
			this.insert4096BytesToolStripMenuItem.Click += new System.EventHandler(this.insertBytesToolStripMenuItem_Click);
			// 
			// insertXBytesToolStripMenuItem
			// 
			this.insertXBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_X;
			this.insertXBytesToolStripMenuItem.Name = "insertXBytesToolStripMenuItem";
			this.insertXBytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.insertXBytesToolStripMenuItem.Text = "Insert ... Bytes";
			this.insertXBytesToolStripMenuItem.Click += new System.EventHandler(this.insertXBytesToolStripMenuItem_Click);
			// 
			// nodeTypesToolStripSeparator
			// 
			this.nodeTypesToolStripSeparator.Name = "nodeTypesToolStripSeparator";
			this.nodeTypesToolStripSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processInfoToolStripStatusLabel,
            this.infoToolStripStatusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 573);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1141, 22);
			this.statusStrip.TabIndex = 1;
			// 
			// processInfoToolStripStatusLabel
			// 
			this.processInfoToolStripStatusLabel.Name = "processInfoToolStripStatusLabel";
			this.processInfoToolStripStatusLabel.Size = new System.Drawing.Size(112, 17);
			this.processInfoToolStripStatusLabel.Text = "No process selected";
			// 
			// infoToolStripStatusLabel
			// 
			this.infoToolStripStatusLabel.Name = "infoToolStripStatusLabel";
			this.infoToolStripStatusLabel.Size = new System.Drawing.Size(23, 17);
			this.infoToolStripStatusLabel.Text = "<>";
			this.infoToolStripStatusLabel.Visible = false;
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.processToolStripMenuItem,
            this.projectToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(1141, 24);
			this.mainMenuStrip.TabIndex = 2;
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attachToProcessToolStripMenuItem,
            this.reattachToProcessToolStripMenuItem,
            this.detachToolStripMenuItem,
            this.toolStripSeparator1,
            this.openProjectToolStripMenuItem,
            this.mergeWithProjectToolStripMenuItem,
            this.clearProjectToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.settingsToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.toolStripSeparator5,
            this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.fileToolStripMenuItem.DropDownOpening += new System.EventHandler(this.fileToolStripMenuItem_DropDownOpening);
			// 
			// attachToProcessToolStripMenuItem
			// 
			this.attachToProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier;
			this.attachToProcessToolStripMenuItem.Name = "attachToProcessToolStripMenuItem";
			this.attachToProcessToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.attachToProcessToolStripMenuItem.Text = "Attach to Process...";
			this.attachToProcessToolStripMenuItem.Click += new System.EventHandler(this.attachToProcessToolStripSplitButton_ButtonClick);
			// 
			// reattachToProcessToolStripMenuItem
			// 
			this.reattachToProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier_Arrow;
			this.reattachToProcessToolStripMenuItem.Name = "reattachToProcessToolStripMenuItem";
			this.reattachToProcessToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.reattachToProcessToolStripMenuItem.Text = "<>";
			this.reattachToProcessToolStripMenuItem.Click += new System.EventHandler(this.reattachToProcessToolStripMenuItem_Click);
			// 
			// detachToolStripMenuItem
			// 
			this.detachToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier_Remove;
			this.detachToolStripMenuItem.Name = "detachToolStripMenuItem";
			this.detachToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.detachToolStripMenuItem.Text = "Detach";
			this.detachToolStripMenuItem.Click += new System.EventHandler(this.detachToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
			// 
			// openProjectToolStripMenuItem
			// 
			this.openProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Folder;
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.openProjectToolStripMenuItem.Text = "Open Project...";
			this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
			// 
			// mergeWithProjectToolStripMenuItem
			// 
			this.mergeWithProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Folder_Add;
			this.mergeWithProjectToolStripMenuItem.Name = "mergeWithProjectToolStripMenuItem";
			this.mergeWithProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.mergeWithProjectToolStripMenuItem.Text = "Merge with Project...";
			this.mergeWithProjectToolStripMenuItem.Click += new System.EventHandler(this.mergeWithProjectToolStripMenuItem_Click);
			// 
			// clearProjectToolStripMenuItem
			// 
			this.clearProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Arrow_Refresh;
			this.clearProjectToolStripMenuItem.Name = "clearProjectToolStripMenuItem";
			this.clearProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.clearProjectToolStripMenuItem.Text = "Clear Project";
			this.clearProjectToolStripMenuItem.Click += new System.EventHandler(this.clearProjectToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(192, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Save;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Save_As;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.saveAsToolStripMenuItem.Text = "Save as...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(192, 6);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Cogs;
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.settingsToolStripMenuItem.Text = "Settings...";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// pluginsToolStripMenuItem
			// 
			this.pluginsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Plugin;
			this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
			this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.pluginsToolStripMenuItem.Text = "Plugins...";
			this.pluginsToolStripMenuItem.Click += new System.EventHandler(this.pluginsToolStripButton_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Quit;
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// processToolStripMenuItem
			// 
			this.processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processInformationsToolStripMenuItem,
            this.memorySearcherToolStripMenuItem,
            this.namedAddressesToolStripMenuItem,
            this.toolStripSeparator17,
            this.loadSymbolToolStripMenuItem,
            this.loadSymbolsToolStripMenuItem,
            this.toolStripSeparator4,
            this.resumeProcessToolStripMenuItem,
            this.suspendProcessToolStripMenuItem,
            this.terminateProcessToolStripMenuItem});
			this.processToolStripMenuItem.Name = "processToolStripMenuItem";
			this.processToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
			this.processToolStripMenuItem.Text = "Process";
			// 
			// processInformationsToolStripMenuItem
			// 
			this.processInformationsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Category;
			this.processInformationsToolStripMenuItem.Name = "processInformationsToolStripMenuItem";
			this.processInformationsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.processInformationsToolStripMenuItem.Text = "Process Informations...";
			this.processInformationsToolStripMenuItem.Click += new System.EventHandler(this.memoryViewerToolStripMenuItem_Click);
			// 
			// memorySearcherToolStripMenuItem
			// 
			this.memorySearcherToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.memorySearcherToolStripMenuItem.Name = "memorySearcherToolStripMenuItem";
			this.memorySearcherToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.memorySearcherToolStripMenuItem.Text = "Memory Searcher...";
			this.memorySearcherToolStripMenuItem.Click += new System.EventHandler(this.memorySearcherToolStripMenuItem_Click);
			// 
			// namedAddressesToolStripMenuItem
			// 
			this.namedAddressesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Custom_Type;
			this.namedAddressesToolStripMenuItem.Name = "namedAddressesToolStripMenuItem";
			this.namedAddressesToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.namedAddressesToolStripMenuItem.Text = "Named Addresses...";
			this.namedAddressesToolStripMenuItem.Click += new System.EventHandler(this.namedAddressesToolStripMenuItem_Click);
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(191, 6);
			// 
			// loadSymbolToolStripMenuItem
			// 
			this.loadSymbolToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Pdb;
			this.loadSymbolToolStripMenuItem.Name = "loadSymbolToolStripMenuItem";
			this.loadSymbolToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.loadSymbolToolStripMenuItem.Text = "Load Symbol...";
			this.loadSymbolToolStripMenuItem.Click += new System.EventHandler(this.loadSymbolToolStripMenuItem_Click);
			// 
			// loadSymbolsToolStripMenuItem
			// 
			this.loadSymbolsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("loadSymbolsToolStripMenuItem.Image")));
			this.loadSymbolsToolStripMenuItem.Name = "loadSymbolsToolStripMenuItem";
			this.loadSymbolsToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.loadSymbolsToolStripMenuItem.Text = "Load all Symbols";
			this.loadSymbolsToolStripMenuItem.Click += new System.EventHandler(this.loadSymbolsToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(191, 6);
			// 
			// resumeProcessToolStripMenuItem
			// 
			this.resumeProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Play;
			this.resumeProcessToolStripMenuItem.Name = "resumeProcessToolStripMenuItem";
			this.resumeProcessToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.resumeProcessToolStripMenuItem.Text = "Resume";
			this.resumeProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// suspendProcessToolStripMenuItem
			// 
			this.suspendProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Pause;
			this.suspendProcessToolStripMenuItem.Name = "suspendProcessToolStripMenuItem";
			this.suspendProcessToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.suspendProcessToolStripMenuItem.Text = "Suspend";
			this.suspendProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// terminateProcessToolStripMenuItem
			// 
			this.terminateProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Stop;
			this.terminateProcessToolStripMenuItem.Name = "terminateProcessToolStripMenuItem";
			this.terminateProcessToolStripMenuItem.Size = new System.Drawing.Size(194, 22);
			this.terminateProcessToolStripMenuItem.Text = "Kill";
			this.terminateProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// projectToolStripMenuItem
			// 
			this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goToClassToolStripMenuItem,
            this.cleanUnusedClassesToolStripMenuItem,
            this.showEnumsToolStripMenuItem,
            this.toolStripSeparator16,
            this.generateCppCodeToolStripMenuItem,
            this.generateCSharpCodeToolStripMenuItem});
			this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
			this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projectToolStripMenuItem.Text = "Project";
			// 
			// goToClassToolStripMenuItem
			// 
			this.goToClassToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Class_Type;
			this.goToClassToolStripMenuItem.Name = "goToClassToolStripMenuItem";
			this.goToClassToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
			this.goToClassToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.goToClassToolStripMenuItem.Text = "Go to class...";
			this.goToClassToolStripMenuItem.Click += new System.EventHandler(this.goToClassToolStripMenuItem_Click);
			// 
			// cleanUnusedClassesToolStripMenuItem
			// 
			this.cleanUnusedClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Chart_Delete;
			this.cleanUnusedClassesToolStripMenuItem.Name = "cleanUnusedClassesToolStripMenuItem";
			this.cleanUnusedClassesToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.cleanUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
			this.cleanUnusedClassesToolStripMenuItem.Click += new System.EventHandler(this.cleanUnusedClassesToolStripMenuItem_Click);
			// 
			// showEnumsToolStripMenuItem
			// 
			this.showEnumsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Category;
			this.showEnumsToolStripMenuItem.Name = "showEnumsToolStripMenuItem";
			this.showEnumsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.showEnumsToolStripMenuItem.Text = "Show Enums...";
			this.showEnumsToolStripMenuItem.Click += new System.EventHandler(this.showEnumsToolStripMenuItem_Click);
			// 
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(195, 6);
			// 
			// generateCppCodeToolStripMenuItem
			// 
			this.generateCppCodeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Code_Cpp;
			this.generateCppCodeToolStripMenuItem.Name = "generateCppCodeToolStripMenuItem";
			this.generateCppCodeToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.generateCppCodeToolStripMenuItem.Text = "Generate C++ Code...";
			this.generateCppCodeToolStripMenuItem.Click += new System.EventHandler(this.generateCppCodeToolStripMenuItem_Click);
			// 
			// generateCSharpCodeToolStripMenuItem
			// 
			this.generateCSharpCodeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Code_Csharp;
			this.generateCSharpCodeToolStripMenuItem.Name = "generateCSharpCodeToolStripMenuItem";
			this.generateCSharpCodeToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.generateCSharpCodeToolStripMenuItem.Text = "Generate C# Code...";
			this.generateCSharpCodeToolStripMenuItem.Click += new System.EventHandler(this.generateCSharpCodeToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Information;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.aboutToolStripMenuItem.Text = "About...";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1141, 595);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.mainMenuStrip);
			this.MainMenuStrip = this.mainMenuStrip;
			this.MinimumSize = new System.Drawing.Size(200, 100);
			this.Name = "MainForm";
			this.Text = "ReClass.NET";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.projectClassContextMenuStrip.ResumeLayout(false);
			this.projectClassesContextMenuStrip.ResumeLayout(false);
			this.projectEnumContextMenuStrip.ResumeLayout(false);
			this.projectEnumsContextMenuStrip.ResumeLayout(false);
			this.selectedNodeContextMenuStrip.ResumeLayout(false);
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		#endregion

		private UI.MemoryViewControl memoryViewControl;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem attachToProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem clearProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem processInformationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem resumeProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem suspendProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem terminateProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem loadSymbolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStripStatusLabel processInfoToolStripStatusLabel;
		private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton saveToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton newClassToolStripButton;
		private System.Windows.Forms.ToolStripDropDownButton addBytesToolStripDropDownButton;
		private UI.IntegerToolStripMenuItem add4BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add8BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add64BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add256BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add1024BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add2048BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add4096BytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripDropDownButton insertBytesToolStripDropDownButton;
		private UI.IntegerToolStripMenuItem insert4BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert8BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert64BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert256BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert1024BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert2048BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert4096BytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addXBytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem insertXBytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator nodeTypesToolStripSeparator;
		private UI.ProjectView projectView;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cleanUnusedClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
		private System.Windows.Forms.ToolStripMenuItem generateCppCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateCSharpCodeToolStripMenuItem;
		private System.Windows.Forms.Timer processUpdateTimer;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
		private System.Windows.Forms.ToolStripMenuItem loadSymbolToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton openProjectToolStripButton;
		private System.Windows.Forms.ToolStripStatusLabel infoToolStripStatusLabel;
		private System.Windows.Forms.ToolStripMenuItem mergeWithProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detachToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem memorySearcherToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reattachToProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton attachToProcessToolStripSplitButton;
		private System.Windows.Forms.ToolStripMenuItem namedAddressesToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip selectedNodeContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem changeTypeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addBytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem1;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem2;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem3;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem4;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem5;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem6;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem insertBytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem8;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem9;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem10;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem11;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem12;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem13;
		private UI.IntegerToolStripMenuItem integerToolStripMenuItem14;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem createClassFromNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem dissectNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem searchForEqualValuesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatAccessesThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatWritesToThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripMenuItem copyNodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem hideNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideChildNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideNodesAboveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideNodesBelowToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
		private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private System.Windows.Forms.ToolStripMenuItem showCodeOfClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shrinkClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem goToClassToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip projectClassContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem deleteClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
		private System.Windows.Forms.ToolStripMenuItem removeUnusedClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
		private System.Windows.Forms.ToolStripMenuItem showCodeOfClassToolStripMenuItem2;
		private System.Windows.Forms.ContextMenuStrip projectClassesContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem enableHierarchyViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autoExpandHierarchyViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
		private System.Windows.Forms.ToolStripMenuItem expandAllClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem collapseAllClassesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
		private System.Windows.Forms.ToolStripMenuItem addNewClassToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip projectEnumContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem editEnumToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip projectEnumsContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem editEnumsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showEnumsToolStripMenuItem;
	}
}

