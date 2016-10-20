using System.Diagnostics.Contracts;

namespace ReClassNET.Nodes
{
	class ClassInstanceArrayNode : BaseArrayNode
	{
		public override int MemorySize => InnerNode.MemorySize * Count;

		public override void Intialize()
		{
			InnerNode = new ClassNode();
			InnerNode.Intialize();
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			return Draw(view, x, y, "Array", HotSpotType.ChangeSkipParent);
		}

		protected override int DrawChild(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			var v = view.Clone();
			v.Address = view.Address.Add(Offset) + InnerNode.MemorySize * CurrentIndex;
			v.Memory = view.Memory.Clone();
			v.Memory.Offset = Offset.ToInt32() + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(v, x, y);
		}
	}
}
