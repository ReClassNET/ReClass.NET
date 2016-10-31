using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.UI
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	class IntegerToolStripMenuItem : ToolStripMenuItem
	{
		public int Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	class TypeToolStripMenuItem : ToolStripMenuItem
	{
		public Type Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	class TypeToolStripButton : ToolStripButton
	{
		public Type Value { get; set; }
	}
}
