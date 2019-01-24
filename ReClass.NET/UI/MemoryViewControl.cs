using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ReClassNET.DataExchange.ReClass;
using ReClassNET.Extensions;
using ReClassNET.Forms;
using ReClassNET.Memory;
using ReClassNET.MemoryScanner;
using ReClassNET.MemoryScanner.Comparer;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.UI
{
	public partial class MemoryViewControl : ScrollableCustomControl
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
			
			/// <summary>
			/// The memory this node uses.
			/// </summary>
			public MemoryBuffer Memory { get; }

			/// <summary>
			/// The address of the node in the remote process.
			/// </summary>
			public IntPtr Address { get; }

			public SelectedNodeInfo(BaseNode node, MemoryBuffer memory, IntPtr address)
			{
				Contract.Requires(node != null);
				Contract.Requires(memory != null);

				Node = node;
				Memory = memory;
				Address = address;
			}
		}

		private ReClassNetProject project;

		private ClassNode classNode;

		private readonly List<HotSpot> hotSpots = new List<HotSpot>();
		private readonly List<HotSpot> selectedNodes = new List<HotSpot>();

		private HotSpot selectionCaret;
		private HotSpot selectionAnchor;

		private readonly FontEx font;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ReClassNetProject Project
		{
			get => project;
			set
			{
				Contract.Requires(value != null);

				project = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ClassNode ClassNode
		{
			get => classNode;
			set
			{
				editBox.Visible = false;

				ClearSelection();

				OnSelectionChanged();

				classNode = value;

				VerticalScroll.Value = 0;
				if (classNode != null && Memory?.Process != null)
				{
					classNode.UpdateAddress(Memory.Process);
				}
				Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MemoryBuffer Memory { get; set; }

		public event EventHandler SelectionChanged;

		private readonly MemoryPreviewPopUp memoryPreviewPopUp;

		public MemoryViewControl()
		{
			InitializeComponent();

			if (Program.DesignMode)
			{
				return;
			}

			font = Program.MonoSpaceFont;

			editBox.Font = font;

			memoryPreviewPopUp = new MemoryPreviewPopUp(font);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			VerticalScroll.Enabled = true;
			VerticalScroll.Visible = true;
			VerticalScroll.SmallChange = 10;
			HorizontalScroll.Enabled = true;
			HorizontalScroll.Visible = true;
			HorizontalScroll.SmallChange = 100;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (DesignMode)
			{
				e.Graphics.FillRectangle(Brushes.White, ClientRectangle);

				return;
			}

			hotSpots.Clear();

			using (var brush = new SolidBrush(Program.Settings.BackgroundColor))
			{
				e.Graphics.FillRectangle(brush, ClientRectangle);
			}

			if (ClassNode == null)
			{
				return;
			}

			if (Memory.Process != null)
			{
				ClassNode.UpdateAddress(Memory.Process);
			}

			if (memoryPreviewPopUp.Visible)
			{
				memoryPreviewPopUp.UpdateMemory();
			}

			Memory.Size = ClassNode.MemorySize;
			Memory.Update(ClassNode.Offset);

			var view = new ViewInfo
			{
				Settings = Program.Settings,
				Context = e.Graphics,
				Font = font,
				Memory = Memory,
				CurrentTime = DateTime.UtcNow,
				ClientArea = ClientRectangle,
				HotSpots = hotSpots,
				Address = ClassNode.Offset,
				Level = 0,
				MultipleNodesSelected = selectedNodes.Count > 1,
			};

			try
			{
				var drawnSize = ClassNode.Draw(
					view,
					-HorizontalScroll.Value,
					-VerticalScroll.Value * font.Height
				);
				drawnSize.Width += 50;

				/*foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
				{
					e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(150, 255, 0, 0)), 1), spot.Rect);
				}*/

				if (drawnSize.Height > ClientSize.Height)
				{
					VerticalScroll.Enabled = true;

					VerticalScroll.LargeChange = ClientSize.Height / font.Height;
					VerticalScroll.Maximum = (drawnSize.Height - ClientSize.Height) / font.Height + VerticalScroll.LargeChange;
				}
				else
				{
					VerticalScroll.Enabled = false;

					VerticalScroll.Value = 0;
				}

				if (drawnSize.Width > ClientSize.Width)
				{
					HorizontalScroll.Enabled = true;

					HorizontalScroll.LargeChange = ClientSize.Width;
					HorizontalScroll.Maximum = drawnSize.Width - ClientSize.Width + HorizontalScroll.LargeChange;
				}
				else
				{
					HorizontalScroll.Enabled = false;

					HorizontalScroll.Value = 0;
				}
			}
			catch (Exception)
			{
				Debug.Assert(false);
			}
		}

		private void OnSelectionChanged()
		{
			SelectionChanged?.Invoke(this, EventArgs.Empty);
		}

		#region Process Input

		protected override void OnMouseClick(MouseEventArgs e)
		{
			Contract.Requires(e != null);

			var invalidate = false;

			foreach (var hotSpot in hotSpots)
			{
				if (hotSpot.Rect.Contains(e.Location))
				{
					try
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

										var first = Utils.Min(selectedNodes[0], hotSpot, h => h.Node.Offset.ToInt32());
										var last = first == hotSpot ? selectedNodes[0] : hotSpot;

										ClearSelection();

										var containerNode = selectedNode.GetParentContainer();
										foreach (var spot in containerNode.Nodes
											.SkipWhile(n => n != first.Node)
											.TakeUntil(n => n == last.Node)
											.Select(n => new HotSpot
											{
												Address = containerNode.Offset.Add(n.Offset),
												Node = n,
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
						else if (hotSpot.Type == HotSpotType.Drop)
						{
							ShowNodeContextMenu(e.Location);

							break;
						}
						else if (hotSpot.Type == HotSpotType.Delete)
						{
							RemoveSelectedNodes();

							invalidate = true;

							break;
						}
						else if (hotSpot.Type == HotSpotType.ChangeClassType)
						{
							if (hitObject is FunctionNode functionNode)
							{
								var noneClass = new ClassNode(false)
								{
									Name = "None"
								};

								using (var csf = new ClassSelectionForm(noneClass.Yield().Concat(project.Classes.OrderBy(c => c.Name))))
								{
									if (csf.ShowDialog() == DialogResult.OK)
									{
										var selectedClassNode = csf.SelectedClass;
										if (selectedClassNode != null)
										{
											if (selectedClassNode == noneClass)
											{
												selectedClassNode = null;
											}

											functionNode.BelongsToClass = selectedClassNode;
										}
									}
								}
							}
							else if (hitObject is BaseWrapperNode refNode)
							{
								using (var csf = new ClassSelectionForm(project.Classes.OrderBy(c => c.Name)))
								{
									if (csf.ShowDialog() == DialogResult.OK)
									{
										var selectedClassNode = csf.SelectedClass;
										if (refNode.CanChangeInnerNodeTo(selectedClassNode))
										{
											if (!refNode.GetRootWrapperNode().ShouldPerformCycleCheckForInnerNode() || IsCycleFree(ClassNode, selectedClassNode))
											{
												refNode.ChangeInnerNode(selectedClassNode);
											}
										}
									}
								}
							}

							break;
						}
						else if (hotSpot.Type == HotSpotType.ChangeWrappedType)
						{
							if (hitObject is BaseWrapperNode wrapperNode)
							{
								var items = NodeTypesBuilder.CreateToolStripMenuItems(t =>
								{
									var node = BaseNode.CreateInstanceFromType(t);
									if (wrapperNode.CanChangeInnerNodeTo(node))
									{
										wrapperNode.ChangeInnerNode(node);
									}
								}, wrapperNode.CanChangeInnerNodeTo(null));

								var menu = new ContextMenuStrip();
								menu.Items.AddRange(items.ToArray());
								menu.Show(this, e.Location);
							}
						}
					}
					catch (Exception ex)
					{
						Program.Logger.Log(ex);
					}
				}
			}

			editBox.Visible = false;

			if (invalidate)
			{
				Invalidate();
			}

			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Contract.Requires(e != null);

			editBox.Visible = false;

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
						editBox.BackColor = Program.Settings.SelectedColor;
						editBox.HotSpot = hotSpot;
						editBox.Visible = true;

						editBox.ReadOnly = hotSpot.Id == HotSpot.ReadOnlyId;

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
				nodeInfoToolTip.Show($"{selectedNodes.Count} Nodes selected, {memorySize} bytes", this, toolTipPosition.OffsetEx(16, 16));
			}
			else
			{
				foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
				{
					if (spot.Rect.Contains(toolTipPosition))
					{
						if (spot.Node.UseMemoryPreviewToolTip(spot, spot.Memory, out var previewAddress))
						{
							memoryPreviewPopUp.InitializeMemory(spot.Memory.Process, previewAddress);

							memoryPreviewPopUp.Show(this, toolTipPosition.OffsetEx(16, 16));
						}
						else
						{
							var text = spot.Node.GetToolTipText(spot, spot.Memory);
							if (!string.IsNullOrEmpty(text))
							{
								nodeInfoToolTip.Show(text, this, toolTipPosition.OffsetEx(16, 16));
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
			if (memoryPreviewPopUp.Visible)
			{
				memoryPreviewPopUp.HandleMouseWheelEvent(e);
			}
			else
			{
				base.OnMouseWheel(e);
			}
		}

		protected override void OnScroll(ScrollEventArgs e)
		{
			Contract.Requires(e != null);

			base.OnScroll(e);

			editBox.Visible = false;
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (editBox.Visible == false) // Only process keys if the edit field is not visible.
			{
				var key = keyData & Keys.KeyCode;
				var modifier = keyData & Keys.Modifiers;

				if (selectedNodes.Count > 0)
				{
					if (key == Keys.Delete)
					{
						RemoveSelectedNodes();

						return true;
					}
					if (key == Keys.Menu)
					{
						ShowNodeContextMenu(new Point(10, 10));

						return true;
					}
					if (modifier == Keys.Control && (key == Keys.C || key == Keys.V))
					{
						if (key == Keys.C)
						{
							CopySelectedNodesToClipboard();
						}
						else if (key == Keys.V)
						{
							PasteNodeFromClipboardToSelection();
						}

						return true;
					}
					if (key == Keys.Down || key == Keys.Up)
					{
						HotSpot toSelect;
						bool isAtEnd;

						var query = hotSpots
							.Where(h => h.Type == HotSpotType.Select)
							.Where(h => h.Node.GetParentContainer() == selectionCaret.Node.GetParentContainer());

						if (key == Keys.Down)
						{
							var temp = query
								.SkipUntil(h => h.Node == selectionCaret.Node)
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

							var first = Utils.Min(selectionAnchor, selectionCaret, h => h.Node.Offset.ToInt32());
							var last = first == selectionAnchor ? selectionCaret : selectionAnchor;

							ClearSelection();

							var containerNode = toSelect.Node.GetParentContainer();
							foreach (var spot in containerNode.Nodes
								.SkipWhile(n => n != first.Node)
								.TakeUntil(n => n == last.Node)
								.Select(n => new HotSpot
								{
									Address = containerNode.Offset.Add(n.Offset),
									Node = n,
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
								DoScroll(ScrollOrientation.VerticalScroll, key == Keys.Down ? 1 : - 1);
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

		private void editBox_Committed(object sender, EventArgs e)
		{
			var hotspotTextBox = sender as HotSpotTextBox;

			var hotSpot = hotspotTextBox?.HotSpot;
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
		}

		private void selectedNodeContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			changeTypeToolStripMenuItem.DropDownItems.Clear();
			changeTypeToolStripMenuItem.DropDownItems.AddRange(NodeTypesBuilder.CreateToolStripMenuItems(ReplaceSelectedNodesWithType, false).ToArray());

			var count = selectedNodes.Count;
			var node = selectedNodes.Select(s => s.Node).FirstOrDefault();
			var parentNode = node?.GetParentContainer();

			var nodeIsClass = node is ClassNode;
			var nodeIsSearchableValueNode = false;
			switch (node)
			{
				case BaseHexNode _:
				case FloatNode _:
				case DoubleNode _:
				case Int8Node _:
				case UInt8Node _:
				case Int16Node _:
				case UInt16Node _:
				case Int32Node _:
				case UInt32Node _:
				case Int64Node _:
				case UInt64Node _:
				case Utf8TextNode _:
				case Utf16TextNode _:
				case Utf32TextNode _:
					nodeIsSearchableValueNode = true;
					break;
			}

			addBytesToolStripMenuItem.Enabled = parentNode != null || nodeIsClass;
			insertBytesToolStripMenuItem.Enabled = count == 1 && parentNode != null;

			changeTypeToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;

			createClassFromNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;
			dissectNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;
			searchForEqualValuesToolStripMenuItem.Enabled = count == 1 && nodeIsSearchableValueNode;

			pasteNodesToolStripMenuItem.Enabled = count == 1 && ReClassClipboard.ContainsNodes;
			removeToolStripMenuItem.Enabled = !nodeIsClass;

			copyAddressToolStripMenuItem.Enabled = !nodeIsClass;

			showCodeOfClassToolStripMenuItem.Enabled = nodeIsClass;
			shrinkClassToolStripMenuItem.Enabled = nodeIsClass;

			hideNodesToolStripMenuItem.Enabled = selectedNodes.All(h => !(h.Node is ClassNode));

			unhideChildNodesToolStripMenuItem.Enabled = count == 1 && node is BaseContainerNode bcn && bcn.Nodes.Any(n => n.IsHidden);
			unhideNodesAboveToolStripMenuItem.Enabled = count == 1 && parentNode != null && parentNode.TryGetPredecessor(node, out var predecessor) && predecessor.IsHidden;
			unhideNodesBelowToolStripMenuItem.Enabled = count == 1 && parentNode != null && parentNode.TryGetSuccessor(node, out var successor) && successor.IsHidden;
		}

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!(sender is IntegerToolStripMenuItem item))
			{
				return;
			}

			AddBytes(item.Value);
		}

		private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!(sender is IntegerToolStripMenuItem item))
			{
				return;
			}

			InsertBytes(item.Value);
		}

		private void createClassFromNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedNodes.Count > 0 && !(selectedNodes[0].Node is ClassNode))
			{
				if (selectedNodes[0].Node.GetParentContainer() is ClassNode parentNode)
				{
					var newClassNode = ClassNode.Create();
					selectedNodes.Select(h => h.Node).ForEach(newClassNode.AddNode);

					var classInstanceNode = new ClassInstanceNode();
					classInstanceNode.ChangeInnerNode(newClassNode);

					parentNode.InsertNode(selectedNodes[0].Node, classInstanceNode);

					selectedNodes.Select(h => h.Node).ForEach(c => parentNode.RemoveNode(c));

					ClearSelection();
				}
			}
		}

		private void dissectNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var hexNodes = selectedNodes.Where(h => h.Node is BaseHexNode).ToList();
			if (!hexNodes.Any())
			{
				return;
			}

			foreach (var g in hexNodes.GroupBy(n => n.Node.GetParentContainer()))
			{
				NodeDissector.DissectNodes(g.Select(h => (BaseHexNode)h.Node), g.First().Memory);
			}

			ClearSelection();
		}

		private void searchForEqualValuesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var selectedNode = selectedNodes.FirstOrDefault();
			if (selectedNode == null)
			{
				return;
			}

			IScanComparer comparer;
			switch (selectedNode.Node)
			{
				case BaseHexNode node:
					comparer = new ArrayOfBytesMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory));
					break;
				case FloatNode node:
					comparer = new FloatMemoryComparer(ScanCompareType.Equal, ScanRoundMode.Normal, 2, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case DoubleNode node:
					comparer = new DoubleMemoryComparer(ScanCompareType.Equal, ScanRoundMode.Normal, 2, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int8Node node:
					comparer = new ByteMemoryComparer(ScanCompareType.Equal, (byte)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt8Node node:
					comparer = new ByteMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int16Node node:
					comparer = new ShortMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt16Node node:
					comparer = new ShortMemoryComparer(ScanCompareType.Equal, (short)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int32Node node:
					comparer = new IntegerMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt32Node node:
					comparer = new IntegerMemoryComparer(ScanCompareType.Equal, (int)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Int64Node node:
					comparer = new LongMemoryComparer(ScanCompareType.Equal, node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case UInt64Node node:
					comparer = new LongMemoryComparer(ScanCompareType.Equal, (long)node.ReadValueFromMemory(selectedNode.Memory), 0);
					break;
				case Utf8TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.UTF8, true);
					break;
				case Utf16TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.Unicode, true);
					break;
				case Utf32TextNode node:
					comparer = new StringMemoryComparer(node.ReadValueFromMemory(selectedNode.Memory), Encoding.UTF32, true);
					break;
				default:
					return;
			}

			LinkedWindowFeatures.StartMemoryScan(comparer);
		}

		private void findOutWhatAccessesThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedNode(false);
		}

		private void findOutWhatWritesToThisAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindWhatInteractsWithSelectedNode(true);
		}

		private void copyNodeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopySelectedNodesToClipboard();
		}

		private void pasteNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PasteNodeFromClipboardToSelection();
		}

		private void hideNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			HideSelectedNodes();
		}

		private void unhideChildNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideChildNodes();
		}

		private void unhideNodesAboveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideNodesAbove();
		}

		private void unhideNodesBelowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UnhideNodesBelow();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedNodes();
		}

		private void copyAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedNodes.Count > 0)
			{
				Clipboard.SetText(selectedNodes.First().Address.ToString("X"));
			}
		}

		private void showCodeOfClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedNodes.FirstOrDefault()?.Node is ClassNode node)
			{
				LinkedWindowFeatures.ShowCodeGeneratorForm(node.Yield());
			}
		}

		private void shrinkClassToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var node = selectedNodes.Select(s => s.Node).FirstOrDefault();
			if (!(node is ClassNode classNode))
			{
				return;
			}

			foreach (var nodeToDelete in classNode.Nodes.Reverse().TakeWhile(n => n is BaseHexNode))
			{
				classNode.RemoveNode(nodeToDelete);
			}
		}

		#endregion

		/// <summary>
		/// Gets informations about all selected nodes.
		/// </summary>
		/// <returns>A list with informations about all selected nodes.</returns>
		public IReadOnlyList<SelectedNodeInfo> GetSelectedNodes()
		{
			return selectedNodes
				.Select(h => new SelectedNodeInfo(h.Node, h.Memory, h.Address))
				.ToList();
		}

		private void ShowNodeContextMenu(Point location)
		{
			ContextMenuStrip?.Show(this, location);
		}

		public void AddBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selectedNodes.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node as BaseContainerNode ?? hotspot.Node.GetParentContainer())?.AddBytes(length);
			}

			Invalidate();
		}

		public void InsertBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selectedNodes.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node as BaseContainerNode ?? hotspot.Node.GetParentContainer())?.InsertBytes(hotspot.Node, length);

				Invalidate();
			}
		}

		public void ReplaceSelectedNodesWithType(Type type)
		{
			Contract.Requires(type != null);
			Contract.Requires(type.IsSubclassOf(typeof(BaseNode)));

			var newSelected = new List<HotSpot>(selectedNodes.Count);

			var hotSpotPartitions = selectedNodes
				.WhereNot(s => s.Node is ClassNode)
				.GroupBy(s => s.Node.GetParentContainer())
				.SelectMany(g => g
					.OrderBy(s => s.Node.Offset, IntPtrComparer.Instance)
					.GroupWhile((h1, h2) => h1.Node.Offset + h1.Node.MemorySize == h2.Node.Offset)
				);

			foreach (var selectedPartition in hotSpotPartitions)
			{
				var hotSpotsToReplace = new Queue<HotSpot>(selectedPartition);
				while (hotSpotsToReplace.Count > 0)
				{
					var selected = hotSpotsToReplace.Dequeue();

					var node = BaseNode.CreateInstanceFromType(type);

					var createdNodes = new List<BaseNode>();
					selected.Node.GetParentContainer().ReplaceChildNode(selected.Node, node, ref createdNodes);

					node.IsSelected = true;

					var hotspot = new HotSpot
					{
						Memory = selected.Memory,
						Address = selected.Address,
						Node = node,
						Level = selected.Level
					};

					newSelected.Add(hotspot);

					if (selectionAnchor.Node == selected.Node)
					{
						selectionAnchor = hotspot;
					}
					if (selectionCaret.Node == selected.Node)
					{
						selectionCaret = hotspot;
					}

					// If more than one node is selected I assume the user wants to replace the complete range with the desired node type.
					if (selectedNodes.Count > 1)
					{
						foreach (var createdNode in createdNodes)
						{
							hotSpotsToReplace.Enqueue(new HotSpot
							{
								Memory = selected.Memory,
								Address = selected.Address.Add(createdNode.Offset.Sub(node.Offset)),
								Node = createdNode,
								Level = selected.Level
							});
						}
					}
				}
			}

			if (newSelected.Count > 0)
			{
				selectedNodes.Clear();

				selectedNodes.AddRange(newSelected);

				OnSelectionChanged();
			}

			Invalidate();
		}

		private void ClearSelection()
		{
			selectedNodes.ForEach(h => h.Node.ClearSelection());

			selectedNodes.Clear();
		}

		private void RemoveSelectedNodes()
		{
			selectedNodes
				.WhereNot(h => h.Node is ClassNode)
				.ForEach(h => h.Node.GetParentContainer().RemoveNode(h.Node));

			selectedNodes.Clear();

			OnSelectionChanged();

			Invalidate();
		}

		private void HideSelectedNodes()
		{
			foreach (var hotSpot in selectedNodes)
			{
				hotSpot.Node.IsHidden = true;
			}

			selectedNodes.Clear();

			OnSelectionChanged();

			Invalidate();
		}

		private void UnhideChildNodes()
		{
			if (selectedNodes.Count != 1)
			{
				return;
			}

			if (!(selectedNodes[0].Node is BaseContainerNode containerNode))
			{
				return;
			}

			foreach (var bn in containerNode.Nodes)
			{
				bn.IsHidden = false;
				bn.IsSelected = false;
			}

			containerNode.IsSelected = false;

			selectedNodes.Clear();

			OnSelectionChanged();

			Invalidate();
		}

		private void UnhideNodesBelow()
		{
			if (selectedNodes.Count != 1)
			{
				return;
			}

			var selectedNode = selectedNodes[0].Node;

			var parentNode = selectedNode.GetParentContainer();
			if (parentNode == null)
			{
				return;
			}

			var hiddenNodeStartIndex = parentNode.FindNodeIndex(selectedNode) + 1;
			if (hiddenNodeStartIndex >= parentNode.Nodes.Count)
			{
				return;
			}

			for (var i = hiddenNodeStartIndex; i < parentNode.Nodes.Count; i++)
			{
				var indexNode = parentNode.Nodes[i];
				if (indexNode.IsHidden)
				{
					indexNode.IsHidden = false;
					indexNode.IsSelected = false;
				}
				else
				{
					break;
				}
			}

			selectedNode.IsSelected = false;

			selectedNodes.Clear();

			OnSelectionChanged();

			Invalidate();
		}

		private void UnhideNodesAbove()
		{
			if (selectedNodes.Count != 1)
			{
				return;
			}

			var selectedNode = selectedNodes[0].Node;

			var parentNode = selectedNode.GetParentContainer();
			if (parentNode == null)
			{
				return;
			}

			var hiddenNodeStartIndex = parentNode.FindNodeIndex(selectedNode) - 1;
			if (hiddenNodeStartIndex < 0)
			{
				return;
			}

			for (var i = hiddenNodeStartIndex; i > -1; i--)
			{
				var indexNode = parentNode.Nodes[i];
				if (indexNode.IsHidden)
				{
					indexNode.IsHidden = false;
					indexNode.IsSelected = false;
				}
				else
				{
					break;
				}
			}

			selectedNode.IsSelected = false;

			selectedNodes.Clear();

			OnSelectionChanged();

			Invalidate();
		}

		private void CopySelectedNodesToClipboard()
		{
			if (selectedNodes.Count > 0)
			{
				ReClassClipboard.Copy(selectedNodes.Select(h => h.Node), Program.Logger);
			}
		}

		private void PasteNodeFromClipboardToSelection()
		{
			var result = ReClassClipboard.Paste(project, Program.Logger);
			foreach (var pastedClassNode in result.Item1)
			{
				if (!project.ContainsClass(pastedClassNode.Uuid))
				{
					project.AddClass(pastedClassNode);
				}
			}

			if (selectedNodes.Count == 1)
			{
				var selectedNode = selectedNodes[0].Node;
				var containerNode = selectedNode.GetParentContainer();
				var classNode = selectedNode.GetParentClass();
				if (containerNode != null && classNode != null)
				{

					foreach (var node in result.Item2)
					{
						if (node is BaseWrapperNode)
						{
							var rootWrapper = node.GetRootWrapperNode();
							Debug.Assert(rootWrapper == node);

							if (rootWrapper.ShouldPerformCycleCheckForInnerNode())
							{
								if (rootWrapper.ResolveMostInnerNode() is ClassNode innerNode)
								{
									if (!IsCycleFree(classNode, innerNode))
									{
										continue;
									}
								}
							}
						}

						containerNode.InsertNode(selectedNode, node);
					}
				}
			}
		}

		private bool IsCycleFree(ClassNode parent, ClassNode node)
		{
			if (ClassUtil.IsCyclicIfClassIsAccessibleFromParent(parent, node, project.Classes))
			{
				MessageBox.Show("Invalid operation because this would create a class cycle.", "Cycle Detected", MessageBoxButtons.OK, MessageBoxIcon.Error);

				return false;
			}

			return true;
		}

		private void FindWhatInteractsWithSelectedNode(bool writeOnly)
		{
			var selectedNode = selectedNodes.FirstOrDefault();
			if (selectedNode == null)
			{
				return;
			}

			LinkedWindowFeatures.FindWhatInteractsWithAddress(selectedNode.Address, selectedNode.Node.MemorySize, writeOnly);
		}
	}
}
