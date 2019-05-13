using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using ReClassNET.Extensions;
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
		protected static readonly int HiddenHeight = 0;

		private static int nodeIndex = 0;

		private string name = string.Empty;
		private string comment = string.Empty;

		/// <summary>Gets or sets the offset of the node.</summary>
		public int Offset { get; set; }

		/// <summary>Gets or sets the name of the node. If a new name was set the property changed event gets fired.</summary>
		public virtual string Name { get => name; set { if (value != null && name != value) { name = value; NameChanged?.Invoke(this); } } }

		/// <summary>Gets or sets the comment of the node.</summary>
		public string Comment { get => comment; set { if (value != null && comment != value) { comment = value; CommentChanged?.Invoke(this); } } }

		/// <summary>Gets or sets the parent node.</summary>
		public BaseNode ParentNode { get; internal set; }

		/// <summary>Gets a value indicating whether this node is wrapped into an other node.</summary>
		public bool IsWrapped => ParentNode is BaseWrapperNode;

		/// <summary>Gets or sets a value indicating whether this node is hidden.</summary>
		public bool IsHidden { get; set; }

		/// <summary>Gets or sets a value indicating whether this node is selected.</summary>
		public bool IsSelected { get; set; }

		/// <summary>Size of the node in bytes.</summary>
		public abstract int MemorySize { get; }

		public event NodeEventHandler NameChanged;
		public event NodeEventHandler CommentChanged;

		protected GrowingList<bool> LevelsOpen { get; } = new GrowingList<bool>(false);

		[ContractInvariantMethod]
		private void ObjectInvariants()
		{
			Contract.Invariant(name != null);
			Contract.Invariant(comment != null);
			Contract.Invariant(Offset >= 0);
			Contract.Invariant(LevelsOpen != null);
		}

		/// <summary>
		/// Creates an instance of the specific node type.
		/// </summary>
		/// <param name="nodeType">The <see cref="Type"/> of the node.</param>
		/// <returns>An instance of the node type or null if the type is not a valid node type.</returns>
		public static BaseNode CreateInstanceFromType(Type nodeType)
		{
			return CreateInstanceFromType(nodeType, true);
		}

		/// <summary>
		/// Creates an instance of the specific node type.
		/// </summary>
		/// <param name="nodeType">The <see cref="Type"/> of the node.</param>
		/// <param name="callInitialize">If true <see cref="Initialize"/> gets called for the new node.</param>
		/// <returns>An instance of the node type or null if the type is not a valid node type.</returns>
		public static BaseNode CreateInstanceFromType(Type nodeType, bool callInitialize)
		{
			var node = Activator.CreateInstance(nodeType) as BaseNode;
			if (callInitialize)
			{
				node?.Initialize();
			}
			return node;
		}

		/// <summary>Constructor which sets a unique <see cref="Name"/>.</summary>
		protected BaseNode()
		{
			Contract.Ensures(name != null);
			Contract.Ensures(comment != null);

			Name = $"N{nodeIndex++:X08}";
			Comment = string.Empty;

			LevelsOpen[0] = true;
		}

		public abstract void GetUserInterfaceInfo(out string name, out Image icon);

		public virtual bool UseMemoryPreviewToolTip(HotSpot spot, out IntPtr address)
		{
			Contract.Requires(spot != null);

			address = IntPtr.Zero;

			return false;
		}

		/// <summary>Gets informations about this node to show in a tool tip.</summary>
		/// <param name="spot">The spot.</param>
		/// <returns>The information to show in a tool tip or null if no information should be shown.</returns>
		public virtual string GetToolTipText(HotSpot spot)
		{
			Contract.Requires(spot != null);

			return null;
		}

		/// <summary>Called when the node was created. Does not get called after loading a project.</summary>
		public virtual void Initialize()
		{

		}

		/// <summary>Initializes this object from the given node. It copies the name and the comment.</summary>
		/// <param name="node">The node to copy from.</param>
		public virtual void CopyFromNode(BaseNode node)
		{
			Contract.Requires(node != null);

			Name = node.Name;
			Comment = node.Comment;
			Offset = node.Offset;
		}

		/// <summary>
		/// Gets the parent container of the node.
		/// </summary>
		/// <returns></returns>
		public BaseContainerNode GetParentContainer()
		{
			var parentNode = ParentNode;
			while (parentNode != null)
			{
				if (parentNode is BaseContainerNode containerNode)
				{
					return containerNode;
				}

				parentNode = parentNode.ParentNode;
			}

			if (this is BaseContainerNode containerNode2)
			{
				return containerNode2;
			}

			return null;
		}

		/// <summary>
		/// Gets the parent class of the node.
		/// </summary>
		/// <returns></returns>
		public ClassNode GetParentClass()
		{
			var parentNode = ParentNode;
			while (parentNode != null)
			{
				if (parentNode is ClassNode classNode)
				{
					return classNode;
				}

				parentNode = parentNode.ParentNode;
			}

			return null;
		}

		/// <summary>
		/// Gets the root wrapper node if this node is the inner node of a wrapper chain.
		/// </summary>
		/// <returns>The root <see cref="BaseWrapperNode"/> or null if this node is not wrapped or isn't itself a wrapper node.</returns>
		public BaseWrapperNode GetRootWrapperNode()
		{
			BaseWrapperNode rootWrapperNode = null;

			var parentNode = ParentNode;
			while (parentNode is BaseWrapperNode wrapperNode)
			{
				rootWrapperNode = wrapperNode;

				parentNode = parentNode.ParentNode;
			}

			// Test if this node is the root wrapper node.
			if (rootWrapperNode == null)
			{
				if (this is BaseWrapperNode wrapperNode)
				{
					return wrapperNode;
				}
			}

			return rootWrapperNode;
		}

		/// <summary>Clears the selection of the node.</summary>
		public virtual void ClearSelection()
		{
			IsSelected = false;
		}

		/// <summary>Draws the node.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		/// <returns>The pixel size the node occupies.</returns>
		public abstract Size Draw(ViewInfo view, int x, int y);

		/// <summary>
		/// Calculates the height of the node if drawn.
		/// This method is used to determine if a node outside the visible area should be drawn.
		/// The returned height must be equal to the height which is returned by the <see cref="Draw(ViewInfo, int, int)"/> method.
		/// </summary>
		/// <param name="view">The view information.</param>
		/// <returns>The calculated height.</returns>
		public abstract int CalculateDrawnHeight(ViewInfo view);

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
			LevelsOpen[level] = !LevelsOpen[level];
		}

		/// <summary>Sets the specific level.</summary>
		/// <param name="level">The level to set.</param>
		/// <param name="open">True to open.</param>
		internal void SetLevelOpen(int level, bool open)
		{
			LevelsOpen[level] = open;
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
				Address = view.Address + Offset,
				Id = id,
				Type = type,
				Node = this,
				Level = view.Level,
				Process = view.Process,
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

				view.Context.DrawStringEx(text, view.Font.Font, color, x, y);
				/*using (var brush = new SolidBrush(color))
				{
					view.Context.DrawString(text, view.Font.Font, brush, x, y);
				}*/
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
				x = AddText(view, x, y, view.Settings.OffsetColor, HotSpot.NoneId, $"{Offset:X04}") + view.Font.Width;
			}

			if (view.Settings.ShowNodeAddress)
			{
				x = AddText(view, x, y, view.Settings.AddressColor, HotSpot.AddressId, (view.Address + Offset).ToString(Constants.AddressHexFormat)) + view.Font.Width;
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

			if (y > view.ClientArea.Bottom || y + height < 0 || IsWrapped)
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

			AddHotSpot(view, new Rectangle(0, y, view.ClientArea.Right - (IsSelected ? 16 : 0), height), string.Empty, HotSpot.NoneId, HotSpotType.Select);
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

			if (id != HotSpot.NoneId)
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
		protected int AddOpenCloseIcon(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0)
			{
				return x + Icons.Dimensions;
			}

			return AddIcon(view, x, y, LevelsOpen[view.Level] ? Icons.OpenCloseOpen : Icons.OpenCloseClosed, 0, HotSpotType.OpenClose);
		}

		/// <summary>Draws a context drop icon if the node is selected.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="y">The y coordinate.</param>
		protected void AddContextDropDownIcon(ViewInfo view, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (view.MultipleNodesSelected || y > view.ClientArea.Bottom || y + Icons.Dimensions < 0 || IsWrapped)
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, 0, y, Icons.DropArrow, 0, HotSpotType.Context);
			}
		}

		/// <summary>Draws a delete icon if the node is selected.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="y">The y coordinate.</param>
		protected void AddDeleteIcon(ViewInfo view, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			if (y > view.ClientArea.Bottom || y + Icons.Dimensions < 0 || IsWrapped)
			{
				return;
			}

			if (IsSelected)
			{
				AddIcon(view, view.ClientArea.Right - Icons.Dimensions, y, Icons.Delete, 0, HotSpotType.Delete);
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
		/// <returns>The size of the drawing.</returns>
		protected Size DrawHidden(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);
			Contract.Requires(view.Context != null);

			using (var brush = new SolidBrush(IsSelected ? view.Settings.SelectedColor : view.Settings.HiddenColor))
			{
				view.Context.FillRectangle(brush, 0, y, view.ClientArea.Right, 1);
			}

			return new Size(0, HiddenHeight);
		}

		/// <summary>Draws an error indicator if the used memory buffer is not valid.</summary>
		/// <param name="view">The view information.</param>
		/// <param name="y">The y coordinate.</param>
		protected void DrawInvalidMemoryIndicatorIcon(ViewInfo view, int y)
		{
			if (!view.Memory.ContainsValidData)
			{
				AddIcon(view, 0, y, Properties.Resources.B16x16_Error, HotSpot.NoneId, HotSpotType.None);
			}
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

		public override Size Draw(ViewInfo view, int x, int y)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}

		public override int CalculateDrawnHeight(ViewInfo view)
		{
			Contract.Requires(view != null);

			throw new NotImplementedException();
		}
	}
}
