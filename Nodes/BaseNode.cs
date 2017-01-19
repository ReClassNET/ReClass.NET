using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Memory;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Nodes
{
	public delegate void NodeEventHandler(BaseNode sender);

	[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
	[ContractClass(typeof(BaseNodeContract))]
	public abstract class BaseNode
	{
		private string DebuggerDisplay => $"Type: {GetType().Name} Name: {Name} Offset: 0x{Offset.ToString("X")}";

		internal static readonly List<INodeInfoReader> NodeInfoReader = new List<INodeInfoReader>();

		protected static readonly int TextPadding = Icons.Dimensions;
		protected const int HiddenHeight = 1;

		private static int NodeIndex = 0;

		private string name = string.Empty;
		private string comment = string.Empty;

		/// <summary>Gets or sets the offset of the node.</summary>
		public IntPtr Offset { get; set; }

		/// <summary>Gets or sets the name of the node. If a new name was set the property changed event gets fired.</summary>
		public virtual string Name { get { return name; } set { if (value != null && name != value) { name = value; NameChanged?.Invoke(this); } } }

		/// <summary>Gets or sets the comment of the node.</summary>
		public string Comment { get { return comment; } set { if (value != null && comment != value) { comment = value; CommentChanged?.Invoke(this); } } }

		/// <summary>Gets or sets the parent node.</summary>
		public BaseContainerNode ParentNode { get; internal set; }

		/// <summary>Gets or sets a value indicating whether this object is hidden.</summary>
		public bool IsHidden { get; protected set; }

		/// <summary>Gets or sets a value indicating whether this object is selected.</summary>
		public bool IsSelected { get; set; }

		/// <summary>Size of the node in bytes.</summary>
		public abstract int MemorySize { get; }

		public event NodeEventHandler NameChanged;
		public event NodeEventHandler CommentChanged;

		protected GrowingList<bool> levelsOpen = new GrowingList<bool>(false);

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(name != null);
			Contract.Invariant(comment != null);
			Contract.Invariant(Offset.ToInt32() >= 0);
			Contract.Invariant(levelsOpen != null);
		}

		/// <summary>Constructor which sets a unique <see cref="Name"/>.</summary>
		protected BaseNode()
		{
			Contract.Ensures(name != null);
			Contract.Ensures(comment != null);

			Name = $"N{NodeIndex++:X08}";
			Comment = string.Empty;

			levelsOpen[0] = true;
		}

		/// <summary>Clears the selection of the node.</summary>
		public virtual void ClearSelection()
		{
			IsSelected = false;
		}

		/// <summary>Initializes this object from the given node. It copies the name and the comment.</summary>
		/// <param name="node">The node to copy from.</param>
		public virtual void CopyFromNode(BaseNode node)
		{
			Contract.Requires(node != null);

			Name = node.Name;
			Comment = node.Comment;
		}


		/// <summary>Called when the node was created. Does not get called after loading a project.</summary>
		public virtual void Intialize()
		{

		}

		public virtual bool UseMemoryPreviewToolTip(HotSpot spot, MemoryBuffer memory, out IntPtr address)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			address = IntPtr.Zero;

			return false;
		}

		/// <summary>Gets informations about this node to show in a tool tip.</summary>
		/// <param name="spot">The spot.</param>
		/// <param name="memory">The process memory.</param>
		/// <returns>The information to show in a tool tip or null if no information should be shown.</returns>
		public virtual string GetToolTipText(HotSpot spot, MemoryBuffer memory)
		{
			Contract.Requires(spot != null);
			Contract.Requires(memory != null);

			return null;
		}

		/// <summary>Draws the node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The height the node occupies.</returns>
		public abstract int Draw(ViewInfo view, int x, int y);

		/// <summary>
		/// Calculates the height of the node if drawn.
		/// This method is used to determine if a node outside the visible area should be drawn.
		/// The returned height must be equal to the height which gets added by the <see cref="Draw(ViewInfo, int, int)"/> method.
		/// </summary>
		/// <param name="view">The view information.</param>
		/// <returns>The calculated height.</returns>
		public abstract int CalculateHeight(ViewInfo view);

		/// <summary>Updates the node from the given <paramref name="spot"/>. Sets the <see cref="Name"/> and <see cref="Comment"/> of the node.</summary>
		/// <param name="spot">The spot.</param>
		public virtual void Update(HotSpot spot)
		{
			Contract.Requires(spot != null);

			if (spot.Id == HotSpot.NameId)
			{
				Name = spot.Text;
			}
			else if (spot.Id == HotSpot.CommentId)
			{
				Comment = spot.Text;
			}
		}

		/// <summary>Toggles the specified level.</summary>
		/// <param name="level">The level to toggle.</param>
		internal void ToggleLevelOpen(int level)
		{
			levelsOpen[level] = !levelsOpen[level];
		}

		/// <summary>Sets the specific level.</summary>
		/// <param name="level">The level to set.</param>
		/// <param name="open">True to open.</param>
		internal void SetLevelOpen(int level, bool open)
		{
			levelsOpen[level] = open;
		}

		/// <summary>Adds a <see cref="HotSpot"/> the user can interact with.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="spot">The spot.</param>
		/// <param name="text">The text to edit.</param>
		/// <param name="id">The id of the spot.</param>
		/// <param name="type">The type of the spot.</param>
		protected void AddHotSpot(ViewInfo view, Rectangle spot, string text, int id, HotSpotType type)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Memory != null);
			Contract.Requires(text != null);

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
				Level = view.Level,
				Memory = view.Memory
			});
		}

		/// <summary>Draws the specific text and adds a <see cref="HotSpot"/> if <paramref name="hitId"/> is not <see cref="HotSpot.NoneId"/>.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="color">The color of the text.</param>
		/// <param name="hitId">Id for the clickable area.</param>
		/// <param name="text">The text to draw.</param>
		/// <returns>The new x coordinate after drawing the text.</returns>
		protected int AddText(ViewInfo view, int x, int y, Color color, int hitId, string text)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);
			Contract.Requires(view.Font != null);
			Contract.Requires(text != null);

			var width = Math.Max(text.Length, hitId != HotSpot.NoneId ? 1 : 0) * view.Font.Width;

			if (y >= -view.Font.Height && y + view.Font.Height <= view.ClientArea.Bottom + view.Font.Height)
			{
				if (hitId != HotSpot.NoneId)
				{
					var rect = new Rectangle(x, y, width, view.Font.Height);
					AddHotSpot(view, rect, text, hitId, HotSpotType.Edit);
				}

				using (var brush = new SolidBrush(color))
				{
					view.Context.DrawString(text, view.Font.Font, brush, x, y);
				}
			}

			return x + width;
		}

		/// <summary>Draws the address and <see cref="Offset"/> of the node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The new x coordinate after drawing the text.</returns>
		protected int AddAddressOffset(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);
			Contract.Requires(view.Font != null);

			if (view.Settings.ShowNodeOffset)
			{
				x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, $"{Offset.ToInt32():X04}") + view.Font.Width;
			}

			if (view.Settings.ShowNodeAddress)
			{
				x = AddText(view, x, y, view.Settings.AddressColor, HotSpot.AddressId, view.Address.Add(Offset).ToString(Constants.StringHexFormat)) + view.Font.Width;
			}

			return x;
		}

		/// <summary>Draws a bar which indicates the selection status of the node. A <see cref="HotSpot"/> for this area gets added too.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="height">The height of the bar.</param>
		protected void AddSelection(ViewInfo view, int x, int y, int height)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (y > view.ClientArea.Bottom || y + height < 0)
			{
				return;
			}

			if (IsSelected)
			{
				using (var brush = new SolidBrush(view.Settings.SelectedColor))
				{
					view.Context.FillRectangle(brush, 0, y, view.ClientArea.Right, height);
				}
			}

			AddHotSpot(view, new Rectangle(0, y, view.ClientArea.Right - (IsSelected ? 16 : 0), height), string.Empty, -1, HotSpotType.Select);
		}

		/// <summary>Draws an icon and adds a <see cref="HotSpot"/> if <paramref name="id"/> is not <see cref="HotSpot.NoneId"/>.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <param name="icon">The icon.</param>
		/// <param name="id">The id of the spot.</param>
		/// <param name="type">The type of the spot.</param>
		/// <returns>The new x coordinate after drawing the icon.</returns>
		protected int AddIcon(ViewInfo view, int x, int y, Image icon, int id, HotSpotType type)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);
			Contract.Requires(icon != null);

			if (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0)
			{
				return x + Icons.Dimensions;
			}

			view.Context.DrawImage(icon, x + 2, y, Icons.Dimensions, Icons.Dimensions);

			if (id != -1)
			{
				AddHotSpot(view, new Rectangle(x, y, Icons.Dimensions, Icons.Dimensions), string.Empty, id, type);
			}

			return x + Icons.Dimensions;
		}

		/// <summary>Adds a togglable Open/Close icon.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The new x coordinate after drawing the icon.</returns>
		protected int AddOpenClose(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0)
			{
				return x + Icons.Dimensions;
			}

			return AddIcon(view, x, y, levelsOpen[view.Level] ? Icons.OpenCloseOpen : Icons.OpenCloseClosed, 0, HotSpotType.OpenClose);
		}

		/// <summary>Draws a delete icon if the node is selected.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		protected void AddDelete(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0)
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, view.ClientArea.Right - Icons.Dimensions, y, Icons.Delete, 0, HotSpotType.Delete);
			}
		}

		/// <summary>Draws a type drop icon if the node is selected.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		protected void AddTypeDrop(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (view.MultiSelected || (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0))
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, 0, y, Icons.DropArrow, 0, HotSpotType.Drop);
			}
		}

		/// <summary>Draws the <see cref="Comment"/>.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The new x coordinate after drawing the comment.</returns>
		protected virtual int AddComment(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);
			Contract.Requires(view.Font != null);

			x = AddText(view, x, y, view.Settings.CommentColor, HotSpot.NoneId, "//");
			x = AddText(view, x, y, view.Settings.CommentColor, HotSpot.CommentId, Comment) + view.Font.Width;

			return x;
		}

		/// <summary>Draws a vertical line to show the hidden state.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The new y coordinate after drawing the line.</returns>
		protected int DrawHidden(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			using (var brush = new SolidBrush(IsSelected ? view.Settings.SelectedColor : view.Settings.HiddenColor))
			{
				view.Context.FillRectangle(brush, 0, y, view.ClientArea.Right, 1);
			}

			return y + HiddenHeight;
		}
	}

	[ContractClassFor(typeof(BaseNode))]
	internal abstract class BaseNodeContract : BaseNode
	{
		public override int MemorySize
		{
			get
			{
				Contract.Ensures(Contract.Result<int>() >= 0);

				throw new NotImplementedException();
			}
		}

		public override int Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}

		public override int CalculateHeight(ViewInfo view)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}
	}
}
