using System;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ClassPtrArrayNode : BaseArrayNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size * Count;

		public override bool PerformCycleCheck => false;

		public override void Intialize()
		{
			var node = ClassNode.Create();
			node.Intialize();
			node.AddBytes(64);
			InnerNode = node;
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "PtrArray", HotSpotType.ChangeType);
		}

		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			var ptr = view.Memory.ReadObject<IntPtr>(Offset + IntPtr.Size * CurrentIndex);

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
