using System;
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

		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		private NodeUuid uuid;
		public NodeUuid Uuid
		{
			get { return uuid; }
			set
			{
				Contract.Requires(uuid != null);

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

#if WIN64
			Address = (IntPtr)0x140000000;
#else
			Address = (IntPtr)0x400000;
#endif

			if (notifyClassCreated)
			{
				ClassCreated?.Invoke(this);
			}
		}

		public static ClassNode Create()
		{
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
		/// <returns>The height the node occupies.</returns>
		public override int Draw(ViewInfo view, int x, int y)
		{
			AddSelection(view, 0, y, view.Font.Height);
			x = AddOpenClose(view, x, y);

			var tx = x;

			x = AddIcon(view, x, y, Icons.Class, -1, HotSpotType.None);
			x = AddText(view, x, y, Program.Settings.OffsetColor, 0, AddressFormula) + view.Font.Width;

			x = AddText(view, x, y, Program.Settings.TypeColor, HotSpot.NoneId, "Class") + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.NameColor, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, Program.Settings.ValueColor, HotSpot.NoneId, $"[{MemorySize}]") + view.Font.Width;
			x = AddComment(view, x, y);

			y += view.Font.Height;
			if (levelsOpen[view.Level])
			{
				var nv = view.Clone();
				nv.Level++;
				foreach (var node in Nodes)
				{
					// If the node is in the visible area draw it.
					if (view.ClientArea.Contains(tx, y))
					{
						y = node.Draw(nv, tx, y);
					}
					else
					{
						// Otherwise calculate the height...
						var height = node.CalculateHeight(nv);

						// and check if the nodes area overlaps with the visible area...
						if (new Rectangle(tx, y, view.ClientArea.Width, height).IntersectsWith(view.ClientArea))
						{
							// then draw the node...
							y = node.Draw(nv, tx, y);
						}
						else
						{
							// or skip drawing and just add the height.
							y += height;
						}
					}
				}
			}

			return y;
		}

		public override int CalculateHeight(ViewInfo view)
		{
			if (IsHidden)
			{
				return HiddenHeight;
			}

			var h = view.Font.Height;
			if (levelsOpen[view.Level])
			{
				var nv = view.Clone();
				nv.Level++;
				h += Nodes.Sum(n => n.CalculateHeight(nv));
			}
			return h;
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

			try
			{
				Offset = memory.Process.ParseAddress(AddressFormula);
			}
			catch (ParseException)
			{
				Offset = IntPtr.Zero;
			}
		}

		public override void InsertBytes(int index, int size)
		{
			base.InsertBytes(index, size);

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

		public override bool ReplaceChildNode(int index, BaseNode node)
		{
			var replaced = base.ReplaceChildNode(index, node);
			if (replaced)
			{
				ChildHasChanged(node);
			}
			return replaced;
		}

		protected internal override void ChildHasChanged(BaseNode child)
		{
			NodesChanged?.Invoke(this);
		}
	}
}
