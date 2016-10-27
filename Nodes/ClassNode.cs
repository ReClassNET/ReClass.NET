using System;
using System.Diagnostics.Contracts;
using System.Linq;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public class ClassNode : BaseContainerNode
	{
		/// <summary>Size of the node in bytes.</summary>
		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		public IntPtr Address
		{
			set
			{
				Contract.Ensures(AddressFormula != null);

				AddressFormula = value.ToString("X");
			}
		}

		public string AddressFormula { get; set; }

		/// <summary>Only the <see cref="ClassManager"/> and the <see cref="DataExchange.SchemaBuilder"/> are allowed to call the constructor.</summary>
		internal ClassNode()
		{
			Contract.Ensures(AddressFormula != null);

#if WIN64
			AddressFormula = "140000000";
#else
			AddressFormula = "400000";
#endif
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
					y = node.Draw(nv, tx, y);
				}
			}

			return y;
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

		public void UpdateAddress(Memory memory)
		{
			Contract.Requires(memory != null);

			Offset = memory.Process.ParseAddress(AddressFormula);
		}

		internal void NotifyMemorySizeChanged()
		{
			ClassManager.Classes.ForEach(c => c.UpdateOffsets());
		}

		internal void AddNode(BaseNode node)
		{
			Contract.Requires(node != null);

			InsertNode(nodes.Count, node);
		}

		public void InsertNode(int index, BaseNode node)
		{
			Contract.Requires(index >= 0);
			Contract.Requires(node != null);

			node.ParentNode = this;

			nodes.Insert(index, node);
		}

		public override void InsertBytes(int index, int size)
		{
			base.InsertBytes(index, size);

			NotifyMemorySizeChanged();
		}

		public override bool RemoveNode(BaseNode node)
		{
			var removed = base.RemoveNode(node);
			if (removed)
			{
				UpdateOffsets();

				NotifyMemorySizeChanged();
			}
			return removed;
		}
	}
}
