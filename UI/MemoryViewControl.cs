using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.DataExchange;
using ReClassNET.Debugger;
using ReClassNET.Memory;
using ReClassNET.Nodes;
using ReClassNET.Util;

namespace ReClassNET.UI
{
	partial class MemoryViewControl : ScrollableCustomControl
	{
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
			get { return project; }
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
			get { return classNode; }
			set
			{
				ClearSelection();

				OnSelectionChanged();

				classNode = value;

				VerticalScroll.Value = 0;
				if (classNode != null && Memory != null)
				{
					classNode.UpdateAddress(Memory);
				}
				Invalidate();
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MemoryBuffer Memory { get; set; }

		public IEnumerable<BaseNode> SelectedNodes => selectedNodes.Select(s => s.Node);

		public event EventHandler SelectionChanged;

		/// <summary>The context menu of a node.</summary>
		public ContextMenuStrip NodeContextMenu => selectedNodeContextMenuStrip;

		public MemoryViewControl()
		{
			InitializeComponent();

			font = new FontEx
			{
				Font = new Font("Courier New", DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
				Width = DpiUtil.ScaleIntX(8),
				Height = DpiUtil.ScaleIntY(16)
			};

			editBox.Font = font;
			memoryPreviewToolTip.Font = font;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			VerticalScroll.Enabled = true;
			VerticalScroll.Visible = true;
			VerticalScroll.SmallChange = 10;
			HorizontalScroll.Enabled = false;
			HorizontalScroll.Visible = false;
		}

		internal void RegisterNodeType(Type type, string name, Image icon)
		{
			Contract.Requires(type != null);
			Contract.Requires(name != null);
			Contract.Requires(icon != null);

			var item = new TypeToolStripMenuItem
			{
				Image = icon,
				Text = name,
				Value = type
			};
			item.Click += memoryTypeToolStripMenuItem_Click;

			changeTypeToolStripMenuItem.DropDownItems.Add(item);
		}

		internal void DeregisterNodeType(Type type)
		{
			Contract.Requires(type != null);

			var item = changeTypeToolStripMenuItem.DropDownItems.OfType<TypeToolStripMenuItem>().FirstOrDefault(i => i.Value == type);
			if (item != null)
			{
				item.Click -= memoryTypeToolStripMenuItem_Click;
				changeTypeToolStripMenuItem.DropDownItems.Remove(item);
			}
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

			ClassNode.UpdateAddress(Memory);

			Memory.Size = ClassNode.MemorySize;
			Memory.Update(ClassNode.Offset);

			var view = new ViewInfo
			{
				Settings = Program.Settings,
				Context = e.Graphics,
				Font = font,
				Address = ClassNode.Offset,
				ClientArea = ClientRectangle,
				Level = 0,
				Memory = Memory,
				MultiSelected = selectedNodes.Count > 1,
				HotSpots = hotSpots
			};

			var scrollY = VerticalScroll.Value * font.Height;
			int maxY;
			try
			{
				BaseHexNode.CurrentHighlightTime = DateTime.Now;

				maxY = ClassNode.Draw(view, 0, -scrollY) + scrollY;
			}
			catch
			{
				return;
			}

			/*foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
			{
				e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.FromArgb(150, 255, 0, 0)), 1), spot.Rect);
			}*/

			if (maxY > ClientSize.Height)
			{
				VerticalScroll.Enabled = true;

				VerticalScroll.LargeChange = ClientSize.Height / font.Height;
				VerticalScroll.Maximum = (maxY - ClientSize.Height) / font.Height + VerticalScroll.LargeChange;
			}
			else
			{
				VerticalScroll.Enabled = false;

				VerticalScroll.Value = 0;
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

			bool invalidate = false;

			editBox.Visible = false;

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
						else if (hotSpot.Type == HotSpotType.Click)
						{
							hitObject.Update(hotSpot);

							invalidate = true;

							break;
						}
						else if (hotSpot.Type == HotSpotType.Select)
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
										if (hitObject.ParentNode != null && selectedNode.ParentNode != hitObject.ParentNode)
										{
											continue;
										}

										var first = Utils.Min(selectedNodes[0], hotSpot, h => h.Node.Offset.ToInt32());
										var last = first == hotSpot ? selectedNodes[0] : hotSpot;

										ClearSelection();

										var containerNode = selectedNode.ParentNode;
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

								selectedNodeContextMenuStrip.Show(this, e.Location);
							}

							invalidate = true;
						}
						else if (hotSpot.Type == HotSpotType.Drop)
						{
							selectedNodeContextMenuStrip.Show(this, e.Location);

							break;
						}
						else if (hotSpot.Type == HotSpotType.Delete)
						{
							RemoveSelectedNodes();

							invalidate = true;

							break;
						}
						else if (hotSpot.Type == HotSpotType.ChangeType)
						{
							IEnumerable<TypeToolStripMenuItem> items = null;

							var functionNode = hitObject as FunctionNode;
							if (functionNode != null)
							{
								var noneClass = new ClassNode(false)
								{
									Name = "None"
								};

								EventHandler handler = (sender2, e2) =>
								{
									var selectedClassNode = (sender2 as TypeToolStripMenuItem)?.Tag as ClassNode;
									if (selectedClassNode == null)
									{
										return;
									}

									if (selectedClassNode == noneClass)
									{
										selectedClassNode = null;
									}

									functionNode.BelongsToClass = selectedClassNode;
								};

								items = noneClass.Yield()
									.Concat(project.Classes.OrderBy(c => c.Name))
									.Select(c =>
									{
										var b = new TypeToolStripMenuItem
										{
											Text = c.Name,
											Tag = c
										};
										b.Click += handler;
										return b;
									});
							}

							var refNode = hitObject as BaseReferenceNode;
							if (refNode != null)
							{
								EventHandler handler = (sender2, e2) =>
								{
									var selectedClassNode = (sender2 as TypeToolStripMenuItem)?.Tag as ClassNode;
									if (selectedClassNode == null)
									{
										return;
									}

									if (IsCycleFree(refNode.ParentNode as ClassNode, selectedClassNode))
									{
										refNode.ChangeInnerNode(selectedClassNode);
									}
								};

								items = project.Classes
									.OrderBy(c => c.Name)
									.Select(c =>
									{
										var b = new TypeToolStripMenuItem
										{
											Text = c.Name,
											Tag = c
										};
										b.Click += handler;
										return b;
									});
							}

							if (items != null)
							{
								var menu = new ContextMenuStrip();
								menu.Items.AddRange(items.ToArray());
								menu.Show(this, e.Location);
							}

							break;
						}
					}
					catch (Exception ex)
					{
						Program.Logger.Log(ex);
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

			base.OnMouseDoubleClick(e);

			editBox.Visible = false;

			foreach (var hotSpot in hotSpots.Where(h => h.Type == HotSpotType.Edit))
			{
				if (hotSpot.Rect.Contains(e.Location))
				{
					editBox.BackColor = Program.Settings.SelectedColor;
					editBox.HotSpot = hotSpot;
					editBox.Visible = true;

					editBox.ReadOnly = hotSpot.Id == HotSpot.ReadOnlyId;

					break;
				}
			}
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
						IntPtr previewAddress;
						if (spot.Node.UseMemoryPreviewToolTip(spot, spot.Memory, out previewAddress))
						{
							memoryPreviewToolTip.UpdateMemory(spot.Memory.Process, previewAddress);

							memoryPreviewToolTip.Show("<>", this, toolTipPosition.OffsetEx(16, 16));
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
				memoryPreviewToolTip.Hide(this);

				ResetMouseEventArgs();
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
						selectedNodeContextMenuStrip.Show(this, 10, 10);

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
							.Where(h => h.Node.ParentNode == selectionCaret.Node.ParentNode);

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

							var containerNode = toSelect.Node.ParentNode;
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
			var count = selectedNodes.Count;
			var node = selectedNodes.Select(s => s.Node).FirstOrDefault();

			var nodeIsClass = node is ClassNode;

			addBytesToolStripMenuItem.Enabled = node?.ParentNode != null || nodeIsClass;
			insertBytesToolStripMenuItem.Enabled = count == 1 && node?.ParentNode != null;

			changeTypeToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;

			createClassFromNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;
			dissectNodesToolStripMenuItem.Enabled = count > 0 && !nodeIsClass;

			pasteNodesToolStripMenuItem.Enabled = count == 1 && ReClassClipboard.ContainsNodes;
			removeToolStripMenuItem.Enabled = !nodeIsClass;

			copyAddressToolStripMenuItem.Enabled = !nodeIsClass;
		}

		private void addBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			AddBytes(item.Value);
		}

		private void insertBytesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as IntegerToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			InsertBytes(item.Value);
		}

