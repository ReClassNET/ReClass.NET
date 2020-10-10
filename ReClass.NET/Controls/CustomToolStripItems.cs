using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.UI
{
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class IntegerToolStripMenuItem : ToolStripMenuItem
	{
		public int Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class TypeToolStripMenuItem : ToolStripMenuItem
	{
		public Type Value { get; set; }
	}

	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class TypeToolStripButton : ToolStripButton
	{
		public Type Value { get; set; }
	}
}
