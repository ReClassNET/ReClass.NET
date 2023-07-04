using System;
using System.Drawing;
using ReClassNET.AddressParser;
using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class PointerNode : BaseWrapperNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size;

		protected override bool PerformCycleCheck => false;

		public PointerNode()
		{
			LevelsOpen.DefaultValue = true;
		}

		public override void Initialize()
		{
			var node = new ClassInstanceNode();
			node.Initialize();
			((BaseContainerNode)node.InnerNode).AddBytes(16 * IntPtr.Size);

			ChangeInnerNode(node);
		}

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "Pointer";
			icon = Properties.Resources.B16x16_Button_Pointer;
		}

		public override bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
		{
			// TODO Should the preview be disabled if an inner node is set?

			address = spot.Memory.ReadIntPtr(Offset);

			return spot.Process.GetNamedAddress(address) != null;
		}

		public override bool CanChangeInnerNodeTo(BaseNode node) =>
			node switch
			{
				ClassNode _ => false,
				VirtualMethodNode _ => false,
				_ => true
			};

		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			if (InnerNode != null)
			{
				x = AddOpenCloseIcon(context, x, y);
			}
			else
			{
				x = AddIconPadding(context, x);
			}
			x = AddIcon(context, x, y, context.IconProvider.Pointer, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Ptr") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}
			if (InnerNode == null)
			{
				x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, "<void>") + context.Font.Width;
			}
			x = AddIcon(context, x, y, context.IconProvider.Change, 4, HotSpotType.ChangeWrappedType) + context.Font.Width;

			var ptr = context.Memory.ReadIntPtr(Offset);

			x = AddText(context, x, y, context.Settings.OffsetColor, HotSpot.NoneId, "->") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.ValueColor, 0, "0x" + ptr.ToString(Constants.AddressHexFormat)) + context.Font.Width;

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level] && InnerNode != null)
			{
				memory.Size = InnerNode.MemorySize;
				memory.UpdateFrom(context.Process, ptr);

				var innerContext = context.Clone();
				innerContext.Address = ptr;
				innerContext.Memory = memory;

				var innerSize = InnerNode.Draw(innerContext, tx, y);

				size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
				size.Height += innerSize.Height;
			}

			return size;
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden && !IsWrapped)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level] && InnerNode != null)
			{
				height += InnerNode.CalculateDrawnHeight(context);
			}
			return height;
		}
		
		public override void PerformPostInitWork()
		{
			base.PerformPostInitWork();

			var parentClass = ParentNode as ClassNode;
			if (parentClass == null)
			{
				return;
			}

			var process = Program.RemoteProcess;
			IntPtr address;
			try
			{
				address = process.ParseAddress(parentClass.AddressFormula);
			}
			catch (ParseException)
			{
				address = IntPtr.Zero;
			}

			var memoryBuffer = new MemoryBuffer() { Size = parentClass.MemorySize};
			memoryBuffer.UpdateFrom(process, address);
			var ptr = memoryBuffer.ReadIntPtr(Offset);

			var classNode = ((ClassInstanceNode)InnerNode)?.InnerNode as ClassNode;
			if (classNode == null)
			{
				return;
			}

			classNode.AddressFormula = ptr.ToString(Constants.AddressHexFormat);
		}
	}
}