		private void memoryTypeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var item = sender as TypeToolStripMenuItem;
			if (item == null)
			{
				return;
			}

			ReplaceSelectedNodesWithType(item.Value);
		}

		private void createClassFromNodesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (selectedNodes.Count > 0 && !(selectedNodes[0].Node is ClassNode))
			{
				var parentNode = selectedNodes[0].Node.ParentNode as ClassNode;
				if (parentNode != null)
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
			if (hexNodes.Any())
			{
				foreach (var g in hexNodes.GroupBy(n => n.Node.ParentNode))
				{
					NodeDissector.DissectNodes(g.Select(h => (BaseHexNode)h.Node), g.First().Memory);
				}

				ClearSelection();
			}
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

		#endregion

		public void AddBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selectedNodes.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node.ParentNode ?? hotspot.Node as ClassNode).AddBytes(length);
			}

			Invalidate();
		}

		public void InsertBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selectedNodes.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node.ParentNode ?? hotspot.Node as ClassNode).InsertBytes(hotspot.Node, length);

				Invalidate();
			}
		}

		/// <summary>Groups the selected nodes in blocks if the nodes are continues.</summary>
		/// <param name="selection">The selection to partition.</param>
		/// <returns>An enumeration with blocks of continues nodes.</returns>
		private static IEnumerable<List<HotSpot>> PartitionSelectedNodes(IEnumerable<HotSpot> selection)
		{
			Contract.Requires(selection != null);

			List<HotSpot> partition;
			using (var it = selection.GetEnumerator())
			{
				if (!it.MoveNext())
				{
					yield break;
				}

				partition = new List<HotSpot> { it.Current };

				var last = it.Current;

				while (it.MoveNext())
				{
					if (it.Current.Address != last.Address + last.Node.MemorySize)
					{
						yield return partition;

						partition = new List<HotSpot>();
					}

					partition.Add(it.Current);

					last = it.Current;
				}
			}

			if (partition.Count != 0)
			{
				yield return partition;
			}
		}

		/// <summary>Recursive replace all splitted nodes.</summary>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="type">The node type.</param>
		/// <param name="nodesToReplace">The nodes to replace.</param>
		/// <returns>The new nodes.</returns>
		private static IEnumerable<BaseNode> RecursiveReplaceNodes(BaseContainerNode parentNode, Type type, IEnumerable<BaseNode> nodesToReplace)
		{
			Contract.Requires(parentNode != null);
			Contract.Requires(type != null);
			Contract.Requires(nodesToReplace != null);
			Contract.Ensures(Contract.Result<IEnumerable<BaseNode>>() != null);

			foreach (var nodeToReplace in nodesToReplace)
			{
				var temp = new List<BaseNode>();
				if (parentNode.ReplaceChildNode(nodeToReplace, type, ref temp))
				{
					var node = temp.First();

					node.IsSelected = true;

					yield return node;

					if (temp.Count > 1)
					{
						foreach (var n in RecursiveReplaceNodes(parentNode, type, temp.Skip(1)))
						{
							yield return n;
						}
					}
				}
			}
		}

		public void ReplaceSelectedNodesWithType(Type type)
		{
			Contract.Requires(type != null);
			Contract.Requires(type.IsSubclassOf(typeof(BaseNode)));

			var newSelected = new List<HotSpot>(selectedNodes.Count);

			// Group the selected nodes in continues selected blocks.
			foreach (var selectedPartition in PartitionSelectedNodes(selectedNodes.WhereNot(s => s.Node is ClassNode)))
			{
				foreach (var selected in selectedPartition)
				{
					var createdNodes = new List<BaseNode>();
					if (selected.Node.ParentNode.ReplaceChildNode(selected.Node, type, ref createdNodes))
					{
						var node = createdNodes.First();

						node.IsSelected = true;

						var hotspot = new HotSpot
						{
							Memory = selected.Memory,
							Address = node.ParentNode.Offset.Add(node.Offset),
							Node = node
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

						// If the block contains more than one node and the replaced node decomposed to more than one node replace the new nodes to.
						if (selectedPartition.Count > 1 && createdNodes.Count > 1)
						{
							newSelected.AddRange(
								RecursiveReplaceNodes(selected.Node.ParentNode, type, createdNodes.Skip(1))
									.Select(n => new HotSpot
									{
										Memory = selected.Memory,
										Address = n.ParentNode.Offset.Add(n.Offset),
										Node = n,
										Level = selected.Level
									})
							);
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
			selectedNodes.WhereNot(h => h.Node is ClassNode).ForEach(h => h.Node.ParentNode.RemoveNode(h.Node));

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
				var selectedNode = selectedNodes.First().Node;
				var parent = selectedNode.ParentNode as ClassNode;
				if (parent != null)
				{
					foreach (var node in result.Item2)
					{
						if (IsCycleFree(parent, node))
						{
							parent.InsertNode(selectedNode, node);
						}
					}
				}
			}
		}

		private bool IsCycleFree(ClassNode parent, BaseNode node)
		{
			var referenceNode = node as BaseReferenceNode;
			if (referenceNode == null)
			{
				return true;
			}

			if (referenceNode.PerformCycleCheck == false)
			{
				return true;
			}

			return IsCycleFree(parent, referenceNode.InnerNode);
		}

		private bool IsCycleFree(ClassNode parent, ClassNode node)
		{
			if (!ClassUtil.IsCycleFree(parent, node, project.Classes))
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

			var debugger = Memory.Process.Debugger;
			if (debugger.AskUserAndStartDebugger())
			{
				if (writeOnly)
				{
					debugger.FindWhatWritesToAddress(selectedNode.Address, selectedNode.Node.MemorySize);
				}
				else
				{
					debugger.FindWhatAccessesAddress(selectedNode.Address, selectedNode.Node.MemorySize);
				}
			}
		}
	}
}
