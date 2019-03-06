using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	public class NodeClickEventArgs : EventArgs
	{
		public BaseNode Node { get; }

		public IntPtr Address { get; }

		public MemoryBuffer Memory { get; }

		public MouseButtons Button { get; }

		public Point Location { get; }

		public NodeClickEventArgs(BaseNode node, IntPtr address, MemoryBuffer memory, MouseButtons button, Point location)
		{
			Contract.Requires(node != null);
			Contract.Requires(memory != null);

			Node = node;
			Address = address;
			Memory = memory;
			Button = button;
			Location = location;
		}
	}

	public delegate void NodeClickEventHandler(object sender, NodeClickEventArgs args);
}
