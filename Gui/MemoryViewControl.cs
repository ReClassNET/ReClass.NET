using ReClassNET.Gui;
using ReClassNET.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReClassNET
{
	partial class MemoryViewControl : ScrollableCustomControl
	{
		public ClassNode ClassNode { get; set; }

		public Memory Memory { get; set; }

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
				Font = new Font("Courier New", 13, GraphicsUnit.Pixel),
				CharSize = new Size(8, 16)
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
				Settings = Settings,
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

									var classNode = selectedNode.ParentNode as ClassNode;
									if (ClassNode == null)
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

									foreach (var spot in classNode.Nodes.Skip(idx1).Take(idx2 - idx1 + 1)
										.Select(n => new HotSpot { Address = classNode.Offset.Add(n.Offset), Node = n }))
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
					else if (hotSpot.Type == HotSpotType.ChangeA || hotSpot.Type == HotSpotType.ChangeX)
					{
						//exchange
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

		private void updateTimer_Tick(object sender, EventArgs e)
		{
			if (DesignMode)
			{
				return;
			}

			Invalidate(false);
		}

		private int FindNodeIndex(BaseNode node)
		{
			var classNode = node.ParentNode as ClassNode;
			if (classNode == null)
			{
				return -1;
			}

			return classNode.Nodes.FindIndex(n => n == node);
		}

		void ReplaceNode(int idx, BaseNode pNewNode)
		{

		}
		void RemoveNodes(int idx, int length)
		{

		}
		void FillNodes(int idx, int Length)
		{

		}
		void ResizeNode(int idx, int before, int After)
		{

		}

		public void AddBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selected.FirstOrDefault();
			if (hotspot != null)
			{
				hotspot.Node.ParentNode.AddBytes(length);
			}

			Invalidate();
		}

		public void InsertBytes(int length)
		{
			Contract.Requires(length >= 0);

			var hotspot = selected.FirstOrDefault();
			if (hotspot != null)
			{
				hotspot.Node.ParentNode.InsertBytes(FindNodeIndex(hotspot.Node), length);

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
				//if (sel.Node.IsValid())

				var node = Activator.CreateInstance(type) as BaseNode;

				node.Intialize();

				sel.Node.ParentNode.ReplaceChildNode(FindNodeIndex(sel.Node), node);

				newSelected.Add(node);
			}

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
			selected.ForEach(h => h.Node.ParentNode.RemoveNode(h.Node));

			selected.Clear();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedNodes();
		}
	}
}
