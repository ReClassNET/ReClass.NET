using ReClassNET.Nodes;

namespace ReClassNET.DataExchange.ReClass.Legacy
{
	public class ClassInstanceArrayNode : BaseClassArrayNode
	{
		protected override bool PerformCycleCheck => true;

		public override BaseNode GetEquivalentNode(int count, ClassNode classNode)
		{
			var classInstanceNode = new ClassInstanceNode();
			classInstanceNode.ChangeInnerNode(classNode);

			var arrayNode = new ArrayNode { Count = count };
			arrayNode.ChangeInnerNode(classInstanceNode);
			arrayNode.CopyFromNode(this);

			return arrayNode;
		}
	}
}
