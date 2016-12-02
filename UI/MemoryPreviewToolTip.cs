using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReClassNET.Memory;
using ReClassNET.Nodes;

namespace ReClassNET.UI
{
	public class MemoryPreviewToolTip : ToolTip
	{
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FontEx Font
		{
			get { return viewInfo.Font; }
			set { viewInfo.Font = value; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public MemoryBuffer Memory
		{
			get { return viewInfo.Memory; }
			set { viewInfo.Memory = value; }
		}

		private readonly List<BaseHexNode> nodes;

		private readonly ViewInfo viewInfo;

		private Size size = new Size();

		public MemoryPreviewToolTip()
		{
			OwnerDraw = true;

#if WIN64
			nodes = new List<BaseHexNode>(Enumerable.Range(0, 10).Select(i => new Hex64Node { Offset = (IntPtr)(i * 8) }));
#else
			nodes = new List<BaseHexNode>(Enumerable.Range(0, 10).Select(i => new Hex32Node { Offset = (IntPtr)(i * 4) }));
#endif

			viewInfo = new ViewInfo
			{
				Memory = new MemoryBuffer { Size = nodes.Select(n => n.MemorySize).Sum() },

				HotSpots = new List<HotSpot>(),
				Classes = new List<ClassNode>()
			};

			Popup += new PopupEventHandler(OnPopup);
			Draw += new DrawToolTipEventHandler(OnDraw);
		}

		private void OnPopup(object sender, PopupEventArgs e)
		{
			size.Width = 604;
			size.Height = nodes.Select(n => n.CalculateHeight(viewInfo)).Sum() + 4;

			e.ToolTipSize = size;

			viewInfo.ClientArea = new Rectangle(2, 2, size.Width - 4, size.Height - 4);
		}

		private void OnDraw(object sender, DrawToolTipEventArgs e)
		{
			viewInfo.HotSpots.Clear();

			viewInfo.Context = e.Graphics;

			e.Graphics.FillRectangle(Brushes.White, e.Bounds);
			using (var pen = new Pen(Brushes.Black, 1))
			{
				e.Graphics.DrawRectangle(pen, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
			}

			int x = -24;
			int y = 2;
			foreach (var node in nodes)
			{
				y = node.Draw(viewInfo, x, y);
			}
		}

		public void UpdateMemory(RemoteProcess process, IntPtr address)
		{
			viewInfo.Memory.Process = process;
			viewInfo.Memory.Update(address);
		}
	}
}
