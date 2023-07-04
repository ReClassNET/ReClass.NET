using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Extensions;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.UI;
using ReClassNET.Util;

namespace ReClassNET.Controls
{
	public partial class MemoryViewControl : UserControl
	{
		/// <summary>
		/// Contains informations about a selected node.
		/// </summary>
		public class SelectedNodeInfo
		{
			/// <summary>
			/// The selected node.
			/// </summary>
			public BaseNode Node { get; }

			public RemoteProcess Process { get; }

			/// <summary>
			/// The memory this node uses.
			/// </summary>
			public MemoryBuffer Memory { get; }

			/// <summary>
			/// The address of the node in the remote process.
			/// </summary>
			public IntPtr Address { get; }

			public int Level { get; }

			public SelectedNodeInfo(BaseNode node, RemoteProcess process, MemoryBuffer memory, IntPtr address, int level)
			{
				Contract.Requires(node != null);
				Contract.Requires(process != null);
				Contract.Requires(memory != null);

				Node = node;
				Process = process;
				Memory = memory;
				Address = address;
				Level = level;
			}
		}

		private readonly List<HotSpot> hotSpots = new List<HotSpot>();
		private readonly List<HotSpot> selectedNodes = new List<HotSpot>();

		private HotSpot selectionCaret;
		private HotSpot selectionAnchor;

		private readonly FontEx font;

		public ContextMenuStrip NodeContextMenuStrip { get; set; }

		public event DrawContextRequestEventHandler DrawContextRequested;
		public event EventHandler SelectionChanged;
		public event NodeClickEventHandler ChangeClassTypeClick;
		public event NodeClickEventHandler ChangeWrappedTypeClick;
		public event NodeClickEventHandler ChangeEnumTypeClick;

		private readonly MemoryPreviewPopUp memoryPreviewPopUp;

		public MemoryViewControl()
		{
			InitializeComponent();

			if (Program.DesignMode)
			{
				return;
			}

			AutoScroll = true;

			font = Program.MonoSpaceFont;

			hotSpotEditBox.Font = font;

			memoryPreviewPopUp = new MemoryPreviewPopUp(font);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (DesignMode)
			{
				e.Graphics.FillRectangle(Brushes.White, ClientRectangle);

				return;
			}

			var args = new DrawContextRequestEventArgs();

			var requestHandler = DrawContextRequested;
			requestHandler?.Invoke(this, args);

			hotSpots.Clear();

			using (var brush = new SolidBrush(Program.Settings.BackgroundColor))
			{
				e.Graphics.FillRectangle(brush, ClientRectangle);
			}

			if (args.Process == null || args.Memory == null || args.Node == null)
			{
				return;
			}

			if (memoryPreviewPopUp.Visible)
			{
				memoryPreviewPopUp.UpdateMemory();
			}

			var view = new DrawContext
			{
				Settings = args.Settings,
				Graphics = e.Graphics,
				Font = font,
				IconProvider = args.IconProvider,
				Process = args.Process,
				Memory = args.Memory,
				CurrentTime = args.CurrentTime,
				ClientArea = ClientRectangle,
				HotSpots = hotSpots,
				Address = args.BaseAddress,
				Level = 0,
				MultipleNodesSelected = selectedNodes.Count > 1
			};

			var scrollPosition = AutoScrollPosition;

			var drawnSize = args.Node.Draw(
				view,
				scrollPosition.X,
				scrollPosition.Y
			);
			drawnSize.Width += 10;

			/*foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
			{
				e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(150, 255, 0, 0)), 1), spot.Rect);
			}*/

			AutoScrollMinSize = new Size(Math.Max(drawnSize.Width, ClientSize.Width), Math.Max(drawnSize.Height, ClientSize.Height));

			// Sometimes setting AutoScrollMinSize resets AutoScrollPosition. This restores the original position.
			AutoScrollPosition = new Point(-scrollPosition.X, -scrollPosition.Y);
		}

