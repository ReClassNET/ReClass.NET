using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class BitFieldNode : BaseNode
	{
		private int size;
		private int bits;

		/// <summary>Gets or sets the bit count.</summary>
		/// <value>Possible values: 64, 32, 16, 8</value>
		public int Bits
		{
			get => bits;
			set
			{
				Contract.Ensures(bits > 0);
				Contract.Ensures(size > 0);

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

			levelsOpen.DefaultValue = true;
		}

		public override void CopyFromNode(BaseNode node)
		{
			base.CopyFromNode(node);

			Bits = node.MemorySize * 8;
		}

		/// <summary>Converts the memory value to a bit string.</summary>
		/// <param name="memory">The process memory.</param>
		/// <returns>The value converted to a bit string.</returns>
		private string ConvertValueToBitString(MemoryBuffer memory)
		{
			Contract.Requires(memory != null);
			Contract.Ensures(Contract.Result<string>() != null);

			string str;
			switch(bits)
			{
				case 64:
					str = Convert.ToString(memory.ReadInt64(Offset), 2);
					break;
				case 32:
					str = Convert.ToString(memory.ReadInt32(Offset), 2);
					break;
				case 16:
					str = Convert.ToString(memory.ReadInt16(Offset), 2);
					break;
				default:
					str = Convert.ToString(memory.ReadUInt8(Offset), 2);
					break;
			}
			return str.PadLeft(bits, '0');
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

			x += TextPadding + Icons.Dimensions;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Bits") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;

			x = AddOpenClose(view, x, y) + view.Font.Width;

			var tx = x - 3;

			for (var i = 0; i < bits; ++i)
			{
				var rect = new Rectangle(x + i * view.Font.Width, y, view.Font.Width, view.Font.Height);
				AddHotSpot(view, rect, string.Empty, i, HotSpotType.Edit);
			}
			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, ConvertValueToBitString(view.Memory)) + view.Font.Width;

			x += view.Font.Width;

			x = AddComment(view, x, y);

			if (levelsOpen[view.Level])
			{
				y += view.Font.Height;

				var format = new StringFormat(StringFormatFlags.DirectionVertical);

				using (var brush = new SolidBrush(view.Settings.ValueColor))
				{
					view.Context.DrawString("1", view.Font.Font, brush, tx + (bits - 1) * view.Font.Width + 1, y, format);

					for (var i = 8; i <= bits; i += 8)
					{
						view.Context.DrawString(i.ToString(), view.Font.Font, brush, tx  + (bits - i) * view.Font.Width, y, format);
					}
				}

				y += 2;
			}

			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x - origX, y - origY + view.Font.Height);
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
				height += view.Font.Height + 2;
			}
			return height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id >= 0 && spot.Id < bits)
			{
				if (spot.Text == "1" || spot.Text == "0")
				{
					var bit = bits - 1 - spot.Id;
					var add = bit / 8;
					bit = bit % 8;

					var val = spot.Memory.ReadUInt8(Offset + add);
					if (spot.Text == "1")
					{
						val |= (byte)(1 << bit);
					}
					else
					{
						val &= (byte)~(1 << bit);
					}
					spot.Memory.Process.WriteRemoteMemory(spot.Address + add, val);
				}
			}
		}
	}
}
