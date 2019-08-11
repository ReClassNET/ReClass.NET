using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

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

			LevelsOpen.DefaultValue = true;
		}

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Bitfield";
			icon = Properties.Resources.B16x16_Button_Bits;
		}

		public override void CopyFromNode(BaseNode node)
		{
			base.CopyFromNode(node);

			Bits = node.MemorySize * 8;
		}

		/// <summary>
		/// Gets the underlaying node for the bit field.
		/// </summary>
		/// <returns></returns>
		public BaseNumericNode GetUnderlayingNode()
		{
			switch (Bits)
			{
				case 8:
					return new UInt8Node();
				case 16:
					return new UInt16Node();
				case 32:
					return new UInt32Node();
				case 64:
					return new UInt64Node();
			}

			throw new Exception(); // TODO
		}

		/// <summary>Converts the memory value to a bit string.</summary>
		/// <param name="memory">The process memory.</param>
		/// <returns>The value converted to a bit string.</returns>
		private string ConvertValueToBitString(MemoryBuffer memory)
		{
			Contract.Requires(memory != null);
			Contract.Ensures(Contract.Result<string>() != null);

			switch(bits)
			{
				case 64:
					return BitString.ToString(memory.ReadInt64(Offset));
				case 32:
					return BitString.ToString(memory.ReadInt32(Offset));
				case 16:
					return BitString.ToString(memory.ReadInt16(Offset));
				default:
					return BitString.ToString(memory.ReadUInt8(Offset));
			}
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			const int BitsPerBlock = 4;

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(view, x, y, view.Font.Height);

			x += TextPadding + Icons.Dimensions;

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Bits") + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			}

			x = AddOpenCloseIcon(view, x, y) + view.Font.Width;

			var tx = x - 3;

			for (var i = 0; i < bits; ++i)
			{
				var rect = new Rectangle(x + (i + i / BitsPerBlock) * view.Font.Width, y, view.Font.Width, view.Font.Height);
				AddHotSpot(view, rect, string.Empty, i, HotSpotType.Edit);
			}

			var value = ConvertValueToBitString(view.Memory);

			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, value) + view.Font.Width;

			x += view.Font.Width;

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicatorIcon(view, y);
			AddContextDropDownIcon(view, y);
			AddDeleteIcon(view, y);

			if (LevelsOpen[view.Level])
			{
				y += view.Font.Height;

				var format = new StringFormat(StringFormatFlags.DirectionVertical);

				using (var brush = new SolidBrush(view.Settings.ValueColor))
				{
					var maxCharCount = bits + (bits / 4 - 1) - 1;

					for (int bitCount = 0, padding = 0; bitCount < bits; bitCount += 4, padding += 5)
					{
						view.Context.DrawString(bitCount.ToString(), view.Font.Font, brush, tx + (maxCharCount - padding) * view.Font.Width, y, format);
					}
				}

				y += 2;
			}

			return new Size(x - origX, y - origY + view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = view.Font.Height;
			if (LevelsOpen[view.Level])
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
					spot.Process.WriteRemoteMemory(spot.Address + add, val);
				}
			}
		}
	}
}
