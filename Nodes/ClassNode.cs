using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ReClassNET.Nodes
{
	class ClassNode : BaseContainerNode
	{
		public delegate void NewClassCreatedEvent(ClassNode sender);
		public static event NewClassCreatedEvent NewClassCreated;

		public static List<ClassNode> Classes = new List<ClassNode>();

		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		public string AddressFormula { get; set; }

		public ClassNode()
			: this(true)
		{

		}

		public ClassNode(bool notifiy)
		{
#if WIN64
			AddressFormula = "0x140000000";
#else
			AddressFormula = "0x400000";
#endif

			Classes.Add(this);

			if (notifiy)
			{
				NewClassCreated?.Invoke(this);
			}
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

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

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
			Contract.Requires(spot != null);

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
			OnPropertyChanged(nameof(Nodes));
		}

		public void AddNode(BaseNode node)
		{
			Contract.Requires(node != null);

			InsertNode(nodes.Count, node);
		}

		public void InsertNode(int index, BaseNode node)
		{
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
			Contract.Requires(node != null);

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