		private void OnSelectionChanged()
		{
			SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		#region Process Input

		protected override void OnMouseClick(MouseEventArgs e)
		{
			Contract.Requires(e != null);

			hotSpotEditBox.Hide();

			var invalidate = false;

			foreach (var hotSpot in hotSpots)
			{
				if (hotSpot.Rect.Contains(e.Location))
				{
					var hitObject = hotSpot.Node;

					if (hotSpot.Type == HotSpotType.OpenClose)
					{
						hitObject.ToggleLevelOpen(hotSpot.Level);

						invalidate = true;

						break;
					}
					if (hotSpot.Type == HotSpotType.Click)
					{
						hitObject.Update(hotSpot);

						invalidate = true;

						break;
					}
					if (hotSpot.Type == HotSpotType.Select)
					{
						if (e.Button == MouseButtons.Left)
						{
							if (ModifierKeys == Keys.None)
							{
								ClearSelection();

								hitObject.IsSelected = true;

								selectedNodes.Add(hotSpot);

								OnSelectionChanged();

								selectionAnchor = selectionCaret = hotSpot;
							}
							else if (ModifierKeys == Keys.Control)
							{
								hitObject.IsSelected = !hitObject.IsSelected;

								if (hitObject.IsSelected)
								{
									selectedNodes.Add(hotSpot);
								}
								else
								{
									selectedNodes.Remove(selectedNodes.FirstOrDefault(c => c.Node == hitObject));
								}

								OnSelectionChanged();
							}
							else if (ModifierKeys == Keys.Shift)
							{
								if (selectedNodes.Count > 0)
								{
									var selectedNode = selectedNodes[0].Node;
									if (hitObject.GetParentContainer() != null && selectedNode.GetParentContainer() != hitObject.GetParentContainer())
									{
										continue;
									}

									if (hotSpot.Node is BaseContainerNode)
									{
										continue;
									}

									var first = Utils.Min(selectedNodes[0], hotSpot, h => h.Node.Offset);
									var last = first == hotSpot ? selectedNodes[0] : hotSpot;

									ClearSelection();

									var containerNode = selectedNode.GetParentContainer();
									foreach (var spot in containerNode.Nodes
										.SkipWhile(n => n != first.Node)
										.TakeWhileInclusive(n => n != last.Node)
										.Select(n => new HotSpot
										{
											Address = (IntPtr)(containerNode.Offset + n.Offset),
											Node = n,
											Process = first.Process,
											Memory = first.Memory,
											Level = first.Level
										}))
									{
										spot.Node.IsSelected = true;
										selectedNodes.Add(spot);
									}

									OnSelectionChanged();

									selectionAnchor = first;
									selectionCaret = last;
								}
							}
						}
						else if (e.Button == MouseButtons.Right)
						{
							// If there is only one selected node, select the node the user clicked at.
							if (selectedNodes.Count <= 1)
							{
								ClearSelection();

								hitObject.IsSelected = true;

								selectedNodes.Add(hotSpot);

								OnSelectionChanged();

								selectionAnchor = selectionCaret = hotSpot;
							}

							ShowNodeContextMenu(e.Location);
						}

						invalidate = true;
					}
					else if (hotSpot.Type == HotSpotType.Context)
					{
						ShowNodeContextMenu(e.Location);

						break;
					}
					else if (hotSpot.Type == HotSpotType.Delete)
					{
						hotSpot.Node.GetParentContainer().RemoveNode(hotSpot.Node);

						invalidate = true;

						break;
					}
					else if (hotSpot.Type == HotSpotType.ChangeClassType || hotSpot.Type == HotSpotType.ChangeWrappedType || hotSpot.Type == HotSpotType.ChangeEnumType)
					{
						var handler = hotSpot.Type switch
						{
							HotSpotType.ChangeClassType => ChangeClassTypeClick,
							HotSpotType.ChangeWrappedType => ChangeWrappedTypeClick,
							HotSpotType.ChangeEnumType => ChangeEnumTypeClick
						};

						handler?.Invoke(this, new NodeClickEventArgs(hitObject, hotSpot.Address, hotSpot.Memory, e.Button, e.Location));

						break;
					}
				}
			}

			if (invalidate)
			{
				Invalidate();
			}

			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Contract.Requires(e != null);

			hotSpotEditBox.Hide();

			var invalidate = false;

			// Order the hotspots: 1. DoubleClick 2. Click 3. Edit 4. Select
			var spots = hotSpots.Where(h => h.Type == HotSpotType.DoubleClick)
				.Concat(hotSpots.Where(h => h.Type == HotSpotType.Click))
				.Concat(hotSpots.Where(h => h.Type == HotSpotType.Edit))
				.Concat(hotSpots.Where(h => h.Type == HotSpotType.Select));

			foreach (var hotSpot in spots)
			{
				if (hotSpot.Rect.Contains(e.Location))
				{
					if (hotSpot.Type == HotSpotType.DoubleClick || hotSpot.Type == HotSpotType.Click)
					{
						hotSpot.Node.Update(hotSpot);

						invalidate = true;

						break;
					}
					if (hotSpot.Type == HotSpotType.Edit)
					{
						hotSpotEditBox.ShowOnHotSpot(hotSpot);

						break;
					}
					if (hotSpot.Type == HotSpotType.Select)
					{
						hotSpot.Node.ToggleLevelOpen(hotSpot.Level);

						invalidate = true;

						break;
					}
				}
			}

			if (invalidate)
			{
				Invalidate();
			}

			base.OnMouseDoubleClick(e);
		}

		private Point toolTipPosition;
		protected override void OnMouseHover(EventArgs e)
		{
			Contract.Requires(e != null);

			base.OnMouseHover(e);

			if (selectedNodes.Count > 1)
			{
				var memorySize = selectedNodes.Sum(h => h.Node.MemorySize);
				nodeInfoToolTip.Show($"{selectedNodes.Count} Nodes selected, {memorySize} bytes", this, toolTipPosition.Relocate(16, 16));
			}
			else
			{
				foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
				{
					if (spot.Rect.Contains(toolTipPosition))
					{
						if (spot.Node.UseMemoryPreviewToolTip(spot, out var previewAddress))
						{
							memoryPreviewPopUp.InitializeMemory(spot.Process, previewAddress);

							memoryPreviewPopUp.Show(this, toolTipPosition.Relocate(16, 16));
						}
						else
						{
							var text = spot.Node.GetToolTipText(spot);
							if (!string.IsNullOrEmpty(text))
							{
								nodeInfoToolTip.Show(text, this, toolTipPosition.Relocate(16, 16));
							}
						}

						return;
					}
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			Contract.Requires(e != null);

			base.OnMouseMove(e);

			if (e.Location != toolTipPosition)
			{
				toolTipPosition = e.Location;

				nodeInfoToolTip.Hide(this);

				if (memoryPreviewPopUp.Visible)
				{
					memoryPreviewPopUp.Close();

					Invalidate();
				}

				ResetMouseEventArgs();
			}
		}

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			hotSpotEditBox.Hide();

			if (memoryPreviewPopUp.Visible)
			{
				memoryPreviewPopUp.HandleMouseWheelEvent(e);
			}
			else
			{
				base.OnMouseWheel(e);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (hotSpotEditBox.Visible == false) // Only process keys if the edit field is not visible.
			{
				var key = keyData & Keys.KeyCode;
				var modifier = keyData & Keys.Modifiers;

				if (selectedNodes.Count > 0)
				{
					if (key == Keys.Menu)
					{
						ShowNodeContextMenu(new Point(10, 10));

						return true;
					}
					if ((key == Keys.Down || key == Keys.Up) && selectionCaret != null && selectionAnchor != null)
					{
						HotSpot toSelect;
						bool isAtEnd;

						var query = hotSpots
							.Where(h => h.Type == HotSpotType.Select)
							.Where(h => h.Node.GetParentContainer() == selectionCaret.Node.GetParentContainer());

						if (key == Keys.Down)
						{
							var temp = query
								.SkipWhile(h => h.Node != selectionCaret.Node)
								.Skip(1)
								.ToList();

							toSelect = temp.FirstOrDefault();
							isAtEnd = toSelect != null && toSelect == temp.LastOrDefault();
						}
						else
						{
							var temp = query
								.TakeWhile(h => h.Node != selectionCaret.Node)
								.ToList();

							toSelect = temp.LastOrDefault();
							isAtEnd = toSelect != null && toSelect == temp.FirstOrDefault();
						}

						if (toSelect != null && !(toSelect.Node is ClassNode))
						{
							if (modifier != Keys.Shift)
							{
								selectionAnchor = selectionCaret = toSelect;
							}
							else
							{
								selectionCaret = toSelect;
							}

							var first = Utils.Min(selectionAnchor, selectionCaret, h => h.Node.Offset);
							var last = first == selectionAnchor ? selectionCaret : selectionAnchor;

							selectedNodes.ForEach(h => h.Node.ClearSelection());
							selectedNodes.Clear();

							var containerNode = toSelect.Node.GetParentContainer();
							foreach (var spot in containerNode.Nodes
								.SkipWhile(n => n != first.Node)
								.TakeWhileInclusive(n => n != last.Node)
								.Select(n => new HotSpot
								{
									Address = (IntPtr)(containerNode.Offset + n.Offset),
									Node = n,
									Process = toSelect.Process,
									Memory = toSelect.Memory,
									Level = toSelect.Level
								}))
							{
								spot.Node.IsSelected = true;
								selectedNodes.Add(spot);
							}

							OnSelectionChanged();

							if (isAtEnd)
							{
								const int ScrollAmount = 3;

								var position = AutoScrollPosition;
								AutoScrollPosition = new Point(-position.X, -position.Y + (key == Keys.Down ? ScrollAmount : -ScrollAmount) * font.Height);
							}

							Invalidate();

							return true;
						}
					}
					else if (key == Keys.Left || key == Keys.Right)
					{
						if (selectedNodes.Count == 1)
						{
							var selected = selectedNodes[0];

							selected.Node.SetLevelOpen(selected.Level, key == Keys.Right);
						}
					}
				}
				else if (key == Keys.Down || key == Keys.Up)
				{
					// If no node is selected, try to select the first one.
					var selection = hotSpots
						.Where(h => h.Type == HotSpotType.Select)
						.WhereNot(h => h.Node is ClassNode)
						.FirstOrDefault();
					if (selection != null)
					{
						selectionAnchor = selectionCaret = selection;

						selection.Node.IsSelected = true;

						selectedNodes.Add(selection);

						OnSelectionChanged();

						return true;
					}
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		#endregion

		#region Event Handler

		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			Invalidate();
		}

		private void repaintTimer_Tick(object sender, EventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			Invalidate(false);
		}

		private void editBox_Committed(object sender, HotSpotTextBoxCommitEventArgs e)
		{
			var hotSpot = e.HotSpot;
			if (hotSpot != null)
			{
				try
				{
					hotSpot.Node.Update(hotSpot);
				}
				catch (Exception ex)
				{
					Program.Logger.Log(ex);
				}

				Invalidate();
			}

			Focus();
		}

		#endregion

		/// <summary>
		/// Gets informations about all selected nodes.
		/// </summary>
		/// <returns>A list with informations about all selected nodes.</returns>
		public IReadOnlyList<SelectedNodeInfo> GetSelectedNodes()
		{
			return selectedNodes
				.Select(h => new SelectedNodeInfo(h.Node, h.Process, h.Memory, h.Address, h.Level))
				.ToList();
		}

		/// <summary>
		/// Selects the given nodes.
		/// </summary>
		/// <param name="nodes"></param>
		public void SetSelectedNodes(IEnumerable<SelectedNodeInfo> nodes)
		{
			selectedNodes.ForEach(h => h.Node.ClearSelection());

			selectedNodes.Clear();

			selectedNodes.AddRange(nodes.Select(i => new HotSpot { Type = HotSpotType.Select, Node = i.Node, Process = i.Process, Memory = i.Memory, Address = i.Address, Level = i.Level }));
			selectedNodes.ForEach(h => h.Node.IsSelected = true);

			OnSelectionChanged();
		}

		/// <summary>
		/// Shows the context menu at the given point.
		/// </summary>
		/// <param name="location">The location where the context menu should be shown.</param>
		private void ShowNodeContextMenu(Point location)
		{
			NodeContextMenuStrip?.Show(this, location);
		}

		public void ShowNodeNameEditBox(BaseNode node)
		{
			if (node == null || node is BaseHexNode)
			{
				return;
			}

			var hotSpot = hotSpots
				.FirstOrDefault(s => s.Node == node && s.Type == HotSpotType.Edit && s.Id == HotSpot.NameId);
			if (hotSpot != null)
			{
				hotSpotEditBox.ShowOnHotSpot(hotSpot);
			}
		}

		/// <summary>
		/// Resets the selection state of all selected nodes.
		/// </summary>
		public void ClearSelection()
		{
			selectionAnchor = selectionCaret = null;

			selectedNodes.ForEach(h => h.Node.ClearSelection());

			selectedNodes.Clear();

			OnSelectionChanged();

			//Invalidate();
		}

		/// <summary>
		/// Resets the control to the initial state.
		/// </summary>
		public void Reset()
		{
			ClearSelection();

			hotSpotEditBox.Hide();

			VerticalScroll.Value = VerticalScroll.Minimum;
		}
		
		public void InitCurrentClassFromRTTI(ClassNode classNode)
		{
			var args = new DrawContextRequestEventArgs { Node = classNode };

			var requestHandler = DrawContextRequested;
			requestHandler?.Invoke(this, args);
			var view = new DrawContext
					   {
						   Settings = args.Settings,
						   Process = args.Process,
						   Memory = args.Memory,
						   CurrentTime = args.CurrentTime,
						   Address = args.BaseAddress,
						   Level = 0,
					   };
			classNode.InitFromRTTI(view);
		}
	}
}
