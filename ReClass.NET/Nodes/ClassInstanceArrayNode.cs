using System.Drawing;
using ReClassNET.Extensions;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ClassInstanceArrayNode : BaseArrayNode
	{
		public override int MemorySize => InnerNode.MemorySize * Count;

		public override bool PerformCycleCheck => true;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Class Instance Array";
			icon = Properties.Resources.B16x16_Button_Array;
		}

		public override void Intialize()
		{
			InnerNode = ClassNode.Create();
			InnerNode.Intialize();
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, "Array", HotSpotType.ChangeClassType);
		}

		protected override Size DrawChild(ViewInfo view, int x, int y)
		{
			var v = view.Clone();
			v.Address = view.Address.Add(Offset) + InnerNode.MemorySize * CurrentIndex;
			v.Memory = view.Memory.Clone();
			v.Memory.Offset += Offset.ToInt32() + InnerNode.MemorySize * CurrentIndex;

			return InnerNode.Draw(v, x, y);
		}
	}
}
