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
		public override int MemorySize => MetaData.UnderlyingTypeSize;

		public EnumMetaData MetaData { get; private set; } = EnumMetaData.Default;

		public EnumNode()
		{
			MetaData = new EnumMetaData
			{
				Name = "TestEnum"
			};
			MetaData.SetData(true, 4, new SortedDictionary<long, string>
			{
				{ 0, "Val0" },
				{ 1, "Val1" },
				{ 2, "Val2" },
				{ 4, "Val4" },
				{ 8, "Val8" },
				{ 16, "Val16" },
				{ 32, "Val32" },
				{ 64, "Val64" },
				{ 128, "Val128" },
				{ 256, "Val256" },
				{ 512, "Val512" },
				{ 1024, "Val1024" }
			});
		}

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Enum";
			icon = Properties.Resources.B16x16_Button_Bits;
		}

		/// <summary>
		/// Gets the underlaying node for the enum field.
		/// </summary>
		/// <returns></returns>
		public BaseNumericNode GetUnderlayingNode()
		{
			switch (MetaData.UnderlyingTypeSize)
			{
				case 1:
					return new UInt8Node();
				case 2:
					return new UInt16Node();
				case 4:
					return new UInt32Node();
				case 8:
					return new UInt64Node();
			}

			throw new Exception(); // TODO
		}

		public long ReadValueFromMemory(MemoryBuffer memory)
		{
			switch (MetaData.UnderlyingTypeSize)
			{
				case 1:
					return memory.ReadInt8(Offset);
				case 2:
					return memory.ReadInt16(Offset);
				case 4:
					return memory.ReadInt32(Offset);
				case 8:
					return memory.ReadInt64(Offset);
			}

			throw new Exception(); // TODO
		}

		private string GetStringRepresentation(long value)
		{
			if (!MetaData.UseFlagsMode)
			{
				var index = MetaData.Values.FindIndex(v => v == value);
				if (index == -1)
				{
					return value.ToString();
				}

				return MetaData.Names[index];
			}

			return GetFlagsStringRepresentation(value);
		}

		private string GetFlagsStringRepresentation(long value)
		{
			var result = (ulong)value;

			var names = MetaData.Names;
			var values = MetaData.Values;

			Contract.Assert(names.Count == values.Count);

			var index = values.Count - 1;
			var retval = new StringBuilder();
			var firstTime = true;
			var saveResult = result;

			while (index >= 0)
			{
				var temp = (ulong)values[index];
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

					retval.Prepend(names[index]);
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
				if (values.Count > 0 && values[0] == 0)
				{
					return names[0];
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
			x = AddIcon(view, x, y, Icons.Change, 4, HotSpotType.ChangeClassType) + view.Font.Width;

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
