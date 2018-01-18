using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ReClassNET.UI
{
	public class PlaceholderTextBox : TextBox
	{
		private Font fontBackup;
		private Color foreColorBackup;
		private Color backColorBackup;

		/// <summary>
		/// The color of the placeholder text.
		/// </summary>
		[DefaultValue(typeof(Color), "ControlDarkDark")]
		public Color PlaceholderColor { get; set; } = SystemColors.ControlDarkDark;

		/// <summary>
		/// The placeholder text.
		/// </summary>
		[DefaultValue("")]
		public string PlaceholderText { get; set; }

		public PlaceholderTextBox()
		{
			fontBackup = Font;
			foreColorBackup = ForeColor;
			backColorBackup = BackColor;

			SetStyle(ControlStyles.UserPaint, true);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			if (string.IsNullOrEmpty(Text))
			{
				if (!GetStyle(ControlStyles.UserPaint))
				{
					fontBackup = Font;
					foreColorBackup = ForeColor;
					backColorBackup = BackColor;

					SetStyle(ControlStyles.UserPaint, true);
				}
			}
			else
			{
				if (GetStyle(ControlStyles.UserPaint))
				{
					SetStyle(ControlStyles.UserPaint, false);

					Font = fontBackup;
					ForeColor = foreColorBackup;
					BackColor = backColorBackup;
				}
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (string.IsNullOrEmpty(Text) && Focused == false)
			{
				using (var brush = new SolidBrush(PlaceholderColor))
				{
					e.Graphics.DrawString(PlaceholderText ?? string.Empty, Font, brush, new PointF(-1.0f, 1.0f));
				}
			}
		}
	}
}
