using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReClassNET.Nodes
{
	abstract class BaseNode : INotifyPropertyChanged
	{
		protected const int TXOFFSET = 16;
		private static int NodeIndex = 0;

		private string name;
		public string Name { get { return name; } set { if (name != value) { name = value; OnPropertyChanged(nameof(Name)); } } }
		public IntPtr Offset { get; set; }
		public string Comment { get; set; }

		public ClassNode ParentNode { get; set; }

		public bool IsHidden { get; protected set; }
		public bool IsSelected { get; set; }

		protected GrowingList<bool> levelsOpen = new GrowingList<bool>(false);

		#region INotifyPropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			PropertyChanged?.Invoke(this, e);
		}

		protected void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		public abstract int MemorySize { get; }

		public BaseNode()
		{
			Name = $"N{NodeIndex++:X08}";

			levelsOpen[0] = true;
		}

		public virtual void ClearSelection()
		{
			IsSelected = false;
		}

		public virtual void CopyFromNode(BaseNode node)
		{
			Name = node.Name;
			Comment = node.Comment;
		}


		/// <summary>Called when the node was created. Does not get called after loading a project.</summary>
		public virtual void Intialize()
		{

		}

		public virtual string GetToolTipText(HotSpot spot, Memory memory, Settings settings)
		{
			return null;
		}

		public abstract int Draw(ViewInfo view, int x, int y);
		public virtual void Update(HotSpot spot)
		{
			if (spot.Id == HotSpot.NameId)
			{
				Name = spot.Text;
			}
			else if (spot.Id == HotSpot.CommentId)
			{
				Comment = spot.Text;
			}
		}

		internal void ToggleLevelOpen(int level)
		{
			levelsOpen[level] = !levelsOpen[level];
		}

		protected void AddClickableArea(ViewInfo view, Rectangle spot, string text, int id, HotSpotType type)
		{
			if (spot.Top > view.ClientArea.Bottom || spot.Bottom < 0)
			{
				return;
			}

			view.HotSpots.Add(new HotSpot
			{
				Rect = spot,
				Text = text,
				Address = view.Address.Add(Offset),
				Id = id,
				Type = type,
				Node = this,
				Level = view.Level
			});
		}

		protected int AddText(ViewInfo view, int x, int y, Color color, int hitId, string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return x;
			}

			var width = text.Length * view.Font.Width;

			if ((y >= -view.Font.Height) && (y + view.Font.Height <= view.ClientArea.Bottom + view.Font.Height))
			{
				if (hitId != HotSpot.NoneId)
				{
					var rect = new Rectangle(x, y, 0, view.Font.Height);
					if (width >= view.Font.Width * 2)
					{
						rect.Width = width;
					}
					else
					{
						rect.Width = view.Font.Width * 2;
					}

					AddClickableArea(view, rect, text, hitId, HotSpotType.Edit);
				}

				view.Context.DrawString(text, view.Font.Font, new SolidBrush(color), x, y);
			}

			return x + width;
		}

		protected int AddAddressOffset(ViewInfo view, int x, int y)
		{
			if (view.Settings.ShowOffset)
			{
				x = AddText(view, x, y, view.Settings.Offset, HotSpot.NoneId, $"{Offset.ToInt32():X04}") + view.Font.Width;
			}

			if (view.Settings.ShowAddress)
			{
#if WIN32
				x = AddText(view, x, y, view.Settings.Address, HotSpot.AddressId, $"{view.Address.Add(Offset).ToInt32():X08}") + view.Font.Width;
#else
				x = AddText(view, x, y, view.Settings.Address, HotSpot.AddressId, $"{view.Address.Add(Offset).ToInt64():X016}") + view.Font.Width;
#endif
			}

			return x;
		}

		protected void AddSelection(ViewInfo view, int x, int y, int height)
		{
			if (y > view.ClientArea.Bottom || y + height < 0)
			{
				return;
			}

			if (IsSelected)
			{
				view.Context.FillRectangle(new SolidBrush(view.Settings.Selected), 0, y, view.ClientArea.Right, height);
			}

			AddClickableArea(view, new Rectangle(0, y, view.ClientArea.Right - (IsSelected ? 16 : 0), height), null,-1, HotSpotType.Select);
		}

		protected int AddIcon(ViewInfo view, int x, int y, Image icon, int id, HotSpotType type)
		{
			const int IconSize = 16;

			if (y > view.ClientArea.Bottom || y + IconSize < 0)
			{
				return x + IconSize;
			}

			view.Context.DrawImage(icon, x + 2, y, 16, 16);

			if (id != -1)
			{
				AddClickableArea(view, new Rectangle(x, y, IconSize, IconSize), null, id, type);
			}

			return x + IconSize;
		}

		protected int AddOpenClose(ViewInfo view, int x, int y)
		{
			if (y > view.ClientArea.Bottom || y + 16 < 0)
			{
				return x + 16;
			}

			return AddIcon(view, x, y, levelsOpen[view.Level] ? Icons.OpenCloseOpen : Icons.OpenCloseClosed, 0, HotSpotType.OpenClose);
		}

		protected void AddDelete(ViewInfo view, int x, int y)
		{
			if (y > view.ClientArea.Bottom || y + 16 < 0)
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, view.ClientArea.Right - 16, y, Icons.Delete, 0, HotSpotType.Delete);
			}
		}

		protected void AddTypeDrop(ViewInfo view, int x, int y)
		{
			if (view.MultiSelected || (y > view.ClientArea.Bottom || y + 16 < 0))
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, 0, y, Icons.DropArrow, 0, HotSpotType.Drop);
			}
		}

		protected int AddRTTI(ViewInfo view, int x, int y)
		{
			return x;
		}

		protected virtual int AddComment(ViewInfo view, int x, int y)
		{
			x = AddText(view, x, y, view.Settings.Comment, HotSpot.NoneId, "//");
			x = AddText(view, x, y, view.Settings.Comment, HotSpot.CommentId, Comment + " ");

			return x;
		}

		protected int DrawHidden(ViewInfo view, int x, int y)
		{
			view.Context.FillRectangle(new SolidBrush(IsSelected ? view.Settings.Selected : view.Settings.Hidden), 0, y, view.ClientArea.Right, 1);

			return y + 1;
		}
	}
}
