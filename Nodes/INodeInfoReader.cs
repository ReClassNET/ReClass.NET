using System;

namespace ReClassNET.Nodes
{
	public interface INodeInfoReader
	{
		string ReadNodeInfo(BaseNode node, IntPtr value, Memory memory);
	}
}
