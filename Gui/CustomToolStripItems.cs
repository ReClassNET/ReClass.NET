using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET.Gui
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
