using System;
using System.Drawing;
using ReClassNET.Extensions;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class ClassInstanceNode : BaseReferenceNode
	{
		public override int MemorySize => InnerNode.MemorySize;

		public override bool PerformCycleCheck => true;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Class Instance";
			icon = Properties.Resources.B16x16_Button_Class_Instance;
		}

		public override void Intialize()
		{
			InnerNode = ClassNode.Create();
			InnerNode.Intialize();
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			DrawInvalidMemoryIndicator(view, y);

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Class, -1, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Instance") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<{InnerNode.Name}>") + view.Font.Width;
			x = AddIcon(view, x, y, Icons.Change, 4, HotSpotType.ChangeClassType) + view.Font.Width;

			x = AddComment(view, x, y);

			AddTypeDrop(view, y);
			AddDelete(view, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (levelsOpen[view.Level])
			{
				var v = view.Clone();
				v.Address = view.Address.Add(Offset);
				v.Memory = view.Memory.Clone();
				v.Memory.Offset += Offset.ToInt32();

				var innerSize = InnerNode.Draw(v, tx, y);
				size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
				size.Height += innerSize.Height;
			}

			return size;
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				height += InnerNode.CalculateDrawnHeight(view);
			}
			return height;
		}
	}
}
