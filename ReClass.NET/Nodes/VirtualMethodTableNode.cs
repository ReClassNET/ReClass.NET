using System;
using System.Drawing;
using System.Linq;
using ReClassNET.Controls;
using ReClassNET.Memory;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public class VirtualMethodTableNode : BaseContainerNode
	{
		private readonly MemoryBuffer memory = new MemoryBuffer();

		public override int MemorySize => IntPtr.Size;

		protected override bool ShouldCompensateSizeChanges => false;

		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			name = "VTable Pointer";
			icon = Properties.Resources.B16x16_Button_VTable;
		}

		public override bool CanHandleChildNode(BaseNode node)
		{
			return node is VirtualMethodNode;
		}

		public override void Initialize()
		{
			for (var i = 0; i < 10; ++i)
			{
				AddNode(CreateDefaultNodeForSize(IntPtr.Size));
			}
		}
		
		protected override int AddComment(DrawContext context, int x, int y)
		{
			x = base.AddComment(context, x, y);
			
			if (context.Settings.ShowCommentRtti)
			{
				var rtti = GetAssociatedRemoteRuntimeTypeInformation(context);
				if (!string.IsNullOrEmpty(rtti))
				{
					x = AddText(context, x, y, context.Settings.OffsetColor, HotSpot.ReadOnlyId, rtti) + context.Font.Width;
				}
			}
			return x;
		}
		
		public string GetAssociatedRemoteRuntimeTypeInformation(DrawContext context)
		{
			var addressFirstVTableFunction = context.Memory.InterpretData64(Offset).IntPtr;
			if (addressFirstVTableFunction != IntPtr.Zero)
			{
				return context.Process.ReadRemoteRuntimeTypeInformation(addressFirstVTableFunction);
			}

			return string.Empty;
		}
		
		public override Size Draw(DrawContext context, int x, int y)
		{
			if (IsHidden && !IsWrapped)
			{
				return DrawHidden(context, x, y);
			}

			var origX = x;
			var origY = y;

			AddSelection(context, x, y, context.Font.Height);

			x = AddOpenCloseIcon(context, x, y);
			x = AddIcon(context, x, y, context.IconProvider.VirtualTable, HotSpot.NoneId, HotSpotType.None);

			var tx = x;
			x = AddAddressOffset(context, x, y);

			x = AddText(context, x, y, context.Settings.VTableColor, HotSpot.NoneId, $"VTable[{Nodes.Count}]") + context.Font.Width;
			if (!IsWrapped)
			{
				x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			}

			x = AddComment(context, x, y);

			DrawInvalidMemoryIndicatorIcon(context, y);
			AddContextDropDownIcon(context, y);
			AddDeleteIcon(context, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level])
			{
				var ptr = context.Memory.ReadIntPtr(Offset);

				memory.Size = Nodes.Count * IntPtr.Size;
				memory.UpdateFrom(context.Process, ptr);

				var innerContext = context.Clone();
				innerContext.Address = ptr;
				innerContext.Memory = memory;

				foreach (var node in Nodes)
				{
					var innerSize = node.Draw(innerContext, tx, y);

					size.Width = Math.Max(size.Width, innerSize.Width + tx - origX);
					size.Height += innerSize.Height;

					y += innerSize.Height;
				}
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
			if (LevelsOpen[context.Level])
			{
				height += Nodes.Sum(n => n.CalculateDrawnHeight(context));
			}
			return height;
		}

		protected override BaseNode CreateDefaultNodeForSize(int size)
		{
			// ignore the size parameter
			return new VirtualMethodNode();
		}
	}
}
