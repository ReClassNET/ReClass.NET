using System.Diagnostics.Contracts;
using System.Drawing;
using System.Text;
using ReClassNET.Controls;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public abstract class BaseTextNode : BaseNode
	{
		public int Length { get; set; }

		public override int MemorySize => Length * CharacterSize;

		/// <summary>The encoding of the string.</summary>
		public abstract Encoding Encoding { get; }

		/// <summary>Size of one character in bytes.</summary>
		private int CharacterSize => Encoding.GuessByteCountPerChar();

		public override void CopyFromNode(BaseNode node)
		{
			Length = node.MemorySize / CharacterSize;
		}

		protected Size DrawText(DrawContext context, int x, int y, string type)
		{
			Contract.Requires(context != null);
			Contract.Requires(type != null);

			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var length = MemorySize / CharacterSize;
			var text = ReadValueFromMemory(context.Memory);

			var origX = x;

			AddSelection(context, x, y, context.Font.Height);

			x = AddIconPadding(context, x);

			x = AddIcon(context, x, y, context.IconProvider.Text, HotSpot.NoneId, HotSpotType.None);
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, type) + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name);
			}
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, "[");
			x = AddText(context, x, y, context.Settings.IndexColor, 0, length.ToString());
			x = AddText(context, x, y, context.Settings.IndexColor, HotSpot.NoneId, "]") + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "= '");
			x = AddText(context, x, y, context.Settings.TextColor, 1, text.LimitLength(150));
			x = AddText(context, x, y, context.Settings.TextColor, HotSpot.NoneId, "'") + context.Font.Width;

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

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				if (int.TryParse(spot.Text, out var val) && val > 0)
				{
					Length = val;

					GetParentContainer()?.ChildHasChanged(this);
				}
			}
			else if (spot.Id == 1)
			{
				var data = Encoding.GetBytes(spot.Text);
				spot.Process.WriteRemoteMemory(spot.Address, data);
			}
		}

		public string ReadValueFromMemory(MemoryBuffer memory)
		{
			return memory.ReadString(Encoding, Offset, MemorySize);
		}
	}
}
