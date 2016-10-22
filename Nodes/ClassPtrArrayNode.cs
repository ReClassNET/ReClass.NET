using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class ClassPtrArrayNode : BaseArrayNode
	{
		private readonly Memory memory = new Memory();

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size * Count;

		public override void Intialize()
		{
			var node = new ClassNode();
			node.Intialize();
			node.AddBytes(64);
			InnerNode = node;
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, "PtrArray", HotSpotType.ChangeAll);
		}

		protected override int DrawChild(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			var ptr = view.Memory.ReadObject<IntPtr>(Offset + InnerNode.MemorySize * CurrentIndex);

			memory.Size = InnerNode.MemorySize;
			memory.Process = view.Memory.Process;
			memory.Update(ptr);

			var v = view.Clone();
			v.Address = ptr;
			v.Memory = memory;

			return InnerNode.Draw(v, x, y);
		}
	}
}
