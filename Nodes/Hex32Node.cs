namespace ReClassNET.Nodes
{
	class Hex32Node : BaseHexCommentNode
	{
		public override int MemorySize => 4;

		public override string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			var value = memory.ReadObject<UInt32FloatData>(Offset);

			return $"Int32: {value.IntValue}\nDWORD: 0x{value.UIntValue:X08}\nFloat: {value.FloatValue:0.000}";
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			return Draw(view, x, y, view.Settings.ShowText ? view.Memory.ReadPrintableASCIIString(Offset, 4) + "     " : null, 4);
		}

		public override void Update(HotSpot spot)
		{
			Update(spot, 4);
		}

		protected override int AddComment(ViewInfo view, int x, int y)
		{
			x = base.AddComment(view, x, y);

			var value = view.Memory.ReadObject<UInt32FloatData>(Offset);

			x = AddComment(view, x, y, value.FloatValue, value.IntPtr, value.UIntPtr);

			return x;
		}
	}
}
