using System;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public interface INodeInfoReader
	{
		string ReadNodeInfo(BaseNode node, IntPtr value, Memory memory);
	}
}
