using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	[DefaultEvent(nameof(ColorChanged))]
	[DefaultBindingProperty(nameof(Color))]
	public partial class ColorBox : UserControl
	{
		private const int DefaultWidth = 123;
		private const int DefaultHeight = 20;

		private bool updateTextBox = true;

		public event EventHandler ColorChanged;

		private Color color;
		public Color Color
		{
			get => color;
			set
			{
				// Normalize the color because Color.Red != Color.FromArgb(255, 0, 0)
				value = Color.FromArgb(value.ToArgb());
				if (color != value)
				{
					color = value;

					colorPanel.BackColor = value;
					if (updateTextBox)
					{
						valueTextBox.Text = ColorTranslator.ToHtml(value);
					}

					OnColorChanged(EventArgs.Empty);
				}

				updateTextBox = true;
			}
		}

		protected virtual void OnColorChanged(EventArgs e)
		{
			Contract.Requires(e != null);

			var eh = ColorChanged;
			eh?.Invoke(this, e);
		}

		public ColorBox()
		{
			InitializeComponent();
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, DefaultWidth, DefaultHeight, specified);
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			try
			{
				var str = valueTextBox.Text;
				if (!str.StartsWith("#"))
				{
					str = "#" + str;
				}

				var newColor = ColorTranslator.FromHtml(str);

				updateTextBox = false;
				Color = newColor;
			}
			catch
			{

			}
		}

		private void OnPanelClick(object sender, EventArgs e)
		{
			using (var cd = new ColorDialog
			{
				FullOpen = true,
				Color = Color
			})
			{
				if (cd.ShowDialog() == DialogResult.OK)
				{
					Color = cd.Color;
				}
			}
		}

		private void OnPanelPaint(object sender, PaintEventArgs e)
		{
			var rect = colorPanel.ClientRectangle;
			rect.Width--;
			rect.Height--;
			e.Graphics.DrawRectangle(Pens.Black, rect);
		}
	}
}
