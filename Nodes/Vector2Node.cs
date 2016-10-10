using System.Runtime.InteropServices;

namespace ReClassNET.Nodes
{
	class Vector2Node : BaseVecNode
	{
		[StructLayout(LayoutKind.Explicit)]
		struct Vector2Data
		{
			[FieldOffset(0)]
			public float X;
			[FieldOffset(4)]
			public float Y;
		}

		public override int MemorySize => 2 * 4;

		public override int Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TXOFFSET;

			x = AddIcon(view, x, y, Icons.Vector, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, "Vec2 ");
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name);
			x = AddOpenClose(view, x, y);

			if (levelsOpen[view.Level])
			{
				var value = view.Memory.ReadObject<Vector2Data>(Offset);

				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, "(");
				x = AddText(view, x, y, view.Settings.Value, 0, $"{value.X:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ",");
				x = AddText(view, x, y, view.Settings.Value, 1, $"{value.Y:0.000}");
				x = AddText(view, x, y, view.Settings.Name, HotSpot.NoneId, ")");
			}

			x += view.Font.Width;

			x = AddComment(view, x, y);

			return y + view.Font.Height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			Update(spot, 2);
		}
	}
}
