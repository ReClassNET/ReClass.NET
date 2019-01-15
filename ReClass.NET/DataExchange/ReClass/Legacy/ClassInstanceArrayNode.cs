using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public class ClassInstanceArrayNode : ClassArrayNode
	{
		protected override bool PerformCycleCheck => true;

		public override BaseNode GetEquivalentNode(int count, ClassNode classNode)
		{
			var arrayNode = new ArrayNode { Count = count };

			var classInstanceNode = new ClassInstanceNode();
			classInstanceNode.CanChangeInnerNodeTo(classNode);

			arrayNode.ChangeInnerNode(classInstanceNode);

			return arrayNode;
		}
	}
}
