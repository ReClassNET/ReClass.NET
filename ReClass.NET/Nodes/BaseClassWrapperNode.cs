namespace ReClassNET.Nodes
{
	public abstract class BaseClassWrapperNode : BaseWrapperNode
	{
		public override void Initialize()
		{
			InnerNode = ClassNode.Create();
			InnerNode.Initialize();
		}

		public override bool CanChangeInnerNodeTo(BaseNode node)
		{
			return node is ClassNode;
		}
	}
}
