using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;

namespace ReClassNET.Nodes
{
	[ContractClass(typeof(NodeInfoReaderContract))]
	public interface INodeInfoReader
	{
		/// <summary>
		/// Used to print custom informations about a node.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="reader">The current <see cref="IRemoteMemoryReader"/>.</param>
		/// <param name="memory">The current <see cref="MemoryBuffer"/>.</param>
		/// <param name="nodeAddress">The absolute address of the node.</param>
		/// <param name="nodeValue">The memory value of the node as <see cref="IntPtr"/>.</param>
		/// <returns>Custom informations about the node or null.</returns>
		string ReadNodeInfo(BaseHexCommentNode node, IRemoteMemoryReader reader, MemoryBuffer memory, IntPtr nodeAddress, IntPtr nodeValue);
	}

	[ContractClassFor(typeof(INodeInfoReader))]
	internal abstract class NodeInfoReaderContract : INodeInfoReader
	{
		public string ReadNodeInfo(BaseHexCommentNode node, IRemoteMemoryReader reader, MemoryBuffer memory, IntPtr nodeAddress, IntPtr nodeValue)
		{
			Contract.Requires(node != null);
			Contract.Requires(reader != null);
			Contract.Requires(memory != null);

			throw new NotImplementedException();
		}
	}
}
