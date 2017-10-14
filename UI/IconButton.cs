using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ReClassNET.UI
{
	/// <summary>
	/// Based on <see cref="ToolStripButton" />.
	/// </summary>
	[DefaultEvent("Click")]
	public class IconButton : Panel
	{
		public bool Pressed { get; set; }
		public bool Selected { get; set; }

		public Image Image { get; set; }
		public Rectangle ImageRectangle { get; } = new Rectangle(3, 3, 16, 16);

		private readonly ProfessionalColorTable colorTable = new ProfessionalColorTable();

		public IconButton()
		{
			DoubleBuffered = true;
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, 23, 22, specified);
		}

		protected override void Select(bool directed, bool forward)
		{
			base.Select(directed, forward);

			Selected = true;
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			Pressed = true;

			Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			Pressed = false;

			Invalidate();
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);

			Selected = true;

			Invalidate();
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);

			Selected = false;
			Pressed = false;

			Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			RenderButtonBackground(e.Graphics);
			RenderImage(e.Graphics);
		}

		private void RenderButtonBackground(Graphics g)
		{
			Contract.Requires(g != null);

			var bounds = new Rectangle(Point.Empty, Size);
			var drawHotBorder = true;

			if (Pressed)
			{
				RenderPressedButtonFill(g, bounds);
			}
			else if (Selected)
			{
				RenderSelectedButtonFill(g, bounds);
			}
			else
			{
				drawHotBorder = false;
				using (var brush = new SolidBrush(BackColor))
				{
					g.FillRectangle(brush, bounds);
				}
			}

			if (drawHotBorder)
			{
				using (var pen = new Pen(colorTable.ButtonSelectedBorder))
				{
					g.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
				}
			}
		}

		private void RenderPressedButtonFill(Graphics g, Rectangle bounds)
		{
			Contract.Requires(g != null);

			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}

			using (var brush = new LinearGradientBrush(bounds, colorTable.ButtonPressedGradientBegin, colorTable.ButtonPressedGradientEnd, LinearGradientMode.Vertical))
			{
				g.FillRectangle(brush, bounds);
			}
		}

		private void RenderSelectedButtonFill(Graphics g, Rectangle bounds)
		{
			Contract.Requires(g != null);

			if (bounds.Width == 0 || bounds.Height == 0)
			{
				return;
			}

			using (var brush = new LinearGradientBrush(bounds, colorTable.ButtonSelectedGradientBegin, colorTable.ButtonSelectedGradientEnd, LinearGradientMode.Vertical))
			{
				g.FillRectangle(brush, bounds);
			}
		}

		private void RenderImage(Graphics g)
		{
			Contract.Requires(g != null);

			var image = Image;
			if (image == null)
			{
				return;
			}

			var imageRect = ImageRectangle;

			if (!Enabled)
			{
				var disposeImage = false;
				if (Pressed)
				{
					imageRect.X += 1;
				}
				if (!Enabled)
				{
					image = ToolStripRenderer.CreateDisabledImage(image);
					disposeImage = true;
				}

				g.DrawImage(image, imageRect);

				if (disposeImage)
				{
					image.Dispose();
				}
				return;
			}

			g.DrawImage(image, imageRect);
		}
	}

	internal class IconButtonDesigner : ControlDesigner
	{
		private IconButtonDesigner()
		{
			AutoResizeHandles = true;
		}

		public override SelectionRules SelectionRules => SelectionRules.Moveable;
	}
}
