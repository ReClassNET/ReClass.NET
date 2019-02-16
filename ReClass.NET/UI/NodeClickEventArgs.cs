using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	public class NodeClickEventArgs : EventArgs
	{
		public BaseNode Node { get; }

		public MouseButtons Button { get; }

		public Point Location { get; }

		public NodeClickEventArgs(BaseNode node, MouseButtons button, Point location)
		{
			Contract.Requires(node != null);

			Node = node;
			Button = button;
			Location = location;
		}
	}

	public delegate void NodeClickEventHandler(object sender, NodeClickEventArgs args);
}
