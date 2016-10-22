using System;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	class IntegerToolStripMenuItem : ToolStripMenuItem
	{
		public int Value { get; set; }
	}

	class TypeToolStripMenuItem : ToolStripMenuItem
	{
		public Type Value { get; set; }
	}

	class TypeToolStripButton : ToolStripButton
	{
		public Type Value { get; set; }
	}
}
