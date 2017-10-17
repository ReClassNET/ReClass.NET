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
			this.classesView = new ReClassNET.UI.ClassNodeView();
			this.memoryViewControl = new ReClassNET.UI.MemoryViewControl();
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
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.hex64ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.hex32ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.hex16ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.hex8ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.int64ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.int32ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.int16ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.int8ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.uint64ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.uint32ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.uint16ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.uint8ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
			this.boolToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.bitFieldToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.floatToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.doubleToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.vec4ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.vec3ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.vec2ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.mat44ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.mat34ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.mat33ToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.utf8TextToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.utf8TextPtrToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.utf16TextToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.utf16TextPtrToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.classInstanceToolStripButton6 = new ReClassNET.UI.TypeToolStripButton();
			this.classPtrToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.arrayToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.ptrArrayToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.vtableToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.fnPtrToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.fnTypeToolStripButton = new ReClassNET.UI.TypeToolStripButton();
			this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
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
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.loadSymbolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadSymbolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.resumeProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.suspendProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.terminateProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cleanUnusedClassesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.generateCppCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateCSharpCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
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
			this.splitContainer.Panel1.Controls.Add(this.classesView);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer.Panel2.Controls.Add(this.memoryViewControl);
			this.splitContainer.Size = new System.Drawing.Size(1141, 524);
			this.splitContainer.SplitterDistance = 201;
			this.splitContainer.TabIndex = 4;
			// 
			// classesView
			// 
			this.classesView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.classesView.Location = new System.Drawing.Point(0, 0);
			this.classesView.Name = "classesView";
			this.classesView.Size = new System.Drawing.Size(201, 524);
			this.classesView.TabIndex = 0;
			this.classesView.SelectionChanged += new ReClassNET.UI.ClassNodeView.SelectionChangedEvent(this.classesView_ClassSelected);
			// 
			// memoryViewControl
			// 
			this.memoryViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.memoryViewControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoryViewControl.Location = new System.Drawing.Point(0, 0);
			this.memoryViewControl.Name = "memoryViewControl";
			this.memoryViewControl.Size = new System.Drawing.Size(936, 524);
			this.memoryViewControl.TabIndex = 0;
			this.memoryViewControl.SelectionChanged += new System.EventHandler(this.memoryViewControl_SelectionChanged);
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
            this.toolStripSeparator8,
            this.hex64ToolStripButton,
            this.hex32ToolStripButton,
            this.hex16ToolStripButton,
            this.hex8ToolStripButton,
            this.toolStripSeparator9,
            this.int64ToolStripButton,
            this.int32ToolStripButton,
            this.int16ToolStripButton,
            this.int8ToolStripButton,
            this.toolStripSeparator10,
            this.uint64ToolStripButton,
            this.uint32ToolStripButton,
            this.uint16ToolStripButton,
            this.uint8ToolStripButton,
            this.toolStripSeparator11,
            this.boolToolStripButton,
            this.bitFieldToolStripButton,
            this.toolStripSeparator18,
            this.floatToolStripButton,
            this.doubleToolStripButton,
            this.toolStripSeparator12,
            this.vec4ToolStripButton,
            this.vec3ToolStripButton,
            this.vec2ToolStripButton,
            this.mat44ToolStripButton,
            this.mat34ToolStripButton,
            this.mat33ToolStripButton,
            this.toolStripSeparator13,
            this.utf8TextToolStripButton,
            this.utf8TextPtrToolStripButton,
            this.utf16TextToolStripButton,
            this.utf16TextPtrToolStripButton,
            this.toolStripSeparator14,
            this.classInstanceToolStripButton6,
            this.classPtrToolStripButton,
            this.toolStripSeparator15,
            this.arrayToolStripButton,
            this.ptrArrayToolStripButton,
            this.vtableToolStripButton,
            this.fnPtrToolStripButton,
            this.fnTypeToolStripButton,
            this.toolStripSeparator19});
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
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
			// 
			// hex64ToolStripButton
			// 
			this.hex64ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.hex64ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Hex_64;
			this.hex64ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hex64ToolStripButton.Name = "hex64ToolStripButton";
			this.hex64ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.hex64ToolStripButton.ToolTipText = "Hex64";
			this.hex64ToolStripButton.Value = typeof(ReClassNET.Nodes.Hex64Node);
			this.hex64ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// hex32ToolStripButton
			// 
			this.hex32ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.hex32ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Hex_32;
			this.hex32ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hex32ToolStripButton.Name = "hex32ToolStripButton";
			this.hex32ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.hex32ToolStripButton.ToolTipText = "Hex32";
			this.hex32ToolStripButton.Value = typeof(ReClassNET.Nodes.Hex32Node);
			this.hex32ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// hex16ToolStripButton
			// 
			this.hex16ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.hex16ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Hex_16;
			this.hex16ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hex16ToolStripButton.Name = "hex16ToolStripButton";
			this.hex16ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.hex16ToolStripButton.ToolTipText = "Hex16";
			this.hex16ToolStripButton.Value = typeof(ReClassNET.Nodes.Hex16Node);
			this.hex16ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// hex8ToolStripButton
			// 
			this.hex8ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.hex8ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Hex_8;
			this.hex8ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.hex8ToolStripButton.Name = "hex8ToolStripButton";
			this.hex8ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.hex8ToolStripButton.ToolTipText = "Hex8";
			this.hex8ToolStripButton.Value = typeof(ReClassNET.Nodes.Hex8Node);
			this.hex8ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
			// 
			// int64ToolStripButton
			// 
			this.int64ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.int64ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Int_64;
			this.int64ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.int64ToolStripButton.Name = "int64ToolStripButton";
			this.int64ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.int64ToolStripButton.ToolTipText = "Int64";
			this.int64ToolStripButton.Value = typeof(ReClassNET.Nodes.Int64Node);
			this.int64ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// int32ToolStripButton
			// 
			this.int32ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.int32ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Int_32;
			this.int32ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.int32ToolStripButton.Name = "int32ToolStripButton";
			this.int32ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.int32ToolStripButton.ToolTipText = "Int32";
			this.int32ToolStripButton.Value = typeof(ReClassNET.Nodes.Int32Node);
			this.int32ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// int16ToolStripButton
			// 
			this.int16ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.int16ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Int_16;
			this.int16ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.int16ToolStripButton.Name = "int16ToolStripButton";
			this.int16ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.int16ToolStripButton.ToolTipText = "Int16";
			this.int16ToolStripButton.Value = typeof(ReClassNET.Nodes.Int16Node);
			this.int16ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// int8ToolStripButton
			// 
			this.int8ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.int8ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Int_8;
			this.int8ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.int8ToolStripButton.Name = "int8ToolStripButton";
			this.int8ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.int8ToolStripButton.ToolTipText = "Int8";
			this.int8ToolStripButton.Value = typeof(ReClassNET.Nodes.Int8Node);
			this.int8ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
			// 
			// uint64ToolStripButton
			// 
			this.uint64ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.uint64ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UInt_64;
			this.uint64ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.uint64ToolStripButton.Name = "uint64ToolStripButton";
			this.uint64ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.uint64ToolStripButton.ToolTipText = "UInt64 / QWORD";
			this.uint64ToolStripButton.Value = typeof(ReClassNET.Nodes.UInt64Node);
			this.uint64ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// uint32ToolStripButton
			// 
			this.uint32ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.uint32ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UInt_32;
			this.uint32ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.uint32ToolStripButton.Name = "uint32ToolStripButton";
			this.uint32ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.uint32ToolStripButton.ToolTipText = "UInt32 / DWORD";
			this.uint32ToolStripButton.Value = typeof(ReClassNET.Nodes.UInt32Node);
			this.uint32ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// uint16ToolStripButton
			// 
			this.uint16ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.uint16ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UInt_16;
			this.uint16ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.uint16ToolStripButton.Name = "uint16ToolStripButton";
			this.uint16ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.uint16ToolStripButton.ToolTipText = "UInt16 / WORD";
			this.uint16ToolStripButton.Value = typeof(ReClassNET.Nodes.UInt16Node);
			this.uint16ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// uint8ToolStripButton
			// 
			this.uint8ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.uint8ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UInt_8;
			this.uint8ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.uint8ToolStripButton.Name = "uint8ToolStripButton";
			this.uint8ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.uint8ToolStripButton.ToolTipText = "UInt8 / BYTE";
			this.uint8ToolStripButton.Value = typeof(ReClassNET.Nodes.UInt8Node);
			this.uint8ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator11
			// 
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
			// 
			// boolToolStripButton
			// 
			this.boolToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.boolToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Bool;
			this.boolToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.boolToolStripButton.Name = "boolToolStripButton";
			this.boolToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.boolToolStripButton.ToolTipText = "Bool";
			this.boolToolStripButton.Value = typeof(ReClassNET.Nodes.BoolNode);
			this.boolToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// bitFieldToolStripButton
			// 
			this.bitFieldToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bitFieldToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Bits;
			this.bitFieldToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.bitFieldToolStripButton.Name = "bitFieldToolStripButton";
			this.bitFieldToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.bitFieldToolStripButton.ToolTipText = "Bit Field";
			this.bitFieldToolStripButton.Value = typeof(ReClassNET.Nodes.BitFieldNode);
			this.bitFieldToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator18
			// 
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			this.toolStripSeparator18.Size = new System.Drawing.Size(6, 25);
			// 
			// floatToolStripButton
			// 
			this.floatToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.floatToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Float;
			this.floatToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.floatToolStripButton.Name = "floatToolStripButton";
			this.floatToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.floatToolStripButton.ToolTipText = "Float";
			this.floatToolStripButton.Value = typeof(ReClassNET.Nodes.FloatNode);
			this.floatToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// doubleToolStripButton
			// 
			this.doubleToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.doubleToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Double;
			this.doubleToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.doubleToolStripButton.Name = "doubleToolStripButton";
			this.doubleToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.doubleToolStripButton.ToolTipText = "Double";
			this.doubleToolStripButton.Value = typeof(ReClassNET.Nodes.DoubleNode);
			this.doubleToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator12
			// 
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			this.toolStripSeparator12.Size = new System.Drawing.Size(6, 25);
			// 
			// vec4ToolStripButton
			// 
			this.vec4ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.vec4ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Vector_4;
			this.vec4ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.vec4ToolStripButton.Name = "vec4ToolStripButton";
			this.vec4ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.vec4ToolStripButton.ToolTipText = "Vector4";
			this.vec4ToolStripButton.Value = typeof(ReClassNET.Nodes.Vector4Node);
			this.vec4ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// vec3ToolStripButton
			// 
			this.vec3ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.vec3ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Vector_3;
			this.vec3ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.vec3ToolStripButton.Name = "vec3ToolStripButton";
			this.vec3ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.vec3ToolStripButton.ToolTipText = "Vector3";
			this.vec3ToolStripButton.Value = typeof(ReClassNET.Nodes.Vector3Node);
			this.vec3ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// vec2ToolStripButton
			// 
			this.vec2ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.vec2ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Vector_2;
			this.vec2ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.vec2ToolStripButton.Name = "vec2ToolStripButton";
			this.vec2ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.vec2ToolStripButton.ToolTipText = "Vector2";
			this.vec2ToolStripButton.Value = typeof(ReClassNET.Nodes.Vector2Node);
			this.vec2ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// mat44ToolStripButton
			// 
			this.mat44ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mat44ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Matrix_4x4;
			this.mat44ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mat44ToolStripButton.Name = "mat44ToolStripButton";
			this.mat44ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.mat44ToolStripButton.ToolTipText = "4x4 Matrix";
			this.mat44ToolStripButton.Value = typeof(ReClassNET.Nodes.Matrix4x4Node);
			this.mat44ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// mat34ToolStripButton
			// 
			this.mat34ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mat34ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Matrix_3x4;
			this.mat34ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mat34ToolStripButton.Name = "mat34ToolStripButton";
			this.mat34ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.mat34ToolStripButton.ToolTipText = "3x4 Matrix";
			this.mat34ToolStripButton.Value = typeof(ReClassNET.Nodes.Matrix3x4Node);
			this.mat34ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// mat33ToolStripButton
			// 
			this.mat33ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.mat33ToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Matrix_3x3;
			this.mat33ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.mat33ToolStripButton.Name = "mat33ToolStripButton";
			this.mat33ToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.mat33ToolStripButton.ToolTipText = "3x3 Matrix";
			this.mat33ToolStripButton.Value = typeof(ReClassNET.Nodes.Matrix3x3Node);
			this.mat33ToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator13
			// 
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			this.toolStripSeparator13.Size = new System.Drawing.Size(6, 25);
			// 
			// utf8TextToolStripButton
			// 
			this.utf8TextToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.utf8TextToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Text;
			this.utf8TextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.utf8TextToolStripButton.Name = "utf8TextToolStripButton";
			this.utf8TextToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.utf8TextToolStripButton.ToolTipText = "UTF8 Text";
			this.utf8TextToolStripButton.Value = typeof(ReClassNET.Nodes.Utf8TextNode);
			this.utf8TextToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// utf8TextPtrToolStripButton
			// 
			this.utf8TextPtrToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.utf8TextPtrToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Text_Pointer;
			this.utf8TextPtrToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.utf8TextPtrToolStripButton.Name = "utf8TextPtrToolStripButton";
			this.utf8TextPtrToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.utf8TextPtrToolStripButton.ToolTipText = "Pointer to UTF8 text";
			this.utf8TextPtrToolStripButton.Value = typeof(ReClassNET.Nodes.Utf8TextPtrNode);
			this.utf8TextPtrToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// utf16TextToolStripButton
			// 
			this.utf16TextToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.utf16TextToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UText;
			this.utf16TextToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.utf16TextToolStripButton.Name = "utf16TextToolStripButton";
			this.utf16TextToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.utf16TextToolStripButton.ToolTipText = "UTF16 / Unicode Text";
			this.utf16TextToolStripButton.Value = typeof(ReClassNET.Nodes.Utf16TextNode);
			this.utf16TextToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// utf16TextPtrToolStripButton
			// 
			this.utf16TextPtrToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.utf16TextPtrToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_UText_Pointer;
			this.utf16TextPtrToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.utf16TextPtrToolStripButton.Name = "utf16TextPtrToolStripButton";
			this.utf16TextPtrToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.utf16TextPtrToolStripButton.ToolTipText = "Pointer to UTF16 / Unicode text";
			this.utf16TextPtrToolStripButton.Value = typeof(ReClassNET.Nodes.Utf16TextPtrNode);
			this.utf16TextPtrToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator14
			// 
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			this.toolStripSeparator14.Size = new System.Drawing.Size(6, 25);
			// 
			// classInstanceToolStripButton6
			// 
			this.classInstanceToolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.classInstanceToolStripButton6.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Instance;
			this.classInstanceToolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.classInstanceToolStripButton6.Name = "classInstanceToolStripButton6";
			this.classInstanceToolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.classInstanceToolStripButton6.ToolTipText = "Class instance";
			this.classInstanceToolStripButton6.Value = typeof(ReClassNET.Nodes.ClassInstanceNode);
			this.classInstanceToolStripButton6.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// classPtrToolStripButton
			// 
			this.classPtrToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.classPtrToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Class_Pointer;
			this.classPtrToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.classPtrToolStripButton.Name = "classPtrToolStripButton";
			this.classPtrToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.classPtrToolStripButton.ToolTipText = "Pointer to class instance";
			this.classPtrToolStripButton.Value = typeof(ReClassNET.Nodes.ClassPtrNode);
			this.classPtrToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator15
			// 
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			this.toolStripSeparator15.Size = new System.Drawing.Size(6, 25);
			// 
			// arrayToolStripButton
			// 
			this.arrayToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.arrayToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Array;
			this.arrayToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.arrayToolStripButton.Name = "arrayToolStripButton";
			this.arrayToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.arrayToolStripButton.ToolTipText = "Array of Classes";
			this.arrayToolStripButton.Value = typeof(ReClassNET.Nodes.ClassInstanceArrayNode);
			this.arrayToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// ptrArrayToolStripButton
			// 
			this.ptrArrayToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.ptrArrayToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Pointer_Array;
			this.ptrArrayToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.ptrArrayToolStripButton.Name = "ptrArrayToolStripButton";
			this.ptrArrayToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.ptrArrayToolStripButton.ToolTipText = "Array of Pointers";
			this.ptrArrayToolStripButton.Value = typeof(ReClassNET.Nodes.ClassPtrArrayNode);
			this.ptrArrayToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// vtableToolStripButton
			// 
			this.vtableToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.vtableToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_VTable;
			this.vtableToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.vtableToolStripButton.Name = "vtableToolStripButton";
			this.vtableToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.vtableToolStripButton.ToolTipText = "Pointer to VTable";
			this.vtableToolStripButton.Value = typeof(ReClassNET.Nodes.VTableNode);
			this.vtableToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// fnPtrToolStripButton
			// 
			this.fnPtrToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.fnPtrToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Function_Pointer;
			this.fnPtrToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fnPtrToolStripButton.Name = "fnPtrToolStripButton";
			this.fnPtrToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.fnPtrToolStripButton.ToolTipText = "Pointer to a function";
			this.fnPtrToolStripButton.Value = typeof(ReClassNET.Nodes.FunctionPtrNode);
			this.fnPtrToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// fnTypeToolStripButton
			// 
			this.fnTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.fnTypeToolStripButton.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Function;
			this.fnTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.fnTypeToolStripButton.Name = "fnTypeToolStripButton";
			this.fnTypeToolStripButton.Size = new System.Drawing.Size(23, 22);
			this.fnTypeToolStripButton.ToolTipText = "Function";
			this.fnTypeToolStripButton.Value = typeof(ReClassNET.Nodes.FunctionNode);
			this.fnTypeToolStripButton.Click += new System.EventHandler(this.memoryTypeToolStripButton_Click);
			// 
			// toolStripSeparator19
			// 
			this.toolStripSeparator19.Name = "toolStripSeparator19";
			this.toolStripSeparator19.Size = new System.Drawing.Size(6, 25);
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
			this.attachToProcessToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.attachToProcessToolStripMenuItem.Text = "Attach to Process...";
			this.attachToProcessToolStripMenuItem.Click += new System.EventHandler(this.attachToProcessToolStripSplitButton_ButtonClick);
			// 
			// reattachToProcessToolStripMenuItem
			// 
			this.reattachToProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier_Arrow;
			this.reattachToProcessToolStripMenuItem.Name = "reattachToProcessToolStripMenuItem";
			this.reattachToProcessToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.reattachToProcessToolStripMenuItem.Text = "<>";
			this.reattachToProcessToolStripMenuItem.Click += new System.EventHandler(this.reattachToProcessToolStripMenuItem_Click);
			// 
			// detachToolStripMenuItem
			// 
			this.detachToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Magnifier_Remove;
			this.detachToolStripMenuItem.Name = "detachToolStripMenuItem";
			this.detachToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.detachToolStripMenuItem.Text = "Detach";
			this.detachToolStripMenuItem.Click += new System.EventHandler(this.detachToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(243, 6);
			// 
			// openProjectToolStripMenuItem
			// 
			this.openProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Folder;
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			this.openProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.openProjectToolStripMenuItem.Text = "Open Project...";
			this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
			// 
			// mergeWithProjectToolStripMenuItem
			// 
			this.mergeWithProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Folder_Add;
			this.mergeWithProjectToolStripMenuItem.Name = "mergeWithProjectToolStripMenuItem";
			this.mergeWithProjectToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.mergeWithProjectToolStripMenuItem.Text = "Merge with Project...";
			this.mergeWithProjectToolStripMenuItem.Click += new System.EventHandler(this.mergeWithProjectToolStripMenuItem_Click);
			// 
			// clearProjectToolStripMenuItem
			// 
			this.clearProjectToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Arrow_Refresh;
			this.clearProjectToolStripMenuItem.Name = "clearProjectToolStripMenuItem";
			this.clearProjectToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.clearProjectToolStripMenuItem.Text = "Clear Project";
			this.clearProjectToolStripMenuItem.Click += new System.EventHandler(this.clearProjectToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(243, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Save;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Save_As;
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.saveAsToolStripMenuItem.Text = "Save as...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(243, 6);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Cogs;
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.settingsToolStripMenuItem.Text = "Settings...";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// pluginsToolStripMenuItem
			// 
			this.pluginsToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Plugin;
			this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
			this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.pluginsToolStripMenuItem.Text = "Plugins...";
			this.pluginsToolStripMenuItem.Click += new System.EventHandler(this.pluginsToolStripButton_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(243, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Quit;
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(246, 22);
			this.quitToolStripMenuItem.Text = "Quit";
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
			// 
			// processToolStripMenuItem
			// 
			this.processToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processInformationsToolStripMenuItem,
            this.memorySearcherToolStripMenuItem,
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
			this.processInformationsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.processInformationsToolStripMenuItem.Text = "Process Informations";
			this.processInformationsToolStripMenuItem.Click += new System.EventHandler(this.memoryViewerToolStripMenuItem_Click);
			// 
			// memorySearcherToolStripMenuItem
			// 
			this.memorySearcherToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Eye;
			this.memorySearcherToolStripMenuItem.Name = "memorySearcherToolStripMenuItem";
			this.memorySearcherToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.memorySearcherToolStripMenuItem.Text = "Memory Searcher";
			this.memorySearcherToolStripMenuItem.Click += new System.EventHandler(this.memorySearcherToolStripMenuItem_Click);
			// 
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(182, 6);
			// 
			// loadSymbolToolStripMenuItem
			// 
			this.loadSymbolToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Pdb;
			this.loadSymbolToolStripMenuItem.Name = "loadSymbolToolStripMenuItem";
			this.loadSymbolToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadSymbolToolStripMenuItem.Text = "Load Symbol...";
			this.loadSymbolToolStripMenuItem.Click += new System.EventHandler(this.loadSymbolToolStripMenuItem_Click);
			// 
			// loadSymbolsToolStripMenuItem
			// 
			this.loadSymbolsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("loadSymbolsToolStripMenuItem.Image")));
			this.loadSymbolsToolStripMenuItem.Name = "loadSymbolsToolStripMenuItem";
			this.loadSymbolsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.loadSymbolsToolStripMenuItem.Text = "Load all Symbols";
			this.loadSymbolsToolStripMenuItem.Click += new System.EventHandler(this.loadSymbolsToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(182, 6);
			// 
			// resumeProcessToolStripMenuItem
			// 
			this.resumeProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Play;
			this.resumeProcessToolStripMenuItem.Name = "resumeProcessToolStripMenuItem";
			this.resumeProcessToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.resumeProcessToolStripMenuItem.Text = "Resume";
			this.resumeProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// suspendProcessToolStripMenuItem
			// 
			this.suspendProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Pause;
			this.suspendProcessToolStripMenuItem.Name = "suspendProcessToolStripMenuItem";
			this.suspendProcessToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.suspendProcessToolStripMenuItem.Text = "Suspend";
			this.suspendProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// terminateProcessToolStripMenuItem
			// 
			this.terminateProcessToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Control_Stop;
			this.terminateProcessToolStripMenuItem.Name = "terminateProcessToolStripMenuItem";
			this.terminateProcessToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
			this.terminateProcessToolStripMenuItem.Text = "Kill";
			this.terminateProcessToolStripMenuItem.Click += new System.EventHandler(this.ControlRemoteProcessToolStripMenuItem_Click);
			// 
			// projectToolStripMenuItem
			// 
			this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cleanUnusedClassesToolStripMenuItem,
            this.toolStripSeparator16,
            this.generateCppCodeToolStripMenuItem,
            this.generateCSharpCodeToolStripMenuItem});
			this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
			this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
			this.projectToolStripMenuItem.Text = "Project";
			// 
			// cleanUnusedClassesToolStripMenuItem
			// 
			this.cleanUnusedClassesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Chart_Delete;
			this.cleanUnusedClassesToolStripMenuItem.Name = "cleanUnusedClassesToolStripMenuItem";
			this.cleanUnusedClassesToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.cleanUnusedClassesToolStripMenuItem.Text = "Remove unused classes";
			this.cleanUnusedClassesToolStripMenuItem.Click += new System.EventHandler(this.cleanUnusedClassesToolStripMenuItem_Click);
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
			this.generateCppCodeToolStripMenuItem.Text = "Generate C++ Code";
			this.generateCppCodeToolStripMenuItem.Click += new System.EventHandler(this.generateCppCodeToolStripMenuItem_Click);
			// 
			// generateCSharpCodeToolStripMenuItem
			// 
			this.generateCSharpCodeToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Page_Code_Csharp;
			this.generateCSharpCodeToolStripMenuItem.Name = "generateCSharpCodeToolStripMenuItem";
			this.generateCSharpCodeToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
			this.generateCSharpCodeToolStripMenuItem.Text = "Generate C# Code";
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
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private UI.TypeToolStripButton hex64ToolStripButton;
		private UI.TypeToolStripButton hex32ToolStripButton;
		private UI.TypeToolStripButton hex16ToolStripButton;
		private UI.TypeToolStripButton hex8ToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private UI.TypeToolStripButton int64ToolStripButton;
		private UI.TypeToolStripButton int32ToolStripButton;
		private UI.TypeToolStripButton int16ToolStripButton;
		private UI.TypeToolStripButton int8ToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private UI.TypeToolStripButton uint64ToolStripButton;
		private UI.TypeToolStripButton uint32ToolStripButton;
		private UI.TypeToolStripButton uint16ToolStripButton;
		private UI.TypeToolStripButton uint8ToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
		private UI.TypeToolStripButton floatToolStripButton;
		private UI.TypeToolStripButton doubleToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private UI.TypeToolStripButton vec4ToolStripButton;
		private UI.TypeToolStripButton vec3ToolStripButton;
		private UI.TypeToolStripButton vec2ToolStripButton;
		private UI.TypeToolStripButton mat44ToolStripButton;
		private UI.TypeToolStripButton mat34ToolStripButton;
		private UI.TypeToolStripButton mat33ToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private UI.TypeToolStripButton utf8TextToolStripButton;
		private UI.TypeToolStripButton utf8TextPtrToolStripButton;
		private UI.TypeToolStripButton utf16TextToolStripButton;
		private UI.TypeToolStripButton utf16TextPtrToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private UI.TypeToolStripButton classInstanceToolStripButton6;
		private UI.TypeToolStripButton classPtrToolStripButton;
		private UI.TypeToolStripButton arrayToolStripButton;
		private UI.TypeToolStripButton vtableToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private UI.TypeToolStripButton fnPtrToolStripButton;
		private UI.ClassNodeView classesView;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cleanUnusedClassesToolStripMenuItem;
		private UI.TypeToolStripButton ptrArrayToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
		private System.Windows.Forms.ToolStripMenuItem generateCppCodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateCSharpCodeToolStripMenuItem;
		private System.Windows.Forms.Timer processUpdateTimer;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
		private System.Windows.Forms.ToolStripMenuItem loadSymbolToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton openProjectToolStripButton;
		private UI.TypeToolStripButton bitFieldToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
		private System.Windows.Forms.ToolStripStatusLabel infoToolStripStatusLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
		private System.Windows.Forms.ToolStripMenuItem mergeWithProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem detachToolStripMenuItem;
		private UI.TypeToolStripButton boolToolStripButton;
		private UI.TypeToolStripButton fnTypeToolStripButton;
		private System.Windows.Forms.ToolStripMenuItem memorySearcherToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reattachToProcessToolStripMenuItem;
		private System.Windows.Forms.ToolStripSplitButton attachToProcessToolStripSplitButton;
	}
}

