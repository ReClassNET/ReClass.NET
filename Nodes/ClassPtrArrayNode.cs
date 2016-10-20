using System;
using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class ClassPtrArrayNode : BaseArrayNode
	{
		private readonly Memory memory = new Memory();

		public override int MemorySize => IntPtr.Size * Count;

		public override void Intialize()
		{
			InnerNode = new ClassNode();
			InnerNode.Intialize();
		}

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
