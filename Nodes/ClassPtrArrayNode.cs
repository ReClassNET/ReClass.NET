using System;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ClassPtrArrayNode : BaseArrayNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => IntPtr.Size * Count;

		public override bool PerformCycleCheck => true;

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
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "PtrArray", HotSpotType.ChangeType);
		}

		protected override int DrawChild(ViewInfo view, int x, int y)
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

		protected override int CalculateChildHeight(ViewInfo view)
		{
			return InnerNode.CalculateHeight(view);
		}
	}
}
