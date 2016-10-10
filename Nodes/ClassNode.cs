using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.Nodes
{
	class ClassNode : BaseNode
	{
		public delegate void NewClassCreatedEvent(ClassNode sender);
		public static event NewClassCreatedEvent NewClassCreated;

		public override int MemorySize => Nodes.Sum(n => n.MemorySize);

		private readonly List<BaseNode> nodes = new List<BaseNode>();
		public IEnumerable<BaseNode> Nodes => nodes;

		public ClassNode()
		{
#if WIN64
			Offset = 0x140000000;
#else
			Offset = 0x400000;
#endif

			NewClassCreated?.Invoke(this);
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

		public void UpdateChildrenOffsets()
		{
			var offset = 0;
			foreach (var node in Nodes)
			{
				node.Offset = offset;
				offset += node.MemorySize;
			}
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			AddSelection(view, 0, y, view.Font.Height);
			x = AddOpenClose(view, x, y);

			var tx = x;

			x = AddIcon(view, x, y, Icons.Class, -1, HotSpotType.None);
			x = AddText(view, x, y, view.Settings.Offset, 0, $"{Offset:X}") + view.Font.Width;

			x = AddText(view, x, y, view.Settings.Type, HotSpot.NoneId, "Class ");
			x = AddText(view, x, y, view.Settings.Name, HotSpot.NameId, Name) + view.Font.Width;
			x = AddText(view, x, y, view.Settings.Value, HotSpot.NoneId, $"[{MemorySize}]") + view.Font.Width;
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
				//m_strOffset.SetString(Spot.Text.GetString());
				//m_Offset = ConvertStrToAddress(m_strOffset);
			}
			else if (spot.Id == 1)
			{
				//RequestPosition = _tcstol(Spot.Text.GetString(), NULL, 10); // RequestPosition = ConvertStrToAddress( Spot.Text );
			}
		}

		public void AddNode(BaseNode node)
		{
			nodes.Add(node);
		}

		internal void NotifyMemorySizeChanged()
		{
			OnPropertyChanged(nameof(Nodes));
		}

		public void ReplaceChildNode(int index, BaseNode node)
		{
			if (node == null)
			{
				return;
			}
			if (index < 0 || index >= nodes.Count)
			{
				return;
			}

			var oldNode = nodes[index];

			node.CopyFromNode(oldNode);
			
			node.ParentNode = this;
			node.ClearSelection();

			nodes[index] = node;

			OnPropertyChanged(nameof(Nodes));

			var oldSize = oldNode.MemorySize;
			var newSize = node.MemorySize;

			if (newSize < oldSize)
			{
				InsertBytes(index + 1, oldSize - newSize);
			}
			else if (newSize > oldSize)
			{
				//RemoveNodes(index + 1, newSize - oldSize);
			}
		}

		public void AddBytes(int size)
		{
			InsertBytes(nodes.Count, size);
		}

		public void InsertBytes(int index, int size)
		{
			if (index < 0 || index > nodes.Count || size == 0)
			{
				return;
			}

			var offset = 0;
			if (index > 0)
			{
				var node = nodes[index - 1];
				offset = node.Offset + node.MemorySize;
			}

			while (size != 0)
			{
				BaseNode node = null;
#if WIN64
				if (size >= 8)
				{
					node = new Hex64Node();
				}
				else 
#endif
				if (size >= 4)
				{
					node = new Hex32Node();
				}
				else if (size >= 2)
				{
					node = new Hex16Node();
				}
				else
				{
					node = new Hex8Node();
				}

				node.ParentNode = this;
				node.Offset = offset;

				nodes.Insert(index, node);

				offset += node.MemorySize;
				size -= node.MemorySize;

				index++;
			}

			NotifyMemorySizeChanged();
		}
	}
}
