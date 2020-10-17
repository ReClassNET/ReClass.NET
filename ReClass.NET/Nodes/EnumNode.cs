using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Project;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class EnumNode : BaseNode
	{
		public override int MemorySize => (int)Enum.Size;

		public EnumDescription Enum { get; private set; } = EnumDescription.Default;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Enum";
			icon = Properties.Resources.B16x16_Button_Enum;
		}

		public void ChangeEnum(EnumDescription @enum)
		{
			Contract.Requires(@enum != null);

			Enum = @enum;

			GetParentContainer()?.ChildHasChanged(this);
		}

		private string GetTextRepresentation(MemoryBuffer memory)
		{
			return Enum.UseFlagsMode ? GetFlagsStringRepresentation(memory) : GetStringRepresentation(memory);
		}

		private long ReadSignedValueFromMemory(MemoryBuffer memory)
		{
			switch (Enum.Size)
			{
				case EnumDescription.UnderlyingTypeSize.OneByte:
					return memory.ReadInt8(Offset);
				case EnumDescription.UnderlyingTypeSize.TwoBytes:
					return memory.ReadInt16(Offset);
				case EnumDescription.UnderlyingTypeSize.FourBytes:
					return memory.ReadInt32(Offset);
				case EnumDescription.UnderlyingTypeSize.EightBytes:
					return memory.ReadInt64(Offset);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private string GetStringRepresentation(MemoryBuffer memory)
		{
			var value = ReadSignedValueFromMemory(memory);
			var index = Enum.Values.FindIndex(kv => kv.Value == value);
			if (index == -1)
			{
				return value.ToString();
			}

			return Enum.Values[index].Key;
		}

		private ulong ReadUnsignedValueFromMemory(MemoryBuffer memory)
		{
			switch (Enum.Size)
			{
				case EnumDescription.UnderlyingTypeSize.OneByte:
					return memory.ReadUInt8(Offset);
				case EnumDescription.UnderlyingTypeSize.TwoBytes:
					return memory.ReadUInt16(Offset);
				case EnumDescription.UnderlyingTypeSize.FourBytes:
					return memory.ReadUInt32(Offset);
				case EnumDescription.UnderlyingTypeSize.EightBytes:
					return memory.ReadUInt64(Offset);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private string GetFlagsStringRepresentation(MemoryBuffer memory)
		{
			var value = ReadUnsignedValueFromMemory(memory);
			var result = value;

			var values = Enum.Values;

			var index = values.Count - 1;
			var retval = new StringBuilder();
			var firstTime = true;
			var saveResult = result;

			while (index >= 0)
			{
				var temp = (ulong)values[index].Value;
				if (index == 0 && temp == 0)
				{
					break;
				}

				if ((result & temp) == temp)
				{
					result -= temp;
					if (!firstTime)
					{
						retval.Prepend(" | ");
					}

					retval.Prepend(values[index].Key);
					firstTime = false;
				}

				index--;
			}

			if (result != 0)
			{
				return value.ToString();
			}

			if (saveResult == 0)
			{
				if (values.Count > 0 && values[0].Value == 0)
				{
					return values[0].Key;
				}

				return "0";
			}

			return retval.ToString();
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, context.IconProvider.Enum, HotSpot.NoneId, HotSpotType.None);

			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Enum") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}
			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, $"<{Enum.Name}>") + context.Font.Width;
			x = AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeEnumType) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "=") + context.Font.Width;

			var text = GetTextRepresentation(context.Memory);

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, text) + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			return new Size(x - origX, context.Font.Height);
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			return IsHidden && !IsWrapped ? HiddenHeight : context.Font.Height;
		}
	}
}
