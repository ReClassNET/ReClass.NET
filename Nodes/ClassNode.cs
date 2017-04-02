using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using ReClassNET.AddressParser;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public delegate void ClassCreatedEventHandler(ClassNode node);

	public class ClassNode : BaseContainerNode
	{
		public static event ClassCreatedEventHandler ClassCreated;

#if WIN64
		public static IntPtr DefaultAddress = (IntPtr)0x140000000;
#else
		public static IntPtr DefaultAddress = (IntPtr)0x400000;
#endif

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		private NodeUuid uuid;
		public NodeUuid Uuid
		{
			get { return uuid; }
			set
			{
				Contract.Requires(value != null);

				uuid = value;
			}
		}

		public IntPtr Address
		{
			set
			{
				Contract.Ensures(AddressFormula != null);

				AddressFormula = value.ToString("X");
			}
		}

		public string AddressFormula { get; set; }

		public event NodeEventHandler NodesChanged;

		internal ClassNode(bool notifyClassCreated)
		{
			Contract.Ensures(AddressFormula != null);

			Uuid = new NodeUuid(true);

			Address = DefaultAddress;

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

		public override void Intialize()
		{
			AddBytes(IntPtr.Size);
		}

		public override void ClearSelection()
		{
			base.ClearSelection();

			foreach (var node in Nodes)
			{
				node.ClearSelection();
			}
		}

		/// <summary>Draws this node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public override Size Draw(ViewInfo view, int x, int y)
		{
			AddSelection(view, 0, y, view.Font.Height);

			var origX = x;
			var origY = y;

			x = AddOpenClose(view, x, y);

			var tx = x;

			x = AddIcon(view, x, y, Icons.Class, -1, HotSpotType.None);
			x = AddText(view, x, y, view.Settings.OffsetColor, 0, AddressFormula) + view.Font.Width;

			x = AddText(view, x, y, view.Settings.TypeColor, HotSpot.NoneId, "Class") + view.Font.Width;
			x = AddText(view, x, y, view.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.ValueColor, HotSpot.NoneId, $"[{MemorySize}]") + view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;

			var size = new Size(x - origX, y - origY);

			if (levelsOpen[view.Level])
			{
				var childOffset = tx - origX;

				var nv = view.Clone();
				nv.Level++;
				foreach (var node in Nodes)
				{
					// Draw the node if it is in the visible area.
					if (view.ClientArea.Contains(tx, y))
					{
						var innerSize = node.Draw(nv, tx, y);

						size = Utils.AggregateNodeSizes(size, innerSize.Extend(childOffset, 0));

						y += innerSize.Height;
					}
					else
					{
						// Otherwise calculate the height...
						var calculatedHeight = node.CalculateDrawnHeight(nv);

						// and check if the node area overlaps with the visible area...
						if (new Rectangle(tx, y, 9999999, calculatedHeight).IntersectsWith(view.ClientArea))
						{
							// then draw the node...
							var innerSize = node.Draw(nv, tx, y);

							size = Utils.AggregateNodeSizes(size, innerSize.Extend(childOffset, 0));

							y += innerSize.Height;
						}
						else
						{
							// or skip drawing and just use the calculated height.
							size = Utils.AggregateNodeSizes(size, new Size(0, calculatedHeight));

							y += calculatedHeight;
						}
					}
				}
			}

			return size;
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
				var nv = view.Clone();
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
				Offset = spot.Memory.Process.ParseAddress(spot.Text);

				AddressFormula = spot.Text;
			}
		}

		public void UpdateAddress(MemoryBuffer memory)
		{
			Contract.Requires(memory != null);
			Contract.Requires(memory.Process != null);

			try
			{
				Offset = memory.Process.ParseAddress(AddressFormula);
			}
			catch (ParseException)
			{
				Offset = IntPtr.Zero;
			}
		}

		public override void InsertBytes(int index, int size, ref List<BaseNode> createdNodes)
		{
			base.InsertBytes(index, size, ref createdNodes);

			ChildHasChanged(null);
		}

		public override void InsertNode(int index, BaseNode node)
		{
			if (node is ClassNode || node is VMethodNode)
			{
				return;
			}

			base.InsertNode(index, node);

			ChildHasChanged(node);
		}

		public override bool RemoveNode(BaseNode node)
		{
			var removed = base.RemoveNode(node);
			if (removed)
			{
				UpdateOffsets();

				ChildHasChanged(node);
			}
			return removed;
		}

		public override bool ReplaceChildNode(int index, Type nodeType, ref List<BaseNode> createdNodes)
		{
			var replaced = base.ReplaceChildNode(index, nodeType, ref createdNodes);
			if (replaced)
			{
				ChildHasChanged(null); //TODO
			}
			return replaced;
		}

		protected internal override void ChildHasChanged(BaseNode child)
		{
			NodesChanged?.Invoke(this);
		}
	}
}
