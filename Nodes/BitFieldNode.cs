using System;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace ReClassNET.Nodes
{
	class BitFieldNode : BaseNode
	{
		private int size;
		private int bits;

		public int Bits
		{
			get { return bits; }
			set
			{
				if (value >= 64)
				{
					bits = 64;
				}
				else if (value >= 32)
				{
					bits = 32;
				}
				else if (value >= 16)
				{
					bits = 16;
				}
				else
				{
					bits = 8;
				}

				size = bits / 8;
			}
		}

		public override int MemorySize => size;

		public BitFieldNode()
		{
			Bits = IntPtr.Size * 8;
		}

		public override void CopyFromNode(BaseNode node)
		{
			Contract.Requires(node != null);

			Bits = node.MemorySize * 8;
		}

		private string ConvertValueToBitString(Memory memory)
		{
			Contract.Requires(memory != null);

			string str;
			switch(bits)
			{
				case 64:
					str = Convert.ToString(memory.ReadObject<long>(Offset), 2);
					break;
				case 32:
					str = Convert.ToString(memory.ReadObject<int>(Offset), 2);
					break;
				case 16:
					str = Convert.ToString(memory.ReadObject<short>(Offset), 2);
					break;
				default:
					str = Convert.ToString(memory.ReadObject<sbyte>(Offset), 2);
					break;
			}
			return str.PadLeft(bits, '0');
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			if (IsHidden)
			{
				return DrawHidden(view, x, y);
			}

			AddSelection(view, x, y, view.Font.Height);
			AddDelete(view, x, y);
			AddTypeDrop(view, x, y);

			x += TXOFFSET + 16;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, "Bits") + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			var tx = x - 3;

			x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, ConvertValueToBitString(view.Memory)) + view.Font.Width;

			x += view.Font.Width;

			x = AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				y += view.Font.Height;

				var format = new StringFormat(StringFormatFlags.DirectionVertical);

				using (var brush = new SolidBrush(Program.Settings.ValueColor))
				{
					view.Context.DrawString("1", view.Font.Font, brush, tx + (bits - 1) * view.Font.Width, y, format);

					for (var i = 8; i <= bits; i += 8)
					{
						view.Context.DrawString(i.ToString(), view.Font.Font, brush, tx  + (bits - i) * view.Font.Width, y, format);
					}
				}

				y += 2;
			}

			return y + view.Font.Height;
		}
	}
}
