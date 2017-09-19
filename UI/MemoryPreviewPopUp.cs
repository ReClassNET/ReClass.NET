using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	[ToolboxItem(false)]
	public class MemoryPreviewPopUp : ToolStripDropDown
	{
		private const int ToolTipWidth = 1000 + ToolTipPadding;
		private const int ToolTipPadding = 4;

		/// <summary>Panel for the memory preview.</summary>
		private class MemoryPreviewPanel : Panel
		{
			private const int MinNodeCount = 10;

			public ViewInfo ViewInfo => viewInfo;

			private readonly ViewInfo viewInfo;
			private readonly List<BaseHexNode> nodes;

			public MemoryPreviewPanel(FontEx font)
			{
				Contract.Requires(font != null);

				DoubleBuffered = true;

				nodes = new List<BaseHexNode>();

				viewInfo = new ViewInfo
				{
					Font = font,

					Memory = new MemoryBuffer(),

					HotSpots = new List<HotSpot>(),
					Classes = new List<ClassNode>()
				};

				SetNodeCount(MinNodeCount);

				CalculateSize();
			}

			/// <summary>Sets the absolute number of nodes and resizes the underlaying memory buffer.</summary>
			/// <param name="count">Number of nodes.</param>
			private void SetNodeCount(int count)
			{
				BaseHexNode CreateNode(int index)
				{
#if RECLASSNET64
					return new Hex64Node { Offset = (IntPtr)(index * 8) };
#else
					return new Hex32Node { Offset = (IntPtr)(index * 4) };
#endif
				}

				if (nodes.Count < count)
				{
					nodes.AddRange(Enumerable.Range(nodes.Count, count - nodes.Count).Select(CreateNode));
				}
				else if (nodes.Count > count && count >= MinNodeCount)
				{
					nodes.RemoveRange(count, nodes.Count - count);
				}

				viewInfo.Memory.Size = nodes.Select(n => n.MemorySize).Sum();
			}

			/// <summary>Changes the number of nodes with the provided delta.</summary>
			/// <param name="delta">The change delta.</param>
			public void ChangeNodeCount(int delta)
			{
				SetNodeCount(nodes.Count + delta);

				CalculateSize();
			}

			/// <summary>Resets the settings of the panel to the defaults.</summary>
			public void Reset()
			{
				SetNodeCount(MinNodeCount);

				CalculateSize();
			}

			/// <summary>Calculates the size of the panel.</summary>
			private void CalculateSize()
			{
				var size = new Size(
					ToolTipWidth,
					nodes.Sum(n => n.CalculateDrawnHeight(viewInfo)) + ToolTipPadding
				);

				viewInfo.ClientArea = new Rectangle(ToolTipPadding / 2, ToolTipPadding / 2, size.Width - ToolTipPadding, size.Height - ToolTipPadding);

				Size = MinimumSize = MaximumSize = size;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				viewInfo.HotSpots.Clear();

				// Some settings are not usefull for the preview.
				viewInfo.Settings = Program.Settings.Clone();
				viewInfo.Settings.ShowNodeAddress = false;

				viewInfo.Context = e.Graphics;

				e.Graphics.FillRectangle(Brushes.White, Bounds);
				using (var pen = new Pen(Brushes.Black, 1))
				{
					e.Graphics.DrawRectangle(pen, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width - 1, Bounds.Height - 1));
				}

				int x = -24;
				int y = 2;
				foreach (var node in nodes)
				{
					y += node.Draw(viewInfo, x, y).Height;
				}
			}
		}

		private readonly MemoryPreviewPanel panel;

		private IntPtr memoryAddress;

		protected override CreateParams CreateParams
		{
			get
			{
				const int WS_EX_NOACTIVATE = 0x08000000;

				var cp = base.CreateParams;
				cp.ExStyle |= WS_EX_NOACTIVATE;
				return cp;
			}
		}

		public MemoryPreviewPopUp(FontEx font)
		{
			Contract.Requires(font != null);

			AutoSize = false;
			AutoClose = false;
			DoubleBuffered = true;
			ResizeRedraw = true;
			TabStop = false;

			panel = new MemoryPreviewPanel(font)
			{
				Location = Point.Empty
			};

			var host = new ToolStripControlHost(panel);
			Padding = Margin = host.Padding = host.Margin = Padding.Empty;
			MinimumSize = panel.MinimumSize;
			panel.MinimumSize = panel.Size;
			MaximumSize = panel.MaximumSize;
			panel.MaximumSize = panel.Size;
			Size = panel.Size;

			panel.SizeChanged += (s, e) => Size = MinimumSize = MaximumSize = panel.Size;

			Items.Add(host);
		}

		protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
		{
			panel.Reset();

			base.OnClosed(e);
		}

		internal void HandleMouseWheelEvent(MouseEventArgs e)
		{
			if (e.Delta != 0)
			{
				panel.ChangeNodeCount(e.Delta < 0 ? 1 : -1);

				UpdateMemory();

				Invalidate();

				if (e is HandledMouseEventArgs he)
				{
					he.Handled = true;
				}
			}
		}

		/// <summary>Initializes the memory buffer.</summary>
		/// <param name="process">The process to use.</param>
		/// <param name="address">The address to read from.</param>
		public void InitializeMemory(RemoteProcess process, IntPtr address)
		{
			Contract.Requires(process != null);

			memoryAddress = address;

			var memory = panel.ViewInfo.Memory;
			memory.Process = process;
			memory.Update(address);
		}

		/// <summary>Updates the memory buffer to get current data.</summary>
		public void UpdateMemory()
		{
			panel.ViewInfo.Memory.Update(memoryAddress);

			panel.Invalidate();
		}
	}
}
