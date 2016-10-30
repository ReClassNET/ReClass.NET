using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.UI
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	class IntegerToolStripMenuItem : ToolStripMenuItem
	{
		public int Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	class TypeToolStripMenuItem : ToolStripMenuItem
	{
		public Type Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.MenuStrip | ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.ContextMenuStrip)]
	class TypeToolStripButton : ToolStripButton
	{
		public Type Value { get; set; }
	}
}
