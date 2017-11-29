using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public partial class ColorBox : UserControl, ISettingsBindable
	{
		private const int DefaultWidth = 123;
		private const int DefaultHeight = 20;

		private PropertyInfo property;
		private Settings source;

		private bool updateTextBox = true;

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

					WriteSetting();
				}

				updateTextBox = true;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SettingName
		{
			get => property?.Name;
			set { property = typeof(Settings).GetProperty(value); ReadSetting(); }
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Settings Source
		{
			get => source;
			set { source = value; ReadSetting(); }
		}

		public ColorBox()
		{
			InitializeComponent();
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			base.SetBoundsCore(x, y, DefaultWidth, DefaultHeight, specified);
		}

		private void ReadSetting()
		{
			if (property != null && source != null)
			{
				var value = property.GetValue(source);
				if (value is Color newColor)
				{
					Color = newColor;
				}
			}
		}

		private void WriteSetting()
		{
			if (property != null && source != null)
			{
				property.SetValue(source, Color);
			}
		}

		private void valueTextBox_TextChanged(object sender, EventArgs e)
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

		private void colorPanel_Click(object sender, EventArgs e)
		{
			using (var cd = new ColorDialog())
			{
				cd.Color = Color;

				if (cd.ShowDialog() == DialogResult.OK)
				{
					Color = cd.Color;
				}
			}
		}

		private void colorPanel_Paint(object sender, PaintEventArgs e)
		{
			var rect = colorPanel.ClientRectangle;
			rect.Width--;
			rect.Height--;
			e.Graphics.DrawRectangle(Pens.Black, rect);
		}
	}
}
