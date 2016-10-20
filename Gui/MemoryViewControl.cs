using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Gui;
using ReClassNET.Nodes;

namespace ReClassNET
{
	partial class MemoryViewControl : ScrollableCustomControl
	{
		[Browsable(false)]
		public ClassNode ClassNode { get; set; }

		[Browsable(false)]
		public Memory Memory { get; set; }

		[Browsable(false)]
		public Settings Settings { get; set; }

		private List<HotSpot> hotSpots;
		private List<HotSpot> selected;

		private FontEx font;

		public MemoryViewControl()
		{
			InitializeComponent();

			DoubleBuffered = true;

			hotSpots = new List<HotSpot>();
			selected = new List<HotSpot>();

			font = new FontEx
			{
				Font = new Font("Courier New", DpiUtil.ScaleIntX(13), GraphicsUnit.Pixel),
				CharSize = new Size(DpiUtil.ScaleIntX(8), DpiUtil.ScaleIntY(16))
			};

			editBox.Font = font;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			VerticalScroll.Enabled = true;
			VerticalScroll.Visible = true;
			HorizontalScroll.Enabled = false;
			HorizontalScroll.Visible = false;
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

			e.Graphics.FillRectangle(new SolidBrush(Settings.BackgroundColor), ClientRectangle);

			if (ClassNode == null)
			{
				return;
			}

			Memory.Size = ClassNode.MemorySize;
			Memory.Update(ClassNode.Offset);

			var view = new ViewInfo
			{
				Context = e.Graphics,
				Font = font,
				Address = ClassNode.Offset,
				ClientArea = ClientRectangle,
				Level = 0,
				Memory = Memory,
				MultiSelected = selected.Count > 1,
				HotSpots = hotSpots
			};

			var scrollY = VerticalScroll.Value * font.Height;
			int maxY = 0;
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

		protected override void OnMouseClick(MouseEventArgs e)
		{
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
						}
						else if (hotSpot.Type == HotSpotType.Click)
						{
							hitObject.Update(hotSpot);
						}
						else if (hotSpot.Type == HotSpotType.Select)
						{
							if (e.Button == MouseButtons.Left)
							{
								if (ModifierKeys == Keys.None)
								{
									ClearSelection();

									hitObject.IsSelected = true;

									selected.Add(hotSpot);
								}
								else if (ModifierKeys == Keys.Control)
								{
									hitObject.IsSelected = !hitObject.IsSelected;

									if (hitObject.IsSelected)
									{
										selected.Add(hotSpot);
									}
									else
									{
										selected.Remove(selected.Where(c => c.Node == hitObject).FirstOrDefault());
									}
								}
								else if (ModifierKeys == Keys.Shift)
								{
									if (selected.Count > 0)
									{
										var selectedNode = selected[0].Node;
										if (selectedNode.ParentNode != hitObject.ParentNode)
										{
											continue;
										}

										var containerNode = selectedNode.ParentNode as BaseContainerNode;
										if (containerNode == null)
										{
											continue;
										}

										var idx1 = FindNodeIndex(selectedNode);
										if (idx1 == -1)
										{
											continue;
										}
										var idx2 = FindNodeIndex(hitObject);
										if (idx2 == -1)
										{
											continue;
										}
										if (idx2 < idx1)
										{
											var temp = idx1;
											idx1 = idx2;
											idx2 = temp;
										}

										ClearSelection();

										foreach (var spot in containerNode.Nodes.Skip(idx1).Take(idx2 - idx1 + 1)
											.Select(n => new HotSpot { Address = containerNode.Offset.Add(n.Offset), Node = n }))
										{
											spot.Node.IsSelected = true;
											selected.Add(spot);
										}
									}
								}
							}
							else if (e.Button == MouseButtons.Right)
							{
								/*ClearSelection();

								hitObject.IsSelected = true;

								selected.Add(hotSpot);*/

								selectedNodeContextMenuStrip.Show(this, e.Location);
							}
						}
						else if (hotSpot.Type == HotSpotType.Drop)
						{
							selectedNodeContextMenuStrip.Show(this, e.Location);
						}
						else if (hotSpot.Type == HotSpotType.Delete)
						{
							RemoveSelectedNodes();
						}
						else if (hotSpot.Type == HotSpotType.ChangeAll || hotSpot.Type == HotSpotType.ChangeSkipParent)
						{
							var refNode = hitObject as BaseReferenceNode;
							if (refNode != null)
							{
								EventHandler changeInnerType = (sender2, e2) =>
								{
									var item = sender2 as TypeToolStripMenuItem;
									if (item == null)
									{
										return;
									}
									var classNode = item.Tag as ClassNode;
									if (classNode == null)
									{
										return;
									}

									refNode.InnerNode = classNode;
								};

								var menu = new ContextMenuStrip();
								menu.Items.AddRange(
									ClassNode.Classes
									.Where(c => hotSpot.Type == HotSpotType.ChangeSkipParent ? hitObject.ParentNode != c : true)
									.OrderBy(c => c.Name)
									.Select(c =>
									{
										var b = new TypeToolStripMenuItem
										{
											Text = c.Name,
											Tag = c
										};
										b.Click += changeInnerType;
										return b;
									})
									.ToArray()
								);
								menu.Show(this, e.Location);
							}
						}
					}
					catch (Exception ex)
					{
						ex.ShowDialog();
					}

