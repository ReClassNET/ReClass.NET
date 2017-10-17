using System;
using ReClassNET.Memory;

namespace ReClassNET.Nodes
{
	public interface INodeInfoReader
	{
		string ReadNodeInfo(BaseNode node, IntPtr value, MemoryBuffer memory);
	}
}
