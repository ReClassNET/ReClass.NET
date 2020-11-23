using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Controls;
using ReClassNET.Extensions;
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

		public override Size Draw(DrawContext context, int x, int y)
		{
			const int BitsPerBlock = 4;

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);
			x = AddIconPadding(context, x);

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Bits") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}

			x = AddOpenCloseIcon(context, x, y) + context.Font.Width;

			var tx = x - 3;

			for (var i = 0; i < bits; ++i)
			{
				var rect = new Rectangle(x + (i + i / BitsPerBlock) * context.Font.Width, y, context.Font.Width, context.Font.Height);
				AddHotSpot(context, rect, string.Empty, i, HotSpotType.Edit);
			}

			var value = ConvertValueToBitString(context.Memory);

			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, value) + context.Font.Width;

			x += context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			if (LevelsOpen[context.Level])
			{
				y += context.Font.Height;

				var format = new StringFormat(StringFormatFlags.DirectionVertical);

				using (var brush = new SolidBrush(context.Settings.ValueColor))
				{
					var maxCharCount = bits + (bits / 4 - 1) - 1;

					for (int bitCount = 0, padding = 0; bitCount < bits; bitCount += 4, padding += 5)
					{
						context.Graphics.DrawString(bitCount.ToString(), context.Font.Font, brush, tx + (maxCharCount - padding) * context.Font.Width, y, format);
					}
				}

				y += 8;
			}

			return new Size(x - origX, y - origY + context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level])
			{
				height += context.Font.Height + 8;
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
