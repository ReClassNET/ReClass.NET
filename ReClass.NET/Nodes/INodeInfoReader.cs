using System;
using System.Diagnostics.Contracts;
using ReClassNET.Memory;

namespace ReClassNET.Nodes
{
	[ContractClass(typeof(NodeInfoReaderContract))]
	public interface INodeInfoReader
	{
		/// <summary>
		/// Used to print custom informations about the node.
		/// </summary>
		/// <param name="node">The current node.</param>
		/// <param name="nodeAddress">The absolute address of the node.</param>
		/// /// <param name="nodeValue">The memory value of the node as <see cref="IntPtr"/>.</param>
		/// <param name="memory">The current <see cref="MemoryBuffer"/>.</param>
		/// <returns>Custom informations about the node or null.</returns>
		string ReadNodeInfo(BaseNode node, IntPtr nodeAddress, IntPtr nodeValue, MemoryBuffer memory);
	}

	[ContractClassFor(typeof(INodeInfoReader))]
	internal abstract class NodeInfoReaderContract : INodeInfoReader
	{
		public string ReadNodeInfo(BaseNode node, IntPtr nodeAddress, IntPtr nodeValue, MemoryBuffer memory)
		{
			Contract.Requires(node != null);
			Contract.Requires(memory != null);

			throw new NotImplementedException();
		}
	}
}
