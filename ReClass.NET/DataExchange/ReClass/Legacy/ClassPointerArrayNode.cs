using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public class ClassPointerArrayNode : BaseClassArrayNode
	{
		protected override bool PerformCycleCheck => false;

		public override BaseNode GetEquivalentNode(int count, ClassNode classNode)
		{
			var classInstanceNode = new ClassInstanceNode();
			classInstanceNode.ChangeInnerNode(classNode);

			var pointerNode = new PointerNode();
			pointerNode.ChangeInnerNode(classInstanceNode);

			var arrayNode = new ArrayNode { Count = count };
			arrayNode.ChangeInnerNode(pointerNode);
			arrayNode.CopyFromNode(this);

			return arrayNode;
		}
	}
}
