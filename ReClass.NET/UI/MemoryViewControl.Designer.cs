namespace ReClassNET.UI
{
	partial class MemoryViewControl
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
			this.selectedNodeContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.changeTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.add4BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add8BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add64BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add256BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add1024BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add2048BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.add4096BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insertBytesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.insert4BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert8BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert64BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert256BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert1024BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert2048BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.insert4096BytesToolStripMenuItem = new ReClassNET.UI.IntegerToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.createClassFromNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
			this.dissectNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.searchForEqualValuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
			this.findOutWhatAccessesThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findOutWhatWritesToThisAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
			this.copyNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
			this.hideNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideChildNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesAboveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.unhideNodesBelowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator18 = new System.Windows.Forms.ToolStripSeparator();
			this.copyAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
			this.showCodeOfClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shrinkClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.repaintTimer = new System.Windows.Forms.Timer(this.components);
			this.editBox = new ReClassNET.UI.HotSpotTextBox();
			this.nodeInfoToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.selectedNodeContextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// selectedNodeContextMenuStrip
			// 
			this.selectedNodeContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeTypeToolStripMenuItem,
            this.addBytesToolStripMenuItem,
            this.insertBytesToolStripMenuItem,
            this.toolStripSeparator1,
            this.createClassFromNodesToolStripMenuItem,
            this.toolStripSeparator13,
            this.dissectNodesToolStripMenuItem,
            this.toolStripSeparator2,
            this.searchForEqualValuesToolStripMenuItem,
            this.toolStripSeparator15,
            this.findOutWhatAccessesThisAddressToolStripMenuItem,
            this.findOutWhatWritesToThisAddressToolStripMenuItem,
            this.toolStripSeparator14,
            this.copyNodeToolStripMenuItem,
            this.pasteNodesToolStripMenuItem,
            this.toolStripSeparator17,
            this.removeToolStripMenuItem,
            this.toolStripSeparator12,
            this.hideNodesToolStripMenuItem,
            this.unhideNodesToolStripMenuItem,
            this.toolStripSeparator18,
            this.copyAddressToolStripMenuItem,
            this.toolStripSeparator16,
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
            this.add4BytesToolStripMenuItem,
            this.add8BytesToolStripMenuItem,
            this.add64BytesToolStripMenuItem,
            this.add256BytesToolStripMenuItem,
            this.add1024BytesToolStripMenuItem,
            this.add2048BytesToolStripMenuItem,
            this.add4096BytesToolStripMenuItem});
			this.addBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_X;
			this.addBytesToolStripMenuItem.Name = "addBytesToolStripMenuItem";
			this.addBytesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.addBytesToolStripMenuItem.Text = "Add Bytes";
			// 
			// add4BytesToolStripMenuItem
			// 
			this.add4BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Add_Bytes_4;
			this.add4BytesToolStripMenuItem.Name = "add4BytesToolStripMenuItem";
			this.add4BytesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
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
			// insertBytesToolStripMenuItem
			// 
			this.insertBytesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.insert4BytesToolStripMenuItem,
            this.insert8BytesToolStripMenuItem,
            this.insert64BytesToolStripMenuItem,
            this.insert256BytesToolStripMenuItem,
            this.insert1024BytesToolStripMenuItem,
            this.insert2048BytesToolStripMenuItem,
            this.insert4096BytesToolStripMenuItem});
			this.insertBytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_X;
			this.insertBytesToolStripMenuItem.Name = "insertBytesToolStripMenuItem";
			this.insertBytesToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
			this.insertBytesToolStripMenuItem.Text = "Insert Bytes";
			// 
			// insert4BytesToolStripMenuItem
			// 
			this.insert4BytesToolStripMenuItem.Image = global::ReClassNET.Properties.Resources.B16x16_Button_Insert_Bytes_4;
			this.insert4BytesToolStripMenuItem.Name = "insert4BytesToolStripMenuItem";
			this.insert4BytesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(266, 6);
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
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(266, 6);
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
			// toolStripSeparator17
			// 
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			this.toolStripSeparator17.Size = new System.Drawing.Size(266, 6);
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
			this.unhideNodesToolStripMenuItem.Click += new System.EventHandler(this.unhideChildNodesToolStripMenuItem_Click);
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
			// toolStripSeparator16
			// 
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			this.toolStripSeparator16.Size = new System.Drawing.Size(266, 6);
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
			// repaintTimer
			// 
			this.repaintTimer.Enabled = true;
			this.repaintTimer.Interval = 250;
			this.repaintTimer.Tick += new System.EventHandler(this.repaintTimer_Tick);
			// 
			// editBox
			// 
			this.editBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.editBox.Location = new System.Drawing.Point(36, 81);
			this.editBox.MinimumWidth = 0;
			this.editBox.Name = "editBox";
			this.editBox.Size = new System.Drawing.Size(100, 13);
			this.editBox.TabIndex = 1;
			this.editBox.TabStop = false;
			this.editBox.Visible = false;
			this.editBox.Committed += new System.EventHandler(this.editBox_Committed);
			// 
			// nodeInfoToolTip
			// 
			this.nodeInfoToolTip.ShowAlways = true;
			// 
			// MemoryViewControl
			// 
			this.Controls.Add(this.editBox);
			this.DoubleBuffered = true;
			this.Name = "MemoryViewControl";
			this.Size = new System.Drawing.Size(150, 162);
			this.selectedNodeContextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip selectedNodeContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem changeTypeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem addBytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add4BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add8BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add64BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add256BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add1024BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add2048BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem add4096BytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem insertBytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert4BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert8BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert64BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert256BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert1024BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert2048BytesToolStripMenuItem;
		private UI.IntegerToolStripMenuItem insert4096BytesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyAddressToolStripMenuItem;
		private System.Windows.Forms.Timer repaintTimer;
		private HotSpotTextBox editBox;
		private System.Windows.Forms.ToolTip nodeInfoToolTip;
		private System.Windows.Forms.ToolStripMenuItem copyNodeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unhideNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
		private System.Windows.Forms.ToolStripMenuItem dissectNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem createClassFromNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatAccessesThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findOutWhatWritesToThisAddressToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
		private System.Windows.Forms.ToolStripMenuItem searchForEqualValuesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
		private System.Windows.Forms.ToolStripMenuItem showCodeOfClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
		private System.Windows.Forms.ToolStripMenuItem shrinkClassToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
		private System.Windows.Forms.ToolStripMenuItem unhideChildNodesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideNodesAboveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem unhideNodesBelowToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator18;
	}
}