					Invalidate();
				}
			}

			base.OnMouseClick(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			base.OnMouseDoubleClick(e);

			editBox.Visible = false;

			foreach (var hotSpot in hotSpots.Where(h => h.Type == HotSpotType.Edit))
			{
				if (hotSpot.Rect.Contains(e.Location))
				{
					editBox.BackColor = Settings.SelectedColor;
					editBox.HotSpot = hotSpot;
					editBox.Visible = true;

					break;
				}
			}
		}

		private Point toolTipPosition;
		protected override void OnMouseHover(EventArgs e)
		{
			base.OnMouseHover(e);

			if (selected.Count > 1)
			{
				var memorySize = selected.Select(h => h.Node.MemorySize).Sum();
				toolTip.Show($"{selected.Count} Nodes selected, {memorySize} bytes", this, toolTipPosition.OffsetEx(16, 16));
			}
			else
			{
				foreach (var spot in hotSpots.Where(h => h.Type == HotSpotType.Select))
				{
					if (spot.Rect.Contains(toolTipPosition))
					{
						var text = spot.Node.GetToolTipText(spot, Memory, Settings);
						if (!string.IsNullOrEmpty(text))
						{
							toolTip.Show(text, this, toolTipPosition.OffsetEx(16, 16));
						}

						return;
					}
				}
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.Location != toolTipPosition)
			{
				toolTipPosition = e.Location;

				toolTip.Hide(this);

				ResetMouseEventArgs();
			}
		}

		private void repaintTimer_Tick(object sender, EventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			Invalidate(false);
		}

		private void updateClassTimer_Tick(object sender, EventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			if (ClassNode != null)
			{
				ClassNode.UpdateAddress(Memory);
			}
		}

		private int FindNodeIndex(BaseNode node)
		{
			var containerNode = node.ParentNode as BaseContainerNode;
			if (containerNode == null)
			{
				return -1;
			}

			return containerNode.Nodes.FindIndex(n => n == node);
		}

		public void AddBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selected.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node.ParentNode ?? hotspot.Node as ClassNode).AddBytes(length);
			}

			Invalidate();
		}

		public void InsertBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selected.FirstOrDefault();
			if (hotspot != null)
			{
				(hotspot.Node.ParentNode ?? hotspot.Node as ClassNode).InsertBytes(FindNodeIndex(hotspot.Node), length);

				Invalidate();
			}
		}

		public void ReplaceSelectedNodesWithType(Type type)
		{
			Contract.Requires(type != null);
			Contract.Requires(type.IsSubclassOf(typeof(BaseNode)));

			var newSelected = new List<BaseNode>(selected.Count);

			foreach (var sel in selected)
			{
				if (!(sel.Node is ClassNode))
				{
					var node = Activator.CreateInstance(type) as BaseNode;

					node.Intialize();

					if (sel.Node.ParentNode.ReplaceChildNode(FindNodeIndex(sel.Node), node))
					{
						newSelected.Add(node);
					}
				}
			}

			if (newSelected.Count > 0)
			{
				selected.Clear();

				foreach (var sel in newSelected)
				{
					sel.IsSelected = true;

					selected.Add(new HotSpot
					{
						Address = sel.ParentNode.Offset.Add(sel.Offset),
						Node = sel
					});
				}
			}

			Invalidate();
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

		private void ClearSelection()
		{
			selected.ForEach(h => h.Node.ClearSelection());

			selected.Clear();
		}

		private void RemoveSelectedNodes()
		{
			selected.Where(h => !(h.Node is ClassNode)).ForEach(h => h.Node.ParentNode.RemoveNode(h.Node));

			selected.Clear();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedNodes();
		}
	}
}
