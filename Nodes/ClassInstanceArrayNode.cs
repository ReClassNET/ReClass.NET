using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class ClassInstanceArrayNode : BaseArrayNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => InnerNode.MemorySize * Count;

		public override bool PerformCycleCheck => true;

		public override void Intialize()
		{
			InnerNode = ClassNode.Create();
			InnerNode.Intialize();
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "Array", HotSpotType.ChangeType);
		}

		protected override int DrawChild(ViewInfo view, int x, int y)
		{
			var v = view.Clone();
			v.Address = view.Address.Add(Offset) + InnerNode.MemorySize * CurrentIndex;
			v.Memory = view.Memory.Clone();
			v.Memory.Offset = Offset.ToInt32() + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(v, x, y);
		}

		protected override int CalculateChildHeight(ViewInfo view)
		{
			return InnerNode.CalculateHeight(view);
		}
	}
}
