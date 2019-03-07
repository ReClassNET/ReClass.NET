using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Project;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class EnumNode : BaseNode
	{
		public override int MemorySize => (int)MetaData.Size;

		public EnumMetaData MetaData { get; private set; } = EnumMetaData.Default;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Enum";
			icon = Properties.Resources.B16x16_Button_Enum;
		}

		public void ChangeEnum(EnumMetaData @enum)
		{
			Contract.Requires(@enum != null);

			MetaData = @enum;
		}

		/// <summary>
		/// Gets the underlaying node for the enum field.
		/// </summary>
		/// <returns></returns>
		public BaseNumericNode GetUnderlayingNode()
		{
			switch (MetaData.Size)
			{
				case EnumMetaData.UnderlyingTypeSize.OneByte:
					return new UInt8Node();
				case EnumMetaData.UnderlyingTypeSize.TwoBytes:
					return new UInt16Node();
				case EnumMetaData.UnderlyingTypeSize.FourBytes:
					return new UInt32Node();
				case EnumMetaData.UnderlyingTypeSize.EightBytes:
					return new UInt64Node();
			}

			throw new Exception(); // TODO
		}

		public long ReadValueFromMemory(MemoryBuffer memory)
		{
			switch (MetaData.Size)
			{
				case EnumMetaData.UnderlyingTypeSize.OneByte:
					return memory.ReadInt8(Offset);
				case EnumMetaData.UnderlyingTypeSize.TwoBytes:
					return memory.ReadInt16(Offset);
				case EnumMetaData.UnderlyingTypeSize.FourBytes:
					return memory.ReadInt32(Offset);
				case EnumMetaData.UnderlyingTypeSize.EightBytes:
					return memory.ReadInt64(Offset);
			}

			throw new Exception(); // TODO
		}

		private string GetStringRepresentation(long value)
		{
			if (!MetaData.UseFlagsMode)
			{
				var index = MetaData.Values.FindIndex(kv => kv.Key == value);
				if (index == -1)
				{
					return value.ToString();
				}

				return MetaData.Values[index].Value;
			}

			return GetFlagsStringRepresentation(value);
		}

		private string GetFlagsStringRepresentation(long value)
		{
			var result = (ulong)value;

			var values = MetaData.Values;

			var index = values.Count - 1;
			var retval = new StringBuilder();
			var firstTime = true;
			var saveResult = result;

			while (index >= 0)
			{
				var temp = (ulong)values[index].Key;
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

					retval.Prepend(values[index].Value);
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
				if (values.Count > 0 && values[0].Key == 0)
				{
					return values[0].Value;
				}

				return "0";
			}
			
			return retval.ToString();
		}

		public override Size Draw(ViewInfo view, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(view, x, y);
			}

			var origX = x;

			AddSelection(view, x, y, view.Font.Height);

			x = AddOpenClose(view, x, y);
			x = AddIcon(view, x, y, Icons.Class, HotSpot.NoneId, HotSpotType.None);

			x = AddAddressOffset(view, x, y);

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Enum") + view.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			}
			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"<{MetaData.Name}>") + view.Font.Width;
			x = AddIcon(view, x, y, Icons.Change, 4, HotSpotType.ChangeEnumType) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, "=") + view.Font.Width;

			var value = ReadValueFromMemory(view.Memory);

			x = AddText(view, x, y, view.Settings.TextColor, HotSpot.NoneId, GetStringRepresentation(value)) + view.Font.Width;

			x = AddComment(view, x, y);

			DrawInvalidMemoryIndicator(view, y);
			AddTypeDrop(view, y);
			AddDelete(view, y);

			return new Size(x - origX, view.Font.Height);
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			return IsHidden && !IsWrapped ? HiddenHeight : view.Font.Height;
		}
	}
}
