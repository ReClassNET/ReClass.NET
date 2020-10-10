namespace ReClassNET.Controls
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
            this.repaintTimer = new System.Windows.Forms.Timer(this.components);
            this.hotSpotEditBox = new HotSpotTextBox();
            this.nodeInfoToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // repaintTimer
            // 
            this.repaintTimer.Enabled = true;
            this.repaintTimer.Interval = 250;
            this.repaintTimer.Tick += new System.EventHandler(this.repaintTimer_Tick);
            // 
            // hotSpotEditBox
            // 
            this.hotSpotEditBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hotSpotEditBox.Location = new System.Drawing.Point(0, 0);
            this.hotSpotEditBox.Name = "hotSpotEditBox";
            this.hotSpotEditBox.Size = new System.Drawing.Size(100, 13);
            this.hotSpotEditBox.TabIndex = 1;
            this.hotSpotEditBox.TabStop = false;
            this.hotSpotEditBox.Visible = false;
            this.hotSpotEditBox.Committed += new HotSpotTextBoxCommitEventHandler(this.editBox_Committed);
            // 
            // nodeInfoToolTip
            // 
            this.nodeInfoToolTip.ShowAlways = true;
            // 
            // MemoryViewControl
            // 
            this.Controls.Add(this.hotSpotEditBox);
            this.DoubleBuffered = true;
            this.Name = "MemoryViewControl";
            this.Size = new System.Drawing.Size(150, 162);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer repaintTimer;
		private HotSpotTextBox hotSpotEditBox;
		private System.Windows.Forms.ToolTip nodeInfoToolTip;
	}
}
