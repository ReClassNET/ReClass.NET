using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.Controls;
using ReClassNET.UI;

namespace ReClassNET.Nodes
{
	public delegate void ClassCreatedEventHandler(ClassNode node);

	public class ClassNode : BaseContainerNode
	{
		public static event ClassCreatedEventHandler ClassCreated;

#if RECLASSNET64
		public static IntPtr DefaultAddress { get; } = (IntPtr)0x140000000;
		public static string DefaultAddressFormula { get; } = "140000000";
#else
		public static IntPtr DefaultAddress { get; } = (IntPtr)0x400000;
		public static string DefaultAddressFormula { get; } = "400000";
#endif

		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		protected override bool ShouldCompensateSizeChanges => true;

		public Guid Uuid { get; set; }

		public string AddressFormula { get; set; } = DefaultAddressFormula;

		public event NodeEventHandler NodesChanged;

		internal ClassNode(bool notifyClassCreated)
		{
			Contract.Ensures(AddressFormula != null);

			LevelsOpen.DefaultValue = true;

			Uuid = Guid.NewGuid();

			if (notifyClassCreated)
			{
				ClassCreated?.Invoke(this);
			}
		}

		public static ClassNode Create()
		{
			Contract.Ensures(Contract.Result<ClassNode>() != null);

			return new ClassNode(true);
		}
		
		/// <summary>
		/// Initializes the class' name and vtable node from RTTI information, if it's not set already 
		/// </summary>
		/// <param name="context"></param>
		public void InitFromRTTI(DrawContext context)
		{
			// first node should be a VTable node or a hex64/32 node
			if (Nodes.Count <= 0)
			{
				return;
			}

			var rttiInfoFromFirstNode = string.Empty;
			var firstNode = Nodes[0];
			if (firstNode is VirtualMethodTableNode vtableNode)
			{
				rttiInfoFromFirstNode = vtableNode.GetAssociatedRemoteRuntimeTypeInformation(context);
			}
			else if (firstNode is BaseHexCommentNode baseHexCommentNode)
			{
				// ask it as if it might point to a vtable
				var value = context.Memory.InterpretData64(Offset);
				rttiInfoFromFirstNode = baseHexCommentNode.GetAssociatedRemoteRuntimeTypeInformation(context, value.IntPtr);
				if (!string.IsNullOrEmpty(rttiInfoFromFirstNode))
				{
					// convert first node to vtable node
					var newVTableNode = BaseNode.CreateInstanceFromType(typeof(VirtualMethodTableNode));
					var createdNodes = new List<BaseNode>();
					this.ReplaceChildNode(firstNode, newVTableNode, ref createdNodes);
				}
			}

			if (string.IsNullOrEmpty(rttiInfoFromFirstNode))
			{
				return;
			}

			var fragments = rttiInfoFromFirstNode.Split(':');
			this.Name = fragments[0];
		}
		
		public override void GetUserInterfaceInfo(out string name, out Image icon)
		{
			throw new InvalidOperationException($"The '{nameof(ClassNode)}' node should not be accessible from the ui.");
		}

		public override bool CanHandleChildNode(BaseNode node)
		{
			switch (node)
			{
				case null:
				case ClassNode _:
				case VirtualMethodNode _:
					return false;
			}

			return true;
		}

		public override void Initialize()
		{
			AddBytes(IntPtr.Size);
		}

		public override Size Draw(DrawContext context, int x, int y)
		{
			AddSelection(context, 0, y, context.Font.Height);

			var origX = x;
			var origY = y;

			x = AddOpenCloseIcon(context, x, y);

			var tx = x;

			x = AddIcon(context, x, y, context.IconProvider.Class, HotSpot.NoneId, HotSpotType.None);
			x = AddText(context, x, y, context.Settings.OffsetColor, 0, AddressFormula) + context.Font.Width;

			x = AddText(context, x, y, context.Settings.TypeColor, HotSpot.NoneId, "Class") + context.Font.Width;
			x = AddText(context, x, y, context.Settings.NameColor, HotSpot.NameId, Name) + context.Font.Width;
			x = AddText(context, x, y, context.Settings.ValueColor, HotSpot.NoneId, $"[{MemorySize}]") + context.Font.Width;
			x = AddComment(context, x, y);

			y += context.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (LevelsOpen[context.Level])
			{
				var childOffset = tx - origX;

				var innerContext = context.Clone();
				innerContext.Level++;
				foreach (var node in Nodes)
				{
					Size AggregateNodeSizes(Size baseSize, Size newSize)
					{
						return new Size(Math.Max(baseSize.Width, newSize.Width), baseSize.Height + newSize.Height);
					}

					Size ExtendWidth(Size baseSize, int width)
					{
						return new Size(baseSize.Width + width, baseSize.Height);
					}

					// Draw the node if it is in the visible area.
					if (context.ClientArea.Contains(tx, y))
					{
						var innerSize = node.Draw(innerContext, tx, y);

						size = AggregateNodeSizes(size, ExtendWidth(innerSize, childOffset));

						y += innerSize.Height;
					}
					else
					{
						// Otherwise calculate the height...
						var calculatedHeight = node.CalculateDrawnHeight(innerContext);

						// and check if the node area overlaps with the visible area...
						if (new Rectangle(tx, y, 9999999, calculatedHeight).IntersectsWith(context.ClientArea))
						{
							// then draw the node...
							var innerSize = node.Draw(innerContext, tx, y);

							size = AggregateNodeSizes(size, ExtendWidth(innerSize, childOffset));

							y += innerSize.Height;
						}
						else
						{
							// or skip drawing and just use the calculated height.
							size = AggregateNodeSizes(size, new Size(0, calculatedHeight));

							y += calculatedHeight;
						}
					}
				}
			}

			return size;
		}

		public override int CalculateDrawnHeight(DrawContext context)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var height = context.Font.Height;
			if (LevelsOpen[context.Level])
			{
				var nv = context.Clone();
				nv.Level++;
				height += Nodes.Sum(n => n.CalculateDrawnHeight(nv));
			}
			return height;
		}

		public override void Update(HotSpot spot)
		{
			base.Update(spot);

			if (spot.Id == 0)
			{
				AddressFormula = spot.Text;
			}
		}

		protected internal override void ChildHasChanged(BaseNode child)
		{
			NodesChanged?.Invoke(this);
		}
	}
}
